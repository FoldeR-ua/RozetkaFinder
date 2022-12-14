using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RozetkaFinder.Helpers;
using RozetkaFinder.Helpers.NotificationCreateHelper;
using RozetkaFinder.Models.GoodObjects;
using RozetkaFinder.Models.Mapping;
using RozetkaFinder.Models.User;
using RozetkaFinder.Repository;
using RozetkaFinder.Services.GoodsServices;
using RozetkaFinder.Services.JSONServices;
using RozetkaFinder.Services.MarkdownServices;
using RozetkaFinder.Services.MonitoringService;
using RozetkaFinder.Services.Notification;
using RozetkaFinder.Services.PasswordServices;
using RozetkaFinder.Services.Security.JwtToken;
using RozetkaFinder.Services.Security.RefreshToken;
using RozetkaFinder.Services.TelegramServices;
using RozetkaFinder.Services.UserServices;
using RozetkaFinder.Services.ValidationServices;
using Swashbuckle.AspNetCore.Filters;
using System.Text;


var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddDbContext<ApplicationContext>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standart Authorization header using the Bearer Scheme ",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddScoped<INotificationCreator, NotificationCreator>();

builder.Services.AddSingleton<ITelegramService, TelegramService>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IIdHelper, IdHelper>();

builder.Services.AddScoped<TelegramNotificationService>();
builder.Services.AddScoped<EmailNotificationService>();

builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<SubscribtionGood>, Repository<SubscribtionGood>>();
builder.Services.AddScoped<IRepository<SubscriptionMarkdown>, Repository<SubscriptionMarkdown>>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGoodsService, GoodsService>();
builder.Services.AddScoped<IMarkdownService, MarkdownService>();

builder.Services.AddScoped<IJsonService, JsonService>();
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddHostedService<MonitoringBackgroundService>();


var app = builder.Build();


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
