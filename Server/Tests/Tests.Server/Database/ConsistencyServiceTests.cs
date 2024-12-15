using Core.Database.Consistency;
using Core.Database.Interfaces;
using Core.Database.Models.Account;
using Core.Database.Models.Player;
using Moq;
using Xunit;

namespace Tests.Server.Database;

public class ConsistencyServiceTests
{
    private readonly Mock<IRepository<AccountModel>> _accountRepositoryMock;
    private readonly Mock<IRepository<PlayerModel>> _playerRepositoryMock;
    private readonly AccountValidator<AccountModel> _accountSyntaxValidator;
    private readonly PlayerValidator<PlayerModel> _playerSyntaxValidator;

    public ConsistencyServiceTests()
    {
        _accountRepositoryMock = new Mock<IRepository<AccountModel>>();
        _playerRepositoryMock = new Mock<IRepository<PlayerModel>>();

        _accountSyntaxValidator = new AccountValidator<AccountModel>(_accountRepositoryMock.Object);
        _playerSyntaxValidator = new PlayerValidator<PlayerModel>(_playerRepositoryMock.Object);
    }

    [Fact]
    public async Task AccountSyntaxValidator_ShouldReturnError_WhenUsernameIsInvalid()
    {
        // Arrange
        var account = new AccountModel
        {
            Username = "Invalid@Name",
            Password = "ValidPass123",
            Email = "valid.email@example.com",
            BirthDate = new DateOnly(2000, 1, 1)
        };

        // Act
        var result = await _accountSyntaxValidator.ValidateAsync(account);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("Username"));
    }

    [Fact]
    public async Task AccountSyntaxValidator_ShouldReturnError_WhenPasswordIsTooShort()
    {
        // Arrange
        var account = new AccountModel
        {
            Username = "ValidName",
            Password = "123",
            Email = "valid.email@example.com",
            BirthDate = new DateOnly(2000, 1, 1)
        };

        // Act
        var result = await _accountSyntaxValidator.ValidateAsync(account);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("Password"));
    }

    [Fact]
    public async Task AccountDataValidator_ShouldReturnError_WhenUsernameAlreadyExists()
    {
        // Arrange
        var account = new AccountModel
        {
            Username = "ExistingName",
            Password = "ValidPass123",
            Email = "valid.email@example.com",
            BirthDate = new DateOnly(2000, 1, 1)
        };

        _accountRepositoryMock.Setup(r => r.ExistEntityAsync(a => a.Username == account.Username))
            .ReturnsAsync(true);

        // Act
        var result = await _accountSyntaxValidator.ValidateAsync(account);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task PlayerSyntaxValidator_ShouldReturnError_WhenStatsAreNull()
    {
        // Arrange
        var player = new PlayerModel
        {
            Name = "ValidPlayer",
            Stats = null,
            Vitals = new Vitals(),
            Position = new Position()
        };

        // Act
        var result = await _playerSyntaxValidator.ValidateAsync(player);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("Stats"));
    }

    [Fact]
    public async Task PlayerDataValidator_ShouldReturnError_WhenNameAlreadyExists()
    {
        // Arrange
        var player = new PlayerModel
        {
            Name = "ExistingPlayer",
            Stats = new Stats(),
            Vitals = new Vitals(),
            Position = new Position()
        };

        _playerRepositoryMock.Setup(r => r.ExistEntityAsync(p => p.Name == player.Name))
            .ReturnsAsync(true);

        // Act
        var result = await _playerSyntaxValidator.ValidateAsync(player);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task AccountSyntaxValidator_ShouldPass_WhenAllFieldsAreValid()
    {
        // Arrange
        var account = new AccountModel
        {
            Username = "ValidName",
            Password = "ValidPass123",
            Email = "valid.email@example.com",
            BirthDate = new DateOnly(2000, 1, 1)
        };

        // Act
        var result = await _accountSyntaxValidator.ValidateAsync(account);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task PlayerSyntaxValidator_ShouldPass_WhenAllFieldsAreValid()
    {
        // Arrange
        var player = new PlayerModel
        {
            Name = "ValidPlayer",
            Stats = new Stats(),
            Vitals = new Vitals(),
            Position = new Position()
        };

        // Act
        var result = await _playerSyntaxValidator.ValidateAsync(player);

        // Assert
        Assert.True(result.IsValid);
    }
}