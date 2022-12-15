using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts.DefaultArea;
using SpaceWatch.Core.Models.DefaultArea;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Core.Services.DefaultArea
{
    public class HomeService : IHomeService
    {
        private readonly IRepository _repo;
        private readonly ILogger<HomeService> _logger;

        public HomeService(IRepository repository,
            ILogger<HomeService> logger)
        {
            _repo = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryItemDetailsModel>> GetCategoryItemDetailsForUser(string userId)
        {
            try
            {
				var catItemsDetail = await _repo.AllReadonly<Content>()
				.Include(c => c.CategoryItem)
				.Include(c => c.CategoryItem.Category)
				.Include(c=>c.CategoryItem.Category.UserCategories)
				.Where(c=>c.CategoryItem.Category.UserCategories.Any(u=>u.UserId == userId && u.CategoryId == c.CategoryItem.Category.Id))
				.Where(c => c.IsActive && c.CategoryItem.IsActive && c.CategoryItem.Category.IsActive)
				.Select(c => new CategoryItemDetailsModel()
				{
					CategoryId = c.CategoryItem.Category.Id,
					CategoryTitle = c.CategoryItem.Category.Title,
					CategoryItemId = c.CategoryItem.Id,
					CategoryItemTitle = c.CategoryItem.Title,
					CategoryItemDescription = c.CategoryItem.Description,
					MediaImagePath = _repo.AllReadonly<MediaType>()
					.Where(m => m.IsActive && m.CategoryItems.Any(c => c.MediaTypeId == m.Id))
					.First(m => m.Id == c.CategoryItem.MediaTypeId).ThumbnailImagePath
				})
				.ToListAsync();

				return catItemsDetail;
				//return await (from catItem in _repo.AllReadonly<CategoryItem>()
				//              join category in _repo.AllReadonly<Category>()
				//              on catItem.CategoryId equals category.Id
				//              join content in _repo.AllReadonly<Content>()
				//              on catItem.Id equals content.CategoryItem.Id
				//              join userCat in _repo.AllReadonly<UserCategory>()
				//              on category.Id equals userCat.CategoryId
				//              join mediaType in _repo.AllReadonly<MediaType>()
				//              on catItem.MediaTypeId equals mediaType.Id
				//              where userCat.UserId == userId
				//              where category.IsActive && catItem.IsActive && content.IsActive && mediaType.IsActive
				//              select new CategoryItemDetailsModel()
				//              {
				//                  CategoryId = category.Id,
				//                  CategoryTitle = category.Title,
				//                  CategoryItemId = catItem.Id,
				//                  CategoryItemTitle = catItem.Title,
				//                  CategoryItemDescription = catItem.Description,
				//                  MediaImagePath = mediaType.ThumbnailImagePath
				//              })
				//         .ToListAsync();
			}
            catch (Exception ex)
            {
                _logger.LogError(nameof(GetCategoryItemDetailsForUser), ex);
                throw new ApplicationException("An error occured while trying to retrieve info from database.", ex);
            }          
        }

        public async Task<IEnumerable<GroupedCategoryItemsByCategory>> GetGroupedCategoryItemsByCategory(IEnumerable<CategoryItemDetailsModel> catItemModels)
        {

            List<GroupedCategoryItemsByCategory> groupedResult = new List<GroupedCategoryItemsByCategory>();

            await Task.Run(() =>
            {
				var groupedCategoryItems = catItemModels.GroupBy(item => item.CategoryId);
				foreach (var item in groupedCategoryItems)
                {
                    groupedResult.Add(new GroupedCategoryItemsByCategory()
                    {
                        Id = item.Key,
                        Title = item.Select(c => c.CategoryTitle).FirstOrDefault(),
                        Items = item
                    });
                }
            });

            return groupedResult;
        }
    }
}
