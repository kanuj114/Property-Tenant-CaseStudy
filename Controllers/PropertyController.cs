using PropertyUI.Data;
using PropertyUI.Exceptions;
using PropertyUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PropertyUI.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        List<string> allowedAddress = new List<string> { "New York", "San Francisco" };
        private readonly ApplicationDbContext _appDbContext;
        public PropertyController(ApplicationDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        //GetProperty(int id) - Retrieves a single property by their PropertyId along with their associated tenants. 
        // If the property is not found, it returns a 404 Not Found. 
        // If found, it returns a 200 OK with the property details and their related tenants.

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProperty(int id)
        {
            var propertyDetails = await _appDbContext.Properties.Include(p => p.Tenants).SingleOrDefaultAsync(p => p.PropertyId == id);
            if (!(propertyDetails == null))
            {
                return Ok(propertyDetails);
            }
            return NotFound();

        }
        //CreateProperty([FromBody] Property propertyModel) - Adds a new propertyModel to the database. 
        // Upon successful creation, it returns a 201 Created with the address of the newly created propertyModel. 
        // A PropertyAddressException is thrown for invalid addresses with 500 status code.

        // [HttpPost]
        // public async Task<IActionResult> CreateProperty([FromBody] Property propertyModel)
        // {
        //     if (!allowedAddress.Contains(propertyModel.Address))
        //     {
        //         throw new PropertyAddressException(propertyModel.Address);
        //     }
        //     _appDbContext.Properties.Add(propertyModel);
        //     await _appDbContext.SaveChangesAsync();
        //     return CreatedAtAction(nameof(GetProperty), new { id = propertyModel.PropertyId }, propertyModel);

        // }

        [HttpPost]
public async Task<IActionResult> CreateProperty([FromBody] Property propertyModel)
{
    try
    {
        if (!allowedAddress.Contains(propertyModel.Address))
        {
            throw new PropertyAddressException(propertyModel.Address);
        }

        _appDbContext.Properties.Add(propertyModel);
        await _appDbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProperty), new { id = propertyModel.PropertyId }, propertyModel);
    }
    catch (PropertyAddressException ex)
    {
        // Only return the message — no stack trace
        return BadRequest(new { message = ex.Message });
    }
    catch (Exception)
    {
        // Optional: Generic error for other exceptions
        return StatusCode(500, new { message = "Something went wrong on the server." });
    }
}


        //GetPropertiesSortedByPriceDesc() - Retrieves all properties sorted by rental price in descending order. 
        // 200 OK is returned with a sorted list of properties and their tenants.

        [HttpGet]
        public async Task<IActionResult> GetPropertiesSortedByPriceDesc()
        {
            var sortedProperties = await _appDbContext.Properties.Include(p => p.Tenants).OrderByDescending(p => p.RentalPrice).ToListAsync();
            if (sortedProperties.Count > 0)
                return Ok(sortedProperties);
            return NoContent();
        }
    }
}