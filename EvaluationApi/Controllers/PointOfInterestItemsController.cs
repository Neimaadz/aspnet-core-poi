using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaluationApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace EvaluationApi.Controllers
{
    [Route("api/poi/")]
    [ApiController]
    //[Authorize] // Decomment this to restrict only for connected user
    public class PointOfInterestItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly List<string> extensions = new List<string>()
        {
            ".png",
            ".jpeg",
            ".jpg",
        };

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
        public async Task<IActionResult> PutPointOfInterestItem(long id, [FromForm] PointOfInterestItemDTO pointOfInterestItemDTO)
        {
            if (id != pointOfInterestItemDTO.Id)
            {
                return BadRequest();
            }

            // Si une image est renseignée on delete l'ancienne image sinon on garde juste l'ancienne
            PointOfInterestItem pointOfInterestItem;
            string relativePathFileName;
            var oldPointOfInterestItem = await _context.PointOfInterestsItems.FindAsync(id);
            if (pointOfInterestItemDTO.Image != null)
            {
                // Vérification de l'extension
                FileInfo fileInfo = new FileInfo(pointOfInterestItemDTO.Image.FileName);
                string fileExtension = fileInfo.Extension;
                if (!this.extensions.Contains(fileExtension)) { return BadRequest("file-not-right-extension"); }

                // Delete old image
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), oldPointOfInterestItem.ImagePath);
                if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);

                // Import new image
                string relativePath = "Ressources/Images";
                string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
                if (!Directory.Exists(absolutePath)) Directory.CreateDirectory(absolutePath);

                string fileName = Utils.RandomString(20, true) + fileExtension;

                using var stream = new FileStream(Path.Combine(absolutePath, fileName), FileMode.Create);
                pointOfInterestItemDTO.Image.CopyTo(stream);

                relativePathFileName = Path.Combine(relativePath, fileName);
            }
            else relativePathFileName = oldPointOfInterestItem.ImagePath;

            pointOfInterestItem = new PointOfInterestItem(pointOfInterestItemDTO.Name, relativePathFileName, pointOfInterestItemDTO.Comment, pointOfInterestItemDTO.Lat, pointOfInterestItemDTO.Lng);
            pointOfInterestItem.Id = pointOfInterestItemDTO.Id;

            _context.Entry(oldPointOfInterestItem).State = EntityState.Detached; // Détache l'objet qui attaché au contexte
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
        /// Create a POI
        /// </summary>
        /// <returns>HTTP code</returns>
        /// <remarks>This function allows to create a POI with its parameters</remarks>
        // POST: api/PointOfInterestItems
        [HttpPost]
        public async Task<ActionResult<PointOfInterestItemDTO>> PostPointOfInterestItem([FromForm] PointOfInterestItemDTO pointOfInterestItemDTO)
        {
            // Get file extension
            FileInfo fileInfo = new FileInfo(pointOfInterestItemDTO.Image.FileName);
            string fileExtension = fileInfo.Extension;
            if (!this.extensions.Contains(fileExtension)) { return BadRequest("file-not-right-extension"); }

            // Path and create folder if not exist
            string relativePath = "Ressources/Images";
            string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
            if (!Directory.Exists(absolutePath)) Directory.CreateDirectory(absolutePath);

            string fileName = Utils.RandomString(20, true) + fileExtension;

            using var stream = new FileStream(Path.Combine(absolutePath, fileName), FileMode.Create);
            pointOfInterestItemDTO.Image.CopyTo(stream);

            PointOfInterestItem pointOfInterestItem = new PointOfInterestItem(pointOfInterestItemDTO.Name, Path.Combine(relativePath, fileName), pointOfInterestItemDTO.Comment, pointOfInterestItemDTO.Lat, pointOfInterestItemDTO.Lng);

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
            if (pointOfInterestItem == null) return NotFound();

            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), pointOfInterestItem.ImagePath);
            if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);

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
