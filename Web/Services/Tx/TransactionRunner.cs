using System;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Bloggie.Web.Services.Tx;

[AutoRegisterService]
public sealed class TransactionRunner(
    NpgsqlDataSource dataSource,
    ILogger<TransactionRunner> logger
)
{
    /// <summary>
    /// execute action in transaction
    /// </summary>
    /// <param name="action"></param>
    /// <param name="cancellationToken"></param>
    public async Task<T> ExecuteAsync<T>(
        Func<DbSession, CancellationToken, Task<T>> action,
        CancellationToken cancellationToken
    )
    {
        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var tx = await conn.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await action(new DbSession(conn, tx), cancellationToken);
            await tx.CommitAsync(cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected Error has occurred.");
            await tx.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
