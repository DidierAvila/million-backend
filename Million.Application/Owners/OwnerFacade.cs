using Million.Application.Owners.Commands;
using Million.Application.Owners.Queries;
using Million.Domain.DTOs;

namespace Million.Application.Owners
{
    public class OwnerFacade
    {
        private readonly IOwnerCommandHandler _commandHandler;
        private readonly IOwnerQueryHandler _queryHandler;

        public OwnerFacade(
            IOwnerCommandHandler commandHandler,
            IOwnerQueryHandler queryHandler)
        {
            _commandHandler = commandHandler;
            _queryHandler = queryHandler;
        }

        // Query Methods
        public Task<IEnumerable<OwnerDto>> GetAllOwnersAsync() =>
            _queryHandler.GetAllOwnersAsync();

        public Task<OwnerDto?> GetOwnerByIdAsync(string id) =>
            _queryHandler.GetOwnerByIdAsync(id);

        public Task<OwnerDto?> GetOwnerByNameAsync(string name) =>
            _queryHandler.GetOwnerByNameAsync(name);

        public Task<IEnumerable<OwnerDto>> GetOwnersByBirthDateRangeAsync(DateTime startDate, DateTime endDate) =>
            _queryHandler.GetOwnersByBirthDateRangeAsync(startDate, endDate);

        // Command Methods
        public Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createDto) =>
            _commandHandler.CreateOwnerAsync(createDto);

        public Task<bool> UpdateOwnerAsync(string id, UpdateOwnerDto updateDto) =>
            _commandHandler.UpdateOwnerAsync(id, updateDto);

        public Task<bool> DeleteOwnerAsync(string id) =>
            _commandHandler.DeleteOwnerAsync(id);
    }
}
