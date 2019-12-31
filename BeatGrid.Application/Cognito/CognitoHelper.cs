using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using BeatGrid.Contracts.Common;
using System.Threading.Tasks;

namespace BeatGrid.Application.Cognito
{
    public interface ICognitoHelper
    {
        Task<TokenData> GetToken(string username, string password);
    }

    public class CognitoHelper : ICognitoHelper
    {
        public const string ClientId = "38qdjeagcn93uqgbnh18qoj4dg";
        public const string PoolId = "us-west-2_CPEOlgCr7";
        public const string Region = "us-west-2";

        private readonly IAmazonCognitoIdentityProvider _cognitoIdentityProvider;

        public CognitoHelper(IAmazonCognitoIdentityProvider cognitoIdentityProvider)
        {
            _cognitoIdentityProvider = cognitoIdentityProvider;
        }

        /// <summary>
        /// Returns token if successful. Throws exception otherwise.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<TokenData> GetToken(string username, string password)
        {
            var userPool = new CognitoUserPool(PoolId, ClientId, _cognitoIdentityProvider);
            var user = new CognitoUser(username, ClientId, userPool, _cognitoIdentityProvider);
            var authRequest = new InitiateSrpAuthRequest { Password = password };

            var authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);

            return new TokenData
            {
                Value = authResponse.AuthenticationResult.IdToken,
                ExpiresIn = authResponse.AuthenticationResult.ExpiresIn
            };
        }
    }
}
