using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;
using SpaceWatch.Core.Services;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data.Entities;
using SpaceWatch.Infrastructure.Data;

namespace SpaceWatch.NUnitTests
{
	[TestFixture]
	public class CategoryItemServiceTests
	{
		private IRepository _repo;
		private ILogger<CategoryItemService> _logger;
		private ICategoryItemService _categoryItemService;
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
		public async Task Test_GetCategoryTitleByCatItemId_ReturnsRightNameForActiveCategory()
		{
			var loggerMock = new Mock<ILogger<CategoryItemService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoryItemService = new CategoryItemService(_repo, _logger);
			await _repo.AddRangeAsync(new List<Category>()
			{
				new Category() { Id = 10, Title = "Title for category 10", Description = "", ThumbnailImagePath = "", IsActive = true },
				new Category() { Id = 20, Title = "Title for category 20", Description = "", ThumbnailImagePath = "", IsActive = true }
			});
			await _repo.AddRangeAsync(new List<CategoryItem>()
			{
				new CategoryItem() { Id = 1, CategoryId = 10, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true },
				new CategoryItem() { Id = 2, CategoryId = 20, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true }
			});
			await _repo.SaveChangesAsync();

			string categoryTitle10 = await _categoryItemService.GetCategoryTitleByCatItemId(1);
			string categoryTitle20 = await _categoryItemService.GetCategoryTitleByCatItemId(2);

			Assert.True(categoryTitle10 == "Title for category 10");
			Assert.True(categoryTitle20 == "Title for category 20");
		}

		[Test]
		public async Task Test_CategoryItem_Edit_ReturnsCorectResultForCorrectId()
		{
			var loggerMock = new Mock<ILogger<CategoryItemService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoryItemService = new CategoryItemService(_repo, _logger);
			await _repo.AddAsync(new CategoryItem()
			{
				Id = 1,
				Title = "t",
				Description = "d",
				MediaTypeId = 1,
				DateTimeItemReleased = DateTime.MinValue,
				IsActive = true
			});
			await _repo.SaveChangesAsync();

			await _categoryItemService.Edit(1, new CategoryItemAddViewModel()
			{
				Description = "CategoryItem is edited",
				Title = "t",
				MediaTypeId = 1,
				CategoryId = 1,
				DateTimeItemReleased = DateTime.MinValue
			});
			var entity = await _repo.GetByIdAsync<CategoryItem>(1);

			Assert.That(entity.Description, Is.EqualTo("CategoryItem is edited"));
			Assert.That(entity.Id, Is.EqualTo(1));
		}

		[Test]
		public async Task Test_GetAllCategoryItemsFromCategory_ReturnsActiveAndCorrectOnes()
		{
			var loggerMock = new Mock<ILogger<CategoryItemService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoryItemService = new CategoryItemService(_repo, _logger);
			await _repo.AddAsync(new Category() { Id = 2, Description = "", IsActive = true, Title = "", ThumbnailImagePath = "" });
			await _repo.AddRangeAsync(new List<CategoryItem>()
			{
				new  CategoryItem() { Id = 1, CategoryId = 2, Title = "t1", Description = "d1", DateTimeItemReleased = DateTime.MinValue, IsActive = true, MediaTypeId = 1 },
				new  CategoryItem() { Id = 5, CategoryId = 2, Title = "t5", Description = "d5", DateTimeItemReleased = DateTime.MinValue, IsActive = true, MediaTypeId = 1 },
				new  CategoryItem() { Id = 3, CategoryId = 2, Title = "t3", Description = "d3", DateTimeItemReleased = DateTime.MinValue, IsActive = false, MediaTypeId = 1 },
				new  CategoryItem() { Id = 8, CategoryId = 3, Title = "t8", Description = "d8", DateTimeItemReleased = DateTime.MinValue, IsActive = true, MediaTypeId = 1 }
			});
			await _repo.SaveChangesAsync();

			var categoryItemModels = await _categoryItemService.GetAllCategoryItemsFromCategory(2);

			Assert.That(categoryItemModels.Any(c => c.Id == 3), Is.False);
			Assert.That(categoryItemModels.Any(c => c.Id == 8), Is.False);
			Assert.That(categoryItemModels.Count(), Is.EqualTo(2));
		}

