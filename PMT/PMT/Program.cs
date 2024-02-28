using Microsoft.EntityFrameworkCore;
using PMT.Data.Models;
using PMT.Data;
using PMT.Data.RepoInterfaces;
using PMT.Services.Email;
using PMT.Services;
using Microsoft.AspNetCore.Identity;
using PMT.Services.ProjectMetrics;

var builder = WebApplication.CreateBuilder(args);

// =========================================== Add services to the container ===========================================
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("AppDbContextConnection")));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(Str.Cookie).AddCookie(Str.Cookie, options =>
{
	options.Cookie.Name = Str.Cookie;
	options.LoginPath = "/Account/Login";
	options.ExpireTimeSpan = TimeSpan.FromDays(1);
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
  options.Cookie.IsEssential = true;
});

builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireUppercase = true;
	options.Password.RequiredLength = 8;
	options.Password.RequiredUniqueChars = 0;
});

// Repositories
// PMT Landmark
builder.Services.AddTransient<ISRSRepo, SRSRepo>();
builder.Services.AddTransient<IColorPaletteRepo, ColorPaletteRepo>();
builder.Services.AddTransient<IFileStructureRepo, FileStructureRepo>();
builder.Services.AddTransient<IModelPlanningRepo, ModelPlanningRepo>();
builder.Services.AddTransient<ITechStackRepo, TechStackRepo>();
builder.Services.AddTransient<IBugReportRepo, BugReportRepo>();
builder.Services.AddTransient<IStoryRepo, StoryRepo>();
builder.Services.AddTransient<IProjectRepo, ProjectRepo>();
builder.Services.AddTransient<IAppUserRepo, AppUserRepo>();
builder.Services.AddTransient<IProject_AppUserRepo, Project_AppUserRepo>();

// Misc. Services
builder.Services.AddTransient<IMyEmailSender, MyEmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

// =========================================== Middleware for the Pipeline ===========================================

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Project}/{action=MyProjects}/{id?}");

// SignalR Hubs
app.MapHub<RazorToJsHub>("projectDash");

app.Run();
