using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Contracts.DefaultArea;
using SpaceWatch.Core.Models;
using SpaceWatch.Core.Services;
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
	public class UsersToCategoryServiceTests
	{
		private IRepository _repo;
		private ILogger<UsersToCategoryService> _logger;
		private IUsersToCategoryService _usersToCategoryService;
		private IDataFunctions _dataFunctions;
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
		public async Task Test_GetSavedSelectedUsersForCategory_ReturnsCorrectAssignedUsers()
		{
			var loggerMock = new Mock<ILogger<UsersToCategoryService>>();
			_logger = loggerMock.Object;
			var _repo = new Repository(_context);
			var _dataFunctions = new DataFunctions(_context);
			_usersToCategoryService = new UsersToCategoryService(_repo, _dataFunctions, _logger);
			await _repo.AddRangeAsync(new List<Category>()
			{
				new Category() { Id = 10, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true },
				new Category() { Id = 20, Title = "", Description = "", ThumbnailImagePath = "", IsActive = true },
				new Category() { Id = 30, Title = "Inactive", Description = "", ThumbnailImagePath = "", IsActive = false }
			});
			await _repo.AddRangeAsync(new List<UserCategory>()
			{
				new UserCategory() { Id = 1, CategoryId = 10, UserId = "userGuid1" },
				new UserCategory() { Id = 2, CategoryId = 10, UserId = "userGuid2" },
				new UserCategory() { Id = 3, CategoryId = 10, UserId = "userGuid3" },
				new UserCategory() { Id = 4, CategoryId = 20, UserId = "userGuid2" },
				new UserCategory() { Id = 5, CategoryId = 30, UserId = "userGuid3" }
			});
			await _repo.SaveChangesAsync();

			var savedUsersIdsForCategory10 = await _usersToCategoryService.GetSavedSelectedUsersForCategory(10);
			var savedUsersIdsForCategory30 = await _usersToCategoryService.GetSavedSelectedUsersForCategory(30);

			Assert.True(savedUsersIdsForCategory10.Count() == 3);
			Assert.True(savedUsersIdsForCategory10.Any(user => user.Id == "userGuid1"));
			Assert.True(savedUsersIdsForCategory10.Any(user => user.Id == "userGuid2"));
			Assert.True(savedUsersIdsForCategory10.Any(user => user.Id == "userGuid3"));
			Assert.True(savedUsersIdsForCategory30.Count() == 0);
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}
	}
}
