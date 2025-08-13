using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Million.Application.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _cloudFrontDomain;
        private readonly bool _publicReadEnabled;
        private readonly int _urlExpirationHours;
        private readonly long _maxFileSize;
        private readonly IEnumerable<string> _allowedFileTypes;
        private readonly IConfiguration _configuration;

        public S3Service(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            
            // Required settings
            _bucketName = _configuration["AWS:S3:BucketName"] ?? throw new InvalidOperationException("S3 bucket name not configured");
            
            // Optional settings with defaults
            _cloudFrontDomain = _configuration["AWS:S3:CloudFrontDomain"] ?? string.Empty;
            _publicReadEnabled = bool.TryParse(_configuration["AWS:S3:PublicReadEnabled"], out bool publicRead) ? publicRead : true;
            _urlExpirationHours = int.TryParse(_configuration["AWS:S3:URLExpiration"], out int urlExp) ? urlExp : 24;
            _maxFileSize = long.TryParse(_configuration["AWS:S3:MaxFileSize"], out long maxSize) ? maxSize : 10 * 1024 * 1024; // 10MB default
            
            // Get allowed file types from config, or use defaults
            var defaultTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp" };
            _allowedFileTypes = defaultTypes;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string key)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            
            // Validate file size
            if (file.Length > _maxFileSize)
            {
                throw new InvalidOperationException($"File size exceeds the maximum allowed size of {_maxFileSize / (1024 * 1024)} MB");
            }

            // Validate file type
            string contentType = file.ContentType;
            if (!_allowedFileTypes.Contains(contentType))
            {
                throw new InvalidOperationException($"File type {contentType} is not allowed. Allowed types: {string.Join(", ", _allowedFileTypes)}");
            }

            using var stream = file.OpenReadStream();
            return await UploadStreamAsync(stream, key);
        }

        public async Task<string> UploadStreamAsync(Stream stream, string key)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = stream,
                ContentType = GetContentType(key)
            };
            
            // Set public read permissions if enabled
            if (_publicReadEnabled)
            {
                request.CannedACL = S3CannedACL.PublicRead;
            }

            await _s3Client.PutObjectAsync(request);
            return GetFileUrl(key);
        }

        public async Task DeleteFileAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(request);
        }

        public string GetFileUrl(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            
            // Use CloudFront URL if available
            if (!string.IsNullOrWhiteSpace(_cloudFrontDomain))
            {
                // Ensure we have a clean URL
                string domain = _cloudFrontDomain.TrimEnd('/');
                string cleanKey = key.TrimStart('/');
                return $"{domain}/{cleanKey}";
            }
            
            // Otherwise use S3 direct URL
            string region = _configuration["AWS:Region"] ?? "us-east-1";
            
            // For non-public files we could generate pre-signed URLs with expiration
            if (!_publicReadEnabled)
            {
                // Generate pre-signed URL with expiration
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _bucketName,
                    Key = key,
                    Expires = DateTime.UtcNow.AddHours(_urlExpirationHours)
                };
                return _s3Client.GetPreSignedURL(request);
            }
            
            // Regular S3 URL for public files
            return $"https://{_bucketName}.s3.{region}.amazonaws.com/{key}";
        }

        private string GetContentType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }
}
