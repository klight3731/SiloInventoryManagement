using SiloInventoryManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;


namespace SiloInventoryManagement.DAL
{
    public class RecordTypesDBA
    {
        public List<RecordType> Get()
        {
            List<RecordType> recordTypes = new List<RecordType>();

            using (SiloInventoryEntityDataModel context = new SiloInventoryEntityDataModel())
            {
                ObjectResult<RecordType> results = context.up_RecordTypes_Get();

                foreach (RecordType result in results)
                    recordTypes.Add(result);
            }

            return recordTypes;
        }

        public ObservableCollection<RecordTypeModel> GetObservableCollection()
        {
            List<RecordType> results = Get();
            ObservableCollection<RecordTypeModel> recordTypes = new ObservableCollection<RecordTypeModel>();

            foreach (RecordType result in results)
            {
                recordTypes.Add(new RecordTypeModel(result));
            }

            return recordTypes;
        }
    }
}
