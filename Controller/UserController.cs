using CountryApi.Data;
using CountryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CountryApi.Controller;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    public UserController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpPost("/create")]
    public IActionResult Index(CountryApi.HelpingModels.User user)
    {
        User user1 = new User();
        IActionResult answer = Helping.Helping.CheckName(user.Name);       
        if (answer is not OkResult)
        {
            return answer;       
        }
        answer = Helping.Helping.CheckPassword(user.Password);
        if (answer is not OkResult)
        {
            return answer;      
        }
        answer = Helping.Helping.CheckEmail(user.Email);
        if (answer is not OkResult)
        {
            return answer;      
        }
        var hashPassword=Helping.Helping.HashPassword(user.Password);
        string token = Helping.Helping.GenerateConfirmationToken();
        user1.SetUserValues(user.Name,user.Email,hashPassword,token);
        try
        {
            var existuser= _context.Users.FirstOrDefault(x => x.Email == user.Email); 
            if (existuser != null)
                return BadRequest("Email  already exists.");
            _context.Users.Add(user1);
            _context.SaveChanges();
            // Create confirmation link
            string confirmationLink = Helping.Helping.CreateConfirmationLink(token, "https://yourapp.com");
            Helping.Helping.SendEmail(user.Email, "Email Confirmation", 
                $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.");
            // return Created("User Created", user);
            return Ok("User registered. Please check your email to confirm your account.");
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine("========================================");
            // Console.WriteLine(e.InnerException);
            Console.WriteLine(e.InnerException?.Message);
            Console.WriteLine("========================================");
            if (e.InnerException?.Message.Contains("UNIQUE constraints failed") == true)
            {
                return BadRequest("Email  already exists.");
            }
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
       
    }
    [HttpGet("confirm-email")]
    public IActionResult ConfirmEmail(string token)
    {
        var user = _context.Users.FirstOrDefault(u => u.ConfirmationToken == token);
        if (user == null)
        {
            return BadRequest("Invalid or expired token.");
        }

        user.IsEmailConfirmed = true;
        _context.SaveChanges();

        return Ok("Email confirmed successfully!");
    }

    [HttpPost("Login")]
    public IActionResult Login(HelpingModels.UserLogin user)
    {
        var user1 = _context.Users.FirstOrDefault(x => x.Email == user.Email);
        if (user1 == null)
            return BadRequest("Invalid email .");
        if(!user1.IsEmailConfirmed)
            return BadRequest("Please confirm your email.");
        if(user1.Password !=Helping.Helping.HashPassword(user.Password))
            return BadRequest("Wrong password.");
        string token = Helping.Helping.GenerateConfirmationToken();
        user1.ConfirmationToken = token;
        _context.SaveChanges();
        return Ok(new
        {
            token,Message="Login Successful"
        });
    }
    [HttpGet("get-user")]
    public IActionResult GetUser(string token)
    {
        var user = _context.Users.FirstOrDefault(u => u.ConfirmationToken == token);
        if (user == null)
        {
            return BadRequest("Invalid or expired token.");
        }
        return Ok(user);
    }
    [HttpPost("Logout")]
    public IActionResult Logout(string token)
    {
        var user = _context.Users.FirstOrDefault(u => u.ConfirmationToken == token);
        if (user == null)
        {
            return BadRequest("Invalid or expired token.");
        }
        user.ConfirmationToken="-";
        _context.SaveChanges();
        return Ok("Logout Successful");
    }

    [HttpPut("Update")]
    public IActionResult Update(CountryApi.HelpingModels.User user, string token)
    {
        var user1 = _context.Users.FirstOrDefault(x => x.ConfirmationToken == token);
        if (user1 == null)
        {
            return BadRequest("Invalid or expired token.");
        }
        var answer = Helping.Helping.CheckName(user.Name);
        if (answer is not OkResult)
        {
            return answer;
        }
        answer = Helping.Helping.CheckPassword(user.Password);
        if (answer is not OkResult)
        {
            return answer;
        }
        answer = Helping.Helping.CheckEmail(user.Email);
        if (answer is not OkResult)
        {
            return answer;
        }
        user1.SetUserValues(user.Name, user.Email,Helping.Helping.HashPassword(user.Password),user1.ConfirmationToken);
        _context.SaveChanges();
        return Ok(user1);
    }
    [HttpDelete("remove")]
    public IActionResult Remove(string token)
    {
        var user = _context.Users.FirstOrDefault(u => u.ConfirmationToken == token);
        if (user == null)
        {
            return BadRequest("Invalid or expired token.");
        }
        _context.Users.Remove(user);
        _context.SaveChanges();
        return Ok("User Deleted");
    }
}

