namespace AbbyyLS.ReSharper
{
	static class IdentifierUtil
	{
		public static string ToLowerCamelCase(string identifier)
		{
			if (identifier.Length == 1)
				return identifier.Substring(0, 1).ToLowerInvariant();

			return identifier.Substring(0, 1).ToLowerInvariant() + identifier.Substring(1);
		}
	}
}