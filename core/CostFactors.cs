using com.knapp.CodingContest.warehouse;


namespace com.knapp.CodingContest.core
{
    public class CostFactors : IWarehouseCostFactors
    {
        public double UnfinishedOrdersPenalty => 1_000_000.0;

        public double UnfinishedProductStillAtEntryCost => 200.0;

        public double UnfinishedProductInStorageCost => 100.0;

        public double DistanceCosts => 0.1;
    }
}
