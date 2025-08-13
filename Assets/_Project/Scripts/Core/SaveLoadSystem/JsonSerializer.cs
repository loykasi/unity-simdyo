using Newtonsoft.Json;

public class JsonSerializer : ISerializer
{
    private JsonSerializerSettings _settings;

    public JsonSerializer()
    {
        _settings = new JsonSerializerSettings();
        _settings.Converters.Add(new Vector3Converter());
        _settings.Converters.Add(new Vector2Converter());
        _settings.Converters.Add(new QuaternionConverter());
        _settings.Formatting = Formatting.Indented;
        _settings.TypeNameHandling = TypeNameHandling.Auto;

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