using FamilyFinace.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.Interfaces
{
    public interface IModelMetaDataProvider
    {
        public List<ModelMetaData> GetMeta<T>();
    }
}
