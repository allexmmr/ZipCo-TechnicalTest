using System.Collections.Generic;

namespace ZipCo.WebAPI.Services.Interface
{
    public interface IService<TResponse, TRequest>
    {
        /// <summary>
        /// List all T objects.
        /// </summary>
        /// <returns>Return a list of T objects.</returns>
        IEnumerable<TResponse> List();

        /// <summary>
        /// Create a T object.
        /// </summary>
        /// <param name="request">TRequest request</param>
        /// <returns>Return the created object.</returns>
        TResponse Create(TRequest request);
    }
}