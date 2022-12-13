using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts.DefaultArea;
using SpaceWatch.Core.Models.DefaultArea;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data;
using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Core.Services.DefaultArea
{
	public class ContentForUserService : IContentForUserService
	{
		private readonly IRepository _repo;
		private readonly ILogger<ContentForUserService> _logger;

		public ContentForUserService(IRepository repo, 
			ILogger<ContentForUserService> logger)
		{
			_repo = repo;
			_logger = logger;
		}

		public async Task AddComment(CommentAddViewModel model)
		{
			try
			{
				var contentExists = await _repo.AllReadonly<Content>()
					.AnyAsync(c => c.Id == model.ContentId && c.IsActive);
				var categoryItemExists = await _repo.AllReadonly<CategoryItem>()
					.AnyAsync(c => c.Id == model.CategoryItemId && c.IsActive);
				var userExists = await _repo.AllReadonly<ApplicationUser>()
					.AnyAsync(c => c.Id == model.UserId);

				if (contentExists && categoryItemExists && userExists)
				{
					var comment = new Comment()
					{
						UserId = model.UserId,
						CommentBody = model.CommentBody,
						ContentId = model.ContentId,
						DatePosted = DateTime.Now
					};

					await _repo.AddAsync(comment);
					await _repo.SaveChangesAsync();
				}
			}
			catch (ArgumentNullException ex)
			{
				_logger.LogError(nameof(AddComment), ex);
				throw new ArgumentNullException("Could not find relevant content to proceed.\nDatabase failed to save info.", ex);
			}
			catch(OperationCanceledException ex)
			{
				_logger.LogError(nameof(AddComment), ex);
				throw new OperationCanceledException(ex.Message, ex.InnerException);
			}
		}

		//public Task DeleteComment(int commentId, string userId)
		//{
		//	throw new NotImplementedException();
		//}

		public async Task<IEnumerable<CommentViewModel>> GetCommentsForContent(int contentId)
		{
			return await _repo.AllReadonly<Comment>()
				.Include(c => c.Content)
				.Include(c => c.User)
				.Where(c => c.IsActive && c.Content.IsActive && c.Content.Id == contentId)
				.Select(c => new CommentViewModel()
				{
					Id = c.Id,
					ContentId = c.ContentId,
					UserId = c.UserId,
					UserName = $"{c.User.FirstName} {c.User.LastName}",
					DatePosted = c.DatePosted,
					CommentBody = c.CommentBody,
					CategoryItemId = c.Content.CatItemId

				})
				.ToListAsync();
		}

		public async Task<DisplayContentForUserViewModel> GetContent(int categoryItemId)
		{
			try
			{
				var content = await _repo.AllReadonly<Content>()
					.Include(c => c.CategoryItem)
					.Where(c => c.IsActive)
					.Where(c => c.CategoryItem.Id == categoryItemId)
					.Select(c => new DisplayContentForUserViewModel()
					{
						Id = c.Id,
						Title = c.Title,
						VideoLink = c.VideoLink,
						HtmlContent = c.HtmlContent,
						CategoryItemId = c.CategoryItem.Id
					})
					.FirstOrDefaultAsync();

				return content;
			}
			catch (ArgumentNullException ex)
			{
				_logger.LogError(nameof(GetContent), ex);
				throw new ArgumentNullException("Could not find relevant content for this CategoryItem in database.", ex);
			}
			catch (OperationCanceledException ex)
			{
				_logger.LogError(nameof(GetContent), ex);
				throw new OperationCanceledException(ex.Message, ex.InnerException);
			}
		}
	}
}
