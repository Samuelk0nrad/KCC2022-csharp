using com.knapp.CodingContest.data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace com.knapp.CodingContest.warehouse
{
    public class RobotLengthExceededException : WarehouseException
    {
        public RobotLengthExceededException(Robot robot, IReadOnlyList<Product> products)
            : base($"{robot} can not load { string.Join(",", products)} - would exceed length")
        {
        }
        public RobotLengthExceededException(string message) : base(message)
        {
        }

        public RobotLengthExceededException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RobotLengthExceededException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
