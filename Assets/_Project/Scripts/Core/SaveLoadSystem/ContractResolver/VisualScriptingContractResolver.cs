using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public class VisualScriptingContractResolver: DefaultContractResolver
{
    private readonly HashSet<Type> _ignoredTypes;

    public VisualScriptingContractResolver(IEnumerable<Type> ignoredTypes)
    {
        _ignoredTypes = new HashSet<Type>(ignoredTypes);
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);
        if (_ignoredTypes.Contains(property.PropertyType))
        {
            property.Ignored = true;
        }

        return property;
    }
}