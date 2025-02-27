using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.data
{
    public class Product
    {
        public string Code { get; private set; }

        public int Length { get; private set; }

        public int Width { get; private set; }


        internal Product ( string code, int length, int width )
        {
            Code = code;
            Length = length;
            Width = width;
        }

        public override string ToString()
        {
            return $"Product[ code={Code}, lenght={Length}, width = {Width}]";
        }

    }
}
