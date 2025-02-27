using com.knapp.CodingContest.data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace com.knapp.CodingContest.warehouse
{
    public class LocationLengthExceededException : WarehouseException
    {
        public LocationLengthExceededException( Location location, IReadOnlyList<Product> products)
            : base($"cannot load {string.Join(",", products)} - would exceed length of {location}")
        {
        }
        public LocationLengthExceededException(string message) : base(message)
        {
        }

        public LocationLengthExceededException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LocationLengthExceededException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
