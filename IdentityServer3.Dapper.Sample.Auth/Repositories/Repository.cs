using System;
using System.Data;

namespace IdentityServer3.Dapper.Sample.Auth.Repositories
{
    public abstract class Repository
    {
        protected readonly Func<IDbConnection> OpenConnection;

        protected Repository(Func<IDbConnection> openConnection)
        {
            OpenConnection = openConnection;
        }
    }
}