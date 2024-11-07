using Domain.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Domain.DomainErrors;

public class Error : ValueObject
{
    public Error(string code)
    {
        Code = code;
    }

    public string Code { get; set; }

    public string Serialize()
    {
        JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        return JsonConvert.SerializeObject(this, jsonSettings);
    }

    public static Error Deserialize(string serialized)
    {
        Error? error = JsonConvert.DeserializeObject<Error>(serialized);
        if (error == null)
        {
            throw new Exception($"Can' deserialize error: {serialized}");
        }

        return error;
    }

    protected override IEnumerable<object?> GetPropertiesForComparison()
    {
        yield return Code;
    }
}
