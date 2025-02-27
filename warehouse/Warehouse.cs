using com.knapp.CodingContest.data;
using System.Collections.Generic;

namespace com.knapp.CodingContest.warehouse
{
    public interface IWarehouse
    {
        // ----------------------------------------------------------------------------
        // operations
        bool HasNextOrder();

        Order NextOrder();

        // ----------------------------------------------------------------------------
        // info

        Storage GetStorage();

        IReadOnlyCollection<Product> GetRemainingProductsAtEntry();

        IReadOnlyCollection<Order> GetRemainingOrders();

        /// <summary>
        /// Information about the current state of the warehouse.
        /// </summary>
        /// <returns>info</returns>
        IWarehouseInfo GetInfoSnapshot();

        /// <summary>
        /// Cost-factors used for calculating the result.
        /// </summary>
        /// <returns>cost-factors</returns>
        IWarehouseCostFactors GetCostFactors();

    }
}
