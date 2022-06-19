using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EvaluationApi.Models;
using Microsoft.AspNetCore.Authorization;
using EvaluationApi.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace EvaluationApi.Controllers
{
    [Route("api/poi/")]
    [ApiController]
    [Authorize] // Decomment this to restrict only for connected user
    public class PointOfInterestItemsController : ControllerBase
    {
        private PointOfInterestItemService _service;

        public PointOfInterestItemsController(PointOfInterestItemService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all POIs
        /// </summary>
        /// <returns>All POIs from the database</returns>
        /// <remarks>This function allows to get all POIs from the database</remarks>
        // GET: api/PointOfInterestItems
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PointOfInterestItem>>> GetPointOfInterestsItems()
        {
            return Ok(await _service.GetPointOfInterestItems());
        }

        /// <summary>
        /// Get a POI by ID
        /// </summary>
        /// <param name="id">The ID associated to the POI</param>
        /// <returns>Returns the POI associated to the ID</returns>
        /// <remarks>This function allows to get a POI via its ID</remarks>
        // GET: api/PointOfInterestItems/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<PointOfInterestItem>> GetPointOfInterestItem(long id)
        {
            var pointOfInterestItem = await _service.GetPointOfInterestItem(id);

            if (pointOfInterestItem == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterestItem);
        }

        /// <summary>
        /// Update a POI by ID
        /// </summary>
        /// <param name="id">The ID associated to the POI</param>
        /// <returns>HTTP code</returns>
        /// <remarks>This function allows to update a POI via its ID</remarks>
        // PUT: api/PointOfInterestItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPointOfInterestItem([FromRoute] long id, [FromForm] PointOfInterestItemDTO pointOfInterestItemDTO)
        {
            long currentUserId = long.Parse(User.FindFirst("id").Value);

            try
            {
                PointOfInterestItem pointOfInterestItem = await _service.GetUserPointOfInterestItem(id, currentUserId);

                return Ok(await _service.PutPointOfInterestItem(id, pointOfInterestItemDTO, pointOfInterestItem, currentUserId));
            }
            catch
            {
                return Forbid();
            }
        }

        /// <summary>
        /// Create a POI
        /// </summary>
        /// <returns>HTTP code</returns>
        /// <remarks>This function allows to create a POI with its parameters</remarks>
        // POST: api/PointOfInterestItems
        [HttpPost]
        public async Task<ActionResult<PointOfInterestItemDTO>> PostPointOfInterestItem([FromForm] PointOfInterestItemDTO pointOfInterestItemDTO)
        {
            long currentUserId = long.Parse(User.FindFirst("id").Value);
            PointOfInterestItem pointOfInterestItem = await _service.PostPointOfInterestItem(pointOfInterestItemDTO, currentUserId);

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
            long currentUserId = long.Parse(User.FindFirst("id").Value);
            PointOfInterestItem pointOfInterestItem;
            try
            {
                pointOfInterestItem = await _service.GetUserPointOfInterestItem(id, currentUserId);
                pointOfInterestItem = await _service.DeletePointOfInterestItem(pointOfInterestItem);

                return Ok(pointOfInterestItem);
            }
            catch
            {
                return Forbid();
            }
        }

    }
}
