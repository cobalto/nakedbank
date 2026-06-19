using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GrpcService = NakedBank.Api.GrpcServices;
using System.Text;
using Microsoft.OpenApi;

namespace NakedBank.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var jwtSecret = builder.Configuration["AuthSettings:Secret"];
            if (string.IsNullOrWhiteSpace(jwtSecret))
            {
                throw new InvalidOperationException(
                    "AuthSettings:Secret is not configured. Set it via User Secrets, environment variables, or appsettings.Development.json.");
            }

            // Configure JWT authentication
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                //x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "NakedIssuer",
                    ValidAudience = "NakedAudience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                };
            });

            builder.Services.AddGrpc().AddJsonTranscoding();
            builder.Services.AddGrpcReflection();

            builder.Services.AddGrpcSwagger();
            builder.Services.AddSwaggerGen(gen =>
            {
                gen.SwaggerDoc("v2",
                    new OpenApiInfo
                    {
                        Title = "NakedBank Service",
                        Version = "v2",
                        Description = "NakedBank OpenAPI",
                        Contact = new OpenApiContact
                        {
                            Name = "Daniel Pessoa",
                            Url = new System.Uri("https://cobalto.dev")
                        }
                    });

                gen.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. <br/>
                      Enter 'Bearer' [space] and then your token in the text input below.
                      <br/>Example: 'Bearer 12345abcdef'",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                gen.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });

                var filePath = Path.Combine(AppContext.BaseDirectory, $"{typeof(Program).Namespace}.xml");

                gen.IncludeXmlComments(filePath);
                gen.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "NakedBank Service v2");
            });

            app.UseAuthentication();
            //app.UseAuthorization();

            app.MapGrpcService<GrpcService.GreeterService>();
            app.MapGrpcService<GrpcService.v2.AccountService>();
            app.MapGrpcService<GrpcService.v2.UserService>();
            app.MapGrpcReflectionService();

            app.Run();
        }
    }
}