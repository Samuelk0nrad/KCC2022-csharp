using com.knapp.CodingContest.warehouse;
using com.knapp.CodingContest.data;
using System.Collections.Generic;
using com.knapp.CodingContest.core;
using System.Linq;
using System;

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

        public Solution( IWarehouse warehouse, InputData input )
        {
            this.warehouse = warehouse;
            this.input = input;

            storage = warehouse.GetStorage();
            entryLocation = storage.EntryLocation;
            exitLocation = storage.ExitLocation;
            robot = storage.Robot;

            ParticipantName = 
            Institute = 

            //TODO: Prepare data structures
        }

        public virtual void Run()
        {
            //YOUR CODE GOES HERE
            //See method Apis below
        }

        /// <summary>
        /// Just for documentation purposes.
        ///
        /// Method may be removed without any side-effects
        ///
        /// divided into 4 sections
        ///
        ///     <li><em>input methods</em>
        ///
        ///     <li><em>main interaction methods</em>
        ///         - these methods are the ones that make (explicit) changes to the warehouse
        ///
        ///     <li><em>information</em>
        ///         - information you might need for your solution
        ///
        ///     <li><em>additional information</em>
        ///         - various other infos: statistics, information about (current) costs, ...
        ///
        /// </summary>
#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
        private void Apis()
#pragma warning restore IDE0051 // Nicht verwendete private Member entfernen
        {
            // ----- input -----

            IReadOnlyList<Product> allProductsAtEntry = input.GetAllProductsAtEntry();

            IReadOnlyList<Order> allOrders = input.GetAllOrders();

            IReadOnlyCollection<Product> remainingProducts = warehouse.GetRemainingProductsAtEntry();

            IReadOnlyCollection<Order> remainingOrders = warehouse.GetRemainingOrders();

            Location location0 = storage.GetLocation(0, 0);
            List<Location> allLocations = storage.GetAllLocations();

            // ----- main interaction methods -----


            Location location;
            location = entryLocation;
            robot.PullFrom(location);

            location = exitLocation;
            robot.PushTo(location);

            Order order = warehouse.NextOrder();

            // ----- information -----

            Product product = order.GetProducts()[0];

            location.GetType();
            int level = location.Level;
            int position = location.Position;
            int length = location.Length;
            location.GetCurrentProducts();

            Location lamLocation = robot.CurrentLocation;
            robot.GetCurrentProducts();
            int robotLength = robot.Length;
            robot.GetRemainingLength();
            robot.GetCurrentMaxWidth();

            // ----- additional information -----
            IWarehouseInfo whi = warehouse.GetInfoSnapshot();
        }
    }
}
