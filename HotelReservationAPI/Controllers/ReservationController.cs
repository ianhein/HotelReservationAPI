﻿using HotelReservationAPI.Context;
using HotelReservationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public ReservationsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/reservations/available - Buscar habitaciones disponibles para un rango de fechas
        [AllowAnonymous]
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Room>>> GetAvailableRooms(DateTime startDate, DateTime endDate)
        {
            if (startDate < DateTime.Now || (endDate - startDate).Days > 30)
                return BadRequest("Rango de fechas no válido.");

            var availableRooms = await _context.Rooms
                .Where(r => !_context.Reservations
                    .Where(res => res.StartDate < endDate && res.EndDate > startDate)
                    .Select(res => res.RoomId)
                    .Contains(r.Id))
                .ToListAsync();

            return Ok(availableRooms);
        }

        // POST: api/reservations - Crear una nueva reserva (solo clientes)
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> MakeReservation(Reservation reservation)
        {
            if (reservation.StartDate < DateTime.Now)
                return BadRequest("No se puede reservar en una fecha pasada.");

            if ((reservation.EndDate - reservation.StartDate).Days > 30)
                return BadRequest("Las reservas no pueden durar más de 30 días.");

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }

        // GET: api/reservations/{id} - Obtener detalles de una reserva específica (disponible para el usuario autenticado)
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }

        // DELETE: api/reservations/{id} - Cancelar una reserva (solo clientes)
        [Authorize(Roles = "Customer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
