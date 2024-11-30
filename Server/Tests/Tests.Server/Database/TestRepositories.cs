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

namespace Tests.Server.Database;

public class TestRepositories
{
    private ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Configuração do banco de dados em memória
        services.AddDbContext<IDbContext, DatabaseContext>(options =>
            options.UseInMemoryDatabase("TestDatabase"));

        // Registro de repositórios, serviços e dependências
        services.AddScoped<IRepository<AccountModel>, Repository<AccountModel>>();
        services.AddScoped<IRepository<PlayerModel>, Repository<PlayerModel>>();
        services.AddScoped<IDatabaseService, DatabaseService>();
        services.AddScoped<IAccountRepository<AccountModel>, AccountRepository<AccountModel>>();
        services.AddScoped<IPlayerRepository<PlayerModel>, PlayerRepository<PlayerModel>>();
        services.AddScoped<ICrypto, Cryptography>();
        services.AddLogging();

        return services.BuildServiceProvider();
    }

    [Fact]
    public async Task DatabaseService_Should_Be_Resolvable()
    {
        // Arrange
        var serviceProvider = ConfigureServices();

        // Act
        var databaseService = serviceProvider.GetService<IDatabaseService>();

        // Assert
        Assert.NotNull(databaseService);
    }

    [Fact]
    public async Task AccountRepository_Should_Add_And_Retrieve_Account()
    {
        // Arrange
        var serviceProvider = ConfigureServices();
        var accountRepository = serviceProvider.GetService<IAccountRepository<AccountModel>>();

        Assert.NotNull(accountRepository);

        var account = new AccountModel
        {
            Username = "testuser",
            Password = "password",
            Email = "testuser@example.com",
            BirthDate = "01/01/2000"
        };

        // Act: Adiciona uma conta
        await accountRepository.AddAccountAsync(account);

        // Debug log
        Debug.Print(account.Password);

        // Act: Recupera a conta
        var retrievedAccount = await accountRepository.GetAccountAsync("testuser", "password");

        // Assert
        retrievedAccount.Item2.Should().NotBeNull();
        retrievedAccount.Item2.Username.Should().Be("testuser");
    }

    [Fact]
    public async Task AccountRepository_Should_Update_Account_With_Player()
    {
        // Arrange
        var serviceProvider = ConfigureServices();
        var accountRepository = serviceProvider.GetService<IAccountRepository<AccountModel>>();
        var playerRepository = serviceProvider.GetService<IPlayerRepository<PlayerModel>>();

        Assert.NotNull(accountRepository);
        Assert.NotNull(playerRepository);

        var account = new AccountModel
        {
            Username = "testuser",
            Password = "password",
            Email = "testuser@example.com",
            BirthDate = "01/01/2000"
        };

        await accountRepository.AddAccountAsync(account);
        var retrievedAccount = await accountRepository.GetAccountAsync("testuser", "password");
        var accountEntity = retrievedAccount.Item2;

        Assert.NotNull(accountEntity);

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

        accountEntity.Players.Add(player);

        // Act: Atualiza a conta com um jogador
        var updateResult = await accountRepository.UpdateAccountAsync(accountEntity);

        // Assert
        updateResult.IsError.Should().BeFalse();
        updateResult.Message.Should().Be("Account updated");

        // Act: Recupera a conta novamente
        var updatedAccount = await accountRepository.GetAccountAsync("testuser", "password");

        // Assert
        updatedAccount.Item2?.Players.Should().HaveCount(1, "Player count should be 1.");

        // Act: Recupera os jogadores associados à conta
        var players = await playerRepository.GetPlayersAsync(updatedAccount.Item2.Id);

        // Assert
        players.Item1.IsError.Should().BeFalse();
        players.Item2.Should().HaveCount(1, "Player count should be 1.");
        var retrievedPlayer = players.Item2.First();
        retrievedPlayer.Position.Should().NotBeNull();
        retrievedPlayer.Vitals.Should().NotBeNull();
        retrievedPlayer.Stats.Should().NotBeNull();
    }
}
