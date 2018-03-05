using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Staff_Tab
{
    /// <summary>
    /// Конвертер для Json-сериализации объектов класса Employee со вложенным свойством Department
    /// </summary>
    class EmployeeConverter : JsonConverter
    {
        #region implemented abstract members of JsonConverter

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject jo = new JObject();
            Type type = value.GetType();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    object propertyValue = propertyInfo.GetValue(value, null);
                    if (propertyValue != null)
                    {
                        if (propertyInfo.Name == "Department")
                        {
                            jo.Add(propertyInfo.Name, JToken.FromObject(((Department)propertyValue).Title, serializer));
                        }
                        else
                        {
                            jo.Add(propertyInfo.Name, JToken.FromObject(propertyValue, serializer));
                        }
                    }
                }
            }
            jo.WriteTo(writer);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject item = JObject.Load(reader);

            if (item["PayFrequency"].Value<string>() == "Salary")
            {
                return item.ToObject<SalaryEmployee>();
            }

            if (item["PayFrequency"].Value<string>() == "Hourly")
            {
                return item.ToObject<HourlyEmployee>();
            }
            throw new Exception();
        }
        public override bool CanConvert(Type objectType)
        {
            return typeof(Employee).IsAssignableFrom(objectType);
        }

        #endregion

    }
}
