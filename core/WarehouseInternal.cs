using com.knapp.CodingContest.data;
using com.knapp.CodingContest.warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.knapp.CodingContest.core
{
    public class WarehouseInternal : IWarehouse
    {
        private readonly InputDataInternal input;

        private readonly MyStorage storage;

        private readonly IWarehouseCostFactors costFactors = new CostFactors();

        private readonly List<WarehouseOperation> operations = new List<WarehouseOperation>();


        public int Moves { get; private set; }

        public long DistanceLevel { get; private set; }
        public long DistancePosition { get; private set; }
        public double DistanceDirect { get; private set; }


        public WarehouseInternal( InputDataInternal input )
        {
            this.input = input;
            storage = new MyStorage(this, input.WarehouseCharacteristics);
        }

        public bool HasNextOrder() => input.HasNextOrder();


        public Order NextOrder()
        {
            Order next = input.NextOrder();

            Add(new WarehouseOperation.NextOrder());

            return next;
        }

        public Storage GetStorage() => storage;

        public IReadOnlyCollection<Product> GetRemainingProductsAtEntry() => input.GetProductInQueue();

        public IReadOnlyCollection<Order> GetRemainingOrders() =>input.GetRemainingOrders();
        

        public IWarehouseInfo GetInfoSnapshot()
        {
            return new WarehouseInfoSnapShot(this);
        }

        public IWarehouseCostFactors GetCostFactors() => costFactors;

        internal MyOrder GetCurrentOrder() => input.GetCurrentOrder();

        public IEnumerable<WarehouseOperation> GetResult() => operations;

        protected void Add(WarehouseOperation operation)
        {
            operations.Add(operation);
        }

        private static Robot CreateRobot(WarehouseInternal whi, int robotLength)
        {
            return new MyRobot(robotLength, whi);
        }

        private static List<List<Location>> CreateLocations(WarehouseInternal whi, WarehouseCharacteristics c)
        {
            List<List<Location>> locations = new List<List<Location>>(c.NumberOfLevels);

            for (int l = 0; l < c.NumberOfLevels; ++l)
            {
                List<Location> levelLocations = new List<Location>(c.NumberOfLocationsPerLevel);
                locations.Add(levelLocations);

                for (int p = 0; p < c.NumberOfLocationsPerLevel; ++p)
                {
                    levelLocations.Add(new StorageLocation(l, p, c.LocationLength));
                }
            }


            return locations;
        }

        //.........................................................................................
        private class MyStorage : Storage
        {
            internal MyStorage(WarehouseInternal whi, WarehouseCharacteristics c)
                : base(c,
                        WarehouseInternal.CreateRobot(whi, c.RobotLength),
                        WarehouseInternal.CreateLocations(whi, c),
                        new EntryLocation(c.EntryLevel, c.EntryPosition, whi.input, c.RobotLength),
                        new ExitLocation(c.ExitLevel, c.ExitPosition, whi.input, c.RobotLength)
                        )
            {
                ((MyRobot)Robot).SetLocation(GetLocation(0, 0));
            }
        }

        //.........................................................................................
        private class MyRobot : Robot
        {
            private readonly WarehouseInternal whi;

            internal MyRobot( int length, WarehouseInternal whi )
                : base( length )
            {
                this.whi = whi;
            }

            internal void SetLocation( Location location )
            {
                CurrentLocation = location;
            }

            public override void PullFrom( Location location )
            {
                base.PullFrom(location);
                whi.Add(new WarehouseOperation.PullFrom(location));
                
                whi.UpdateMovements( CurrentLocation, location) ;
                SetLocation(location);
            }

            public override void PushTo(Location location)
            {
                base.PushTo(location);
                
                whi.Add(new WarehouseOperation.PushTo( location ));
                whi.UpdateMovements(CurrentLocation, location);
                SetLocation(location);
            }
        }

        //.........................................................................................
        internal class EntryLocation : Location
        {
            private readonly InputDataInternal inputData;
            private readonly int robotLength;

            internal EntryLocation(int level, int position, InputDataInternal inputData, int robotLength)
                : base(Location.LocationType.Entry, level, position, 0)
            {
                this.inputData = inputData;
                this.robotLength = robotLength;
            }

            public override string ToString()
            {
                int count = 0;
                var iter = inputData.GetProductInQueue().AsEnumerable().GetEnumerator();

                for (int length = 0; iter.MoveNext() && length < robotLength;)
                {
                    length += iter.Current.Length;
                    count++;
                }

                string products;

                if (count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < count; ++i)
                    {
                        if (i > 0)
                        {
                            sb.Append(" ,");
                        }

                        sb.Append(inputData.GetProductInQueue().ElementAt(i));
                    }

                    products = sb.ToString();
                }
                else
                {
                    products = "<empty>";
                }

                return $"EntryLocation[level={Level}, position={Position}, length={Length}] {{entry.products(lim(robot.length)) = {products}}}";
            }

            public override void CheckPull(int numberOfProducts)
            {
                if (numberOfProducts > inputData.GetProductInQueue().Count)
                {
                    throw new InsufficientProductsException(this, numberOfProducts, inputData.GetProductInQueue().Count);
                }
            }

            public override IReadOnlyCollection<Product> GetCurrentProducts()
            {
                return inputData.GetProductInQueue();
            }

            public override List<Product> Pull(int numberOfProducts)
            {
                CheckPull(numberOfProducts);

                var pulled = new List<Product>(numberOfProducts);

                for (int i = 0; i < numberOfProducts; ++i)
                {
                    pulled.Add(inputData.GetProductInQueue().First());

                    inputData.GetProductInQueue().RemoveFirst();
                }

                return pulled;
            }

            public override void CheckPush(List<Product> products)
            {
                throw new CantPushToEntryException(this, products);
            }

            public override void Push(List<Product> products)
            {
                CheckPush(products);
                throw new CantPushToEntryException(this, products);
            }
        }

        internal class ExitLocation : Location
        {

            private readonly InputDataInternal inputData;
            private readonly int robotLength;

            internal ExitLocation(int level, int position, InputDataInternal inputData, int robotLength)
                : base(Location.LocationType.Exit, level, position, 0)
            {
                this.inputData = inputData;
                this.robotLength = robotLength;
            }


            public override void CheckPull(int numberOfProducts)
            {
                throw new CantPullFromExitException( this, numberOfProducts );
            }

            public override List<Product> Pull(int numberOfProducts)
            {
                CheckPull(numberOfProducts);
                throw new CantPullFromExitException(this, numberOfProducts);

            }

            public override void CheckPush(List<Product> products)
            {
                if( inputData.GetCurrentOrder() == null )
                {
                    throw new InvalidOperationException($"{GetType().Name}:can't push() unless nextOrder() has been called ");
                }

                CheckProductsNeeded(inputData.GetCurrentOrder(), products);
            }

            private void CheckProductsNeeded( MyOrder myOrder, List<Product> products )
            {
                var requested = new Dictionary<string, int[]>();

                //var requested = new Dictionary<>
                foreach( var p in products  )
                {
                    if ( !myOrder.GetRequested().ContainsKey( p.Code ) )
                    {
                        throw new IllegalProductForOrderException(myOrder, p);
                    }

                    if( ! requested.ContainsKey( p.Code ) )
                    {
                        requested.Add(p.Code, new int[] { myOrder.GetRequested()[p.Code][0] });
                    }

                    var pr = requested[p.Code];

                    if( pr[0] <= 0 )
                    {
                        throw new ProductSurplusException(myOrder, p);
                    }

                    pr[0]--;
                }
            }

            public override void Push(List<Product> products)
            {
                CheckPush(products);

                foreach( var p in products )
                {
                    inputData.GetCurrentOrder().GetRequested()[p.Code][0]--;
                }
            }

            public override string ToString()
            {
                return $"ExitLoation[level={Level}, position={Position}, length={Length}] {{currentOrder={inputData.GetCurrentOrder()}}}";
            }
        }

        private class StorageLocation : Location
        {
            internal StorageLocation(int level, int position, int length)
                : base(LocationType.Storage, level, position, length)
            { }

            public override string ToString()
            {
                return $"StorageLocation[level={Level}, position={Position}, length={Length} ({GetRemainingLength()})]{string.Join(";", currentProducts)}";
            }

            public override void CheckPull(int numberOfProducts)
            {
                WarehouseInternal.GetSubList(this, GetCurrentProducts(), numberOfProducts);
            }

            public override List<Product> Pull(int numberOfProducts)
            {
                CheckPull(numberOfProducts);

                List<Product> pulled = new List<Product>(numberOfProducts);

                for (int i = 0; i < numberOfProducts; ++i)
                {
                    pulled.Add(currentProducts.First());
                    currentProducts.RemoveFirst();
                }

                return pulled;
            }

            public override void CheckPush(List<Product> products)
            {
                if (GetRemainingLength() < WarehouseInternal.GetTotalLength(products))
                {
                    throw new LocationLengthExceededException(this, products);
                }
            }

            public override void Push(List<Product> products)
            {
                CheckPush(products);
                foreach (var p in products)
                {
                    currentProducts.AddFirst(p);
                }
            }
        }
        //.........................................................................................

        private static int GetTotalLength(List<Product> products) => products.Sum(p => p.Length);

        public bool IsCurrentOrderFinished() //TODO!!
        {
            return input.GetCurrentOrder() == null || input.GetCurrentOrder().IsComplete();
        }

        private void UpdateMovements(Location from, Location to)
        {
            Moves++;
            int dl = Math.Abs(to.Level - from.Level);
            int dp = Math.Abs(to.Position - from.Position);

            DistanceLevel += dl;
            DistancePosition += dp;
            DistanceDirect += Math.Sqrt((dl * dl) + (dp * dp));
        }

        private static List<Product> GetSubList( Object owner, IReadOnlyCollection<Product> products, int numberOfProducts )
        {
            if (numberOfProducts > products.Count)
            {
                throw new InsufficientProductsException(owner, numberOfProducts, products.Count);
            }


            List<Product> sublist = new List<Product>();

            for (int i = 0; i < numberOfProducts; ++i)
            {
                sublist.Add(products.ElementAt(i));
            }

            return sublist;

        }
    }
}
