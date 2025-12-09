using GW.Application.Repository;
using GW.Application.Sevices;
using GW.Core.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



//builder.Services.AddOpenApi();
builder.Services.AddDbContext<SupervisorContext>(opt =>
{
    opt.EnableSensitiveDataLogging();
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SupervisorConnection"));

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
RegisterServices.Configure(builder.Services);

#region Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:key"])),
    };

    //// Add these for SignalR support
    //options.Events = new JwtBearerEvents
    //{
    //    OnMessageReceived = context =>
    //    {
    //        var accessToken = context.Request.Query["access_token"];
    //        var path = context.HttpContext.Request.Path;
    //        if (!string.IsNullOrEmpty(accessToken) &&
    //            path.StartsWithSegments("/deviceHub"))
    //        {
    //            context.Token = accessToken;
    //        }
    //        return Task.CompletedTask;
    //    }
    //};
});


builder.Services.AddAuthorization();

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.Run();


