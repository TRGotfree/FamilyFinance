using FamilyFinace.DTOModels;
using FamilyFinace.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FamilyFinace.Services
{
    public class ModelMetaDataProvider : IModelMetaDataProvider
    {
        public List<ModelMetaData> GetMeta<T>()
        {

            List<ModelMetaData> propsAndDisplayNames = new List<ModelMetaData>(0);
            var bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

            var costProperties = typeof(T).GetProperties(bindingFlags);
            costProperties = costProperties.Where(c => c.CustomAttributes != null).ToArray();

            if (costProperties == null)
                throw new Exception("MetaData for user tasks not found!");

            foreach (var prop in costProperties)
            {
                var displayAttribute = prop.GetCustomAttributes(typeof(DisplayAttribute), true)
                    .Select(attr => (DisplayAttribute)attr)
                    .FirstOrDefault() as DisplayAttribute;

                if (displayAttribute == null)
                    continue;

                propsAndDisplayNames.Add(new ModelMetaData { PropertyName = string.Format("{0}{1}", prop.Name.Substring(0, 1).ToLower(), prop.Name.Substring(1)), DisplayName = displayAttribute.Name });
            }

            return propsAndDisplayNames;
        }
    }
}
