using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaluationApi.Models;
using EvaluationApi.Services;

namespace EvaluationApi.Controllers
{
    [Route("api/trip/")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private TripService _service;


        public TripsController(TripService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all Trips
        /// </summary>
        /// <returns>a List of all Trips</returns>
        /// <remarks>This function call the database to get all trips</remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trip>>> GetTrip()
        {
            return Ok(await _service.GetTrips());
        }

        /// <summary>
        /// Get all Trips defined by origin and destination
        /// </summary>
        /// <param name="origin">The origin</param>
        /// <param name="destination">The destination</param>
        /// <returns>a List of all Trips filtered by origin and destination</returns>
        /// <remarks>This function call the database to get all trips</remarks>
        [HttpGet("origin-destination")]
        public async Task<ActionResult<IEnumerable<Trip>>> GetTripOriginDestination([FromQuery(Name = "origin")] string origin, [FromQuery(Name = "destination")] string destination)
        {
            Console.WriteLine(origin + destination);
            return Ok(await _service.GetTripOriginDestination(origin, destination));
        }

        /// <summary>
        /// Get a trip by id
        /// </summary>
        /// <param name="id">The id of trip</param>
        /// <returns>a signle trip by id else http.code 400</returns>
        /// <remarks>This function call the database to get one trip by id</remarks>
        [HttpGet("{id}")]
        public async Task<ActionResult<Trip>> GetTrip(long id)
        {
            var trip = await _service.GetTrip(id);

            if (trip == null)
            {
                return NotFound();
            }

            return trip;
        }

        /// <summary>
        /// Update a trip by id
        /// </summary>
        /// <param name="id">The id of trip</param>
        /// <param name="tripDTO">The payload to update</param>
        /// <returns>http.code 200 else http.code 403</returns>
        /// <remarks>This function update the trip</remarks>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrip([FromRoute] long id, [FromForm] TripDTO tripDTO)
        {
            long currentUserId = long.Parse(User.FindFirst("id").Value);

            try
            {
                Trip trip = await _service.GetUserTrip(id, currentUserId);

                return Ok(await _service.PutTrip(id, tripDTO, trip, currentUserId));
            }
            catch
            {
                return Forbid();
            }
        }

        /// <summary>
        /// Post a new trip
        /// </summary>
        /// <param name="tripDTO">The payload to update</param>
        /// <returns>The trip added</returns>
        /// <remarks>This function add a trip</remarks>
        [HttpPost]
        public async Task<ActionResult<Trip>> PostTrip([FromForm] TripDTO tripDTO)
        {
            Console.WriteLine(tripDTO.PoisId);
            long currentUserId = long.Parse(User.FindFirst("id").Value);
            Trip trip = await _service.PostTrip(tripDTO, currentUserId);

            return CreatedAtAction("GetTrip", new { id = trip.Id }, trip);
        }

        /// <summary>
        /// delete a trip by id
        /// </summary>
        /// <param name="id">The id of trip to delete</param>
        /// <returns>http.code 200 with trip info deleted else http.code 403</returns>
        /// <remarks>This function delete a trip</remarks>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Trip>> DeleteTrip(long id)
        {
            long currentUserId = long.Parse(User.FindFirst("id").Value);
            Trip trip;
            try
            {
                trip = await _service.GetUserTrip(id, currentUserId);
                trip = await _service.DeleteTrip(trip, currentUserId);

                return Ok(trip);
            }
            catch
            {
                return Forbid();
            }
        }

    }
}
