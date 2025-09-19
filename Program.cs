using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using BuggyApp.Data;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var dbFullPath = Path.Combine(builder.Environment.ContentRootPath, "invoices.db");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbFullPath}"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseCors();

var contentRootProvider = new PhysicalFileProvider(app.Environment.ContentRootPath);
app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = contentRootProvider });
app.UseStaticFiles(new StaticFileOptions { FileProvider = contentRootProvider });

// Enable Swagger UI in all environments for convenience
app.UseSwagger();
app.UseSwaggerUI();

var dbPath = dbFullPath;
using (var connection = new SqliteConnection($"Data Source={dbPath}"))
{
    connection.Open();
    bool needsInit = !File.Exists(dbPath);
    if (!needsInit)
    {
        using var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='Invoices'";
        var result = checkCmd.ExecuteScalar();
        needsInit = result == null || result == DBNull.Value;
    }
    if (needsInit)
    {
        var initSqlPath = Path.Combine(builder.Environment.ContentRootPath, "init.sql");
        if (File.Exists(initSqlPath))
        {
            var sql = File.ReadAllText(initSqlPath, Encoding.UTF8);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
    }
    connection.Close();
}

app.MapControllers();

app.Run();



