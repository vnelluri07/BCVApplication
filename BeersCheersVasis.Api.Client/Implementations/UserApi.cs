using BeersCheersVasis.Api.Client;
using BeersCheersVasis.Api.Models.User;

namespace BeersCheersVasis.Api.Client.Implementations
{
    public class UserApi : IUserApi
    {
        private readonly BcvHttpClient _httpClient;

        public UserApi(BcvHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<UserResponse>> ListAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<UserResponse>>("User/AllUsers");
            if (result == null)
            {
                throw new Exception("No roles matching request.");
            }
            return result;
        }

        //public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
        //{
        //    if (request is null)
        //    {
        //        throw new ArgumentNullException(nameof(CreateUserRequest));
        //    }

        //    var result = await _httpClient.PostAsJsonAsync<CreateUserRequest, CreateUserResponse>("User/CreateUser", request);
        //    if (request == null)
        //    {
        //        throw new Exception($"Invalid response for {nameof(CreateUserAsync)} method.");
        //    }

        //    return result;
        //}



        //public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request)
        //{
        //    if (request is null)
        //    {
        //        throw new ArgumentNullException(nameof(UpdateUserRequest));
        //    }

        //    var result = await _httpClient.PostAsJsonAsync<UpdateUserRequest, UpdateUserResponse>("User/UpdateUser", request);
        //    if (request == null)
        //    {
        //        throw new Exception($"Invalid response for {nameof(UpdateUserAsync)} method.");
        //    }

        //    return result;
        //}
    }
}
