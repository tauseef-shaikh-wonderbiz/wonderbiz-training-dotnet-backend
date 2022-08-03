using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Domain;

namespace API.Controllers
{
    public class CarController : BaseApiController
    {
        private readonly DataContext _context;
        public CarController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Car>>> GetCars()
        {
            return await _context.Cars.ToListAsync();
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<Car>> GetActivity(Guid id)
        // {
        //     return await _context.Cars.FindAsync(id);
        // }
    }
        
    
}