using System;
using System.Data;

namespace IdentityServer3.Dapper
{
    public class DapperServiceOptions
    {
        public Func<IDbConnection> OpenConnection;
        public DapperServiceOptions(Func<IDbConnection> openConnection)
        {
            if (openConnection == null) throw new ArgumentNullException(nameof(openConnection));
            OpenConnection = openConnection;
        }
        /// <summary>
        /// Currently not in use
        /// </summary>
        public string Schema { get; set; }
    }
}