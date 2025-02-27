using com.knapp.CodingContest.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.warehouse
{
    public abstract class Robot
    {
        public int Length { get; private set; }
        
        public Location CurrentLocation { get; protected set; }

        private readonly LinkedList<Product> currentProducts = new LinkedList<Product>();

        protected Robot( int length )
        {
            Length = length;
        }

        public int GetRemainingLength() => Length - GetTotalLength(currentProducts);

        public int GetCurrentMaxWidth() => GetCurrentMaxWidthImpl( currentProducts );


        public IReadOnlyCollection<Product> GetCurrentProducts() => currentProducts;
    
        public virtual void PullFrom( Location location )
        {
            CheckNull("location", location);
            CheckLoad(location);
            Load(location.Pull(1));
        }


        public virtual void PushTo( Location location )
        {
            CheckNull("location", location);
            CheckStore( location );
            location.Push(Store(1));
        }

        //.........................................................................................

        private void CheckNull( string name, Object value )
        {
            if (value == null)
            {
                throw new ArgumentNullException($"{name} must not be <null>");
            }
        }

        private void CheckLoad( Location location )
        {
            location.CheckPull( 1 );

            var ps = GetSubList(location, location.GetCurrentProducts(), 1);

            if( GetRemainingLength() < GetTotalLength( ps ) )
            {
                throw new RobotLengthExceededException(this, ps);
            }

            CheckGrabWidth( location, ps );
        }

        private void CheckGrabWidth( Location location, IReadOnlyCollection<Product> ps )
        {
            int maxWidth = GetCurrentMaxWidthImpl(GetCurrentProducts());

            foreach( var p in ps )
            {
                if( p.Width < maxWidth )
                {
                    throw new UnableToGrabWidthException(this, location, ps );
                }
                maxWidth = (Math.Max(maxWidth, p.Width));
            }
        }


        //.........................................................................................

        private void CheckStore(Location location)
        {
            var ps = GetSubList(this, currentProducts, 1);

            location.CheckPush(ps);
        }

        private void Load( IReadOnlyList<Product> products )
        {
            foreach(var p in products)
            {
                currentProducts.AddFirst(p);
            }
        }

        private List<Product> Store( int numberOfProducts )
        {
            var pulled = new List<Product>();

            for( int i = 0; i < numberOfProducts; ++i )
            {
                pulled.Add(currentProducts.First());
                currentProducts.RemoveFirst();
            }

            return pulled;
        }

        //.........................................................................................

        private int GetTotalLength(IReadOnlyCollection<Product> ps) => ps.Sum(p => p.Length);

        private int GetCurrentMaxWidthImpl(IReadOnlyCollection<Product> ps) => ps.Any() ? ps.Max(p => p.Width) : 0;

        private List<Product> GetSubList( Object owner, IReadOnlyCollection<Product> products, int numberOfProducts)
        {
            if( numberOfProducts > products.Count )
            {
                throw new InsufficientProductsException( owner, numberOfProducts, products.Count );
            }


            List<Product> sublist = new List<Product>();

            for( int i = 0; i < numberOfProducts; ++i )
            {
                sublist.Add(products.ElementAt(i));
            }

            return sublist;
        }

    }
}
