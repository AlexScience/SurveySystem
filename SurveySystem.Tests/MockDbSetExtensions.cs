using Microsoft.EntityFrameworkCore;
using Moq;

namespace SurveySystem.Tests;

public static class MockDbSetExtensions
{
    public static DbSet<T> ReturnsDbSet<T>(this Mock<DbSet<T>> dbSet, IQueryable<T> data) where T : class
    {
        dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
        dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        return dbSet.Object;
    }
}