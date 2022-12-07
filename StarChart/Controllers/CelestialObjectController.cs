using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name ="GetById")]
        public IActionResult GetById(int id)
        {
            var item = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);

            if (item == null) { return NotFound(); }

            SetSatellites(item);
            return Ok(item);
        }

        private void SetSatellites(CelestialObject item)
        {
            item.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == item.Id).ToList();
        }

        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var items = _context.CelestialObjects.Where(o => o.Name == name).ToList();

            if (!items.Any()) { return NotFound(); }

            items.ForEach(SetSatellites);

            return Ok(items);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _context.CelestialObjects.ToList();
            items.ForEach(SetSatellites);
            
            return base.Ok(items);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject co)
        {
            _context.CelestialObjects.Add(co);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { Id = co.Id }, co);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject co)
        {
            var item = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);

            if (item == null) { return NotFound(); }

            item.Name = co.Name;
            item.OrbitalPeriod = co.OrbitalPeriod;
            item.OrbitedObjectId = co.OrbitedObjectId;

            _context.Update(item);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var item = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);

            if (item == null) { return NotFound(); }

            item.Name = name;

            _context.Update(item);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);

            if (item == null) { return NotFound(); }

            _context.RemoveRange(item); 
            _context.SaveChanges();

            return NoContent(); 
        }
    }
}
