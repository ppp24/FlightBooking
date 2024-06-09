using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FlightBooking.Areas.Identity.Data;
using AutoMapper;
using FlightBooking.BLL;
using System.Data;
using FlightBooking.Services;
using Twilio;
using System.Configuration;
using FlightBooking.Middleware;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<ApplicationUser, ApplicationRoles>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(Program));

//builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("TwilioSettings"));

var accountSid = builder.Configuration["Twilio:AccountSID"];
var authToken = builder.Configuration["Twilio:AuthToken"];
TwilioClient.Init(accountSid, authToken);


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequiresTwoFactor", x => x.RequireClaim("amr", "mfa"));

    options.AddPolicy("Admin", policy =>
        policy.RequireRole("Admin"));
    options.AddPolicy("Customer", policy =>
        policy.RequireRole("Customer"));
    options.AddPolicy("Manager", policy =>
        policy.RequireRole("Manager"));
});
builder.Services.Configure<TwilioVerifySettings>(builder.Configuration.GetSection("Twilio"));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<ISMSSenderService, SMSSenderService>();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var serviceProvider = scope.ServiceProvider;
//    var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRoles>>();

//    // Define the roles you want to create
//    var roles = new[] { "Admin", "Customer", "Manager" };

//    // Iterate over the roles and create them if they don't exist
//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new ApplicationRoles(role));
//        }
//    }
//}

//using (var scope = app.Services.CreateScope())
//{
//    var serviceProvider = scope.ServiceProvider;
//    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//    var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRoles>>();

//    if (!await roleManager.RoleExistsAsync(Enums.RoleName.admin.ToString()))
//    {
//        await roleManager.CreateAsync(new ApplicationRoles(Enums.RoleName.admin.ToString()));
//    }

//    if (!await roleManager.RoleExistsAsync(Enums.RoleName.customer.ToString()))
//    {
//        await roleManager.CreateAsync(new ApplicationRoles(Enums.RoleName.customer.ToString()));
//    }

//    if (!await roleManager.RoleExistsAsync(Enums.RoleName.staff.ToString()))
//    {
//        await roleManager.CreateAsync(new ApplicationRoles(Enums.RoleName.staff.ToString()));
//    }

//    var user = new ApplicationUser { UserName = "admin@example.com", Email = "admin@example.com", FirstName = "admin", LastName = "test" };

//    var result = await userManager.CreateAsync(user, "Admin@123");

//    if (result.Succeeded)
//    {
//        await userManager.AddToRoleAsync(user, Enums.RoleName.admin.ToString());
//    }
//    else
//    {
//        // Handle user creation failure
//    }
//}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();
app.UseMiddleware<AuditMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
