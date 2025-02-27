using com.knapp.CodingContest.warehouse;
using com.knapp.CodingContest.data;
using System.Collections.Generic;
using com.knapp.CodingContest.core;
using System.Linq;
using System;
using System.Diagnostics.CodeAnalysis;

namespace com.knapp.CodingContest.solution
{
    public class Solution
    {

        public string ParticipantName { get; protected set; }

        public Institutes Institute { get; protected set; }

        protected readonly InputData input;
        protected readonly IWarehouse warehouse;

        protected readonly Storage storage;
        protected readonly Location entryLocation;
        protected readonly Location exitLocation;
        protected readonly Robot robot;

        protected Dictionary<Product, Dictionary<Location, int>> products =
            new Dictionary<Product, Dictionary<Location, int>>();

        protected Dictionary<Product, int> orderProducts = new Dictionary<Product, int>();

        public Solution(IWarehouse warehouse, InputData input)
        {
            this.warehouse = warehouse;
            this.input = input;

            storage = warehouse.GetStorage();
            entryLocation = storage.EntryLocation;
            exitLocation = storage.ExitLocation;
            robot = storage.Robot;

            ParticipantName = "";
            Institute = Institutes.HTL_Weiz;

            //TODO: Prepare data structures
        }

        public virtual void Run()
        {
            while (warehouse.HasNextOrder())
            {
                Order order;
                try
                {
                    order = warehouse.NextOrder();
                }
                catch (Exception e)
                {
                    order = warehouse.GetCurrentOrder();
                }

                orderProducts = order.GetProducts().GroupBy(p => p).ToDictionary(g => g.Key, g => g.Count());

                var entryProducts = warehouse.GetRemainingProductsAtEntry();
                while (orderProducts.Count > 0 && entryProducts.Count > 0)
                {
                    entryProducts = warehouse.GetRemainingProductsAtEntry();
                    TryPullFromEntry();
                    if (orderProducts.Count < 0)
                    {
                        break;
                    }
                }

                if (orderProducts.Count == 0)
                {
                    continue;
                }

                while (orderProducts.Count > 0)
                {
                    GetProductsFromStorage(orderProducts.First().Key, orderProducts.First().Value);
                }
            }
        }

        private void TryPullFromEntry()
        {
            try
            {
                robot.PullFrom(entryLocation);
            }
            catch (RobotLengthExceededException e)
            {
                StoreProduct();
            }
            catch (UnableToGrabWidthException e)
            {
                StoreProduct();
            }
        }

        private void GetProductsFromStorage(Product product, int quantity)
        {
            var remainingLength = robot.GetRemainingLength();
            var maxWidth = robot.GetCurrentMaxWidth();
            var neededLength = quantity * product.Length;

            // Check if robot has enough space
            if (remainingLength < neededLength || maxWidth > product.Width)
            {
                StoreProduct();
            }

            // Get products from storage
            var remaining = quantity;
            while (remaining > 0)
            {
                var actualLocation = robot.CurrentLocation;
                var nearestLocation = GetNearestLocationWithProduct(product, actualLocation);
                // No location with product found
                if (nearestLocation == null)
                {
                    StoreProduct();
                    break;
                }

                // Get products from location
                var locationProducts = nearestLocation.GetCurrentProducts();
                foreach (var locationProduct in locationProducts.ToList())
                {
                    if (locationProduct.Width > robot.GetCurrentMaxWidth())
                    {
                        robot.PullFrom(nearestLocation);
                        if (products.ContainsKey(product))
                        {
                            if (products[product].ContainsKey(nearestLocation))
                            {
                                products[product][nearestLocation]--;
                                if (products[product][nearestLocation] <= 0)
                                {
                                    products[product].Remove(nearestLocation);
                                }
                            }
                        }

                        if (product == locationProduct)
                        {
                            remaining--;
                            if (remaining <= 0)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        StoreProduct();
                    }
                }
            }
        }

        private void StoreProduct()
        {
            while (robot.GetCurrentProducts().Count > 0)
            {
                var robotProducts = robot.GetCurrentProducts();
                var product = robotProducts.First();
                if (orderProducts.ContainsKey(product))
                {
                    robot.PushTo(exitLocation);
                    orderProducts[product]--;
                    if (orderProducts[product] <= 0)
                    {
                        orderProducts.Remove(product);
                    }
                }
                else
                {
                    var pLocation = GetNearestLocationForStorage(product, robot.CurrentLocation);
                    robot.PushTo(pLocation);
                    if (products.ContainsKey(product))
                    {
                        if (products[product].ContainsKey(pLocation))
                        {
                            products[product][pLocation]++;
                        }
                        else
                        {
                            products[product].Add(pLocation, 1);
                        }
                    }
                    else
                    {
                        products.Add(product, new Dictionary<Location, int> { { pLocation, 1 } });
                    }
                }
            }
        }

        private Location GetNearestLocationWithProduct(Product product, Location location)
        {
            return GetNearestLocationListWithProduct(product, location)?.FirstOrDefault();
        }

        private IOrderedEnumerable<Location> GetNearestLocationListWithProduct(Product product, Location location,
            Func<Location, bool> filter = null)
        {
            if (!products.TryGetValue(product, out var product1))
            {
                return null;
            }

            var productLocations = product1.Keys.ToList();
            if (filter != null)
            {
                productLocations = productLocations.ToList().Where(filter).ToList();
            }

            var sortedLocations = productLocations.OrderBy(p =>
            {
                var distance = Math.Abs(p.Level - location.Level) + Math.Abs(p.Position - location.Position);
                return distance;
            });

            return sortedLocations;
        }

        private Location GetNearestLocationForStorage(Product product, Location location)
        {
            var locationsWithProduct = GetNearestLocationListWithProduct(product, location,
                l => l.GetRemainingLength() >= product.Length);
            var allLocations = storage.GetAllLocations();
            var sortedLocations = allLocations.Where(l => l.GetCurrentProducts().Count == 0).OrderBy(p =>
            {
                var distance = Math.Abs(p.Level - location.Level) + Math.Abs(p.Position - location.Position);
                return distance;
            });

            Location result = null;
            var locationWithProduct = locationsWithProduct?.FirstOrDefault();
            var locationWithProductDistance = int.MaxValue;
            if (locationWithProduct != null)
            {
                locationWithProductDistance = Math.Abs(locationWithProduct.Level - location.Level) +
                                              Math.Abs(locationWithProduct.Position - location.Position);
            }

            var emptyLocation = sortedLocations.FirstOrDefault();
            var emptyLocationDistance = int.MaxValue;
            if (emptyLocation != null)
            {
                emptyLocationDistance = Math.Abs(emptyLocation.Level - location.Level) +
                                        Math.Abs(emptyLocation.Position - location.Position);
            }

            if (emptyLocation != null || locationWithProduct != null)
            {
                result = locationWithProductDistance <= emptyLocationDistance ? locationWithProduct : emptyLocation;
            }

            if (result != null)
            {
                return result;
            }

            sortedLocations = allLocations.Where(l => l.GetRemainingLength() >= product.Length).OrderBy(p =>
            {
                var distance = Math.Abs(p.Level - location.Level) + Math.Abs(p.Position - location.Position);
                return distance;
            });
            return sortedLocations.FirstOrDefault();
        }
    }
}
