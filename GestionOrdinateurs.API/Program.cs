using GestionOrdinateurs.API;
using GestionOrdinateurs.API.Services;
using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddScoped<IOrdinateurService, OrdinateurService>();
builder.Services.AddDbContext<ParcInformatiqueContext>();
builder.Services.AddScoped<ReparationService>();
builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<JournalService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GestionOrdinateurs",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Entrez : Bearer {token}"
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.MetadataAddress =
        "http://keycloak:8080/realms/GestionOrdinateurs/.well-known/openid-configuration";

    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            ValidateIssuer = true,
            ValidIssuer =
                "http://localhost:8180/realms/GestionOrdinateurs",

            ValidateAudience = false
        };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            Console.WriteLine(
                $"Utilisateur utilisant un Token Valide --- {context.Principal?.Identity?.Name ?? "Anonymous"}"
            );

            return Task.CompletedTask;
        },

        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Token Invalide");
            Console.WriteLine(context.Exception.Message);

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowBlazorClient");

//Initialisation de la base de données
using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ParcInformatiqueContext>();
    
    db.Database.Migrate();

    if(!db.Users.Any())
    {
        db.Users.AddRange(
            new User { Login="user1", Role=RoleEnum.Admin },
            new User { Login="user2", Role=RoleEnum.Gestionnaire },
            new User { Login="user3", Role=RoleEnum.Gestionnaire }
        );

        db.SaveChanges();
    }
}

//Config de swagger
app.UseSwagger();
app.UseSwaggerUI();
//-------------------

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();