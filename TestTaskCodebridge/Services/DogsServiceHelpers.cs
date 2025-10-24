using System.Linq.Expressions;
using TestTaskCodebridge.DataAccess.Entitites;

namespace TestTaskCodebridge.Services
{
    public static class DogsServiceHelpers
    {
        public static IOrderedQueryable<DogEntity> OrderByWithDirection(
            this IQueryable<DogEntity> q,
            Expression<Func<DogEntity, object>> keySelector,
            string direction)
        {
            return direction switch
            {
                "asc" => q.OrderBy(keySelector),
                "desc" => q.OrderByDescending(keySelector),
                _ => throw new ArgumentException("Not a valid direction to sort")
            };
        }
    }
}