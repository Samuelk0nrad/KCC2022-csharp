using com.knapp.CodingContest.data;
using System;
using System.Runtime.Serialization;

namespace com.knapp.CodingContest.warehouse
{
    public class NoSuchLocationException : WarehouseException
    {
        public NoSuchLocationException( WarehouseCharacteristics c, int level, int position )
            : base($"do not have a location with level/ position= {level}/ {position}([0 - {c.NumberOfLevels - 1}]/ [0 - {c.NumberOfLocationsPerLevel - 1}])")
        {
        }
        public NoSuchLocationException(string message) : base(message)
        {
        }

        public NoSuchLocationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoSuchLocationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
