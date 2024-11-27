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

var api = builder.AddProject<Projects.KnowYourToolset_BackEnd_Api>("knowyourtoolset-backend-api");
#if POSTGRESQL || MSSQL
api.WaitFor(db)
    .WithReference(db);
#endif

builder.Build().Run();
