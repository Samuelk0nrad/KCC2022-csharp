namespace com.knapp.CodingContest.warehouse
{
    public interface IWarehouseInfo
    {
        /// <summary>
        /// number of unfinished orders
        /// </summary>
        /// <returns>number of unfinished orders</returns>
        int GetUnfinishedOrderCount();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>number of unfinished products still at entry</returns>
        int GetUnfinishedProductStillAtEntryCount();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>number of unfinished product in storage</returns>
        int GetUnfinishedProductInStorageCount();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>current distance(s) of movements</returns>
        long GetDistanceLevel();

        long GetDistancePosition();

        double GetDistanceDirect();


        //------------------------------------------------------------------------------------------


        /// <summary>
        /// 
        /// </summary>
        /// <returns>costs of current distance</returns>
        double GetDistanceCost();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>costs of current unfinished orders</returns>
        double GetUnfinishedOrdersCost();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>costs of unfinished products still at entry</returns>
        double GetUnfinishedProductsStillAtEntryCost();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>costs of unfinished products in storage</returns>
        double GetUnfinishedProductsInStorageCost();

        /// <summary>
        /// The total result used for ranking.
        ///
        ///  (Excludes time-based ranking factor)
        /// </summary>
        /// <returns></returns>
        double GetTotalCost();
    }
}
