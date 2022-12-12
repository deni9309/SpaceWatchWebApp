using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;
using SpaceWatch.Core.Services;
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
	public class MediaTypeServiceTests
	{
		private IRepository _repo;
		private ILogger<MediaTypeService> _logger;
		private IMediaTypeService _mediaTypeService;
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
		public async Task Test_MediaType_Add()
		{
			var loggerMock = new Mock<ILogger<MediaTypeService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_mediaTypeService = new MediaTypeService(_repo, _logger);
			var model = new MediaTypeViewModel()
			{
				ThumbnailImagePath = "",
				Title = "This is a test"
			};

			await _mediaTypeService.Add(model);
			var entity = await _repo.AllReadonly<MediaType>()
				.FirstAsync(mt => mt.Title == "This is a test");

			Assert.That(entity.Title, Is.EqualTo("This is a test"));
			Assert.That(entity.Id, Is.GreaterThan(0));
		}

		[Test]
		public async Task Test_MediaType_Edit()
		{
			var loggerMock = new Mock<ILogger<MediaTypeService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_mediaTypeService = new MediaTypeService(_repo, _logger);
			await _repo.AddAsync(new MediaType()
			{
				Id = 121,
				ThumbnailImagePath = "",
				Title = ""
			});
			await _repo.SaveChangesAsync();

			await _mediaTypeService.Edit(121, new MediaTypeViewModel()
			{
				ThumbnailImagePath = "",
				Title = "successfully edited"
			});
			var entity = await _repo.GetByIdAsync<MediaType>(121);

			Assert.That(entity.Title, Is.EqualTo("successfully edited"));
			Assert.That(entity.Id, Is.EqualTo(121));
		}

		[Test]
		public async Task Test_MediaType_Delete()
		{
			var loggerMock = new Mock<ILogger<MediaTypeService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_mediaTypeService = new MediaTypeService(_repo, _logger);
			await _repo.AddAsync(new MediaType()
			{
				Id = 1,
				ThumbnailImagePath = "",
				Title = "to be deleted"
			});
			await _repo.SaveChangesAsync();

			await _mediaTypeService.Delete(1);
			var deletedEntity = await _repo.GetByIdAsync<MediaType>(1);

			Assert.That(deletedEntity.IsActive, Is.EqualTo(false));
		}

		[Test]
		public async Task Test_MediaType_GetAll_ReturnsActiveOnesAndCorrectCount()
		{
			var loggerMock = new Mock<ILogger<MediaTypeService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			_mediaTypeService = new MediaTypeService(_repo, _logger);
			await _repo.AddRangeAsync(new List<MediaType>()
			{
				new  MediaType() { Id = 10, Title = "t10", ThumbnailImagePath = "", IsActive = false },
				new  MediaType() { Id = 20, Title = "t20",  ThumbnailImagePath = "", IsActive = true },
				new  MediaType() { Id = 5, Title = "t5",  ThumbnailImagePath = "", IsActive = true },
				new  MediaType() { Id = 35, Title = "t35",  ThumbnailImagePath = "", IsActive = true }
			});
			await _repo.SaveChangesAsync();

			var collection = await _mediaTypeService.GetAll();

			Assert.That(collection.Count(), Is.EqualTo(3));
			Assert.That(collection.Any(mt => mt.Id == 10), Is.False);
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}
	}
}
