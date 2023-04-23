using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = false,
        ValidateIssuer=false,
        ValidateAudience=false,
        ValidateLifetime=false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("particlesmoke111111111111111111111111"))
    };
        
    
});

builder.Services.AddAuthorization(options =>
{
    AuthorizationPolicy defaultPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
    options.DefaultPolicy = defaultPolicy;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.Use(async (context, next) =>
{
    var user = context.User;
    if (user?.Identity?.IsAuthenticated ?? false)
    {
        foreach (var claim in user.Claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type} - Claim Value : {claim.Value}");
        }
    }
    await next();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
