using GW.Application.Repository;
using GW.Application.Sevices;
using GW.Core.Context;
using Microsoft.EntityFrameworkCore;

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
app.Run();


