﻿using System;
using Microsoft.AspNet.Identity.EntityFramework;
using uTest.Auth.DAL.EF;
using uTest.Auth.DAL.Identity;
using uTest.Auth.DAL.Interfaces;
using uTest.Entities.Identity;

namespace uTest.Auth.DAL.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private readonly AuthContext _db;

        public IdentityUnitOfWork(string connectionString)
        {
            _db = new AuthContext(connectionString);
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db));
            ClientManager = new ClientManager(_db);
        }

        public ApplicationUserManager UserManager { get; }

        public IClientManager ClientManager { get; }

        public ApplicationRoleManager RoleManager { get; }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            //All unmanaged resources was disposed so GC needn't to call finalize method
            GC.SuppressFinalize(this);
        }
        private bool _disposed;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Frees resources
                    UserManager.Dispose();
                    RoleManager.Dispose();
                    ClientManager.Dispose();
                }
                _disposed = true;
            }
        }
    }
}