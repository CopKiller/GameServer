using Core.Database.Models.Account;
using Core.Database.Models.Player;
using Core.Logger.Interface;
using Core.Server.Database.Interface;
using Core.Server.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Core.Tests.Database;

public class TestRepositories
{
    private ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Registro de repositórios, serviços e dependências

        services.AddDatabase(useInMemory: true);
        services.AddServerDatabase();
        services.AddCryptography();
        services.AddLogger(LogLevel.Debug);
        services.AddSingleton<ILogOutput, LoggerOutput>();

        return services.BuildServiceProvider();
    }

    [Fact]
    public void DatabaseService_Should_Be_Resolvable()
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
            BirthDate = new DateOnly(2000, 1, 1)
        };

        // Act: Adiciona uma conta
        var result = await accountRepository.AddAccountAsync(account);

        // Assert
        result.Item1.IsValid.Should().BeTrue();
        result.Item2.Should().NotBeNull();
        result.Item2.Password.Should().NotBe("password", "Password should be hashed.");

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
            Username = "testuser1",
            Password = "password1",
            Email = "testuser1@example.com",
            BirthDate = new DateOnly(2000, 1, 1)
        };

        await accountRepository.AddAccountAsync(account);
        var retrievedAccount = await accountRepository.GetAccountAsync("testuser1", "password1");
        var accountEntity = retrievedAccount.Item2;

        Assert.NotNull(accountEntity);
        accountEntity.Players.Should().HaveCount(0, "Player count should be 0.");

        var player = new PlayerModel
        {
            Name = "testplayer1",
            Level = 1,
            Experience = 0,
            Golds = 0,
            Vitals = new Vitals(),
            Position = new Position(),
            Stats = new Stats()
        };

        accountEntity.Players.Add(player);

        // Act: Atualiza a conta com um jogador
        var updateResult = await accountRepository.UpdateAccountAsync(accountEntity);

        // Assert
        updateResult.IsValid.Should().BeTrue();

        // Act: Recupera a conta novamente
        var updatedAccount = await accountRepository.GetAccountAsync("testuser1", "password1");

        // Assert
        updatedAccount.Item2?.Players.Should().HaveCount(1, "Player count should be 1.");

        updatedAccount.Item2.Should().NotBeNull("Account should not be null.");

        // Act: Recupera os jogadores associados à conta
        var players = await playerRepository.GetPlayersAsync(updatedAccount.Item2.Id);

        // Assert
        players.Item1.IsValid.Should().BeTrue();
        players.Item2.Should().HaveCount(1, "Player count should be 1.");
        var retrievedPlayer = players.Item2.First();
        retrievedPlayer.Position.Should().NotBeNull();
        retrievedPlayer.Vitals.Should().NotBeNull();
        retrievedPlayer.Stats.Should().NotBeNull();
    }
}