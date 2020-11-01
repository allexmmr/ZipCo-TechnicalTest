using ZipCo.WebAPI.ViewModels;

namespace ZipCo.WebAPI.Services.Interface
{
    public interface IUserService : IService<UserResponse, UserRequest>
    {
        /// <summary>
        /// Get a user by email address.
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <returns>Return a user.</returns>
        UserResponse Get(string emailAddress);
    }
}