using Microsoft.AspNetCore.Mvc.Rendering;
using SpaceWatch.Infrastructure.Data.Extensions.Interfaces;
namespace SpaceWatch.Infrastructure.Data.Extensions
{
    public static class ConvertExtensions
    {
		public static List<SelectListItem> ConvertToSelectList<T>(this IEnumerable<T> collection, int selectedValue) where T : IPrimaryProperties
		{
			//return collection.Select(c => new SelectListItem()
			//{
			//	Text = c.Title,
			//	Value = c.Id.ToString(),
			//	Selected = (c.Id == selectedValue)
			//})
			//	.ToList();
			return (from item in collection
					select new SelectListItem
					{
						Text = item.Title,
						Value = item.Id.ToString(),
						Selected = (item.Id == selectedValue)
					})
					.ToList();
		}
	}
}
