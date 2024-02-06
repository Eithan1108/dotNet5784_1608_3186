namespace BO
{
    /// <summary>
    /// Exception thrown when a Data Access Layer (DAL) does not exist.
    /// </summary>
    [Serializable]
    public class BlDoesNotExistException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DalDoesNotExistException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public BlDoesNotExistException(string? message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when attempting to create a Data Access Layer (DAL) that already exists.
    /// </summary>
    [Serializable]
    public class BlAlreadyExistsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DalAlreadyExistsException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public BlAlreadyExistsException(string? message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when the deletion of a Data Access Layer (DAL) is not possible.
    /// </summary>
    [Serializable]
    public class BlDeletionImpossible : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DalDeletionImpossible"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public BlDeletionImpossible(string? message) : base(message) { }
    }

    [Serializable]
    public class BlFileLoadCreateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DalXMLFileLoadCreateException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public BlFileLoadCreateException(string? message) : base(message) { }
    }

    [Serializable]
    public class BlBadIdException : Exception
    {
        /// <summary>
        /// /// Initializes a new instance of the <see cref="DalBadIdException"/> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public BlBadIdException(string? message) : base(message) { }
    }

    [Serializable]

    public class BlBadAliasException : Exception
    { /// <summary>
      /// /// Initializes a new instance of the <see cref="DalBadAliasException"/> class with a specified error message.
      /// </summary>
      /// <param name="message"></param>
        public BlBadAliasException(string? message) : base(message) { }
    }

    [Serializable]
    public class BlBadNameException : Exception
    { /// <summary>
      /// /// Initializes a new instance of the <see cref="DalBadNameException"/> class with a specified error message.
      /// </summary>
      /// <param name="message"></param>
        public BlBadNameException(string? message) : base(message) { }
    }

    [Serializable]
    public class BlBadEmailException : Exception
    { /// <summary>
      /// /// Initializes a new instance of the <see cref="DalBadEmailException"/> class with a specified error message.
      /// </summary>
      /// <param name="message"></param>
        public BlBadEmailException(string? message) : base(message) { }
    }

    [Serializable]
    public class BlBadCostException : Exception
    {
        /// <summary>
        /// /// Initializes a new instance of the <see cref="DalBadCostException"/> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public BlBadCostException(string? message) : base(message) { }
    }

    [Serializable]
    public class BlBadDateException : Exception
    {
        /// <summary>
        /// /// Initializes a new instance of the <see cref="DalBadCostException"/> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public BlBadDateException(string? message) : base(message) { }
    }

    [Serializable]
    public class BlBadLevelException : Exception
    {
        /// <summary>
        /// /// Initializes a new instance of the <see cref="DalBadCostException"/> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public BlBadLevelException(string? message) : base(message) { }
    }
}
