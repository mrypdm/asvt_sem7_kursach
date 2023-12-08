using Shared.Helpers;

namespace Shared.Tests.Helpers;

public class ConfigurationHelperTests
{
    private class Options
    {
        public string FontFamily { get; set; }
        
        public int FontSize { get; set; }
    }
    
    [Test]
    public void GetOptionsByTypeTest()
    {
        // Arrange
        
        var configuration = ConfigurationHelper.BuildFromJson("./Jsons/5.json");
        
        // Act

        var options = configuration.GetOptions<Options>();
        
        // Assert
        
        Assert.Multiple(() =>
        {
            Assert.That(options.FontFamily, Is.EqualTo("Font"));
            Assert.That(options.FontSize, Is.EqualTo(24));
        });
    }
    
    [Test]
    public void GetOptionsByNameTest()
    {
        // Arrange
        
        var configuration = ConfigurationHelper.BuildFromJson("./Jsons/6.json");
        
        // Act

        var options = configuration.GetOptions<Options>();
        var anotherOptions = configuration.GetOptions<Options>("AnotherOptions");
        
        // Assert
        
        Assert.Multiple(() =>
        {
            Assert.That(anotherOptions.FontFamily, Is.EqualTo("Font"));
            Assert.That(anotherOptions.FontSize, Is.EqualTo(24));
            Assert.That(options, Is.EqualTo(default(Options)));
        });
    }
    
    [Test]
    public void GetUnExistingOptionsTest()
    {
        // Arrange
        
        var configuration = ConfigurationHelper.BuildFromJson("./Jsons/5.json");
        
        // Act

        var options = configuration.GetOptions<Options>("InvalidOptions");
        
        // Assert
        
        Assert.That(options, Is.EqualTo(default(Options)));
    }
}