var builder = DistributedApplication.CreateBuilder(args);

#if POSTGRESQL
var db = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .AddDatabase("DbConn", "knowyourtoolset");
#endif
#if MSSQL
var db = builder.AddSqlServer("sql")
    .AddDatabase("DbConn", "knowyourtoolset");
#endif

var api = builder.AddProject<Projects.KnowYourToolset_BackEnd_Api>("knowyourtoolset-backend-api")
    .WithHttpsHealthCheck("/health");

#if POSTGRESQL || MSSQL
api.WaitFor(db)
    .WithReference(db);
#endif

#if ANGULAR
var ui = builder.AddProject<Projects.KnowYourToolset_BackEnd_Bff>("ui-with-bff")
    .WaitFor(api)
    .WithEnvironment("RemoteApis__api", api.GetEndpoint("https"));
#endif

#if ANGULAR && (POSTGRESQL || MSSQL)
ui.WithReference(db);

#endif
builder.Build().Run();