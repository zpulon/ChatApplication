using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiCore.JsonFilter
{
    public class DateJsonConverter : JsonConverter<DateTime>
	{
		private readonly string _dateFormatString;

		private readonly DateTimeZoneHandling _dateTimeZoneHandling;

		private readonly DateTimeKind _defualtDateTimeKing;

		private readonly Dictionary<DateTimeZoneHandling, Func<DateTime, DateTime>> _dateProc = new Dictionary<DateTimeZoneHandling, Func<DateTime, DateTime>>
	{
		{
			DateTimeZoneHandling.Local,
			(DateTime dt) => dt.ToLocalTime()
		},
		{
			DateTimeZoneHandling.UTC,
			(DateTime dt) => dt.ToUniversalTime()
		}
	};

		public DateJsonConverter(string dateFormatString, DateTimeZoneHandling dateTimeZoneHandling)
			: this(dateFormatString, dateTimeZoneHandling, DateTimeKind.Utc)
		{
		}

		public DateJsonConverter(string dateFormatString, DateTimeZoneHandling dateTimeZoneHandling, DateTimeKind defaultDateKind)
		{
			_dateFormatString = dateFormatString;
			_dateTimeZoneHandling = dateTimeZoneHandling;
			_defualtDateTimeKing = defaultDateKind;
		}

		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (DateTime.TryParse(reader.GetString(), out var result))
			{
				if (result.Kind == DateTimeKind.Unspecified)
				{
					result = ((_defualtDateTimeKing == DateTimeKind.Utc) ? result.ToUniversalTime() : result.ToLocalTime());
					return _dateProc[_dateTimeZoneHandling](result);
				}
				return result;
			}
			return DateTime.MinValue;
		}

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			value = ((value.Kind != 0) ? value : ((_defualtDateTimeKing == DateTimeKind.Utc) ? value.ToUniversalTime() : value.ToLocalTime()));
			writer.WriteStringValue(_dateProc[_dateTimeZoneHandling](value).ToString(_dateFormatString));
		}
	}
}
