using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff_Tab
{
    class EmployeeConverter : JsonConverter
    {
        #region implemented abstract members of JsonConverter

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
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
