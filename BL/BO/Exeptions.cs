namespace BO
{
    [Serializable]
    public class BadIdException : Exception
    {
        public BadIdException(string? message) : base(message) { }
    }

    [Serializable]
    public class BlAlreadyExsistExeption : Exception
    {
        public BlAlreadyExsistExeption(string? message) : base(message) { }
    }

    [Serializable]
    public class BadAliasException : Exception
    {
        public BadAliasException(string? message) : base(message) { }
    }

    [Serializable]
    public class DalBadIdException : Exception
    {
        public DalBadIdException(string? message) : base(message) { }
    }


    [Serializable]
    public class BadInputException : Exception
    {
        public BadInputException(string? message) : base(message) { }
    }

    [Serializable]
    public class BadNameException : Exception
    {
        public BadNameException(string? message) : base(message) { }
    }

    [Serializable]
    public class BadEmailException : Exception
    {
        public BadEmailException(string? message) : base(message) { }
    }

    [Serializable]
    public class BadCostException : Exception
    {
        public BadCostException(string? message) : base(message) { }
    }

    [Serializable]
    public class BlBadIdException : Exception
    {
        public BlBadIdException(string? message) : base(message) { }
    }


    [Serializable]
    public class BlNotFoundException : Exception
    {
        public BlNotFoundException(string? message) : base(message) { }
    }



}
