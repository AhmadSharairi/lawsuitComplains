
using System.Globalization;
using System.Net;
using System.Text;
using API.Helpers;
using AutoMapper;
using Core.Interfaces;
using Core.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stripe.Terminal;


var builder = WebApplication.CreateBuilder(args);

//This config make Applied in diffrent Domain from Api_url and Angular_url when call in the Backend
builder.Services.AddCors(option =>
{
    option.AddPolicy("MyPolicy", builder =>
                   builder.AllowAnyOrigin().
                    AllowAnyMethod().
                    AllowAnyHeader());

});
    
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAutoMapper(typeof(MappingProfiles)); // to mapp Dtos

builder.Services.AddScoped<IComplaintRepository , ComplaintRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDemandRepository, DemandRepository>();
builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddScoped<ILocalizationService, LocalizationService>();




builder.Services.AddDbContext<ComplaintContext>(options =>
{
        options.UseSqlServer(builder.Configuration.GetConnectionString("LawsuitConnection"));

});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en-US"),
        new CultureInfo("ar-EG")
    };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});





// Config JWT after added to Usercontroller 
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("veryverysecret......")),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew= TimeSpan.Zero, // make specific time and detemine because the time in here at least 5min
     };
});

builder.Services.AddLocalization(options => options.ResourcesPath = "API");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseCors("MyPolicy"); // First , UseCors Must Be Above of the Authentications methods Call 
app.UseRequestLocalization();
app.UseAuthentication(); // Second ,UseAuthentication must be above of Authorizations Method
app.UseAuthorization(); //  Third , last  Use Authorization

app.MapControllers();

app.Run();
