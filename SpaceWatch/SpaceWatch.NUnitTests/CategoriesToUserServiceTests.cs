using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SpaceWatch.Core.Contracts.DefaultArea;
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
	public class CategoriesToUserServiceTests
	{
		private IRepository _repo;
		private ILogger<CategoriesToUserService> _logger;
		private ICategoriesToUserService _categoriesToUserService;
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
		public async Task Test_GetCategoriesThatHaveContent_ReturnsUniqueAndActiveCategoriesOnly()
		{
			var loggerMock = new Mock<ILogger<CategoriesToUserService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoriesToUserService = new CategoriesToUserService(_repo, _logger);
			await _repo.AddRangeAsync(new List<Category>()
			{
				new Category() { Id = 10, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true },
				new Category() { Id = 20, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true },
				new Category() { Id = 30, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true }
			});
			await _repo.AddRangeAsync(new List<CategoryItem>()
			{
				new CategoryItem() { Id = 1, CategoryId = 10, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true },
				new CategoryItem() { Id = 2, CategoryId = 10, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true },
				new CategoryItem() { Id = 3, CategoryId = 20, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true },
				new CategoryItem() { Id = 4, CategoryId = 30, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = false },
				new CategoryItem() { Id = 5, CategoryId = 30, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true },
			});
			await _repo.AddRangeAsync(new List<Content>()
			{
				new Content() { Id = 1, CategoryId = 10, CatItemId = 1, Title = "", HtmlContent = "", VideoLink = "", IsActive = true },
				new Content() { Id = 2, CategoryId = 10, CatItemId = 2, Title = "", HtmlContent = "", VideoLink = "", IsActive = true },
				new Content() { Id = 3, CategoryId = 20, CatItemId = 3, Title = "", HtmlContent = "", VideoLink = "", IsActive = true },
				new Content() { Id = 4, CategoryId = 30, CatItemId = 4, Title = "", HtmlContent = "", VideoLink = "", IsActive = false },
			});
			await _repo.SaveChangesAsync();

			var returnedCategories = await _categoriesToUserService.GetCategoriesThatHaveContent();

			Assert.NotNull(returnedCategories);
			Assert.False(returnedCategories.Any(category => category.Id == 30));
			Assert.True(returnedCategories.Count() == 2);
			Assert.True(returnedCategories.Any(category => category.Id == 10));
			Assert.True(returnedCategories.Any(category => category.Id == 20));
		}

		[Test]
		public async Task Test_GetCategoriesCurrentlySavedForUser_ReturnsOnlyActiveOnes()
		{
			var loggerMock = new Mock<ILogger<CategoriesToUserService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoriesToUserService = new CategoriesToUserService(_repo, _logger);
			await _repo.AddRangeAsync(new List<Category>()
			{
				new Category() { Id = 1, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true },
				new Category() { Id = 3, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true },
				new Category() { Id = 5, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true }
			});
			await _repo.AddRangeAsync(new List<CategoryItem>()
			{
				new CategoryItem() { Id = 10, CategoryId = 1, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true },
				new CategoryItem() { Id = 12, CategoryId = 1, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true },
				new CategoryItem() { Id = 30, CategoryId = 3, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true },
				new CategoryItem() { Id = 20, CategoryId = 5, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = false },
			});
			await _repo.AddAsync(new ApplicationUser()
			{
				Id = "a69a80c9-b70a-4049-b2ef-5b30c2196bb4",
				UserName = "",
				NormalizedUserName = "",
				FirstName = "",
				LastName = "",
				Email = "",
				NormalizedEmail = "",
				Address1 = "",
				PostCode = "",
				PhoneNumber = "",
				PasswordHash = ""
			});
			await _repo.AddRangeAsync(new List<UserCategory>()
			{
				new UserCategory() { Id = 1, CategoryId = 1, UserId = "a69a80c9-b70a-4049-b2ef-5b30c2196bb4" },
				new UserCategory() { Id = 2, CategoryId = 3, UserId = "a69a80c9-b70a-4049-b2ef-5b30c2196bb4" },
				new UserCategory() { Id = 3, CategoryId = 5, UserId = "a69a80c9-b70a-4049-b2ef-5b30c2196bb4" },
			});
			await _repo.SaveChangesAsync();

			var categoryIdsSavedForUser = await _categoriesToUserService.GetCategoriesCurrentlySavedForUser("a69a80c9-b70a-4049-b2ef-5b30c2196bb4");

			Assert.NotNull(categoryIdsSavedForUser);
			Assert.That(categoryIdsSavedForUser.Count(), Is.EqualTo(2));
			Assert.False(categoryIdsSavedForUser.Any(c => c.Id == 5));
		}

		[Test]
		public async Task Test_GetCategoriesToAddForUser()
		{
			var loggerMock = new Mock<ILogger<CategoriesToUserService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoriesToUserService = new CategoriesToUserService(_repo, _logger);
			string userId = "a69a80c9-b70a-4049-b2ef-5b30c2196bb4";
			string[] categoriesSelected = new string[] { "8", "2", "5", "20" };

			List<UserCategory> returnedList = await _categoriesToUserService.GetCategoriesToAddForUser(categoriesSelected, userId);

			Assert.That(returnedList.Count(), Is.EqualTo(4));
			Assert.True(returnedList.Any(uc => uc.CategoryId == 8));
			Assert.True(returnedList.Any(uc => uc.CategoryId == 2));
			Assert.True(returnedList.Any(uc => uc.CategoryId == 5));
			Assert.True(returnedList.Any(uc => uc.CategoryId == 20));
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}
	}
}
