using BeersCheersVasis.Api.Models.User;

namespace BeersCheersVasis.Api.Client
{
    public interface IUserApi
    {
        public Task<IEnumerable<UserResponse>> ListAsync();
        //public Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request);
        //public Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request);
    }
}
