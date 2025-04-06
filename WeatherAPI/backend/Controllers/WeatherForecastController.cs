using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [EnableCors("AllowVueFrontend")]
    [ApiController]
    [Route("api/weather")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherDbContext _context;

        public WeatherForecastController(WeatherDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherRecords()
        {
            return await _context.WeatherRecords.ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, WeatherForecast forecast)
        {
            if (id != forecast.Id)
                return BadRequest();

            _context.Entry(forecast).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.WeatherRecords.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.WeatherRecords.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("delete-multiple")]
        public async Task<IActionResult> DeleteMultipleProducts([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest("No product IDs provided.");

            var productsToDelete = await _context.WeatherRecords
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();

            if (!productsToDelete.Any())
                return NotFound("No matching products found.");

            _context.WeatherRecords.RemoveRange(productsToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
