using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts.DefaultArea;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data;
using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Core.Services.DefaultArea
{
	public class UserAuthService : IUserAuthService
	{
		private readonly IRepository _repo;
		private readonly ILogger<UserAuthService> _logger;

		public UserAuthService(IRepository repo,
			ILogger<UserAuthService> logger)
		{
			_repo = repo;
			_logger = logger;
		}

		public async Task AddCategoryToNewUserAsync(string userId, int categoryId)
		{
			UserCategory userCategory = new UserCategory()
			{
				CategoryId = categoryId,
				UserId = userId
			};

			try
			{
				await _repo.AddAsync(userCategory);
				await _repo.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(nameof(AddCategoryToNewUserAsync), ex);
				throw new ApplicationException("An error occured! Information could not be saved to database.", ex);
			}			
		}

		public async Task<bool> UserNameExists(string userName)
		{

			bool userNameExists = await _repo.AllReadonly<ApplicationUser>()
				.AnyAsync(u => u.UserName.ToUpper() == userName.ToUpper());

			if (userNameExists)
			{
				return true;
			}

			return false;
		}
	}
}
