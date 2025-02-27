using com.knapp.CodingContest.data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace com.knapp.CodingContest.warehouse
{
    public class CantPushToEntryException : WarehouseException
    {
        public CantPushToEntryException( Location location, List<Product> products) 
            : base($"cant push {string.Join(", ", products)} @ {location}")
        {
        }
        public CantPushToEntryException(string message) : base(message)
        {
        }

        public CantPushToEntryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CantPushToEntryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
