using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Staff_Tab
{
    /// <summary>
    /// Whether the employee was employed full- (F) or part-time (P).
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum JobStatus
    {
        FullTime,
        PartTime
    }
}