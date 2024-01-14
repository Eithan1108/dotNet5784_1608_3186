using DO;
using System;
using System.Collections.Generic;

namespace DalApi
{
    /// <summary>
    /// Interface for CRUD (Create, Read, Update, Delete) operations on entities in the Data Access Layer (DAL).
    /// </summary>
    /// <typeparam name="T">Type parameter representing the entity type.</typeparam>
    public interface ICrud<T> where T : class
    {
        /// <summary>
        /// Creates a new entity object in the Data Access Layer (DAL).
        /// </summary>
        /// <param name="item">The entity object to be created.</param>
        /// <returns>The identifier of the created entity.</returns>
        int Create(T item);

        /// <summary>
        /// Reads the object based on the provided filter.
        /// </summary>
        /// <param name="filter">The filter condition to apply while reading the object.</param>
        /// <returns>The matching entity object, or null if not found.</returns>
        T? Read(Func<T, bool> filter);

        /// <summary>
        /// Reads all entity objects based on the optional filter condition.
        /// </summary>
        /// <param name="filter">Optional filter condition to apply while reading all objects.</param>
        /// <returns>An enumerable of matching entity objects, or all objects if no filter is provided.</returns>
        IEnumerable<T?> ReadAll(Func<T, bool>? filter = null);

        /// <summary>
        /// Updates an existing entity object in the Data Access Layer (DAL).
        /// </summary>
        /// <param name="item">The entity object to be updated.</param>
        void Update(T item);

        /// <summary>
        /// Deletes an entity object by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity object to be deleted.</param>
        void Delete(int id);
    }
}
