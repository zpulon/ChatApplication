using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace ApiCore.JsonFilter
{
    public class JsonHelper : IJsonHelper
	{
		private JsonSerializerOptions setting;

		public JsonHelper()
			: this(new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				PropertyNameCaseInsensitive = true,
				WriteIndented = true,
				DefaultIgnoreCondition= JsonIgnoreCondition.Never
			})
		{
			setting.Converters.Add(new JsonStringEnumConverter());
			setting.Converters.Add(new DateJsonConverter("yyyy-MM-dd HH:mm:ss", DateTimeZoneHandling.Local));
			setting.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
		}

		public JsonHelper(JsonSerializerOptions setting)
		{
			this.setting = setting;
		}

		public string ToJson(object obj)
		{
			if (obj == null)
			{
				return "";
			}
			return JsonSerializer.Serialize(obj, setting);
		}

		
		public object ToObject(string json, Type type)
		{
			if (string.IsNullOrEmpty(json))
			{
				return null;
			}
			return JsonSerializer.Deserialize(json, type, setting);
		}

		public TObject ToObject<TObject>(string json)
		{
			return (TObject)ToObject(json, typeof(TObject));
		}

	
	}

}
