using System;
using uTest.Entities.Identity;

namespace uTest.Auth.DAL.Interfaces
{
    /// <summary>
    /// Describes interaction with client profile
    /// </summary>
    public interface IClientManager : IDisposable
    {
        /// <summary>
        /// Creates new client profile
        /// </summary>
        /// <param name="item">Profile to create</param>
        void Create(ClientProfile item);
    }
}