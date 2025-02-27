using com.knapp.CodingContest.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.warehouse
{
    public class Storage
    {
        private readonly WarehouseCharacteristics c;

        private readonly List<List<Location>> locations;

        public Location EntryLocation { get; private set; }
        public Location ExitLocation { get; private set; }
        
        public Robot Robot { get; private set; }


        protected Storage ( WarehouseCharacteristics c, Robot robot, List<List<Location>> locations, Location entryLocation, Location exitLocation )
        {
            this.c = c;
            this.locations = locations;
            
            EntryLocation = entryLocation;
            ExitLocation = exitLocation;
            Robot = robot;
        }

        public List<Location> GetAllLocations()
        {
            return locations.SelectMany(level => level).ToList();
        }

        public Location GetLocation( int level, int position )
        {
            if( level < 0 
                || level >= c.NumberOfLevels
                || position < 0 
                || position >= c.NumberOfLocationsPerLevel
                )
            {
                throw new NoSuchLocationException(c, level, position);
            }

            return locations[level][position];
        }
    }
}
