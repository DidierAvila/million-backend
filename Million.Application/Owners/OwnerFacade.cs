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
        public async Task<IEnumerable<OwnerDto>> GetAllOwnersAsync(CancellationToken cancellationToken) =>
            await _queryHandler.GetAllOwnersAsync(cancellationToken);

        public async Task<OwnerDto> GetOwnerByIdAsync(string id, CancellationToken cancellationToken) =>
            await _queryHandler.GetOwnerByIdAsync(id, cancellationToken);

        public async Task<OwnerDto> GetOwnerByNameAsync(string name, CancellationToken cancellationToken) =>
            await _queryHandler.GetOwnerByNameAsync(name, cancellationToken);

        public async Task<IEnumerable<OwnerDto>> GetOwnersByBirthDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken) =>
            await _queryHandler.GetOwnersByBirthDateRangeAsync(startDate, endDate, cancellationToken);

        // Command Methods
        public async Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createDto, CancellationToken cancellationToken) =>
            await _commandHandler.CreateOwnerAsync(createDto, cancellationToken);

        public async Task<OwnerDto> UpdateOwnerAsync(string id, UpdateOwnerDto updateDto, CancellationToken cancellationToken) =>
            await _commandHandler.UpdateOwnerAsync(id, updateDto, cancellationToken);

        public async Task<OwnerDto> DeleteOwnerAsync(string id, CancellationToken cancellationToken) =>
            await _commandHandler.DeleteOwnerAsync(id, cancellationToken);
    }
}
