using SpaceWatch.Infrastructure.Data.Entities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SpaceWatch.Comparers
{
    public class CompareCategories : IEqualityComparer<Category>
    {
        public bool Equals(Category x, Category y)
        {
            if (x == null) return false;
            if (x.Id == y.Id) return true;
            return false;
        }

        public int GetHashCode([DisallowNull] Category obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
