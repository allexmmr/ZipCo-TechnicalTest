using ZipCo.Data.Entities;

namespace ZipCo.WebAPI.ViewModels
{
    /// <summary>
    /// Account Response Class.
    /// </summary>
    public class AccountResponse : BaseResponse
    {
        /// <summary>
        /// Account.
        /// </summary>
        public Account Account { get; set; }
    }
}