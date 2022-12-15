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
	public class ContentServiceTests
	{
		private IRepository _repo;
		private ILogger<ContentService> _logger;
		private IContentService _contentService;
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
		public async Task Test_ContentDetailsByCategoryItemId_ReturnsInfoForActiveContent()
		{
			var loggerMock = new Mock<ILogger<ContentService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_contentService = new ContentService(_repo, _logger);
			await _repo.AddAsync(new Category() { Id = 1, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true });
			await _repo.AddRangeAsync(new List<CategoryItem>()
			{
				new CategoryItem() { Id = 1, CategoryId = 1, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true },
				new CategoryItem() { Id = 3, CategoryId = 1, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true }
			});
			await _repo.AddRangeAsync(new List<Content>()
			{
				new Content() { Id = 11, CategoryId = 1, CatItemId = 1, Title = "This shoud be returned content", VideoLink = "", HtmlContent="", IsActive = true },
				new Content() { Id = 15, CategoryId = 1, CatItemId = 3, Title = "", VideoLink = "", HtmlContent="", IsActive = false }
			});
			await _repo.SaveChangesAsync();

			var returnedForActiveContent = await _contentService.ContentDetailsByCategoryItemId(1);

			Assert.True(returnedForActiveContent.CatItemId == 1);
			Assert.True(returnedForActiveContent.Title == "This shoud be returned content");
			Assert.CatchAsync<ApplicationException>(async ()
				=> await _contentService.ContentDetailsByCategoryItemId(3), "Could not find Content details for given categoryItemId");
		}

		[Test]
		public async Task Test_ContentDetailsById_ShouldThrowForInvalidId()
		{
			var loggerMock = new Mock<ILogger<ContentService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_contentService = new ContentService(_repo, _logger);
			await _repo.AddAsync(new Category() { Id = 1, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true });
			await _repo.AddAsync(new CategoryItem() { Id = 1, CategoryId = 1, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true });
			await _repo.AddAsync(new Content() { Id = 2, CategoryId = 1, CatItemId = 1, Title = "", VideoLink = "", HtmlContent = "", IsActive = true });
			await _repo.SaveChangesAsync();

			Assert.CatchAsync<Exception>(async ()
				=> await _contentService.ContentDetailsById(222), "Could not find Content details for given Id");
		}

		[Test]
		public async Task Test_Content_Add()
		{
			var loggerMock = new Mock<ILogger<ContentService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_contentService = new ContentService(_repo, _logger);
			var model = new ContentViewModel()
			{
				CategoryId = 2,
				Title = "this is content's title",
				CatItemId = 3,
				VideoLink = "",
				HtmlContent = ""
			};
		
			int id = await _contentService.Add(model);
			var content = await _repo.GetByIdAsync<Content>(id);

			Assert.That(content.Id, Is.EqualTo(1));
			Assert.That(content.Title, Is.EqualTo("this is content's title"));
		}

		[Test]
		public async Task Test_Content_Edit()
		{
			var loggerMock = new Mock<ILogger<ContentService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_contentService = new ContentService(_repo, _logger);
			await _repo.AddAsync(new Content()
			{
				Id = 1,
				CategoryId = 2,
				Title = "",
				CatItemId = 3,
				VideoLink = "",
				HtmlContent = ""			
			});
			await _repo.SaveChangesAsync();

			await _contentService.Edit(1, new ContentViewModel()
			{
				CategoryId = 2,
				Title = "content successfully edited",			
				CatItemId = 3,
				VideoLink = "",
				HtmlContent = ""
			});
			var content = await _repo.GetByIdAsync<Content>(1);

			Assert.That(content.Title, Is.EqualTo("content successfully edited"));
		}

		[Test]
		public async Task Test_ContentExists_ReturnsTrueIfActiveWithValidId()
		{
			var loggerMock = new Mock<ILogger<ContentService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_contentService = new ContentService(_repo, _logger);
			await _repo.AddRangeAsync(new List<Content>()
			{
				new Content() { Id = 1, CategoryId = 1, CatItemId = 10, Title = "", VideoLink = "", HtmlContent="", IsActive = false },
				new Content() { Id = 5, CategoryId = 1, CatItemId = 11, Title = "", VideoLink = "", HtmlContent="", IsActive = true }
			});
			await _repo.SaveChangesAsync();

			var content1_Exists = await _contentService.ContentExists(1);
			var content5_Exists = await _contentService.ContentExists(5);
			var content100_Exists = await _contentService.ContentExists(100);

			Assert.That(content1_Exists, Is.EqualTo(false));
			Assert.That(content5_Exists, Is.EqualTo(true));
			Assert.That(content100_Exists, Is.EqualTo(false));
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}
	}
}
