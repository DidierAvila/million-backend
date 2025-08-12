using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Million.Application.Properties.Commands;
using Million.Domain.DTOs;
using Million.Domain.Entities;
using Million.Domain.Repositories;
using Moq;

namespace Million.Test.Properties
{
    public class PropertyCommandHandlerTests
    {
        private readonly Mock<IPropertyRepository> _mockPropertyRepository;
        private readonly Mock<IPropertyTraceRepository> _mockPropertyTraceRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<PropertyCommandHandler>> _mockLogger;
        private readonly PropertyCommandHandler _handler;

        public PropertyCommandHandlerTests()
        {
            _mockPropertyRepository = new Mock<IPropertyRepository>();
            _mockPropertyTraceRepository = new Mock<IPropertyTraceRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<PropertyCommandHandler>>();

            _handler = new PropertyCommandHandler(
                _mockPropertyRepository.Object,
                _mockPropertyTraceRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task CreatePropertyAsync_ShouldCreateProperty_WhenDataIsValid()
        {
            // Arrange
            var createDto = new CreatePropertyDto
            {
                Name = "Test Property",
                Address = "123 Main St",
                Price = 250000,
                Year = 2020,
                InternalCode = "PROP001",
                IdOwner = "owner123"
            };

            var property = new Property
            {
                Id = "property123",
                Name = createDto.Name,
                Address = createDto.Address,
                Price = createDto.Price,
                Year = createDto.Year,
                InternalCode = createDto.InternalCode,
                IdOwner = createDto.IdOwner
            };

            var propertyDto = new PropertyDto
            {
                Id = property.Id,
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                Year = property.Year,
                InternalCode = property.InternalCode,
                IdOwner = property.IdOwner,
                Success = true
            };

            _mockMapper.Setup(m => m.Map<Property>(createDto)).Returns(property);
            _mockMapper.Setup(m => m.Map<PropertyDto>(It.IsAny<Property>())).Returns(propertyDto);

            _mockPropertyRepository
                .Setup(r => r.AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(property);

            // Act
            var result = await _handler.CreatePropertyAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            _mockPropertyRepository.Verify(r => r.AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockPropertyTraceRepository.Verify(r => r.AddAsync(It.IsAny<PropertyTrace>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePropertyAsync_ShouldUpdateProperty_WhenPropertyExists()
        {
            // Arrange
            var propertyId = "property123";
            var updateDto = new UpdatePropertyDto
            {
                Name = "Updated Property",
                Address = "456 New St",
                Price = 300000,
                Year = 2021,
                InternalCode = "PROP001-UPD",
                IdOwner = "owner123"
            };

            var existingProperty = new Property
            {
                Id = propertyId,
                Name = "Original Property",
                Address = "123 Main St",
                Price = 250000,
                Year = 2020,
                InternalCode = "PROP001",
                IdOwner = "owner123"
            };

            var updatedProperty = new Property
            {
                Id = propertyId,
                Name = updateDto.Name,
                Address = updateDto.Address,
                Price = updateDto.Price,
                Year = updateDto.Year,
                InternalCode = updateDto.InternalCode,
                IdOwner = updateDto.IdOwner
            };

            var propertyDto = new PropertyDto
            {
                Id = updatedProperty.Id,
                Name = updatedProperty.Name,
                Address = updatedProperty.Address,
                Price = updatedProperty.Price,
                Year = updatedProperty.Year,
                InternalCode = updatedProperty.InternalCode,
                IdOwner = updatedProperty.IdOwner,
                Success = true
            };

            _mockPropertyRepository
                .Setup(r => r.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProperty);

            _mockMapper.Setup(m => m.Map(updateDto, existingProperty)).Returns(updatedProperty);
            _mockMapper.Setup(m => m.Map<PropertyDto>(It.IsAny<Property>())).Returns(propertyDto);

            _mockPropertyRepository
                .Setup(r => r.UpdateAsync(propertyId, It.IsAny<Property>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.UpdatePropertyAsync(propertyId, updateDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            _mockPropertyRepository.Verify(r => r.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()), Times.Once);
            _mockPropertyRepository.Verify(r => r.UpdateAsync(propertyId, It.IsAny<Property>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockPropertyTraceRepository.Verify(r => r.AddAsync(It.IsAny<PropertyTrace>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePropertyAsync_ShouldReturnError_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyId = "nonexistent123";
            var updateDto = new UpdatePropertyDto
            {
                Name = "Updated Property",
                Address = "456 New St",
                Price = 300000,
                Year = 2021,
                InternalCode = "PROP001-UPD",
                IdOwner = "owner123"
            };

            _mockPropertyRepository
                .Setup(r => r.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Property?)null);

            // Act
            var result = await _handler.UpdatePropertyAsync(propertyId, updateDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Messages.Should().NotBeNull().And.Contain("not found");
            _mockPropertyRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<Property>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeletePropertyAsync_ShouldDeleteProperty_WhenPropertyExists()
        {
            // Arrange
            var propertyId = "property123";
            var existingProperty = new Property
            {
                Id = propertyId,
                Name = "Property to Delete",
                Address = "123 Main St",
                Price = 250000,
                Year = 2020,
                InternalCode = "PROP001",
                IdOwner = "owner123"
            };

            var propertyDto = new PropertyDto
            {
                Id = existingProperty.Id,
                Name = existingProperty.Name,
                Address = existingProperty.Address,
                Price = existingProperty.Price,
                Year = existingProperty.Year,
                InternalCode = existingProperty.InternalCode,
                IdOwner = existingProperty.IdOwner,
                Success = true
            };

            _mockPropertyRepository
                .Setup(r => r.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProperty);

            _mockMapper.Setup(m => m.Map<PropertyDto>(It.IsAny<Property>())).Returns(propertyDto);

            _mockPropertyRepository
                .Setup(r => r.DeleteAsync(propertyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.DeletePropertyAsync(propertyId, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            _mockPropertyRepository.Verify(r => r.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()), Times.Once);
            _mockPropertyRepository.Verify(r => r.DeleteAsync(propertyId, It.IsAny<CancellationToken>()), Times.Once);
            _mockPropertyTraceRepository.Verify(r => r.AddAsync(It.IsAny<PropertyTrace>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeletePropertyAsync_ShouldReturnError_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyId = "nonexistent123";

            _mockPropertyRepository
                .Setup(r => r.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Property?)null);

            // Act
            var result = await _handler.DeletePropertyAsync(propertyId, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Messages.Should().NotBeNull().And.Contain("not found");
            _mockPropertyRepository.Verify(r => r.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
