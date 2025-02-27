namespace com.knapp.CodingContest.warehouse
{
    public interface IWarehouseCostFactors
    {
        /// <summary>
        /// costs if there are any unfinished order(s)
        /// </summary>
        double UnfinishedOrdersPenalty { get;  }

        /// <summary>
        /// costs per unfinished product still at entry
        /// </summary>
        double UnfinishedProductStillAtEntryCost { get;  }

  
        /// <summary>
        /// costs per unfinished product in storage
        /// </summary>
        double UnfinishedProductInStorageCost { get; }

        /// <summary>
        /// costs per distance-unit
        /// </summary>
        double DistanceCosts { get; }

    }
}
