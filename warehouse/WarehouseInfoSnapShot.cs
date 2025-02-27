using com.knapp.CodingContest.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.warehouse
{
    internal class WarehouseInfoSnapShot : IWarehouseInfo
    {

        private readonly int unfinishedOrderCount;
        private readonly int unfinishedProductStillAtEntryCount;
        private readonly int unfinishedProductInStorageCount;

        private readonly long distanceLevel;
        private readonly long distancePosition;
        private readonly double distanceDirect;

        private readonly double unfinishedOrdersCost;
        private readonly double unfinishedProductsStillAtEntryCost;
        private readonly double unfinishedProductsInStorageCost;
        
        private readonly double distanceCost;
        private readonly double totalCost;


        internal WarehouseInfoSnapShot( WarehouseInternal wh )
        {
            unfinishedOrderCount = wh.GetRemainingOrders().Count()
                                    + (IsFinished( wh ) ? 0 : 1)
                                    ;

            Dictionary<string, int> unfinishedProducts = new Dictionary<string, int>();

            foreach( var product in wh.GetRemainingOrders().SelectMany(o => o.GetProducts() ) )
            {
                if( !unfinishedProducts.ContainsKey( product.Code ) )
                {
                    unfinishedProducts.Add(product.Code, 1);
                }
                else
                {
                    unfinishedProducts[product.Code]++;
                }
            }
            if( ! wh.IsCurrentOrderFinished() )
            {
                foreach( var e in wh.GetCurrentOrder().GetRequested() )
                {
                    if( e.Value[0] > 0 )
                    {
                        if (!unfinishedProducts.ContainsKey(e.Key))
                        {
                            unfinishedProducts.Add(e.Key, 1);
                        }
                        else
                        {
                            unfinishedProducts[e.Key]++;
                        }
                    }
                }
            }

            var storageProducts = wh.GetStorage().GetAllLocations()
                                                    .SelectMany(l => l.GetCurrentProducts())
                                                    .GroupBy(p => p.Code)
                                                    .Select(o => new { Code = o.Key, Count = o.Count() })
                                                    ;
            unfinishedProductStillAtEntryCount = 0;
            foreach( var ufp in unfinishedProducts )
            {
                var sp = storageProducts.FirstOrDefault(p => p.Code == ufp.Key);

                if( sp == default )
                {
                    unfinishedProductStillAtEntryCount += ufp.Value;
                }
                else
                {
                    unfinishedProductStillAtEntryCount += ufp.Value - Math.Min( ufp.Value, sp.Count);
                }
            }

            unfinishedProductInStorageCount = unfinishedProducts.Sum( p => p.Value ) - unfinishedProductStillAtEntryCount;

            distanceLevel = wh.DistanceLevel;
            distancePosition = wh.DistancePosition;
            distanceDirect = wh.DistanceDirect;

            var c = wh.GetCostFactors();

            distanceCost = distanceDirect * c.DistanceCosts;
            unfinishedOrdersCost = (unfinishedOrderCount > 0  ? c.UnfinishedOrdersPenalty : 0);

            unfinishedProductsInStorageCost = unfinishedProductInStorageCount * c.UnfinishedProductInStorageCost;
            unfinishedProductsStillAtEntryCost = unfinishedProductStillAtEntryCount * c.UnfinishedProductStillAtEntryCost;

            totalCost = distanceCost + unfinishedOrdersCost + unfinishedProductsInStorageCost + unfinishedProductsStillAtEntryCost;
        }

        public int GetUnfinishedOrderCount() => unfinishedOrderCount;

        public int GetUnfinishedProductStillAtEntryCount() => unfinishedProductStillAtEntryCount;

        public int GetUnfinishedProductInStorageCount() => unfinishedProductInStorageCount;

        public long GetDistanceLevel() => distanceLevel;

        public long GetDistancePosition() => distancePosition;

        public double GetDistanceDirect() => distanceDirect;


        public double GetUnfinishedOrdersCost() => unfinishedOrdersCost;

        public double GetUnfinishedProductsStillAtEntryCost() => unfinishedProductsStillAtEntryCost;

        public double GetUnfinishedProductsInStorageCost() => unfinishedProductsInStorageCost;
        public double GetDistanceCost() => distanceCost;

        public double GetTotalCost() => totalCost;

        private static bool IsFinished(WarehouseInternal wh ) => wh.IsCurrentOrderFinished();
    }
}
