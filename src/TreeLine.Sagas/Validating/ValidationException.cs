using System;
using System.Runtime.Serialization;

namespace TreeLine.Sagas.Validating
{
    [Serializable]
    public sealed class ValidationException : Exception
    {
        public ValidationException()
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        private ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}