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
    public class MyListConverter : JsonConverterFactory
    {
        private readonly JsonSerializerOptions _options;

        public MyListConverter(JsonSerializerOptions options)
        {
            _options = new(options);
        }

        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
                return false;

            Type generic = typeToConvert.GetGenericTypeDefinition();
            if (generic != typeof(List<>))
                return false;

            return true;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type elementType = typeToConvert.GetGenericArguments()[0];

            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
              typeof(GenericListConverter<>).MakeGenericType(elementType),
              BindingFlags.Instance | BindingFlags.Public,
              binder: null,
              args: new object[] { _options },
              culture: null);

            return converter;
        }
    }

    public class GenericListConverter<T> : JsonConverter<List<T>> where T : IHasFieldStatus
    {
        private readonly JsonSerializerOptions _options;

        public GenericListConverter(JsonSerializerOptions options)
        {
            _options = options;
        }

        public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Dictionary<string, object> dic = new(StringComparer.OrdinalIgnoreCase);
            List<T> result = (List<T> )Activator.CreateInstance(typeof(List<T>), Array.Empty<object>());
            //while (reader.Read())
            //{
            //    if (reader.TokenType == JsonTokenType.PropertyName)
            //    {
            //        var propertyName = reader.GetString();
            //        var propertyInfo = result.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            //        dic.Add(propertyName, null);
            //        reader.Read();
            //        switch (reader.TokenType)
            //        {
            //            case JsonTokenType.StartObject:
            //            case JsonTokenType.StartArray:
            //            case JsonTokenType.String:
            //            case JsonTokenType.Number:
            //                dic[propertyName] = JsonSerializer.Deserialize<JsonElement>(ref reader, _options);
            //                break;
            //            case JsonTokenType.True:
            //            case JsonTokenType.False:
            //                dic[propertyName] = reader.GetBoolean();
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //}
            return result;
        }

        public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), _options);
        }
    }
}

