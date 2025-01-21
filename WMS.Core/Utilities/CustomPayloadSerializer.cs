using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using WMS.MessageBroker.Abstraction;

namespace WMS.Core.Utilities;

public static class CustomPayloadSerializer
{
    private static readonly JsonSerializerSettings _jsonSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Formatting = Formatting.Indented
    };
    
    public static string SerializeCustomPayload(dynamic data, Dictionary<string, string> customPayload)
    {
        if (customPayload == null || !customPayload.Any())
            return null;
        
        var serializedPayload = new Dictionary<string, object>();
        
        string content = JsonConvert.SerializeObject(PrepareRequestBody(data), _jsonSettings);
        
        foreach (var payload in customPayload)
        {
            var propertyValue = GetPointerValue(payload.Value, content);
            try
            {
                JToken.Parse(propertyValue);
                serializedPayload.Add(payload.Key, JsonConvert.DeserializeObject<Dictionary<string, object>>(propertyValue));
            }
            catch
            {
                serializedPayload.Add(payload.Key, propertyValue);
            }
        }

        return GetJsonPayload(serializedPayload);
    }

    private static Dictionary<string, object> PrepareRequestBody(dynamic data)
    {
        return new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase)
        {
            { "data", data }
        };
    }

    private static string GetPointerValue(string pointerString, string jsonData)
    {
        var documentToken = JToken.Parse(jsonData);
        var jPointer = new Microsoft.Json.Pointer.JsonPointer(pointerString);
        var arrayElementToken = jPointer.Evaluate(documentToken);
        return arrayElementToken.ToString();
    }
    
    public static string GetJsonPayload(dynamic payload)
    {
        return JsonConvert.SerializeObject(payload, _jsonSettings);
    }
}