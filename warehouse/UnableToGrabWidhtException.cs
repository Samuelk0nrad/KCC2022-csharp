using com.knapp.CodingContest.data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace com.knapp.CodingContest.warehouse
{
    public class UnableToGrabWidthException : WarehouseException
    {
        public UnableToGrabWidthException(Robot robot, Location location, IReadOnlyCollection<Product> products )
            : base($"{robot}: cannot grab products smaller than already on Robot (or about to be loaded) - #{products.Count} from {location}")
        {
        }
        public UnableToGrabWidthException(string message) : base(message)
        {
        }

        public UnableToGrabWidthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnableToGrabWidthException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
