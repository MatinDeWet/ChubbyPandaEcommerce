global using ChubbyPandaEcommerce.Shared;
using ChubbyPandaEcommerce.Server.Data;
using ChubbyPandaEcommerce.Server.Services.ProductService;
using ChubbyPandaEcommerce.Server.Services.CategoryService;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//add context
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConntetion"));
});

//add swager ui
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add services
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//add swager ui
app.UseSwaggerUI();
app.UseSwagger();


app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
