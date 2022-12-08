using SpaceWatch.Core.Models;
using System.Diagnostics.CodeAnalysis;
#nullable disable

namespace SpaceWatch.Core.Comparers
{
    public class CompareUsers : IEqualityComparer<UserModel>
    {
        public bool Equals(UserModel x, UserModel y)
        {
            if (y == null) return false;

            if (x.Id == y.Id) return true;

            return false;
        }

        public int GetHashCode([DisallowNull] UserModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
