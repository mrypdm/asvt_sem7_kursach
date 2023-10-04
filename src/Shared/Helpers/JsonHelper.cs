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
        await using var file = File.OpenWrite(filePath);
        await JsonSerializer.SerializeAsync(file, obj, new JsonSerializerOptions { WriteIndented = true });
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
        return await JsonSerializer.DeserializeAsync<TEntity>(file);
    }
}