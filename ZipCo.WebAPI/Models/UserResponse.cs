using ZipCo.Data.Entities;

namespace ZipCo.WebAPI.ViewModels
{
    /// <summary>
    /// User Response Class.
    /// </summary>
    public class UserResponse : BaseResponse
    {
        /// <summary>
        /// User.
        /// </summary>
        public User User { get; set; }
    }
}