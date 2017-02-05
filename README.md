# IdentityServer3.Dapper
IdentityServer3.Dapper is extention for IdentityServer v3 configuration data that uses dapper-dot-net as it's database abstraction. The library is written in C#.

For more information on identity server 3, visit the [IdentityServer3](https://github.com/IdentityServer/IdentityServer3)

## Installing 
In most cases, you're going to want to install the IdentityServer3.Dapper NuGet package.  Open up the Package Manager Console in Visual Studio, and run this command:

```
PM> Install-Package IdentityServer3.Dapper
```

# How to Use

```C#
// DB setup (https://github.com/ranga543/IdentityServer3.Dapper/blob/master/Sql/all.sql)
// Please refer to sample project
// Add following code under Configuration method of Startup.cs in your project
var dapperServiceOptions = new DapperServiceOptions(() => new SqlConnection(ConfigurationManager.ConnectionStrings["YourDBConnectionStringName"].ConnectionString));
// Register dapper services
var factory = new IdentityServerServiceFactory();
factory.RegisterConfigurationServices(dapperServiceOptions);
factory.RegisterOperationalServices(dapperServiceOptions);
// After that can assign factory instance to identity server options instance of factory property
```
## Building the Source
If you want to build the source, clone the repository, and open up IdentityServer3.Dapper.sln.  

```
git clone https://github.com/ranga543/IdentityServer3.Dapper.git
explorer IdentityServer3.Dapper\IdentityServer3.Dapper.sln
```
## Questions?
Feel free to submit an issue on the repository.
