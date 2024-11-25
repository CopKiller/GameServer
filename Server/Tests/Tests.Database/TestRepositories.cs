using System.Diagnostics;
using Core.Cryptography;
using Core.Cryptography.Interface;
using Core.Database;
using Core.Database.Interfaces;
using Core.Database.Models.Account;
using Core.Database.Models.Player;
using Core.Database.Repositorys;
using Core.Server.Database;
using Core.Server.Database.Interface;
using Core.Server.Database.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.Database;

public class TestRepositories
{
    [Fact]
    public async Task TestDatabaseService()
    {
        // Arrange
        var services = new ServiceCollection();

        // Configurar o banco de dados em memória
        services.AddDbContext<IDbContext, DatabaseContext>(options =>
            options.UseInMemoryDatabase("TestDatabase"));

        // Repositórios e serviços
        services.AddScoped<IRepository<AccountModel>, Repository<AccountModel>>();
        services.AddScoped<IRepository<PlayerModel>, Repository<PlayerModel>>();
        services.AddScoped<IDatabaseService, DatabaseService>();
        services.AddScoped<IAccountRepository<AccountModel>, AccountRepository<AccountModel>>();
        services.AddScoped<IPlayerRepository<PlayerModel>, PlayerRepository<PlayerModel>>();
        services.AddLogging();
        
        // crypto
        services.AddScoped<ICrypto, Cryptography>();

        var serviceProvider = services.BuildServiceProvider();

        // Act
        var databaseService = serviceProvider.GetService<IDatabaseService>();

        // Assert
        Assert.NotNull(databaseService);

        // Act
        var accountRepository = serviceProvider.GetService<IAccountRepository<AccountModel>>();

        // Assert
        Assert.NotNull(accountRepository);

        var account = new AccountModel
        {
            Username = "testuser",
            Password = "password",
            Email = "testuser@example.com",
            BirthDate = "01/01/2000"
        };

        // Act
        await accountRepository.AddAccountAsync(account);
        
        // mensagem pro debug de tests
        Debug.Print(account.Password);
        
        var retrievedAccount = await accountRepository.GetAccountAsync("testuser", "password");

        // Assert
        retrievedAccount.Item2.Should().NotBeNull();
        retrievedAccount.Item2.Username.Should().Be("testuser");

        // Act
        var playerRepository = serviceProvider.GetService<IPlayerRepository<PlayerModel>>();

        // Assert
        Assert.NotNull(playerRepository);

        // Act
        var player = new PlayerModel
        {
            Name = "testplayer",
            Level = 1,
            Experience = 0,
            Gold = 0,
            Vitals = new Vitals(),
            Position = new Position(),
            Stats = new Stats()
        };

        var lastAccount = retrievedAccount.Item2;
        lastAccount.Players.Add(player);
        var result = await accountRepository.UpdateAccountAsync(lastAccount);

        // Assert
        result.IsError.Should().BeFalse();
        result.Message.Should().Be("Account updated");

        // Act
        var accountRetrieve = await accountRepository.GetAccountAsync("testuser", "password");

        // Assert
        accountRetrieve.Item2?.Players.Should().HaveCount(1, "Player count should be 1.");
        
        // Act
        var playerRetrieve = await playerRepository.GetPlayersAsync(accountRetrieve.Item2.Id);
        
        // Assert
        playerRetrieve.Item1.IsError.Should().BeFalse();
        playerRetrieve.Item2.Should().HaveCount(1, "Player count should be 1.");
        playerRetrieve.Item2.First().Position.Should().NotBeNull();
        playerRetrieve.Item2.First().Vitals.Should().NotBeNull();
        playerRetrieve.Item2.First().Stats.Should().NotBeNull();
    }
}