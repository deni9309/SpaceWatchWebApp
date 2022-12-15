using SpaceWatch.Core.Extensions;

namespace SpaceWatch.NUnitTests
{
	[TestFixture]
	public class ExtensionsTests
	{
		[Test]
		public void Test_FormatUtilities_TrimVideoLink_ReturnsValidYouTubeEmbedUrl()
		{
			string copiedEmbedCode = "<iframe width=\"853\" height=\"480\" src=\"https://www.youtube.com/embed/Mx_OexsUI2M\" title=\"Rihanna - Lift Me Up (From Black Panther: Wakanda Forever)\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>";

			string? urlResult = FormatUtilities.TrimVideoLink(copiedEmbedCode);

			Assert.True(urlResult == "https://www.youtube.com/embed/Mx_OexsUI2M");
		}
	}
}
