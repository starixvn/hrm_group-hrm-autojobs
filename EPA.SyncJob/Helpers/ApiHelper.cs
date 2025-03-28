namespace EPA.SyncJob.Helpers;

using Newtonsoft.Json.Linq;

public static class ApiHelper
{
    public static T HandleDataResponse<T>(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }

        JObject jsonObject = JObject.Parse(json);
        if (IsSuccess(jsonObject))
        {
            var result = jsonObject.ToObject<T>();
            return result;
        }

        return default;
    }
    private static bool IsSuccess(JObject jObject) => jObject.ContainsKey("access_token");
}