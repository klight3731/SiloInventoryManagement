using SiloInventoryManagement.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;


namespace SiloInventoryManagement.DAL
{
    public class LocationsDBA
    {
        public List<Location> Get()
        {
            List<Location> locations = new List<Location>();

            using (SiloInventoryEntityDataModel context = new SiloInventoryEntityDataModel())
            {
                ObjectResult<Location> results = context.up_Locations_Get();

                foreach (Location result in results)
                    locations.Add(result);
            }

            return locations;
        }

        public ObservableCollection<LocationModel> GetObservableCollection()
        {
            List<Location> results = Get();
            ObservableCollection<LocationModel> locations = new ObservableCollection<LocationModel>();

            foreach (Location result in results)
            {
                locations.Add(new LocationModel(result));
            }

            return locations;
        }
    }
}
