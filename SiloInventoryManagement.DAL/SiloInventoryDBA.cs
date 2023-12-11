using SiloInventoryManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;

namespace SiloInventoryManagement.DAL
{
    public class SiloInventoryDBA
    {
        public List<SiloInventory> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            List<SiloInventory> siloInventories = new List<SiloInventory>();

            using (SiloInventoryEntityDataModel context = new SiloInventoryEntityDataModel())
            {
                ObjectResult<SiloInventory> results = context.up_SiloInventory_GetByDateRange(startDate, endDate);

                foreach (SiloInventory result in results)
                    siloInventories.Add(result);
            }

            return siloInventories;
        }

        public ObservableCollection<SiloInventoryModel> GetByDateRangeObservableCollection(DateTime startDate, DateTime endDate)
        {
            List<SiloInventory> results = GetByDateRange(startDate, endDate);
            ObservableCollection<SiloInventoryModel> siloInventories = new ObservableCollection<SiloInventoryModel>();

            foreach (SiloInventory result in results)
            {
                siloInventories.Add(new SiloInventoryModel(result));
            }

            return siloInventories;
        }

        public SiloInventoryModel Insert(SiloInventoryModel siloInventory)
        {
            int id = -1;
            ObjectParameter primaryKey = new ObjectParameter("SiloInventoryID", typeof(int));

            SiloInventoryModel model = new SiloInventoryModel();

            model.SiloInventoryID = siloInventory.SiloInventoryID;
            model.RecordTypeID = siloInventory.RecordTypeID;
            model.LocationID = siloInventory.LocationID;
            model.Date = siloInventory.Date;
            model.Value = siloInventory.Value;
            model.Comment = siloInventory.Comment;
            model.EnteredBy = Environment.UserName;
            model.DateModified = DateTime.Now;

            using (SiloInventoryEntityDataModel context = new SiloInventoryEntityDataModel())
            {
                context.up_SiloInventory_Insert(  model.RecordTypeID
                                                , model.LocationID
                                                , model.Date
                                                , model.Value
                                                , model.Comment
                                                , model.EnteredBy
                                                , model.DateModified
                                                , primaryKey);

                if (primaryKey != null)
                    id = Convert.ToInt32(primaryKey.Value);
                else
                    throw new System.Exception("There was an error while inserting the new Silo Inventory record into the database.");
            }

            model.SiloInventoryID = id;            

            return model;
        }

        public SiloInventoryModel Update(SiloInventoryModel siloInventory, string @operator)
        {
            using (SiloInventoryEntityDataModel context = new SiloInventoryEntityDataModel())
            {
                context.up_SiloInventory_Update(siloInventory.SiloInventoryID
                                                , siloInventory.LocationID
                                                , siloInventory.Date
                                                , siloInventory.Value
                                                , siloInventory.Comment
                                                , siloInventory.EnteredBy);
            }

            siloInventory.DateModified = DateTime.Now;
            siloInventory.EnteredBy = @operator;

            return siloInventory;
        }
    }
}
