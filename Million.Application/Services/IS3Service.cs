using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Million.Application.Services
{
    public interface IS3Service
    {
        /// <summary>
        /// Uploads a file to AWS S3
        /// </summary>
        /// <param name="file">The file to upload</param>
        /// <param name="key">The key (filename) to use in S3</param>
        /// <returns>The URL of the uploaded file</returns>
        Task<string> UploadFileAsync(IFormFile file, string key);

        /// <summary>
        /// Uploads a stream to AWS S3
        /// </summary>
        /// <param name="stream">The stream to upload</param>
        /// <param name="key">The key (filename) to use in S3</param>
        /// <returns>The URL of the uploaded file</returns>
        Task<string> UploadStreamAsync(Stream stream, string key);

        /// <summary>
        /// Deletes a file from AWS S3
        /// </summary>
        /// <param name="key">The key (filename) to delete from S3</param>
        Task DeleteFileAsync(string key);

        /// <summary>
        /// Gets the URL for a file in S3
        /// </summary>
        /// <param name="key">The key (filename) in S3</param>
        /// <returns>The URL of the file</returns>
        string GetFileUrl(string key);
    }
}
