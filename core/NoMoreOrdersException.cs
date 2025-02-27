using com.knapp.CodingContest.data;
using System;
using System.Runtime.Serialization;

namespace com.knapp.CodingContest.warehouse
{
    public class NoMoreOrdersException : WarehouseException
    {
        public NoMoreOrdersException()
            : base($"no more orders to process")
        {
        }
        public NoMoreOrdersException(string message) : base(message)
        {
        }

        public NoMoreOrdersException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoMoreOrdersException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
