using Npgsql;

namespace Bloggie.Web.Services.Tx;

public sealed record DbSession(NpgsqlConnection Connection, NpgsqlTransaction Transaction);
