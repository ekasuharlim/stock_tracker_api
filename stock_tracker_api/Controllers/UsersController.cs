namespace StockTrackingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using StockTrackingApi.Data;
using StockTrackingApi.Models;

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
    public IActionResult CreateUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetUsers), new { id = user.UserID }, user);
    }
}
