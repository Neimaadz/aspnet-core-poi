using EvaluationApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationApi.Services
{
    public class PointOfInterestItemService
    {
        private readonly AppDbContext _context;

        public PointOfInterestItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PointOfInterestItem>> GetPointOfInterestItems()
        {
            return await _context.PointOfInterestsItems.ToListAsync();
        }
        public async Task<IEnumerable<PointOfInterestItem>> GetUserPointOfInterestsItemsWithoutTrip(long currentUserId)
        {
            return await _context.PointOfInterestsItems.Where(poi => poi.UserId == currentUserId && poi.TripId == 0).ToListAsync();
        }
        public async Task<PointOfInterestItem> GetPointOfInterestItem(long id)
        {
            try
            {
                return await _context.PointOfInterestsItems.Where(poi => poi.Id == id).FirstAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<PointOfInterestItem> GetUserPointOfInterestItem(long id, long currentUserId)
        {
            try
            {
                return await _context.PointOfInterestsItems.Where(poi => poi.UserId == currentUserId && poi.Id == id).FirstAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<PointOfInterestItem> PutPointOfInterestItem(long id, PointOfInterestItemDTO pointOfInterestItemDTO, PointOfInterestItem pointOfInterestItem, long currentUserId)
        {
            // Delete old image
            if (pointOfInterestItemDTO.Image != null)
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), pointOfInterestItem.ImagePath);
                if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
            }

            try
            {
                string relativePathFileName = GenerateImagePath(pointOfInterestItemDTO);

                pointOfInterestItem.UserId = currentUserId;
                pointOfInterestItem.Name = pointOfInterestItemDTO.Name;
                pointOfInterestItem.ImagePath = relativePathFileName != "" ? relativePathFileName : pointOfInterestItem.ImagePath;
                pointOfInterestItem.Comment = pointOfInterestItemDTO.Comment;
                pointOfInterestItem.Lat = Double.Parse(pointOfInterestItemDTO.Lat, System.Globalization.CultureInfo.InvariantCulture);
                pointOfInterestItem.Lng = Double.Parse(pointOfInterestItemDTO.Lng, System.Globalization.CultureInfo.InvariantCulture);
                await _context.SaveChangesAsync();

                return pointOfInterestItem;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<PointOfInterestItem> PostPointOfInterestItem(PointOfInterestItemDTO pointOfInterestItemDTO, long currentUserId)
        {
            PointOfInterestItem pointOfInterestItem;

            try
            {
                pointOfInterestItem = PointOfinterestItemDTOMapper(pointOfInterestItemDTO, currentUserId);

                _context.PointOfInterestsItems.Add(pointOfInterestItem);
                await _context.SaveChangesAsync();

                return pointOfInterestItem;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<PointOfInterestItem> DeletePointOfInterestItem(PointOfInterestItem pointOfInterestItem)
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), pointOfInterestItem.ImagePath);
            if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);

            _context.PointOfInterestsItems.Remove(pointOfInterestItem);
            await _context.SaveChangesAsync();

            return pointOfInterestItem;
        }

        public PointOfInterestItem PointOfinterestItemDTOMapper(PointOfInterestItemDTO pointOfInterestItemDTO, long currentUserId)
        {
            try
            {
                string relativePathFileName = GenerateImagePath(pointOfInterestItemDTO);
                return new PointOfInterestItem(currentUserId, pointOfInterestItemDTO.Name, relativePathFileName, pointOfInterestItemDTO.Comment,
                    Double.Parse(pointOfInterestItemDTO.Lat, System.Globalization.CultureInfo.InvariantCulture),
                    Double.Parse(pointOfInterestItemDTO.Lng, System.Globalization.CultureInfo.InvariantCulture));
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static string GenerateImagePath(PointOfInterestItemDTO pointOfInterestItemDTO)
        {
            List<string> extensions = new List<string>()
            {
                ".png",
                ".PNG",
                ".jpeg",
                ".jpg",
                ".JPG",
            };

            if (pointOfInterestItemDTO.Image != null)
            {
                // Vérification de l'extension
                FileInfo fileInfo = new FileInfo(pointOfInterestItemDTO.Image.FileName);
                string fileExtension = fileInfo.Extension;
                if (!extensions.Contains(fileExtension)) { throw new Exception("file-not-right-extension"); }

                // Import new image
                string relativePath = "Ressources/Images";
                string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
                if (!Directory.Exists(absolutePath)) Directory.CreateDirectory(absolutePath);

                string fileName = Utils.RandomString(20, true) + fileExtension;

                using var stream = new FileStream(Path.Combine(absolutePath, fileName), FileMode.Create);
                pointOfInterestItemDTO.Image.CopyTo(stream);

                return relativePath + "/" + fileName;
            }
            else return "";
        }
        public bool PointOfInterestItemExists(long id)
        {
            return _context.PointOfInterestsItems.Any(e => e.Id == id);
        }
    }
}
