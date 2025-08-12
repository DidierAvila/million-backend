using Million.Application.Owners.Commands;
using Million.Domain.DTOs;
using Million.Domain.Entities;
using Million.Domain.Repositories;
using Moq;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Million.Test.Owners
{
    public class OwnerCommandHandlerTests
    {
        private readonly Mock<IOwnerRepository> _mockOwnerRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<OwnerCommandHandler>> _mockLogger;
        private readonly OwnerCommandHandler _handler;

        public OwnerCommandHandlerTests()
        {
            _mockOwnerRepository = new Mock<IOwnerRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<OwnerCommandHandler>>();

            _handler = new OwnerCommandHandler(
                _mockOwnerRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Fact]
        public void BasicTest_ShouldPass()
        {
            // Arrange & Act
            var result = true;
            
            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task CreateOwnerAsync_ShouldCreateOwner_WhenNameIsUnique()
        {
            // Arrange
            var createDto = new CreateOwnerDto
            {
                Name = "John Doe",
                Address = "123 Main St",
                BirthDate = new DateTime(1980, 1, 1)
            };

            var owner = new Owner
            {
                Id = "owner123",
                Name = createDto.Name,
                Address = createDto.Address,
                BirthDate = createDto.BirthDate
            };

            var ownerDto = new OwnerDto
            {
                Id = owner.Id,
                Name = owner.Name,
                Address = owner.Address,
                BirthDate = owner.BirthDate,
                PropertiesCount = 0
            };

            _mockMapper.Setup(m => m.Map<Owner>(createDto)).Returns(owner);
            _mockMapper.Setup(m => m.Map<OwnerDto>(owner)).Returns(ownerDto);

            _mockOwnerRepository
                .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Owner, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mockOwnerRepository
                .Setup(r => r.AddAsync(owner, It.IsAny<CancellationToken>()))
                .ReturnsAsync(owner);

            // Act
            var result = await _handler.CreateOwnerAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be("owner123");
            result.Name.Should().Be("John Doe");
            result.Success.Should().BeTrue();
            _mockOwnerRepository.Verify(r => r.AddAsync(owner, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateOwnerAsync_ShouldReturnError_WhenNameAlreadyExists()
        {
            // Arrange
            var createDto = new CreateOwnerDto
            {
                Name = "Existing Owner",
                Address = "123 Main St",
                BirthDate = new DateTime(1980, 1, 1)
            };

            var owner = new Owner
            {
                Name = createDto.Name,
                Address = createDto.Address,
                BirthDate = createDto.BirthDate
            };

            _mockMapper.Setup(m => m.Map<Owner>(createDto)).Returns(owner);

            _mockOwnerRepository
                .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Owner, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.CreateOwnerAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Messages.Should().NotBeNull().And.Contain("already exists");
            _mockOwnerRepository.Verify(r => r.AddAsync(It.IsAny<Owner>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateOwnerAsync_ShouldUpdateOwner_WhenOwnerExists()
        {
            // Arrange
            var ownerId = "owner123";
            var updateDto = new UpdateOwnerDto
            {
                Name = "Updated Name",
                Address = "456 New Address",
                BirthDate = new DateTime(1985, 5, 5)
            };

            var existingOwner = new Owner
            {
                Id = ownerId,
                Name = "Original Name",
                Address = "123 Main St",
                BirthDate = new DateTime(1980, 1, 1)
            };

            var updatedOwner = new Owner
            {
                Id = ownerId,
                Name = updateDto.Name,
                Address = updateDto.Address,
                BirthDate = updateDto.BirthDate
            };

            var ownerDto = new OwnerDto
            {
                Id = updatedOwner.Id,
                Name = updatedOwner.Name,
                Address = updatedOwner.Address,
                BirthDate = updatedOwner.BirthDate,
                PropertiesCount = 2
            };

            _mockOwnerRepository
                .Setup(r => r.GetByIdAsync(ownerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingOwner);

            _mockMapper.Setup(m => m.Map(updateDto, existingOwner)).Returns(updatedOwner);
            _mockMapper.Setup(m => m.Map<OwnerDto>(It.IsAny<Owner>())).Returns(ownerDto);

            _mockOwnerRepository
                .Setup(r => r.UpdateAsync(ownerId, It.IsAny<Owner>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.UpdateOwnerAsync(ownerId, updateDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            _mockOwnerRepository.Verify(r => r.UpdateAsync(ownerId, It.IsAny<Owner>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOwnerAsync_ShouldReturnError_WhenOwnerNotFound()
        {
            // Arrange
            var ownerId = "nonexistent123";
            var updateDto = new UpdateOwnerDto
            {
                Name = "Updated Name",
                Address = "456 New Address",
                BirthDate = new DateTime(1985, 5, 5)
            };

            _mockOwnerRepository
                .Setup(r => r.GetByIdAsync(ownerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Owner?)null);

            // Act
            var result = await _handler.UpdateOwnerAsync(ownerId, updateDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Messages.Should().NotBeNull().And.Contain("not found");
            _mockOwnerRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<Owner>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOwnerAsync_ShouldDeleteOwner_WhenOwnerExists()
        {
            // Arrange
            var ownerId = "owner123";
            var existingOwner = new Owner
            {
                Id = ownerId,
                Name = "John Doe",
                Address = "123 Main St",
                BirthDate = new DateTime(1980, 1, 1)
            };

            var ownerDto = new OwnerDto
            {
                Id = existingOwner.Id,
                Name = existingOwner.Name,
                Address = existingOwner.Address,
                BirthDate = existingOwner.BirthDate
            };

            _mockOwnerRepository
                .Setup(r => r.GetByIdAsync(ownerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingOwner);

            _mockMapper.Setup(m => m.Map<OwnerDto>(It.IsAny<Owner>())).Returns(ownerDto);

            _mockOwnerRepository
                .Setup(r => r.DeleteAsync(ownerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.DeleteOwnerAsync(ownerId, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            _mockOwnerRepository.Verify(r => r.DeleteAsync(ownerId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteOwnerAsync_ShouldReturnError_WhenOwnerNotFound()
        {
            // Arrange
            var ownerId = "nonexistent123";

            _mockOwnerRepository
                .Setup(r => r.GetByIdAsync(ownerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Owner?)null);

            // Act
            var result = await _handler.DeleteOwnerAsync(ownerId, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Messages.Should().NotBeNull().And.Contain("not found");
            _mockOwnerRepository.Verify(r => r.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
