using Microsoft.AspNetCore.Mvc.Rendering;
using SpaceWatch.Core.Models;

namespace SpaceWatch.Core.Contracts
{
	/// <summary>
	/// Provides methods that serve MediaType business logic for Admin role
	/// </summary>
	public interface IMediaTypeService
	{
		/// <summary>
		/// Gets all active MediaTypes from database.
		/// </summary>
		/// <returns>IEnumerable of type MediaTypeViewModel</returns>
		Task<IEnumerable<MediaTypeViewModel>> GetAll();

		/// <summary>
		/// Gets collection of media types. Only primary properties: Id and Title.
		/// Represents an item in SelectList.
		/// </summary>
		/// <returns>IEnumerable of type SelectListItem</returns>
		Task<IEnumerable<SelectListItem>> GetMediaTypesForSelectList();

		/// <summary>
		/// Adds new MediaType to database. Method uses object of type MediaTypeViewModel.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task Add(MediaTypeViewModel model);

		/// <summary>
		///  Checks (by its id) whether active MediaType exists in database or not.
		/// </summary>
		/// <param name="mediaTypeId"></param>
		/// <returns>true or false</returns>
		Task<bool> MediaTypeExists(int mediaTypeId);

		/// <summary>
		/// Gets details about specific active MediaType by its id.  
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Object of type MediaTypeViewModel</returns>
		Task<MediaTypeViewModel> MediaTypeDetailsById(int id);

		/// <summary>
		/// Updates MediaType by its id and MediaTypeViewModel
		/// </summary>
		/// <param name="mediaTypeId"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		Task Edit(int mediaTypeId, MediaTypeViewModel model);

		/// <summary>
		/// "Deletes" MediaType by setting IsActive property to "false". Method does not really remove the record from database.
		/// </summary>
		/// <param name="mediaTypeId"></param>
		/// <returns></returns>
		Task Delete(int mediaTypeId);
    }
}
