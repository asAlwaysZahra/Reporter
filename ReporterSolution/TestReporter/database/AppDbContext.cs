using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace TestReporter.database;

public class AppDbContext : DbContext
{
    public AppDbContext() : base()
    {

    }

    public DbSet<Log> LogSet { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ReporterLogsDB;Trusted_Connection=True;Encrypt=false;");
    }
}