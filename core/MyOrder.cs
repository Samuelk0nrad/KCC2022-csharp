using com.knapp.CodingContest.data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.knapp.CodingContest.core
{
    public class MyOrder : Order
    {
        private readonly Dictionary<string, int[]> requested = new Dictionary<string, int[]>();

        internal MyOrder( Order order )
            : base ( order.Code, order.GetProducts() )
        {
            foreach (var p in order.GetProducts())
            {
                if (!requested.ContainsKey(p.Code))
                {
                    requested.Add(p.Code, new int[] { 0, 0 });
                }

                requested[p.Code][0]++;
                requested[p.Code][1]++;
            }
        }

        internal bool IsComplete() => requested.Values.Any(a => a[0] == 0);

        internal Dictionary<string, int[]> GetRequested() => requested;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"MyOrder[code={Code}] {{");
            foreach(var entry in requested )
            {
                int req = entry.Value[1];
                int rem = entry.Value[0];
                int prc = req - rem;

                sb.AppendLine($"{entry.Key} ({prc}/ {req})");
            }

            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
