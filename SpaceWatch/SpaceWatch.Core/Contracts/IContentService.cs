using SpaceWatch.Core.Models;

namespace SpaceWatch.Core.Contracts
{
	public interface IContentService
	{
		Task<IEnumerable<ContentViewModel>> GetAll();

		Task<int> Add(ContentViewModel model);

		Task<bool> ContentExists(int ContentId);

		Task<CategoryViewModel> GetCategoryForContentAsync(int contentId);

		//Task<string> GetCatItemNameAsync(int contentId);

		Task<ContentViewModel> ContentDetailsById(int id);

        Task<ContentViewModel> ContentDetailsByCategoryItemId(int categoryItemId);

        Task Edit(int id, ContentViewModel model);

		Task Delete(int id);
	}
}
