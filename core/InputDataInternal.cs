using com.knapp.CodingContest.data;
using com.knapp.CodingContest.warehouse;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.knapp.CodingContest.core
{
    public class InputDataInternal : InputData
    {

        private readonly LinkedList<Product> productInqueue = new LinkedList<Product>();

        private readonly LinkedList<MyOrder> remainingOrders = new LinkedList<MyOrder>();

        private MyOrder currentOrder;

        internal InputDataInternal( )
            : base()
        {  }


        public override void Load()
        {
            base.Load();

            foreach( var p in productsAtEntry )
            {
                productInqueue.AddLast(p);
            }

            foreach( var o in allOrders )
            {
                remainingOrders.AddLast(new MyOrder(o));
            }
        }

        public LinkedList<Product> GetProductInQueue() => productInqueue;

        public LinkedList<MyOrder> GetRemainingOrders() => remainingOrders;

        public bool HasNextOrder() => remainingOrders.Any();

        public MyOrder GetCurrentOrder() => currentOrder;

        internal Order NextOrder()
        {
            if( currentOrder != null && !currentOrder.IsComplete() )
            {
                throw new OrderIncompleteException(currentOrder);
            }

            if( remainingOrders.Count == 0 )
            {
                throw new NoMoreOrdersException();
            }

            currentOrder = remainingOrders.First();
            remainingOrders.RemoveFirst();

            return currentOrder;
        }
    }
}
