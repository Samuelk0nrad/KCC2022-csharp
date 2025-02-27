using com.knapp.CodingContest.data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace com.knapp.CodingContest.warehouse
{
    public class IllegalProductForOrderException : WarehouseException
    {
        public IllegalProductForOrderException(Order order, Product product)
            : base($"{order} does not need any {product}")
        {
        }
        public IllegalProductForOrderException(string message) : base(message)
        {
        }

        public IllegalProductForOrderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalProductForOrderException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}

