using Microsoft.EntityFrameworkCore;
using RaknaAPI.Models;
using Microsoft.AspNetCore.Identity;
using RaknaAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.CodeAnalysis.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("RaknaAPIContextConnection") ?? throw new InvalidOperationException("Connection string 'RaknaAPIContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RaknaDBContext>(options =>
options.UseSqlServer(connectionString: "defaultconnection"));
builder.Services.AddRazorPages();

/*builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<RaknaAPIContext>()
    .AddDefaultTokenProviders();*/
builder.Services.AddDbContext<RaknaAPIContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<RaknaAPIContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(Option =>
    {
        Option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            //ValidIssuer = _Configuration["jwt:Issuer"],
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"]))


        
        };
    });
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    _ = app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    _ = endpoints.MapRazorPages();
});
app.Run();
