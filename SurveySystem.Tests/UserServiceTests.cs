using Microsoft.AspNetCore.Identity;
using Moq;
using SurveySystem.API.Services;
using SurveySystem.Models.Models;

namespace SurveySystem.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task CreateUserAsync_Should_Create_User_With_Valid_Data()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        var userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var userService = new UserService(userManagerMock.Object);

        var user = await userService.CreateUserAsync("username", "email@example.com", "password123", "Full Name");

        Assert.NotNull(user);
        Assert.Equal("username", user.UserName);
        Assert.Equal("email@example.com", user.Email);
    }

    [Fact]
    public async Task CreateUserAsync_Should_Throw_If_Creation_Fails()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        var userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

        var userService = new UserService(userManagerMock.Object);

        var exception = await Assert.ThrowsAsync<Exception>(async () =>
            await userService.CreateUserAsync("username", "email@example.com", "password123", "Full Name"));

        Assert.Contains("Failed to create user", exception.Message);
    }

    [Fact]
    public async Task GetUserByIdAsync_Should_Return_User()
    {
        // Arrange
        var userStoreMock = new Mock<IUserStore<User>>();
        var userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        var testUser = new User { Id = Guid.NewGuid().ToString(), UserName = "username" };

        userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(testUser);

        var userService = new UserService(userManagerMock.Object);

        // Act
        var result = await userService.GetUserByIdAsync(testUser.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testUser.Id, result.Id);
    }

    [Fact]
    public async Task GetAllUsersAsync_Should_Return_All_Users()
    {
        // Arrange
        var userStoreMock = new Mock<IUserStore<User>>();
        var userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        var users = new List<User> { new User { UserName = "User1" }, new User { UserName = "User2" } };

        userManagerMock.Setup(um => um.Users)
            .Returns(users.AsQueryable());

        var userService = new UserService(userManagerMock.Object);

        // Act
        var result = await userService.GetAllUsersAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, u => u.UserName == "User1");
        Assert.Contains(result, u => u.UserName == "User2");
    }
}