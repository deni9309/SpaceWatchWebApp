using Microsoft.AspNetCore.Mvc.Rendering;
using SpaceWatch.Core.Extensions.Interfaces;

namespace SpaceWatch.Core.Extensions
{
	public static class ConvertExtensions
	{
		/// <summary>
		/// Generic method. Converts an IEnumerable<T> to new List of type SelectListItem.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="selectedValue"></param>
		/// <returns></returns>
		public static List<SelectListItem> ConvertToSelectList<T>(this IEnumerable<T> collection, int selectedValue) where T : IPrimaryProperties
		{
			return (from item in collection
					select new SelectListItem()
					{
						Text = item.Title,
						Value = item.Id.ToString(),
						Selected = item.Id == selectedValue
					})
					.ToList();
		}
	}
}
