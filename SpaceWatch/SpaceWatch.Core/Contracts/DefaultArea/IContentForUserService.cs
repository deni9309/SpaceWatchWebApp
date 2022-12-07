using SpaceWatch.Core.Models.DefaultArea;

namespace SpaceWatch.Core.Contracts.DefaultArea
{
    public interface IContentForUserService
	{
		/// <summary>
		/// Gets details about specific content by categoryItemId. Content must be active to be displayed.   
		/// </summary>
		/// <param name="categoryItemId"></param>
		/// <returns>Active content details</returns>
		Task<DisplayContentForUserViewModel> GetContent(int categoryItemId);

		Task<IEnumerable<CommentViewModel>> GetCommentsForContent(int contentId);

		Task AddComment(CommentAddViewModel model);

		Task DeleteComment(int commentId, string userId);
	}
}
