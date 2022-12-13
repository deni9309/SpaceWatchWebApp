using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;
using SpaceWatch.Core.Services;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data;
using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.NUnitTests
{
	[TestFixture]
	public class CategoryServiceTests
	{
		private IRepository _repo;
		private ILogger<CategoryService> _logger;
		private ICategoryService _categoryService;
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
		public async Task Test_Category_Edit()
		{
			var loggerMock = new Mock<ILogger<CategoryService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoryService = new CategoryService(_repo, _logger);
			await _repo.AddAsync(new Category()
			{ 
			 Id = 1,
			 Title = "title1",
			 Description = "description1",
			 ThumbnailImagePath = "thumbnailImagePath1",
			 IsActive = true
			});
			await _repo.SaveChangesAsync();

			await _categoryService.Edit(1, new CategoryViewModel()
			{
				Description = "Category edited",
				Title = "title1",
				ThumbnailImagePath = "thumbnailImagePath1"
			});
			var entity = await _repo.GetByIdAsync<Category>(1);

			Assert.That(entity.Description, Is.EqualTo("Category edited"));
		}

		[Test]
		public async Task Test_Category_GetAll_ReturnsOnlyActiveOnes()
		{
			var loggerMock = new Mock<ILogger<CategoryService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoryService = new CategoryService(_repo, _logger);
			await _repo.AddRangeAsync(new List<Category>()
			{
				new  Category() { Id = 1, Title = "t1", Description = "d1", ThumbnailImagePath = "tp1", IsActive = true },
				new  Category() { Id = 5, Title = "t5", Description = "d5", ThumbnailImagePath = "tp5", IsActive = true },
				new  Category() { Id = 3, Title = "t3", Description = "d3", ThumbnailImagePath = "tp3", IsActive = false },
				new  Category() { Id = 10, Title = "t10", Description = "d10", ThumbnailImagePath = "tp10", IsActive = true }
			});
			await _repo.SaveChangesAsync();

			var categoryModels = await _categoryService.GetAll();

			Assert.That(categoryModels.Any(cm => cm.Id == 3), Is.False);
			Assert.That(categoryModels.Count(), Is.EqualTo(3));
		}

		[Test]
		public async Task Test_CategorytDeatilsById()
		{
			var loggerMock = new Mock<ILogger<CategoryService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoryService = new CategoryService(_repo, _logger);
			await _repo.AddAsync(new Category()
			{
				Id = 10,
				Title = "t10",
				Description = "d10",
				ThumbnailImagePath = "tp10",
				IsActive = true
			});
			await _repo.SaveChangesAsync();

			var categoryModel = await _categoryService.CategoryDetailsById(10);

			Assert.That(categoryModel.Id, Is.EqualTo(10));
			Assert.That(categoryModel.Title, Is.EqualTo("t10"));
			Assert.That(categoryModel.Description, Is.EqualTo("d10"));
			Assert.That(categoryModel.ThumbnailImagePath, Is.EqualTo("tp10"));		
		}

		[Test]
		public async Task Test_Category_Add()
		{
			var loggerMock = new Mock<ILogger<CategoryService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoryService = new CategoryService(_repo, _logger);
			var model = new CategoryViewModel()
			{
				Title = "This category tests Add method",
				Description = "description",
				ThumbnailImagePath = "thumbnailImagePath"
			};

			await _categoryService.Add(model);		
			var entity = await _repo.AllReadonly<Category>()
				.FirstAsync(c=>c.Title == "This category tests Add method");

			Assert.That(entity.Title, Is.EqualTo("This category tests Add method"));
		}

		[Test]
		public async Task Test_Category_Delete()
		{
			var loggerMock = new Mock<ILogger<CategoryService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoryService = new CategoryService(_repo, _logger);
			await _repo.AddAsync(new Category()
			{
				Id = 1,
				Title = "title",
				Description = "description",
				ThumbnailImagePath = "thumbnailImagePath",
				IsActive = true
			});
			await _repo.SaveChangesAsync();

			await _categoryService.Delete(1);
			var deletedEntity = await _repo.GetByIdAsync<Category>(1);

			Assert.That(deletedEntity.IsActive, Is.EqualTo(false));
		}

		[Test]
		public async Task Test_DeleteReletedCategoryItemsAndContentFromCategory()
		{
			var loggerMock = new Mock<ILogger<CategoryService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoryService = new CategoryService(_repo, _logger);
			await _repo.AddAsync(new Category() { Id = 1, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true });
			await _repo.AddAsync(new CategoryItem() { Id = 2, CategoryId = 1, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true });
			await _repo.AddAsync(new Content() { Id = 3, CategoryId = 1, CatItemId = 2, Title = "", HtmlContent = "", VideoLink = "", IsActive = true });
			await _repo.SaveChangesAsync();

			await _categoryService.DeleteReletedCategoryItemsAndContentFromCategory(1);
			var deletedCategoryItem = await _repo.GetByIdAsync<CategoryItem>(2);
			var deletedContent = await _repo.GetByIdAsync<Content>(3);

			Assert.True(deletedCategoryItem.CategoryId == 1);
			Assert.That(deletedCategoryItem.IsActive, Is.EqualTo(false));
			Assert.True(deletedContent.CategoryId == 1);
			Assert.That(deletedContent.IsActive, Is.EqualTo(false));
		}

		[Test]
		public async Task Test_CategoryExists_ReturnsTrueIfActiveWithValidId()
		{
			var loggerMock = new Mock<ILogger<CategoryService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_categoryService = new CategoryService(_repo, _logger);
			await _repo.AddRangeAsync(new List<Category>()
			{
				new Category() { Id = 1, Title = "", Description = "", ThumbnailImagePath = "", IsActive = false },
				new Category() { Id = 5, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true }
			});
			await _repo.SaveChangesAsync();

			var category1_Exists = await _categoryService.CategoryExists(1);
			var category5_Exists = await _categoryService.CategoryExists(5);
			var category100_Exists = await _categoryService.CategoryExists(100);

			Assert.That(category1_Exists, Is.EqualTo(false));
			Assert.That(category5_Exists, Is.EqualTo(true));
			Assert.That(category100_Exists, Is.EqualTo(false));
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}
	}
}