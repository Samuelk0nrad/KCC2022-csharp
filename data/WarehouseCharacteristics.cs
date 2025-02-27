using com.knapp.CodingContest.core;
using System.Text;

namespace com.knapp.CodingContest.data
{
    public class WarehouseCharacteristics
    {
        public int NumberOfLevels { get; private set; }

        public int NumberOfLocationsPerLevel { get; private set; }

        public int LocationLength{ get; private set; }

        public int LocationMaxWidth { get; private set; }

        public int RobotLength { get; private set; }

        public int EntryLevel { get; private set; }

        public int EntryPosition { get; private set; }

        public int ExitLevel { get; private set; }

        public int ExitPosition { get; private set; }

        public WarehouseCharacteristics( JavaPropertiesFile properties )
        {

            NumberOfLevels = properties.GetInt("numberOfLevels");
            NumberOfLocationsPerLevel = properties.GetInt("numberOfLocationsPerLevel");
            LocationLength = properties.GetInt("locationLength");
            LocationMaxWidth = properties.GetInt("locationMaxWidth");
            RobotLength = properties.GetInt("robotLength");

            EntryLevel = properties.GetInt("entryLevel");
            EntryPosition = properties.GetInt("entryPosition");
            ExitLevel = properties.GetInt("exitLevel");
            ExitPosition = properties.GetInt("exitPosition");

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"WarehouseCharacteristics[");

            sb.AppendLine($"   NumberOfLevels            = {NumberOfLevels}");
            sb.AppendLine($"   NumberOfLocationsPerLevel = {NumberOfLocationsPerLevel}");
            sb.AppendLine($"   LocationLength            = {LocationLength}");
            sb.AppendLine($"   LocationMaxWidth          = {LocationMaxWidth}");
            sb.AppendLine($"   RobotLenth                = {RobotLength}");
            sb.AppendLine($"");
            sb.AppendLine($"   EntryLevel                = {EntryLevel}");
            sb.AppendLine($"   EntryPosition             = {EntryPosition}");
            sb.AppendLine($"   ExitLevel                 = {ExitLevel}");
            sb.AppendLine($"   ExitPosition              = {ExitPosition}");


            sb.AppendLine($"]");


            return sb.ToString();
        }
    }
}
