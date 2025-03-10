using Microsoft.AspNetCore.Authentication;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Okta.AspNetCore;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Configuration;
using Portal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
})
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
           .AddCookie()
           .AddOktaMvc(new OktaMvcOptions
           {
               // Replace these values with your Okta configuration
               OktaDomain = builder.Configuration.GetValue<string>("Okta:OktaDomain"),
               AuthorizationServerId = builder.Configuration.GetValue<string>("Okta:AuthorizationServerId"),
               ClientId = builder.Configuration.GetValue<string>("Okta:ClientId"),
               ClientSecret = builder.Configuration.GetValue<string>("Okta:ClientSecret"),
               GetClaimsFromUserInfoEndpoint = true,
               //Scope = new List<string> { "openid", "profile", "email" },
               Scope = new List<string> { "openid", "profile", "email", "offline_access", "groups" }
           });

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseOktaRefreshToken();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
