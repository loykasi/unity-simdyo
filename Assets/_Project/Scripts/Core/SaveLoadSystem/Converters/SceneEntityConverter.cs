using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SceneEntityConverter : JsonConverter<EntityData>
{
    public override bool CanWrite => false;

    public override EntityData ReadJson(JsonReader reader, Type objectType, EntityData existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        var obj = JObject.Load(reader);

        EntityType type = obj["Type"].ToObject<EntityType>(serializer);

        EntityData entityData = type switch
        {
            EntityType.Box => new BoxEntityData(),
            EntityType.Circle => new CircleEntityData(),
            EntityType.Polygon => new PolygonEntityData(),
            _ => new EntityData(),
        };
        serializer.Populate(obj.CreateReader(), entityData);

        return entityData;
    }

    public override void WriteJson(JsonWriter writer, EntityData value, Newtonsoft.Json.JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}