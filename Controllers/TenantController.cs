using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyUI.Data;
using PropertyUI.Models;

namespace PropertyUI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;
        public TenantController(ApplicationDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        //GetTenants() //- Retrieves a list of all tenants along with their associated properties. 
        // If no tenants are found, it returns a 204 No Content. Otherwise, 
        // it returns a 200 OK with a list of tenants and their related property details.

        [HttpGet]
        public async Task<IActionResult> GetTenants()
        {
            var tenantsList = await this._appDbContext.Tenants.Include(t => t.MyProperty).ToListAsync();

            if (tenantsList.Count > 0)
            {
                return Ok(tenantsList);
            }
            return NoContent();
        }

        // CreateTenant([FromBody] Tenant tenant) //- Adds a new tenant to the database. 
        // If the PropertyId is not provided, it returns a 400 Bad Request with an error message. 
        // Upon successful creation, it returns a 201 Created with the location of the newly created tenant.
        // [HttpPost]
        // public async Task<IActionResult> CreateTenant([FromBody] Tenant tenant)
        // {
        //     if (!tenant.PropertyId.HasValue)
        //     {
        //         return BadRequest("Tenant Cannot be added without Property ID");
        //     }

        //     this._appDbContext.Tenants.Add(tenant);
        //     await _appDbContext.SaveChangesAsync();
        //     return CreatedAtAction(nameof(GetTenants), new { id = tenant.TenantId }, tenant); //it returns a 201 Created with the location of the newly created tenant.
        // }
        [HttpPost("WithProperty")]
        public async Task<IActionResult> CreateTenantWithProperty([FromBody] Tenant tenant)
        {
            if (tenant.MyProperty != null)
            {
                _appDbContext.Properties.Add(tenant.MyProperty);
                await _appDbContext.SaveChangesAsync();
                tenant.PropertyId = tenant.MyProperty.PropertyId;
            }

            _appDbContext.Tenants.Add(tenant);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTenants), new { id = tenant.TenantId }, tenant);
        }


        // UpdateTenant(int id, [FromBody] Tenant tenant) //- Updates an existing tenant identified by id. 
        // If the provided id does not match the TenantId in the request body or if the tenant does not exist,
        //  404 Not Found, respectively. On successful update, it returns 204 No Content.

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTenant(int id, [FromBody] Tenant tenant)
        {
            if (id != tenant.TenantId)
            {
                return BadRequest("Provided Id does not match with Id given in Tenant Body");
            }
            var tenantToUpdate = await this._appDbContext.Tenants.FindAsync(id);
            if (tenantToUpdate == null)
            {
                return NotFound("This Tenant ID Does not exist!");
            }
            tenantToUpdate.Name = tenant.Name;
            tenantToUpdate.Email = tenant.Email;
            tenantToUpdate.PropertyId = tenant.PropertyId;
            await this._appDbContext.SaveChangesAsync();
            return NoContent();
        }


    }
}