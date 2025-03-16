using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryBackend.Data;
using FoodDeliveryBackend.Models;

namespace FoodDeliveryBackend.Controllers
{
    /// <summary>
    /// Controls region data API calls.
    /// </summary>
    [Route("api/region")]
    [ApiController]
    public class DeliveryRegionController(AppDbContext context) : ControllerBase
    {
        // GET: api/region
        /// <summary>
        /// Get a region rule using an integer identifier.
        /// </summary>
        [SecureApi]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryRegionRule>>> GetRegionRuleById([FromQuery] int id)
        {
            var detectedRule = await context.DeliveryRegionRules.Where(d => d.Id == id).ToListAsync();

            if(detectedRule == null)
            {
                return NotFound("Requested ID not found.");
            }

            if(id == 0)
                return Ok(await context.DeliveryRegionRules.ToListAsync());
            else
                return Ok(detectedRule);
        }

        // PUT: api/region/update
        /// <summary>
        /// Update a region rule using an integer identifier inside of the DeliveryRegionRule model class.
        /// </summary>
        [SecureApi]
        [HttpPut("update")]
        public async Task<ActionResult<IEnumerable<DeliveryRegionRule>>> UpdateRegionRule([FromBody] DeliveryRegionRule rule)
        {
            var deliveryRegionRule = await context.DeliveryRegionRules.FindAsync(rule.Id);

            if (deliveryRegionRule == null)
                return NotFound("Unable to find specified Id.");

            context.Entry(deliveryRegionRule).CurrentValues.SetValues(rule);
            await context.SaveChangesAsync();

            return Ok($"Updated {deliveryRegionRule.RegionName} region rules."); 
        }

        // POST: api/region/add
        /// <summary>
        /// Add a new region rule.
        /// </summary>
        [SecureApi]
        [HttpPost("add")]
        public async Task<ActionResult<IEnumerable<DeliveryRegionRule>>> AddRegionRule([FromBody] DeliveryRegionRule rule)
        {
            if (rule == null)
                return BadRequest("Invalid region rule data.");

            var existingRule = await context.DeliveryRegionRules.FindAsync(rule.Id);

            if (existingRule != null)
                return BadRequest("Requested region rule already exists.");

            context.DeliveryRegionRules.Add(rule);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRegionRuleById), new { id = rule.Id }, rule);
        }

        // DELETE: api/region/remove
        /// <summary>
        /// Delete a region rule using an integer identifier.
        /// </summary>
        [SecureApi]
        [HttpDelete("remove")]
        public async Task<ActionResult<IEnumerable<DeliveryRegionRule>>> RemoveRegionRule([FromQuery] int id)
        {
            var rule = await context.DeliveryRegionRules.FindAsync(id);

            if (rule == null)
                return NotFound("Region rule was not found.");

            context.DeliveryRegionRules.Remove(rule);
            await context.SaveChangesAsync();

            return Ok($"Region rule {rule.RegionName} has been removed.");
        }
    }
}