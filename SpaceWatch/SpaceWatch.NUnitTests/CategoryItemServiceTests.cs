using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;
using SpaceWatch.Core.Services;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data.Entities;
using SpaceWatch.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

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
		public async Task TestCategoryItemEditReturnsCorectResultForCorrectId()
		{
			//Arrange
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

			//Act
			await _categoryItemService.Edit(1, new CategoryItemAddViewModel()
			{
				Description = "CategoryItem is edited",
				Title = "t",
				MediaTypeId = 1,
				CategoryId = 1,
				DateTimeItemReleased = DateTime.MinValue
			});
			var dbSpaceWatch = await _repo.GetByIdAsync<CategoryItem>(1);

			//Assert
			Assert.That(dbSpaceWatch.Description, Is.EqualTo("CategoryItem is edited"));
			Assert.That(dbSpaceWatch.Id, Is.EqualTo(1));
		}

		[Test]
		public async Task Test_GetAllCategoryItemsFromCategory_ReturnsActiveAndCorrectOnes()
		{
			//Arrange
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

			//Act
			var categoryItemModels = await _categoryItemService.GetAllCategoryItemsFromCategory(2);

			//Assert
			Assert.That(categoryItemModels.Any(c => c.Id == 3), Is.False);
			Assert.That(categoryItemModels.Any(c => c.Id == 8), Is.False);
			Assert.That(2, Is.EqualTo(categoryItemModels.Count()));
		}

		[Test]
		public async Task Test_CategorytItemDeatilsById_ReturnsCorrectResult()
		{
			//Arrange
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

			//Act
			var entityToCheck = await _categoryItemService.CategoryItemDetailsById(10);

			//Assert
			Assert.That(entityToCheck.Id, Is.EqualTo(10));
			Assert.That(entityToCheck.Title, Is.EqualTo("t10"));
			Assert.That(entityToCheck.Description, Is.EqualTo("d10"));
			Assert.That(entityToCheck.DateTimeItemReleased, Is.EqualTo(DateTime.Parse("22/09/18")));
			Assert.That(entityToCheck.CategoryId, Is.EqualTo(12));
		}

		[Test]
		public async Task TestCategoryItemAdd()
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

			//Act
			int id = await _categoryItemService.Add(model);
			var expected = await _repo.GetByIdAsync<CategoryItem>(id);

			//Assert
			Assert.That(expected.Description, Is.EqualTo("This CategoryItem is added now"));
		}

		[Test]
		public async Task TestCategoryItemDelete()
		{
			//Arrange
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
				IsActive = true,				
			});
			await _repo.SaveChangesAsync();

			//Act
			await _categoryItemService.Delete(100);
			var deletedEntity = await _repo.GetByIdAsync<CategoryItem>(100);

			//Assert
			Assert.That(deletedEntity.IsActive, Is.EqualTo(false));
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}
	}
}

