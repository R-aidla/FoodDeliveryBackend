using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryBackend.Data;
using FoodDeliveryBackend.Models;

namespace FoodDeliveryBackend.Controllers
{
    /// <summary>
    /// Controls company API calls.
    /// </summary>
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController(AppDbContext context) : ControllerBase
    {
        // GET: api/companies
        /// <summary>
        /// Get the current list of companies in the database.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await context.Companies.ToListAsync();
        }
    }
}