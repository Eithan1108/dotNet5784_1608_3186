namespace DO
{
    /// <summary>
    /// Exception thrown when a Data Access Layer (DAL) does not exist.
    /// </summary>
    [Serializable]
    public class DalDoesNotExistException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DalDoesNotExistException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DalDoesNotExistException(string? message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when attempting to create a Data Access Layer (DAL) that already exists.
    /// </summary>
    [Serializable]
    public class DalAlreadyExistsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DalAlreadyExistsException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DalAlreadyExistsException(string? message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when the deletion of a Data Access Layer (DAL) is not possible.
    /// </summary>
    [Serializable]
    public class DalDeletionImpossible : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DalDeletionImpossible"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DalDeletionImpossible(string? message) : base(message) { }
    }

    [Serializable]
    public class DalXMLFileLoadCreateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DalXMLFileLoadCreateException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DalXMLFileLoadCreateException(string? message) : base(message) { }
    }
}
