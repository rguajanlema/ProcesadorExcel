using System.Text;
using Microsoft.EntityFrameworkCore;
using ProcesadorExcel.Infrastructure.Persistence;
using ProcesadorExcel.Application.Interfaces;
using ProcesadorExcel.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Enable code page provider required by ExcelDataReader
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Add services to the container.
builder.Services.AddControllers();

// Configuration: SQL Server
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connection));

// DI registrations
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IExcelValidator, ExcelValidator>();
builder.Services.AddScoped<ExcelProcessorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
