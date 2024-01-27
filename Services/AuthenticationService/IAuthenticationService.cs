using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWI.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<ServiceResponse<string>> Login(UserLoginDto userRequest);
        Task<ServiceResponse<string>> Register(UserRequestDto userRequest);
        RefreshToken GenerateRefreshToken(string userName);
        Task<ServiceResponse<string>> GenerateJwtToken(string username, string refreshToken);
    }
}