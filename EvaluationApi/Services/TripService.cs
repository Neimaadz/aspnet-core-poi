using EvaluationApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationApi.Services
{
    public class TripService
    {
        private readonly AppDbContext _context;

        public TripService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trip>> GetTrips()
        {
            IEnumerable<Trip> trips = await _context.Trips.ToListAsync();
            IEnumerable<PointOfInterestItem> pois = await _context.PointOfInterestsItems.ToListAsync();

            foreach (var trip in trips)
            {
                List<PointOfInterestItem> poisTrip = new List<PointOfInterestItem>();
                foreach (var poi in pois)
                {
                    if (trip.Id == poi.TripId)
                    {
                        poisTrip.Add(poi);
                    }
                }
                trip.Pois = poisTrip;
            }
            return trips;
        }

        public async Task<IEnumerable<Trip>> GetTripOriginDestination( string origin, string destination)
        {
            IEnumerable<Trip> trips = await _context.Trips.Where(trip => trip.Origin == origin && trip.Destination == destination).ToListAsync();
            IEnumerable<PointOfInterestItem> pois = await _context.PointOfInterestsItems.ToListAsync();

            foreach (var trip in trips)
            {
                List<PointOfInterestItem> poisTrip = new List<PointOfInterestItem>();
                foreach (var poi in pois)
                {
                    if (trip.Id == poi.TripId)
                    {
                        poisTrip.Add(poi);
                    }
                }
                trip.Pois = poisTrip;
            }
            return trips;
        }

        public async Task<Trip> GetTrip(long id)
        {
            try
            {
                Trip trip = await _context.Trips.Where(trip => trip.Id == id).FirstAsync();
                List<PointOfInterestItem> pois = await _context.PointOfInterestsItems.Where(poi => poi.TripId == id).ToListAsync();
                trip.Pois = pois;

                return trip;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Trip> GetUserTrip(long id, long currentUserId)
        {
            try
            {
                Trip trip = await _context.Trips.Where(trip => trip.UserId == currentUserId && trip.Id == id).FirstAsync();
                List<PointOfInterestItem> pois = await _context.PointOfInterestsItems.Where(poi => poi.TripId == id).ToListAsync();
                trip.Pois = pois;

                return trip;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Trip> PostTrip(TripDTO tripDTO, long currentUserId)
        {
            Trip trip;
            List<long> poisTripIdDTO = tripDTO.PoisId;
            List<PointOfInterestItem> pois = await _context.PointOfInterestsItems.Where(poi => poi.UserId == currentUserId && poi.TripId == 0).ToListAsync();

            try
            {
                trip = TripDTOMapper(tripDTO, currentUserId);

                _context.Trips.Add(trip);
                await _context.SaveChangesAsync();

                foreach (var poiTripIdDTO in poisTripIdDTO)
                {
                    foreach (var poi in pois)
                    {
                        if (poiTripIdDTO == poi.Id)
                        {
                            poi.TripId = trip.Id;
                        }
                    }
                }
                await _context.SaveChangesAsync();

                return trip;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Trip> PutTrip(long id, TripDTO tripDTO, Trip trip, long currentUserId)
        {
            trip.Name = tripDTO.Name;
            trip.Description = tripDTO.Description;
            trip.Origin = tripDTO.Origin;
            trip.Destination = tripDTO.Destination;

            await _context.SaveChangesAsync();

            return trip;
        }

        public async Task<Trip> DeleteTrip(Trip trip, long currentUserId)
        {
            IEnumerable<PointOfInterestItem> pointOfInterestItems = await _context.PointOfInterestsItems.Where(poi => poi.UserId == currentUserId && poi.TripId == trip.Id).ToListAsync();

            foreach (var pointOfInterestItem in pointOfInterestItems)
            {
                pointOfInterestItem.TripId = 0;
            }

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            return trip;
        }






        public Trip TripDTOMapper(TripDTO tripDTO, long currentUserId)
        {
            Trip trip = new Trip();

            trip.UserId = currentUserId;
            trip.Name = tripDTO.Name;
            trip.Description = tripDTO.Description;
            trip.Origin = tripDTO.Origin;
            trip.Destination = tripDTO.Destination;

            return trip;
        }
        private bool TripExists(long id)
        {
            return _context.Trips.Any(e => e.Id == id);
        }
    }
}
