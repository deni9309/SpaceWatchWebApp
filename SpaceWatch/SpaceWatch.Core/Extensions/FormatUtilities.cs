
namespace SpaceWatch.Core.Extensions
{
	public static class FormatUtilities
	{
		public static string? TrimVideoLink(string? link)
		{
			if (link != null && link.Length >= 80)
			{
				return link.Substring(38, 41);
			}
			return link;
		}
	}
}
