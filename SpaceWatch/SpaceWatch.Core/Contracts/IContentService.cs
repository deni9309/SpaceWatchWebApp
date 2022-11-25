using SpaceWatch.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWatch.Core.Contracts
{
	public interface IContentService
	{
		Task<IEnumerable<ContentViewModel>> GetAll();

		Task Add(ContentViewModel model);

		Task<bool> ContentExists(int ContentId);

		Task<CategoryViewModel> GetCategoryAsync(int contentId);

		Task<string> GetCatItemNameAsync(int contentId);

		Task<ContentViewModel> ContentDetailsById(int id);

		Task Edit(int mediaTypeId, MediaTypeViewModel model);

		Task Delete(int mediaTypeId);
	}
}
