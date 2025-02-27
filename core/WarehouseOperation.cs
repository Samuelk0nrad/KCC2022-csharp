using com.knapp.CodingContest.data;
using System.Text;

namespace com.knapp.CodingContest.core
{
    public abstract class WarehouseOperation
    {
        private readonly string resultString;

        protected WarehouseOperation( params object[] args )
        {
            StringBuilder sb = new StringBuilder();

            sb.Append( GetType().Name ).Append( ";" );

            foreach ( var arg in args )
            {
                sb.Append( arg.ToString() ).Append( ";" );
            }

            resultString = sb.ToString();
        }

        public override string ToString()
        {
            return resultString;
        }

        public string ToResultString()
        {
            return resultString;
        }

        internal class NextOrder : WarehouseOperation
        {
            public NextOrder()
                : base()
            { }

            public override string ToString()
            {
                return GetType().Name;
            }
        }

        internal class PullFrom : WarehouseOperation
        {
            public PullFrom(Location location)
                : base(location.Type, location.Level, location.Position, 1)
            { }
        }

        internal class PushTo : WarehouseOperation
        {
            public PushTo( Location location )
                : base( location.Type, location.Level, location.Position, 1 )
            { }
        }

    }
}