		[Test]
		public async Task Test_CategorytItemDeatilsById_ReturnsCorrectResult()
		{
			var loggerMock = new Mock<ILogger<CategoryItemService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoryItemService = new CategoryItemService(_repo, _logger);
			await _repo.AddAsync(new CategoryItem()
			{
				Id = 10,
				Title = "t10",
				Description = "d10",
				DateTimeItemReleased = DateTime.Parse("22/09/18"),
				IsActive = true,
				MediaTypeId = 11,
				CategoryId = 12
			});
			await _repo.SaveChangesAsync();

			var entityToCheck = await _categoryItemService.CategoryItemDetailsById(10);

			Assert.That(entityToCheck.Id, Is.EqualTo(10));
			Assert.That(entityToCheck.Title, Is.EqualTo("t10"));
			Assert.That(entityToCheck.Description, Is.EqualTo("d10"));
			Assert.That(entityToCheck.DateTimeItemReleased, Is.EqualTo(DateTime.Parse("22/09/18")));
			Assert.That(entityToCheck.CategoryId, Is.EqualTo(12));
		}

		[Test]
		public async Task Test_CategoryItem_Add()
		{
			var loggerMock = new Mock<ILogger<CategoryItemService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoryItemService = new CategoryItemService(_repo, _logger);
			var model = new CategoryItemAddViewModel()
			{
				Title = "",
				Description = "This CategoryItem is added now",
				CategoryId = 1,
				MediaTypeId = 1,
				DateTimeItemReleased = DateTime.MinValue
			};

			int id = await _categoryItemService.Add(model);
			var addedEntity = await _repo.GetByIdAsync<CategoryItem>(id);

			Assert.That(addedEntity.Description, Is.EqualTo("This CategoryItem is added now"));
		}

		[Test]
		public async Task Test_CategoryItem_Delete()
		{
			var loggerMock = new Mock<ILogger<CategoryItemService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoryItemService = new CategoryItemService(_repo, _logger);
			await _repo.AddAsync(new CategoryItem()
			{
				Id = 100,
				CategoryId = 2,
				MediaTypeId = 2,
				Title = "title",
				Description = "description",
				DateTimeItemReleased = DateTime.MinValue,
				IsActive = true				
			});
			await _repo.SaveChangesAsync();

			await _categoryItemService.Delete(100);
			var deletedEntity = await _repo.GetByIdAsync<CategoryItem>(100);

			Assert.That(deletedEntity.IsActive, Is.EqualTo(false));
		}

		[Test]
		public async Task Test_CategoryItem_DeleteReletedContent()
		{
			var loggerMock = new Mock<ILogger<CategoryItemService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoryItemService = new CategoryItemService(_repo, _logger);
			await _repo.AddAsync(new CategoryItem() { Id = 1, CategoryId = 1, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true });
			await _repo.AddAsync(new Content() { Id = 1, CategoryId = 1, CatItemId = 1, Title = "", HtmlContent = "", VideoLink = "", IsActive = true });
			await _repo.SaveChangesAsync();

			await _categoryItemService.DeleteReletedContent(1);
			var deletedContent = await _repo.GetByIdAsync<Content>(1);

			Assert.True(deletedContent.CategoryItem.Id == 1);
			Assert.That(deletedContent.IsActive, Is.EqualTo(false));
		}

		[Test]
		public async Task Test_CategoryItemExists_ReturnsTrueIfActiveWithValidId()
		{
			var loggerMock = new Mock<ILogger<CategoryItemService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoryItemService = new CategoryItemService(_repo, _logger);
			await _repo.AddRangeAsync(new List<CategoryItem>()
			{
				new CategoryItem() { Id = 1, CategoryId = 2, MediaTypeId = 2, Title = "", Description = "", DateTimeItemReleased = DateTime.MinValue, IsActive = false },
				new CategoryItem() { Id = 5, CategoryId = 2, MediaTypeId = 2, Title = "", Description = "", DateTimeItemReleased = DateTime.MinValue, IsActive = true }
			});
			await _repo.SaveChangesAsync();

			var catItem1_Exists = await _categoryItemService.CategoryItemExists(1);
			var catItem5_Exists = await _categoryItemService.CategoryItemExists(5);
			var catItem100_Exists = await _categoryItemService.CategoryItemExists(100);

			Assert.That(catItem1_Exists, Is.EqualTo(false));
			Assert.That(catItem5_Exists, Is.EqualTo(true));
			Assert.That(catItem100_Exists, Is.EqualTo(false));
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}
	}
}

