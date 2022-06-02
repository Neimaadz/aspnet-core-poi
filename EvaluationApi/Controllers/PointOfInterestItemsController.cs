using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaluationApi.Models;

namespace EvaluationApi.Controllers
{
    [Route("api/poi")]
    [ApiController]
    public class PointOfInterestItemsController : ControllerBase
    {
        private readonly IPointOfInterestsRepository _repository;

        public PointOfInterestItemsController(IPointOfInterestsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/PointOfInterestItems
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestItem>> GetPointOfInterestsItems()
        {
            return _repository.GetAll();
        }

        // TODO
        // GET: api/PointOfInterestItems/5
        // [HttpGet("{id}")]
        /* 
        public async Task<ActionResult<PointOfInterestItem>> GetPointOfInterestItem(long id)
        {
            var pointOfInterestItem = await _context.PointOfInterestsItems.FindAsync(id);

            if (pointOfInterestItem == null)
            {
                return NotFound();
            }

            return pointOfInterestItem;
        }*/

        // TODO
        // PUT: api/PointOfInterestItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /*[HttpPut("{id}")]
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
        }*/

        // TODO
        // POST: api/PointOfInterestItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /*[HttpPost]
        public async Task<ActionResult<PointOfInterestItem>> PostPointOfInterestItem(PointOfInterestItem pointOfInterestItem)
        {
            _context.PointOfInterestsItems.Add(pointOfInterestItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPointOfInterestItem", new { id = pointOfInterestItem.Id }, pointOfInterestItem);
        }*/

        // TODO
        // DELETE: api/PointOfInterestItems/5
        /*[HttpDelete("{id}")]
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
        }*/

        // TODO
        /*private bool PointOfInterestItemExists(long id)
        {
            return _context.PointOfInterestsItems.Any(e => e.Id == id);
        }*/
    }
}
