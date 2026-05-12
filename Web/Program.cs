using System;
using System.Globalization;
using AutoRegisterAnnotation;
using Bloggie.Web.ServiceCollectionExtensions;
using FluentValidation;
using KozLibraries.DapperDateOnlySupport;
using KozLibraries.JsonMessages;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Dapper DateOnly Handler
Dapper.SqlMapper.AddTypeHandler(new DateOnlyHandler());
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// Add services to the container.
builder.Services.AddRazorPages();

// Add NpgsqlDataSource
builder.Services.AddNpgsqlDataSource(builder.Configuration);

// Add Auto Register Services
builder.Services.AddAutoRegisterServices<Program>(
    (srvType, lifetime) =>
    {
        Console.WriteLine($"Registering service: {srvType.Name} with lifetime: {lifetime}");
    }
);

// Configure Json Message Resources
builder.Services.ConfigureRequestLocalization(() => [new CultureInfo("en"), new CultureInfo("ja")]);

// Add Fluent Validations
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
