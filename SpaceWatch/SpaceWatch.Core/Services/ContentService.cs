using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWatch.Core.Services
{
	public class ContentService : IContentService
	{
		public Task Add(ContentViewModel model)
		{
			throw new NotImplementedException();
		}

		public Task<ContentViewModel> ContentDetailsById(int id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> ContentExists(int ContentId)
		{
			throw new NotImplementedException();
		}

		public Task Delete(int mediaTypeId)
		{
			throw new NotImplementedException();
		}

		public Task Edit(int mediaTypeId, MediaTypeViewModel model)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ContentViewModel>> GetAll()
		{
			throw new NotImplementedException();
		}

		public Task<CategoryViewModel> GetCategoryAsync(int contentId)
		{
			throw new NotImplementedException();
		}

		public Task<string> GetCatItemNameAsync(int contentId)
		{
			throw new NotImplementedException();
		}
	}
}
