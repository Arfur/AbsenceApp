/*
===============================================================================
 File        : AsyncQueryHelper.cs
 Namespace   : AbsenceApp.Tests.Helpers
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Provides an in-memory IQueryable<T> that supports EF Core
               async extension methods (ToListAsync, FirstOrDefaultAsync).
               Used in Moq-based service tests to mock IQueryable-returning
               repository methods such as IXxxRepository.Query().
-------------------------------------------------------------------------------
 Description :
   EF Core's ToListAsync() requires the IQueryProvider to implement
   IAsyncQueryProvider (Microsoft.EntityFrameworkCore.Query). For plain
   in-memory lists, this interface is not present, causing a runtime error.
   TestAsyncEnumerable<T> wraps an IEnumerable<T> and provides the required
   async provider so that LINQ-to-objects + EF Core async methods co-operate.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Usage: mockRepo.Setup(r => r.Query()).Returns(list.AsAsyncQueryable());
   - ExecuteAsync wraps the synchronous LINQ result in Task.FromResult so
     the await in async service methods receives the correct value.
===============================================================================
*/

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace AbsenceApp.Tests.Helpers;

// =========================================================================
// Extension — convenience method to wrap a list as an async-queryable
// =========================================================================

internal static class AsyncQueryableExtensions
{
    internal static IQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> source)
        => new TestAsyncEnumerable<T>(source);
}

// =========================================================================
// TestAsyncEnumerable<T> — wraps an in-memory sequence as IQueryable +
// IAsyncEnumerable so EF Core async extensions can operate on it
// =========================================================================

internal sealed class TestAsyncEnumerable<T>
    : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
    public TestAsyncEnumerable(Expression expression) : base(expression) { }

    // Override Provider so EF Core resolves our async-capable implementation.
    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken ct = default)
        => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
}

// =========================================================================
// TestAsyncQueryProvider<T> — forwards sync queries to EnumerableQuery and
// wraps results in Task.FromResult for EF Core async extension methods
// =========================================================================

internal sealed class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    public TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

    public IQueryable CreateQuery(Expression expression)
        => new TestAsyncEnumerable<TEntity>(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        => (IQueryable<TElement>)new TestAsyncEnumerable<TElement>(expression);

    public object? Execute(Expression expression) => _inner.Execute(expression);

    public TResult Execute<TResult>(Expression expression)
        => _inner.Execute<TResult>(expression);

    // =========================================================================
    // ExecuteAsync — called by ToListAsync / FirstOrDefaultAsync etc.
    // TResult is Task<SomeType>; evaluates sync then wraps in Task.FromResult.
    // =========================================================================
    public TResult ExecuteAsync<TResult>(
        Expression expression,
        CancellationToken cancellationToken = default)
    {
        var resultType = typeof(TResult).GetGenericArguments()[0];
        var result = _inner.Execute(expression);
        return (TResult)typeof(Task)
            .GetMethod(nameof(Task.FromResult))!
            .MakeGenericMethod(resultType)
            .Invoke(null, new[] { result })!;
    }
}

// =========================================================================
// TestAsyncEnumerator<T> — primary constructor; adapts sync IEnumerator<T>
// to IAsyncEnumerator<T> required by await foreach / ToListAsync
// =========================================================================

internal sealed class TestAsyncEnumerator<T>(IEnumerator<T> inner)
    : IAsyncEnumerator<T>
{
    public T Current => inner.Current;

    public ValueTask DisposeAsync()
    {
        inner.Dispose();
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> MoveNextAsync()
        => ValueTask.FromResult(inner.MoveNext());
}
