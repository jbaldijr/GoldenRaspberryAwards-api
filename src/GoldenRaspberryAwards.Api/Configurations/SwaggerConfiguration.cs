using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace GoldenRaspberryAwards.Api.Configurations
{
    public class SwaggerConfiguration
    {
        public static void ConfigureSwaggerGen(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Golden Raspberry Awards API",
                Version = "v1",
                Description = "API para processar arquivos CSV de filmes e obter informações sobre os produtores com os menores e maiores intervalos entre premiações.",
                Contact = new OpenApiContact
                {
                    Name = "João Carlos Baldi Júnior",
                    Email = "joao.baldi@icloud.com",
                    Url = new Uri("https://github.com/jbaldijr"),
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT"),
                }
            });

            // Configuração para upload de arquivos
            options.MapType<IFormFile>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "binary"
            });
            options.OperationFilter<FileUploadOperationFilter>();

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        }

        public static void ConfigureSwaggerUI(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Golden Raspberry Awards API");
        }
    }

    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasFileParameter = context.MethodInfo.GetParameters()
                .Any(p => p.ParameterType == typeof(IFormFile));

            if (hasFileParameter)
            {
                operation.Parameters.Clear();
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties =
                                    {
                                        ["file"] = new OpenApiSchema
                                        {
                                            Type = "string",
                                            Format = "binary"
                                        }
                                    },
                                Required = new HashSet<string> { "file" }
                            }
                        }
                    }
                };
            }
        }
    }
}
