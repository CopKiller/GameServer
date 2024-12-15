using Core.Cryptography.Interface;
using Core.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.Server.Cryptography;

public class CryptographyTests
{
    private IServiceProvider ConfigureServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddCryptography();

        return services.BuildServiceProvider();
    }
    
    [Fact]
    public void Cryptography_ShouldEncryptAndDecryptData()
    {
        var serviceProvider = ConfigureServiceProvider();
        var cryptographyManager = serviceProvider.GetRequiredService<ICrypto>();

        var data = "Hello, World!";

        var encryptedData = cryptographyManager.HashString(data);
        
        Assert.NotNull(encryptedData);
        encryptedData.Should().HaveLength(60, "The encrypted data should have a fixed length.");
        
        var decryptedData = cryptographyManager.CompareHash(data,encryptedData);

        Assert.True(decryptedData);
        
        decryptedData = cryptographyManager.CompareHash("Hello, World1!",encryptedData);
        
        Assert.False(decryptedData);
    }
}