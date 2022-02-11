namespace LogCore.Log
{
    public static class LogLayoutTemplates
	{
		public static readonly string SimpleLayout = "[${longdate}][${level:uppercase=true}][${logger}] ${message}";

		public static readonly string TestLayout = "${message}";
	}
}
