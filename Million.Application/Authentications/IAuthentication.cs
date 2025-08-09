using Million.Domain.DTOs;

namespace Million.Application.Authentications
{
    public interface IAuthentication
    {
        Task<LoginResponse> Login(LoginRequest autorizacion, CancellationToken cancellationToken);
    }
}