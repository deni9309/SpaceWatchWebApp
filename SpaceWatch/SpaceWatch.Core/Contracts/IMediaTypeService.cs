using Microsoft.AspNetCore.Mvc.Rendering;
using SpaceWatch.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWatch.Core.Contracts
{
	public interface IMediaTypeService
	{
		Task<IEnumerable<MediaTypeViewModel>> GetAll();
        Task<IEnumerable<SelectListItem>> GetMediaTypesForSelectList();
        Task Add(MediaTypeViewModel model);

        Task<bool> MediaTypeExists(int mediaTypeId);

        Task<MediaTypeViewModel> MediaTypeDetailsById(int id);

        Task Edit(int mediaTypeId, MediaTypeViewModel model);

        Task Delete(int mediaTypeId);
    }
}
