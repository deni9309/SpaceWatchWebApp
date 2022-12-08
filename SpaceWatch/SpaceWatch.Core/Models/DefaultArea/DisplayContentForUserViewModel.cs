
namespace SpaceWatch.Core.Models.DefaultArea
{
	public class DisplayContentForUserViewModel
	{
		public int Id { get; set; }

		public string Title { get; set; } = null!;

		public string? HtmlContent { get; set; }

		public string? VideoLink { get; set; }

		public int CategoryItemId { get; set; }

		public IEnumerable<CommentViewModel>? Comments { get; set; } = null;
	}
}
