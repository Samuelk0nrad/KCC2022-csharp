using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.data
{
    public abstract class Location
    {
        public enum LocationType { Storage, Entry, Exit };


        public LocationType Type { get; private set; }

        public int Level { get; private set; }

        public int Position { get; private set; }

        public int Length { get; private set; }

        protected readonly LinkedList<Product> currentProducts = new LinkedList<Product>();

        protected Location( LocationType type, int level, int position, int length )
        {
            Type = type;
            Level = level;
            Position = position;
            Length = length;
        }

        public int GetRemainingLength() => Length - currentProducts.Sum(p => p.Length);

        public virtual IReadOnlyCollection<Product> GetCurrentProducts() => currentProducts;
        public abstract override string ToString();

        public abstract void CheckPull(int numberOfProducts);
        
        public abstract List<Product> Pull(int numberOfProducts);

        public abstract void CheckPush(List<Product> products);

        public abstract void Push(List<Product> products);
    }
}
