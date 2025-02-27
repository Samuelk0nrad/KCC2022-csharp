using com.knapp.CodingContest.data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace com.knapp.CodingContest.warehouse
{
    public class CantPullFromExitException : WarehouseException
    {
        public CantPullFromExitException(Location location, int numberOfProducts)
            : base($"cant pull # {numberOfProducts} @ {location}")
        {
        }
        public CantPullFromExitException(string message) : base(message)
        {
        }

        public CantPullFromExitException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CantPullFromExitException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
