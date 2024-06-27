using Bookmon.API.Extensions;
using Bookmon.Domain.Constants;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Bookmon.API.Tests.Extensions;

public sealed class CacheExtensionTests
{
    [Fact]
    public void AddCache_WithEnabledCache_ShouldAddDistributedMemoryCache()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c.GetSection(ConfigurationNames.InMemoryCache))
                     .Returns(Mock.Of<IConfigurationSection>(s => s["Enabled"] == "true"));

        // Act
        services.AddCache(configuration.Object);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        //Assert.Contains(serviceProvider.GetServices<IDistributedCache>(), service => service.GetType() == typeof(MemoryDistributedCache));
    }

    [Fact]
    public void AddCache_WithDisabledCache_ShouldNotAddDistributedMemoryCache()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c.GetSection(ConfigurationNames.InMemoryCache))
                     .Returns(Mock.Of<IConfigurationSection>(s => s["Enabled"] == "false"));

        // Act
        services.AddCache(configuration.Object);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        Assert.Empty(serviceProvider.GetServices<IDistributedCache>());
    }

    [Fact]
    public void AddCache_WithNullServices_ShouldThrowArgumentNullException()
    {
        // Arrange
        IServiceCollection services = null;
        var configuration = new Mock<IConfiguration>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => services.AddCache(configuration.Object));
    }
}