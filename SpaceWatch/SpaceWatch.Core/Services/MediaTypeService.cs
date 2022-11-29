using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data.Entities;
using SpaceWatch.Infrastructure.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWatch.Core.Services
{
	public class MediaTypeService : IMediaTypeService
	{
		private readonly IRepository _repo;
		public MediaTypeService(IRepository repository)
		{
			_repo = repository;
		}

        public async Task Add(MediaTypeViewModel model)
        {
			var mediaType = new MediaType()
			{
				Title = model.Title,
				ThumbnailImagePath = model.ThumbnailImagePath
			};

			await _repo.AddAsync(mediaType);
			await _repo.SaveChangesAsync();
        }

        public async Task Edit(int mediaTypeId, MediaTypeViewModel model)
        {
			var mediaType = await _repo.GetByIdAsync<MediaType>(mediaTypeId);

			mediaType.Title = model.Title;
			mediaType.ThumbnailImagePath = model.ThumbnailImagePath;
			
			await _repo.SaveChangesAsync();
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
            var mediaType = await _repo.GetByIdAsync<MediaType>(mediaTypeId);

            mediaType.IsActive = false;

            await _repo.SaveChangesAsync();
        }

        public async Task<MediaTypeViewModel> MediaTypeDetailsById(int id)
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

        public async Task<bool> MediaTypeExists(int mediaTypeId)
		{
            return await _repo.AllReadonly<MediaType>()
               .AnyAsync(m => m.Id == mediaTypeId && m.IsActive);
        }

      
    }
}
