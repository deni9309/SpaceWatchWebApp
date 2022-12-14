using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SpaceWatch.Core.Contracts.DefaultArea;
using SpaceWatch.Core.Models.DefaultArea;
using SpaceWatch.Core.Services.DefaultArea;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data;
using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.NUnitTests
{
	[TestFixture]
	public class ContentForUserServiceTests
	{
		private IRepository _repo;
		private ILogger<ContentForUserService> _logger;
		private IContentForUserService _contentForUserService;
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
		public async Task Test_AddComment_AddsNewCommentOnValidAndExistingContentAndUser()
		{
			var loggerMock = new Mock<ILogger<ContentForUserService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_contentForUserService = new ContentForUserService(_repo, _logger);
			await _repo.AddAsync(new Category() { Id = 1, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true });
			await _repo.AddAsync(new CategoryItem() { Id = 1, CategoryId = 1, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true });
			await _repo.AddAsync(new Content() { Id = 1, CategoryId = 1, CatItemId = 1, Title = "", HtmlContent = "", VideoLink = "", IsActive = true });
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
			await _repo.SaveChangesAsync();
			var model = new CommentAddViewModel()
			{
				CategoryItemId = 1,
				ContentId = 1,
				CommentBody = "",
				UserId = "a69a80c9-b70a-4049-b2ef-5b30c2196bb4"
			};

			await _contentForUserService.AddComment(model);
			var addedComment = await _repo.GetByIdAsync<Comment>(1);

			Assert.That(addedComment, Is.Not.Null);
			Assert.That(addedComment.UserId, Is.EqualTo("a69a80c9-b70a-4049-b2ef-5b30c2196bb4"));
			Assert.That(addedComment.Content.Id, Is.EqualTo(1));
		}

		[Test]
		public async Task Test_AddComment_DoesNotAddNewCommentOnInvalidUser()
		{
			var loggerMock = new Mock<ILogger<ContentForUserService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_contentForUserService = new ContentForUserService(_repo, _logger);
			await _repo.AddAsync(new Category() { Id = 1, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true });
			await _repo.AddAsync(new CategoryItem() { Id = 1, CategoryId = 1, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true });
			await _repo.AddAsync(new Content() { Id = 1, CategoryId = 1, CatItemId = 1, Title = "", HtmlContent = "", VideoLink = "", IsActive = true });
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
			await _repo.SaveChangesAsync();
			var model = new CommentAddViewModel()
			{
				CategoryItemId = 1,
				ContentId = 1,
				CommentBody = "",
				UserId = "00000000-0000-0000-0000-000000000000"
			};

			await _contentForUserService.AddComment(model);
			var addedComment = await _repo.GetByIdAsync<Comment>(1);

			Assert.That(addedComment, Is.Null);
		}

		[Test]
		public async Task Test_GetContent_ShouldReturnActiveContentForValidCategoryItem()
		{
			var loggerMock = new Mock<ILogger<ContentForUserService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_contentForUserService = new ContentForUserService(_repo, _logger);
			await _repo.AddAsync(new CategoryItem() { Id = 1, CategoryId = 1, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true });
			await _repo.AddAsync(new Content() { Id = 1, CategoryId = 1, CatItemId = 1, Title = "Successfully returned", HtmlContent = "", VideoLink = "", IsActive = true });
			await _repo.SaveChangesAsync();

			var returnedContent = await _contentForUserService.GetContent(1);

			Assert.NotNull(returnedContent);
			Assert.IsTrue(returnedContent.CategoryItemId == 1);
			Assert.IsTrue(returnedContent.Title == "Successfully returned");
		}

		[Test]
		public async Task Test_GetContent_ShouldNotReturnInactiveContent()
		{
			var loggerMock = new Mock<ILogger<ContentForUserService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_contentForUserService = new ContentForUserService(_repo, _logger);
			await _repo.AddAsync(new Category() { Id = 1, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true });
			await _repo.AddAsync(new CategoryItem() { Id = 1, CategoryId = 1, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true });
			await _repo.AddAsync(new Content() { Id = 1, CategoryId = 1, CatItemId = 1, Title = "", HtmlContent = "", VideoLink = "", IsActive = false });
			await _repo.SaveChangesAsync();

			var returnedContent = await _contentForUserService.GetContent(1);

			Assert.IsNull(returnedContent);
		}

		[Test]
		public async Task Test_GetCommentsForContentReturnsCorrectCount()
		{
			var loggerMock = new Mock<ILogger<ContentForUserService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_contentForUserService = new ContentForUserService(_repo, _logger);
			await _repo.AddAsync(new Category() { Id = 1, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true });
			await _repo.AddAsync(new CategoryItem() { Id = 1, CategoryId = 1, MediaTypeId = 1, Title = "", DateTimeItemReleased = DateTime.MinValue, Description = "", IsActive = true });
			await _repo.AddAsync(new Content() { Id = 1, CategoryId = 1, CatItemId = 1, Title = "", HtmlContent = "", VideoLink = "", IsActive = true });
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
			await _repo.AddRangeAsync(new List<Comment>()
			{
				new Comment() { Id = 1, ContentId = 1, CommentBody = "comment1", DatePosted = DateTime.MinValue, UserId = "a69a80c9-b70a-4049-b2ef-5b30c2196bb4" },
				new Comment() { Id = 2, ContentId = 1, CommentBody = "comment2", DatePosted = DateTime.MinValue, UserId = "a69a80c9-b70a-4049-b2ef-5b30c2196bb4" },
				new Comment() { Id = 3, ContentId = 1, CommentBody = "comment3", DatePosted = DateTime.MinValue, UserId = "a69a80c9-b70a-4049-b2ef-5b30c2196bb4" },
			});
			await _repo.SaveChangesAsync();

			var commentsForContent = await _contentForUserService.GetCommentsForContent(1);

			Assert.NotNull(commentsForContent);
			Assert.That(commentsForContent.Count(), Is.EqualTo(3));
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}
	}
}
