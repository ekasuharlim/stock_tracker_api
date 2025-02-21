using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions; // Add this for NullLogger
using StockTrackingApi.Controllers;
using StockTrackingApi.Data;
using StockTrackingApi.Models;
using StockTrackingApi.Dtos.Users;
using System.Collections.Generic;
using System.Linq;

public class UsersControllerTests
{
    private readonly UsersController _controller;
    private readonly StockTrackingDbContext _dbContext;

    public UsersControllerTests()
    {
        // Use InMemoryDatabase for testing
        var options = new DbContextOptionsBuilder<StockTrackingDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var logger = NullLogger<StockTrackingDbContext>.Instance;

        _dbContext = new StockTrackingDbContext(options, logger);
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        // Seed data
        _dbContext.Users.AddRange(new List<User>
        {
            new User { UserID = 1, Username = "Alice123", Email = "alice@example.com", PasswordHash = "hashedpassword1" },
            new User { UserID = 2, Username = "Bob456", Email = "bob@example.com", PasswordHash = "hashedpassword2" }
        });
        _dbContext.SaveChanges();

        _controller = new UsersController(_dbContext);
    }

    [Fact]
    public void GetUsers_ReturnsOkResult_WithListOfUsers()
    {
        // Act
        var result = _controller.GetUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsers = Assert.IsType<List<User>>(okResult.Value);
        Assert.Equal(2, returnedUsers.Count);
    }

    [Fact]
    public void CreateUser_ReturnsCreatedAtActionResult_AndAddsUser()
    {
        // Arrange
        var newUser = new CreateUserDto
        {
            Username = "Charlie789",
            Email = "charlie@example.com",
        };

        // Act
        var result = _controller.CreateUser(newUser);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedUser = Assert.IsType<User>(createdAtActionResult.Value);
        Assert.Equal("Charlie789", returnedUser.Username);
        Assert.Equal("charlie@example.com", returnedUser.Email);
        
        // Verify that the user was actually added to the database
        Assert.Equal(3, _dbContext.Users.Count());
        Assert.Contains(_dbContext.Users, u => u.Username == "Charlie789");
    }    
}
