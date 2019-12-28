using BeatGrid.Application.Cognito;
using BeatGrid.Contracts.Response;
using System;
using System.Threading.Tasks;

namespace BeatGrid.Application.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(string username, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly ICognitoHelper _cognito;

        public AuthService(ICognitoHelper cognito)
        {
            _cognito = cognito;
        }

        public async Task<LoginResponse> Login(string username, string password)
        {
            var result = new LoginResponse();

            try
            {
                result.Token = await _cognito.GetToken(username, password);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
