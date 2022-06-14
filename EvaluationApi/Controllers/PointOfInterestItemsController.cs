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
        // GET: api/PointOfInterestItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestItem>>> GetPointOfInterestsItems()
        {
            return await _context.PointOfInterestsItems.ToListAsync();
        }

        /// <summary>
        /// This the summary
        /// </summary>
        /// <param name="id">This is a param Id</param>
        /// <returns>This the returned</returns>
        /// <remarks>This is a remarks</remarks>
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

        // PUT: api/PointOfInterestItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
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

        // POST: api/PointOfInterestItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PointOfInterestItem>> PostPointOfInterestItem(PointOfInterestItem pointOfInterestItem)
        {
            _context.PointOfInterestsItems.Add(pointOfInterestItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPointOfInterestItem", new { id = pointOfInterestItem.Id }, pointOfInterestItem);
        }

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
