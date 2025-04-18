using Microsoft.AspNetCore.Mvc;
using CountryApi.Models;
using CountryApi.Data;
namespace CountryApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly AppDbContext _context;
    public CountryController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var countries = _context.Countries.ToList();
        return Ok(countries);
    }

    [HttpPost]
    public IActionResult Create(Country country)
    {
        _context.Countries.Add(country);
        _context.SaveChanges();
        return Ok();
    }
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var country = _context.Countries.Find(id);
        if (country == null) return NotFound();
        return Ok(country);
    }
    [HttpPut("{id}")]
    public IActionResult Update(int id, Country country)
    {
        var countryToUpdate = _context.Countries.Find(id);
        if (countryToUpdate == null) return NotFound();
        
        countryToUpdate.Name = country.Name;
        _context.SaveChanges();
        return Ok(countryToUpdate);
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var countryToDelete = _context.Countries.Find(id);
        if (countryToDelete == null) return NotFound();
        
        _context.Countries.Remove(countryToDelete);
        _context.SaveChanges();
        return Ok("Deleted");
    }
}