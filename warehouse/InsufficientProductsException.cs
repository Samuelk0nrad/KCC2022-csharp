using com.knapp.CodingContest.data;
using System;
using System.Runtime.Serialization;

namespace com.knapp.CodingContest.warehouse
{
    public class InsufficientProductsException : WarehouseException
    {
        public InsufficientProductsException( Object owner, int requested, int available )
            : base($"not enoughproducts ({requested}/ {available}) @ {owner}")
        {
        }
        public InsufficientProductsException(string message) : base(message)
        {
        }

        public InsufficientProductsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InsufficientProductsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
