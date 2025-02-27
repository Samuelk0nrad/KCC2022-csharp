using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.data
{
    public class Order
    {
        public string Code { get; private set; }

        private readonly List<Product> products = new List<Product>();

        internal Order ( string code, List<Product> productsArg )
        {
            Code = code;
            products = productsArg.ToList();
        }

        public List<Product> GetProducts() => products;

        public override string ToString()
        {
            return $"Order[ code = {Code}, products = [ { string.Join(",", products) } ]]";
        }

    }
}
