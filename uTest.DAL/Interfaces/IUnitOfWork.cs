﻿using System;
using uTest.Entities.General;

namespace uTest.DAL.Interfaces
{
    /// <summary>
    /// Interface for implementation of unit of work pattern.
    /// Presantation layer will work with BLL using this one.
    /// Simply: unite all the implementations of generic repository together in one place.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gives access to tests repository
        /// </summary>
        IRepository<Test> Tests { get; }

        /// <summary>
        /// Gives access to questions repository
        /// </summary>
        IRepository<Question> Questions { get; }

        /// <summary>
        /// Gives access to answers repository
        /// </summary>
        IRepository<Answer> Answers { get; }

        /// <summary>
        /// Gives access to solved tests repository
        /// </summary>
        IRepository<SolvedTest> SolvedTests { get; }

        /// <summary>
        /// Gives access to tasks repository
        /// </summary>
        IRepository<Task> Tasks { get; }

        /// <summary>
        /// Saves all changes to the database
        /// </summary>
        void Save();
    }
}
