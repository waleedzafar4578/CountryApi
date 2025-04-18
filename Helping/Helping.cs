using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;

using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
namespace CountryApi.Helping;

public static class Helping
{
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        byte[] hash= sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        string hashedString = BitConverter.ToString(hash).Replace("-", "");
        return hashedString;
    }

    public static IActionResult CheckName(string name)
    {
        if (name.Length < 3 || name.Length > 20)
            return new BadRequestObjectResult("Name must be between 3 and 20 characters long");
        if(!name.All(Char.IsLetter))
            return new BadRequestObjectResult("Name must contain only letters");
        return new OkResult();
    }
    
    public static IActionResult CheckPassword(string password)
    {
        // Check password length
        if (password.Length < 8 || password.Length > 15)
            return new BadRequestObjectResult("Password must be between 8 and 15 characters long.");
        
        if (!password.Any(char.IsUpper))
            return new BadRequestObjectResult("Password must contain at least one uppercase letter.");
        
        if (!password.Any(char.IsLetterOrDigit)) 
            return new BadRequestObjectResult("Password must contain at least one special character.");

        
        return new OkResult(); 
    }

    public static IActionResult CheckEmail(string email)
    {
        // Regular expression pattern for validating email addresses
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (!Regex.IsMatch(email, emailPattern))
            return new BadRequestObjectResult("Invalid email address.");

        // Email is valid
        return new OkResult();
    }
    
    public static void SendEmail(string recipientEmail, string subject, string body)
    {
        try
        {
            // Configure the SMTP client
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587, // TLS
                Credentials = new NetworkCredential("dailywater737@gmail.com", "jcxp zdjd xddl renn "),
                EnableSsl = true // Secure connection
            };

            // Create the email
            var mailMessage = new MailMessage
            {
                From = new MailAddress("dailywater737@gmail.com"),
                Subject = subject,  
                Body = body,
                IsBodyHtml = true // Enable HTML content
            };

            // Add recipient
            mailMessage.To.Add(recipientEmail);

            // Send the email
            smtpClient.Send(mailMessage);

            Console.WriteLine("Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    public static string GenerateConfirmationToken()
    {
        return Guid.NewGuid().ToString(); // Generate a unique token
    }

    public static string CreateConfirmationLink(string token, string baseUrl)
    {
        return $"http://localhost:5141/api/confirm-email?token={token}";
    }
    
}