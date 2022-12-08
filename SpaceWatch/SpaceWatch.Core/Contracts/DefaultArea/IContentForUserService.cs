using SpaceWatch.Core.Models.DefaultArea;

namespace SpaceWatch.Core.Contracts.DefaultArea
{
	/// <summary>
	/// Provides methods that hold all logic for displaying Content and Comments to user.
	/// </summary>
	public interface IContentForUserService
	{
		/// <summary>
		/// Gets details about specific content by categoryItemId. Content must be active to be displayed.   
		/// </summary>
		/// <param name="categoryItemId"></param>
		/// <returns>Active content details of type DisplayContentForUserViewModel</returns>
		Task<DisplayContentForUserViewModel> GetContent(int categoryItemId);
		/// <summary>
		/// Gets all active comments for specific content.
		/// </summary>
		/// <param name="contentId"></param>
		/// <returns>IEnumerable of CommentViewModel</returns>
		Task<IEnumerable<CommentViewModel>> GetCommentsForContent(int contentId);
		/// <summary>
		/// Adds new comment
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task AddComment(CommentAddViewModel model);
		/// <summary>
		/// "Deletes" Comment by setting IsActive property to "false". Method does not really remove the record from database. 
		/// </summary>
		/// <param name="commentId"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task DeleteComment(int commentId, string userId);
	}
}
