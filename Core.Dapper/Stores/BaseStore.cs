using System;

namespace IdentityServer3.Dapper
{
    public abstract class BaseStore
    {
        protected readonly DapperServiceOptions Options;
        protected BaseStore(DapperServiceOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            Options = options;
        }
    }
}