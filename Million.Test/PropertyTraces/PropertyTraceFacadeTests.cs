using FluentAssertions;
using Million.Application.PropertyTraces;
using Million.Application.PropertyTraces.Commands;
using Million.Application.PropertyTraces.Queries;
using Million.Domain.DTOs;
using Moq;

namespace Million.Test.PropertyTraces
{
    public class PropertyTraceFacadeTests
    {
        private readonly Mock<IPropertyTraceCommandHandler> _mockCommandHandler;
        private readonly Mock<IPropertyTraceQueryHandler> _mockQueryHandler;
        private readonly PropertyTraceFacade _facade;

        public PropertyTraceFacadeTests()
        {
            _mockCommandHandler = new Mock<IPropertyTraceCommandHandler>();
            _mockQueryHandler = new Mock<IPropertyTraceQueryHandler>();
            _facade = new PropertyTraceFacade(_mockCommandHandler.Object, _mockQueryHandler.Object);
        }

        [Fact]
        public void ShouldPassBasicTest()
        {
            // Act
            var result = true;
            
            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAllPropertyTracesAsync_ShouldReturnAllTraces()
        {
            // Arrange
            var expectedTraces = new List<PropertyTraceDto>
            {
                new PropertyTraceDto { 
                    IdPropertyTrace = "1", 
                    PropertyId = "prop1", 
                    Date = DateTime.Now, 
                    Value = 100000, 
                    Tax = 5000,
                    Name = "Test Trace 1",
                    Operation = "CREATE"
                },
                new PropertyTraceDto { 
                    IdPropertyTrace = "2", 
                    PropertyId = "prop2", 
                    Date = DateTime.Now, 
                    Value = 200000, 
                    Tax = 10000,
                    Name = "Test Trace 2",
                    Operation = "UPDATE"
                }
            };

            _mockQueryHandler
                .Setup(h => h.GetAllPropertyTracesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTraces);

            // Act
            var result = await _facade.GetAllPropertyTracesAsync(CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedTraces);
            _mockQueryHandler.Verify(h => h.GetAllPropertyTracesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetPropertyTraceByIdAsync_ShouldReturnTrace()
        {
            // Arrange
            var traceId = "trace123";
            var expectedTrace = new PropertyTraceDto 
            { 
                IdPropertyTrace = traceId, 
                PropertyId = "prop1", 
                Date = DateTime.Now, 
                Value = 100000, 
                Tax = 5000,
                Name = "Test Trace",
                Operation = "CREATE"
            };

            _mockQueryHandler
                .Setup(h => h.GetPropertyTraceByIdAsync(traceId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTrace);

            // Act
            var result = await _facade.GetPropertyTraceByIdAsync(traceId, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedTrace);
            _mockQueryHandler.Verify(h => h.GetPropertyTraceByIdAsync(traceId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetTracesByPropertyIdAsync_ShouldReturnTraces()
        {
            // Arrange
            var propertyId = "property123";
            var expectedTraces = new List<PropertyTraceDto>
            {
                new PropertyTraceDto 
                { 
                    IdPropertyTrace = "trace1", 
                    PropertyId = propertyId, 
                    Date = DateTime.Now.AddDays(-5), 
                    Value = 100000, 
                    Tax = 5000,
                    Name = "Create Trace",
                    Operation = "CREATE"
                },
                new PropertyTraceDto 
                { 
                    IdPropertyTrace = "trace2", 
                    PropertyId = propertyId, 
                    Date = DateTime.Now, 
                    Value = 120000, 
                    Tax = 6000,
                    Name = "Update Trace",
                    Operation = "UPDATE"
                }
            };

            _mockQueryHandler
                .Setup(h => h.GetTracesByPropertyIdAsync(propertyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTraces);

            // Act
            var result = await _facade.GetTracesByPropertyIdAsync(propertyId, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedTraces);
            _mockQueryHandler.Verify(h => h.GetTracesByPropertyIdAsync(propertyId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetTracesByDateRangeAsync_ShouldReturnTraces()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(-10);
            var endDate = DateTime.Now;
            var expectedTraces = new List<PropertyTraceDto>
            {
                new PropertyTraceDto 
                { 
                    IdPropertyTrace = "trace1", 
                    PropertyId = "prop1", 
                    Date = DateTime.Now.AddDays(-5), 
                    Value = 100000, 
                    Tax = 5000,
                    Name = "Trace in range 1",
                    Operation = "CREATE"
                },
                new PropertyTraceDto 
                { 
                    IdPropertyTrace = "trace2", 
                    PropertyId = "prop2", 
                    Date = DateTime.Now.AddDays(-2), 
                    Value = 200000, 
                    Tax = 10000,
                    Name = "Trace in range 2",
                    Operation = "UPDATE"
                }
            };

            _mockQueryHandler
                .Setup(h => h.GetTracesByDateRangeAsync(startDate, endDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTraces);

            // Act
            var result = await _facade.GetTracesByDateRangeAsync(startDate, endDate, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedTraces);
            _mockQueryHandler.Verify(h => h.GetTracesByDateRangeAsync(startDate, endDate, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreatePropertyTraceAsync_ShouldCreateTrace()
        {
            // Arrange
            var createDto = new CreatePropertyTraceDto
            {
                PropertyId = "prop123",
                Date = DateTime.Now,
                Value = 150000,
                Tax = 7500,
                Name = "New Trace",
                Operation = "CREATE"
            };

            var expectedTrace = new PropertyTraceDto
            {
                IdPropertyTrace = "newTrace123",
                PropertyId = createDto.PropertyId,
                Date = createDto.Date,
                Value = createDto.Value,
                Tax = createDto.Tax,
                Name = createDto.Name,
                Operation = createDto.Operation
            };

            _mockCommandHandler
                .Setup(h => h.CreatePropertyTraceAsync(createDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTrace);

            // Act
            var result = await _facade.CreatePropertyTraceAsync(createDto, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedTrace);
            _mockCommandHandler.Verify(h => h.CreatePropertyTraceAsync(createDto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeletePropertyTraceAsync_ShouldDeleteTrace()
        {
            // Arrange
            var traceId = "traceToDelete123";
            
            _mockCommandHandler
                .Setup(h => h.DeletePropertyTraceAsync(traceId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _facade.DeletePropertyTraceAsync(traceId, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _mockCommandHandler.Verify(h => h.DeletePropertyTraceAsync(traceId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
