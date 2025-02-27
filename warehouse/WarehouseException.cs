using com.knapp.CodingContest.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.warehouse
{
    public abstract class WarehouseException : Exception
    {
        public WarehouseException(string message) : base(message)
        {
        }

        public WarehouseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WarehouseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
