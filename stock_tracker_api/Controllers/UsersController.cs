namespace StockTrackingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using StockTrackingApi.Data;
using StockTrackingApi.Models;
using StockTrackingApi.Dtos.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly StockTrackingDbContext _context;
    public UsersController(StockTrackingDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(_context.Users.ToList());
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserDto userDto)
    {
        if (userDto == null) return BadRequest("Invalid user data.");

        // ✅ Create a new User object from the DTO
        var user = new User
        {
            Username = userDto.Username,
            Email = userDto.Email,
            PasswordHash = "newPasswordHash"
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetUsers), new { id = user.UserID }, user);
    }
    
}
