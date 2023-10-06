using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Helpers;

/// <summary>
/// Helper for JSON serialization and deserialization
/// </summary>
public static class JsonHelper
{
    /// <summary>
    /// Serialize object to file
    /// </summary>
    /// <param name="obj">Object to serialize</param>
    /// <param name="filePath">Path to file</param>
    public static async Task SerializeToFileAsync(object obj, string filePath)
    {
        var json = await Task.Run(() =>
            JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(filePath, json);
    }

    /// <summary>
    /// Deserialize file to <typeparamref name="TEntity"/>
    /// </summary>
    /// <param name="filePath">Path to file</param>
    /// <typeparam name="TEntity">Type to deserialize to</typeparam>
    /// <returns>Deserialized file</returns>
    public static async ValueTask<TEntity> DeserializeFileAsync<TEntity>(string filePath)
    {
        await using var file = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<TEntity>(file,
            new JsonSerializerOptions { AllowTrailingCommas = true });
    }

    /// <summary>
    /// Validates JSON text
    /// </summary>
    /// <param name="jsonString">JSON string</param>
    /// <typeparam name="TEntity">Type to deserialize to</typeparam>
    /// <returns>null if JSON is correct, error message if not</returns>
    public static async Task<string> ValidateJson<TEntity>(string jsonString)
    {
        try
        {
            await Task.Run(() =>
                JsonSerializer.Deserialize<TEntity>(jsonString,
                    new JsonSerializerOptions { AllowTrailingCommas = true }));
            return null;
        }
        catch (JsonException e)
        {
            return e.Message;
        }
    }
}