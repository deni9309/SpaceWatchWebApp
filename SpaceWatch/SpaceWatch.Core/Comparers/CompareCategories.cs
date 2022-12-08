using SpaceWatch.Core.Models;
using System.Diagnostics.CodeAnalysis;
#nullable disable
namespace SpaceWatch.Core.Comparers
{
	public class CompareCategories : IEqualityComparer<CategoryViewModel>
	{
		public bool Equals(CategoryViewModel x, CategoryViewModel y)
		{
			if (x == null) return false;
			if (x.Id == y.Id) return true;
			return false;
		}

		public int GetHashCode([DisallowNull] CategoryViewModel obj)
		{
			return obj.Id.GetHashCode();
		}
	}
}
