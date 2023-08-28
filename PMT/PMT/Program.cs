using Microsoft.EntityFrameworkCore;
using PMT.Data.Models;
using PMT.Data;
using PMT.Data.RepoInterfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using PMT.Services.Email;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("AppDbContextConnection")));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddTransient<ISRSRepo, SRSRepo>();
builder.Services.AddTransient<IColorPaletteRepo, ColorPaletteRepo>();
builder.Services.AddTransient<IFileStructureRepo, FileStructureRepo>();
builder.Services.AddTransient<IModelPlanningRepo, ModelPlanningRepo>();
builder.Services.AddTransient<ITechStackRepo, TechStackRepo>();
builder.Services.AddTransient<IBugReportRepo, BugReportRepo>();
builder.Services.AddTransient<IStoryRepo, StoryRepo>();
builder.Services.AddTransient<IProjectRepo, ProjectRepo>();
builder.Services.AddTransient<IMyEmailSender, MyEmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Project}/{action=MyProjects}/{id?}");

app.Run();
