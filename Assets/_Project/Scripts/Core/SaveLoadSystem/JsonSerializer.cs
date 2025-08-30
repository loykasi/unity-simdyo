using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class JsonSerializer : ISerializer
{
    private JsonSerializerSettings _settings;

    public JsonSerializer()
    {
        _settings = new JsonSerializerSettings();
        _settings.Converters.Add(new Vector3Converter());
        _settings.Converters.Add(new Vector2Converter());
        _settings.Converters.Add(new QuaternionConverter());
        _settings.Converters.Add(new VariableConverter());
        _settings.Converters.Add(new SceneEntityConverter());
        _settings.Converters.Add(new StringEnumConverter());
        _settings.Formatting = Formatting.Indented;
        _settings.TypeNameHandling = TypeNameHandling.None;

        _settings.ContractResolver = new VisualScriptingContractResolver
        (
            new[]
            {
                typeof(InputTrigger),
                typeof(OutputTrigger),
                typeof(InputValue),
                typeof(OutputValue)
            }
        );
    }

    public string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj, _settings);
    }

    public T Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, _settings);
    }
}