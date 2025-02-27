using com.knapp.CodingContest.data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace com.knapp.CodingContest.warehouse
{
    public class ProductSurplusException : WarehouseException
    {
        public ProductSurplusException(Order order, Product product)
            : base($"{order} does not need more of {product}")
        {
        }
        public ProductSurplusException(string message) : base(message)
        {
        }

        public ProductSurplusException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductSurplusException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}

