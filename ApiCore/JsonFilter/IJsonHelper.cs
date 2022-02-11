using System;

namespace ApiCore.JsonFilter
{
    public interface IJsonHelper
	{
		string ToJson(object obj);

		object ToObject(string json, Type type);

		TObject ToObject<TObject>(string json);
	}

}
