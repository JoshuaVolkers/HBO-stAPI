using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace PoohAPI.Logic.Common
{
    public class DataReaderTypeConverter<T> : ITypeConverter<IDataReader, T> where T : class, new()
    {
        public T Convert(IDataReader source, T destination, ResolutionContext context)
        {
            if (destination == null)
            {
                destination = new T();
            }

            typeof(T).GetProperties()
                .ToList()
                .ForEach(property =>
                {
                    //Check if the property exists as a column in the source
                    if (Enumerable.Range(0, source.FieldCount).Any(i => source.GetName(i) == property.Name))
                        if (source[property.Name].GetType() != typeof(DBNull))
                            property.SetValue(destination, source[property.Name]);
                });

            return destination;
        }
    }
}
