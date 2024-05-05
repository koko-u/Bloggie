using Bloggie.Db.Data;
using Bloggie.Repo;
using Bloggie.Repo.Profiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add Repositories
builder.Services.AddScoped<IBlogPostsRepository, BlogPostsRepository>();

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BloggieDbContext>(opt => { opt.UseSqlServer(connectionString); });

// Add AutoMapper profiles
builder.Services.AddAutoMapper(config => { config.AddProfile<BlogPostProfile>(); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
