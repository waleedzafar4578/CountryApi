using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CountryApi.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    
    public int Id { get; set; }
    [Column(TypeName = "varchar(255)")]
    public string Name { get; set; }=string.Empty;
    [Column(TypeName = "varchar(255)")]
    public string Email { get; set; }=string.Empty;
    [Column(TypeName = "varchar(255)")]
    public string Password{ get; set; }=string.Empty;
    public bool IsEmailConfirmed { get; set; } = false;
    public string ConfirmationToken { get; set; }
    public void SetUserValues(string name,string email,string password,string confirmationToken)
    {
        Name = name;
        Email = email;
        Password = password;
        ConfirmationToken = confirmationToken;
    }
}