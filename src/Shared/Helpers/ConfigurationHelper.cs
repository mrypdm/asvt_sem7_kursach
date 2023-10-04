using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Shared.Helpers;

public static class ConfigurationHelper
{
    public const string DefaultJsonFile = "appsettings.json";

    public static IConfigurationRoot BuildFromJson(string filePath = DefaultJsonFile) =>
        new ConfigurationBuilder().AddJsonFile(filePath, optional: false, reloadOnChange: true).Build();

    public static Task SaveToJson(IDictionary<string, object> values, string filePath = DefaultJsonFile) =>
        JsonHelper.SerializeToFileAsync(values, filePath);

    public static TOption GetOptions<TOption>(this IConfigurationRoot configuration, string section = null)
        where TOption : new()
    {
        var options = new TOption();
        var configurationSection = configuration.GetSection<TOption>(section);
        configurationSection?.Bind(options);
        return options;
    }

    public static IConfigurationSection GetSection<TOption>(this IConfiguration configuration, string section = null) =>
        configuration.GetSection(section ?? typeof(TOption).Name);
}