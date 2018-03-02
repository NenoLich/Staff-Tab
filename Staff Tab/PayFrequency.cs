using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Staff_Tab
{
    /// <summary>
    /// Defines whether an employee is paid on an hourly basis or salary basis
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    enum PayFrequency
    {
        Salary,
        Hourly
    }
}