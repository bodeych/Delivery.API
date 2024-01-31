using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Delivery.API.ServiceCollectionExtensions;

public class SwaggerSnakeCaseFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema?.Properties != null)
        {
            var snakeCaseProperties = schema.Properties.ToDictionary(
                entry => ToSnakeCase(entry.Key), // Convert to snake_case
                entry => entry.Value);

            schema.Properties.Clear();
            foreach (var snakeCaseProperty in snakeCaseProperties)
            {
                schema.Properties.Add(snakeCaseProperty.Key, snakeCaseProperty.Value);
            }
        }
    }

    private string ToSnakeCase(string input)
    {
        // Your snake_case conversion logic
        // Example: Convert "MyProperty" to "my_property"
        return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + char.ToLower(x) : x.ToString())).ToLower();
    }
}