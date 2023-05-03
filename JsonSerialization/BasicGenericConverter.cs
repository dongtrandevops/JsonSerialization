using System.Reflection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerialization.ViewModel;
using System.Linq;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections;

namespace JsonSerialization
{
    public class BasicMyConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        { 
            return true;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
               typeof(GenericConverter<>).MakeGenericType(typeToConvert),
               BindingFlags.Instance | BindingFlags.Public,
               binder: null,
               args: new object[] { options },
               culture: null);

            return converter;
        }

        private class GenericConverter<T> : JsonConverter<T>
        {
            private readonly JsonSerializerOptions _options;
        
            public GenericConverter(JsonSerializerOptions options)
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
                }

                return result;
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                //RemoveFieldStatus(value);
                JsonSerializer.Serialize(writer, value, value.GetType(), _options);
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

