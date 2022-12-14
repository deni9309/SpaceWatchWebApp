using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SpaceWatch.Core.Contracts.DefaultArea;
using SpaceWatch.Core.Models.DefaultArea;
using SpaceWatch.Core.Services.DefaultArea;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data;
using SpaceWatch.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWatch.NUnitTests
{
	[TestFixture]
	public class HomeServiceTests
	{
		private IRepository _repo;
		private ILogger<HomeService> _logger;
		private IHomeService _homeService;
		private ApplicationDbContext _context;

		[SetUp]
		public void Setup()
		{
			var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("SpaceWatchDB")
				.Options;

			_context = new ApplicationDbContext(contextOptions);

			_context.Database.EnsureDeleted();
			_context.Database.EnsureCreated();
		}

		[Test]
		public async Task Test_GetCategoryItemDetailsForUser()
		{
			var loggerMock = new Mock<ILogger<HomeService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_homeService = new HomeService(_repo, _logger);
			await _repo.AddAsync(new MediaType() { Id = 1, Title = "", ThumbnailImagePath = "", IsActive = true });
			await _repo.AddRangeAsync(new List<Category>()
			{
				new Category() { Id = 10, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true },
				new Category() { Id = 20, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true },
				new Category() { Id = 30, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true }
			});
			await _repo.AddRangeAsync(new List<CategoryItem>()
			{
				new CategoryItem() { Id = 1, CategoryId = 10, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true },
				new CategoryItem() { Id = 2, CategoryId = 20, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true },
				new CategoryItem() { Id = 3, CategoryId = 30, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true }
			});
			await _repo.AddRangeAsync(new List<Content>()
			{
				new Content() { Id = 1, CategoryId = 10, CatItemId = 1, Title = "", VideoLink = "", HtmlContent="", IsActive = true },
				new Content() { Id = 2, CategoryId = 20, CatItemId = 2, Title = "", VideoLink = "", HtmlContent="", IsActive = true },
				new Content() { Id = 3, CategoryId = 30, CatItemId = 3, Title = "", VideoLink = "", HtmlContent="", IsActive = true }
			});
			await _repo.AddRangeAsync(new List<UserCategory>()
			{
				new UserCategory() { Id = 1, CategoryId = 10, UserId = "userGuid1" },
				new UserCategory() { Id = 2, CategoryId = 10, UserId = "userGuid2" },
				new UserCategory() { Id = 3, CategoryId = 20, UserId = "userGuid1" },
				new UserCategory() { Id = 4, CategoryId = 30, UserId = "userGuid2" }
			});
			await _repo.SaveChangesAsync();

			var returnedCategories = await _homeService.GetCategoryItemDetailsForUser("userGuid2");

			Assert.True(returnedCategories.Count() == 2);
			Assert.True(returnedCategories.Any(c=>c.CategoryId == 10));
			Assert.True(returnedCategories.Any(c => c.CategoryId == 30));
		}

		[Test]
		public async Task Test_GetGroupedCategoryItemsByCategory()
		{
			var loggerMock = new Mock<ILogger<HomeService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_homeService = new HomeService(_repo, _logger);
			List<CategoryItemDetailsModel> models = new List<CategoryItemDetailsModel>()
			{
				new CategoryItemDetailsModel() { CategoryId = 10, CategoryItemId = 11, CategoryTitle = "", CategoryItemDescription = "", CategoryItemTitle = "",  MediaImagePath = "" },
				new CategoryItemDetailsModel() { CategoryId = 10, CategoryItemId = 22, CategoryTitle = "", CategoryItemDescription = "", CategoryItemTitle = "",  MediaImagePath = "" },
				new CategoryItemDetailsModel() { CategoryId = 10, CategoryItemId = 16, CategoryTitle = "", CategoryItemDescription = "", CategoryItemTitle = "",  MediaImagePath = "" },
				new CategoryItemDetailsModel() { CategoryId = 10, CategoryItemId = 18, CategoryTitle = "", CategoryItemDescription = "", CategoryItemTitle = "",  MediaImagePath = "" },
				new CategoryItemDetailsModel() { CategoryId = 5, CategoryItemId = 1, CategoryTitle = "", CategoryItemDescription = "", CategoryItemTitle = "",  MediaImagePath = "" },
				new CategoryItemDetailsModel() { CategoryId = 5, CategoryItemId = 9, CategoryTitle = "", CategoryItemDescription = "", CategoryItemTitle = "",  MediaImagePath = "" }
			};

			var groupedResult = await _homeService.GetGroupedCategoryItemsByCategory(models);

			Assert.IsTrue(groupedResult.Count() == 2);
			Assert.IsTrue(groupedResult.Select(groupKey => groupKey.Id == 10).Any());
			Assert.IsTrue(groupedResult.Select(groupKey => groupKey.Id == 5).Any());
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}
	}
}
