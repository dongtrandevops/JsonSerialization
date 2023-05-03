using JsonSerialization.ViewModel;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonSerialization
{
    public class CustomSingleGenericConverter : JsonConverterFactory
    {
        private readonly JsonSerializerOptions _options;

        public CustomSingleGenericConverter(JsonSerializerOptions options)
        {
            _options = new(options);
        }

        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.FullName.StartsWith("System"))
            {
                return false;
            }
            if (typeof(IHasFieldStatus).IsAssignableFrom(typeToConvert))
            {
                return true;
            }
            return false;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
             typeof(SingleGenericConverter<>).MakeGenericType(typeToConvert),
             BindingFlags.Instance | BindingFlags.Public,
             binder: null,
             args: new object[] { _options },
             culture: null);

            return converter;
        }

        private class SingleGenericConverter<T> : JsonConverter<T> where T : IHasFieldStatus
        {
            private readonly JsonSerializerOptions _options;

            public SingleGenericConverter(JsonSerializerOptions options)
            {
                _options = options;
            }

            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                Dictionary<string, object> dic = new(StringComparer.OrdinalIgnoreCase);
                T result = (T)Activator.CreateInstance(typeof(T), Array.Empty<object>());

                var propertiesNameOfType = typeToConvert.GetProperties().Select(c => c.Name).ToList();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        var propertyName = reader.GetString();
                        var propertyInfo = result.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        dic.Add(propertyName, null);
                        reader.Read();
                        switch (reader.TokenType)
                        {
                            case JsonTokenType.StartObject:
                            case JsonTokenType.StartArray:
                            case JsonTokenType.String:
                            case JsonTokenType.Number:
                                dic[propertyName] = JsonSerializer.Deserialize<JsonElement>(ref reader, _options);
                                break;
                            case JsonTokenType.True:
                            case JsonTokenType.False:
                                dic[propertyName] = reader.GetBoolean();
                                break;
                            default:
                                break;
                        }
                    }
                }

                foreach (var item in dic)
                {
                    var property = result.GetType().GetProperty(item.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        if (item.Value.GetType() == typeof(JsonElement))
                        {
                            var jsonElement = (JsonElement)item.Value;
                            var data = jsonElement.Deserialize(property.PropertyType, _options);
                            property.SetValue(result, data);
                        }
                        else
                        {
                            property.SetValue(result, item.Value);
                        }
                    }
                }

                UpdateFieldStatus(result, dic);
                return result;
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value, _options);
            }
        }

        private static void UpdateFieldStatus(IHasFieldStatus result, Dictionary<string, object> dic)
        {
            result.FieldStatus ??= new();

            foreach (var property in result.GetType().GetProperties())
            {
                if (typeof(IHasFieldStatus).IsAssignableFrom(property.PropertyType))
                {
                    var newDic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    if (dic.TryGetValue(property.Name, out object value))
                    {
                        JsonElement jsonElement = (JsonElement)value;
                        var propertyJsonElement = jsonElement.EnumerateObject().ToList();
                        foreach (var i in propertyJsonElement)
                        {
                            newDic.Add(i.Name, i.Value);
                        }

                        var obj = property.GetValue(result);
                        UpdateFieldStatus(obj as IHasFieldStatus, newDic);
                    }
                }
                else if (typeof(IEnumerable<IHasFieldStatus>).IsAssignableFrom(property.PropertyType))
                {
                    if (dic.TryGetValue(property.Name, out object value))
                    {
                        JsonElement jsonElement = (JsonElement)value;
                        var elements = jsonElement.EnumerateArray().ToList();

                        for (int i = 0; i < elements.Count; i++)
                        {
                            var newDic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                            var propertyJsonElement = elements[i].EnumerateObject().ToList();

                            foreach (var pje in propertyJsonElement)
                            {
                                newDic.Add(pje.Name, pje.Value);
                            }

                            dynamic objs = property.GetValue(result);

                            UpdateFieldStatus(objs[i] as IHasFieldStatus, newDic);
                        }
                    }
                }
                else
                {
                    if (dic.ContainsKey(property.Name))
                    {
                        result.FieldStatus.Add(property.Name, true);
                    }
                }
            }
        }

        private static void RemoveFieldStatus(IHasFieldStatus result)
        {
            result.FieldStatus = null;

            foreach (var property in result.GetType().GetProperties())
            {
                if (typeof(IHasFieldStatus).IsAssignableFrom(property.PropertyType))
                {
                    var obj = property.GetValue(result);
                    if (obj != null)
                    {
                        RemoveFieldStatus(obj as IHasFieldStatus);
                    }
                }
                else if (typeof(IEnumerable<IHasFieldStatus>).IsAssignableFrom(property.PropertyType))
                {
                    var objs = (IEnumerable<IHasFieldStatus>)property.GetValue(result);
                    for (int i = 0; i < objs?.Count(); i++)
                    {
                        RemoveFieldStatus(objs.ToList()[i]);
                    }
                }
            }
        }
    }
}
