using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaluationApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace EvaluationApi.Controllers
{
    [Route("api/poi/")]
    [ApiController]
    //[Authorize] // Decomment this to restrict only for connected user
    public class PointOfInterestItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PointOfInterestItemsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all POIs
        /// </summary>
        /// <returns>All POIs from the database</returns>
        /// <remarks>This function allows to get all POIs from the database</remarks>
        // GET: api/PointOfInterestItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestItem>>> GetPointOfInterestsItems()
        {
            return await _context.PointOfInterestsItems.ToListAsync();
        }

        /// <summary>
        /// Get a POI by ID
        /// </summary>
        /// <param name="id">The ID associated to the POI</param>
        /// <returns>Returns the POI associated to the ID</returns>
        /// <remarks>This function allows to get a POI via its ID</remarks>
        // GET: api/PointOfInterestItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PointOfInterestItem>> GetPointOfInterestItem(long id)
        {
            var pointOfInterestItem = await _context.PointOfInterestsItems.FindAsync(id);

            if (pointOfInterestItem == null)
            {
                return NotFound();
            }

            return pointOfInterestItem;
        }

        /// <summary>
        /// Update a POI by ID
        /// </summary>
        /// <param name="id">The ID associated to the POI</param>
        /// <returns>HTTP code</returns>
        /// <remarks>This function allows to update a POI via its ID</remarks>
        // PUT: api/PointOfInterestItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPointOfInterestItem(long id, PointOfInterestItem pointOfInterestItem)
        {
            if (id != pointOfInterestItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(pointOfInterestItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PointOfInterestItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Create a POIs
        /// </summary>
        /// <returns>HTTP code</returns>
        /// <remarks>This function allows to create a POI with its parameters</remarks>
        // POST: api/PointOfInterestItems
        [HttpPost]
        public async Task<ActionResult<PointOfInterestItem>> PostPointOfInterestItem(PointOfInterestItem pointOfInterestItem)
        {
            _context.PointOfInterestsItems.Add(pointOfInterestItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPointOfInterestItem", new { id = pointOfInterestItem.Id }, pointOfInterestItem);
        }

        /// <summary>
        /// Delete a POI by ID
        /// </summary>
        /// <param name="id">The ID associated to the POI</param>
        /// <returns>HTTP code</returns>
        /// <remarks>This function allows to delete a POI via its ID</remarks>
        // DELETE: api/PointOfInterestItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PointOfInterestItem>> DeletePointOfInterestItem(long id)
        {
            var pointOfInterestItem = await _context.PointOfInterestsItems.FindAsync(id);
            if (pointOfInterestItem == null)
            {
                return NotFound();
            }

            _context.PointOfInterestsItems.Remove(pointOfInterestItem);
            await _context.SaveChangesAsync();

            return pointOfInterestItem;
        }

        private bool PointOfInterestItemExists(long id)
        {
            return _context.PointOfInterestsItems.Any(e => e.Id == id);
        }
    }
}
