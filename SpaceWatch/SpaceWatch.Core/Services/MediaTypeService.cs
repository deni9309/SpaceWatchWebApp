using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Extensions;
using SpaceWatch.Core.Models;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Core.Services
{
	public class MediaTypeService : IMediaTypeService
	{
		private readonly IRepository _repo;
		private readonly ILogger<MediaTypeService> _logger;
		public MediaTypeService(IRepository repository,
			ILogger<MediaTypeService> logger)
		{
			_repo = repository;
			_logger = logger;
		}

        public async Task Add(MediaTypeViewModel model)
        {
			var mediaType = new MediaType()
			{
				Title = model.Title,
				ThumbnailImagePath = model.ThumbnailImagePath
			};

			try
			{
				await _repo.AddAsync(mediaType);
				await _repo.SaveChangesAsync();
			}
			catch (Exception ex) 
			{
				_logger.LogError(nameof(Add), ex);
				throw new ApplicationException("Database failed to save info.", ex);
			}		
        }

        public async Task Edit(int mediaTypeId, MediaTypeViewModel model)
        {
			try
			{
				var mediaType = await _repo.GetByIdAsync<MediaType>(mediaTypeId);

				mediaType.Title = model.Title;
				mediaType.ThumbnailImagePath = model.ThumbnailImagePath;

				await _repo.SaveChangesAsync();
			}
			catch (ArgumentNullException ex)
			{
				_logger.LogError(nameof(Edit), ex);
				throw new ArgumentNullException(ex.Message, ex.InnerException);
			}
			catch (Exception ex)
			{
				_logger.LogError(nameof(Edit), ex);
				throw new ApplicationException("An error occured while trying to update entity.", ex);
			}
        }

        public async Task<IEnumerable<MediaTypeViewModel>> GetAll()
		{
			return await _repo.AllReadonly<MediaType>()
				.Where(m => m.IsActive)
				.Select(m => new MediaTypeViewModel()
				{
					Id = m.Id,
					ThumbnailImagePath = m.ThumbnailImagePath,
					Title = m.Title
				})
				.ToListAsync();
		}

        public async Task<IEnumerable<SelectListItem>> GetMediaTypesForSelectList()
        {
          var mediaTypes=  await _repo.AllReadonly<MediaType>()
                 .Where(m => m.IsActive)
                 .Select(m => new MediaTypeSelectListModel()
                 {
                     Id = m.Id,
                     Title = m.Title
                 })
                 .ToListAsync();

			return mediaTypes.ConvertToSelectList(0);
        }

        public async Task Delete(int mediaTypeId)
        {
			try
			{
				if ((await MediaTypeExists(mediaTypeId)) == true)
				{
					var mediaType = await _repo.GetByIdAsync<MediaType>(mediaTypeId);

					mediaType.IsActive = false;

					await _repo.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(nameof(Delete), ex);
				throw new ApplicationException(ex.Message, ex.InnerException);
			}			
        }

        public async Task<MediaTypeViewModel> MediaTypeDetailsById(int id)
        {
			try
			{
				return await _repo.AllReadonly<MediaType>()
					.Where(m => m.IsActive)
					.Where(m => m.Id == id)
					.Select(m => new MediaTypeViewModel()
					{
						ThumbnailImagePath = m.ThumbnailImagePath,
						Title = m.Title,
						Id = m.Id
					})
					.FirstAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(nameof(MediaTypeDetailsById), ex);
				throw new ApplicationException(ex.Message, ex.InnerException);
			}		
        }

        public async Task<bool> MediaTypeExists(int mediaTypeId)
		{
            return await _repo.AllReadonly<MediaType>()
               .AnyAsync(m => m.Id == mediaTypeId && m.IsActive);
        }

      
    }
}
