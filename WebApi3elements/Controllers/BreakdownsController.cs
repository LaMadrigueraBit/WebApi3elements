using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi3elements.Models;

using System.Linq.Expressions;
using WebApi3elements.DTOs;

namespace WebApi3elements.Controllers
{
    public class BreakdownsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private static readonly Expression<Func<Breakdown, BreakdownDto>> AsBreakdownDto =
            x => new BreakdownDto
            {
                date = x.date,
                solved = x.solved,
                deviceName = x.Device.name
            };


        // GET: api/Breakdowns
        public IQueryable<BreakdownDto> GetBreakdowns()
        {
            return db.Breakdowns.Include(b => b.Device).Select(AsBreakdownDto);
        }

        // GET: api/Breakdowns/5
        [ResponseType(typeof(Breakdown))]
        public IHttpActionResult GetBreakdown(int id)
        {
            Breakdown breakdown = db.Breakdowns.Find(id);
            if (breakdown == null)
            {
                return NotFound();
            }

            return Ok(breakdown);
        }

        // GET: api/Breakdowns?startDate=value&endDate=value
        [ResponseType(typeof(BreakdownDto))]
        public IQueryable<BreakdownDto> GetBreakdownsByDate([FromUri]DateTime startDate, [FromUri]DateTime endDate)
        {
               return db.Breakdowns.Include(b => b.Device).Where(b => (DbFunctions.TruncateTime(b.date) >= DbFunctions.TruncateTime(startDate)) && (DbFunctions.TruncateTime(b.date) <= DbFunctions.TruncateTime(endDate))).Select(AsBreakdownDto);
        }

        // GET: api/Breakdowns?deviceId=value
        [ResponseType(typeof(BreakdownDto))]
        public IQueryable<BreakdownDto> GetBreakdownsByDeviceId([FromUri]string deviceId)
        {
            return db.Breakdowns.Include(b => b.Device).Where(b => (b.deviceId == deviceId)).Select(AsBreakdownDto);
        }

        // GET: api/Breakdowns?deviceName=value
        [ResponseType(typeof(BreakdownDto))]
        public IQueryable<BreakdownDto> GetBreakdownsByDeviceName([FromUri]string deviceName)
        {
            return db.Breakdowns.Include(b => b.Device).Where(b => (b.Device.name == deviceName)).Select(AsBreakdownDto);
        }

        // PUT: api/Breakdowns/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBreakdown(int id, Breakdown breakdown)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != breakdown.breakdownId)
            {
                return BadRequest();
            }

            db.Entry(breakdown).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BreakdownExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Breakdowns
        [ResponseType(typeof(BreakdownDto))]
        public IHttpActionResult PostBreakdown(Breakdown breakdown)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Breakdowns.Add(breakdown);
            db.SaveChanges();

            // Cargar datos de Device en la variable measure
            db.Entry(breakdown).Reference(x => x.Device).Load();
            var dto = new BreakdownDto()
            {
                date = breakdown.date,
                solved = breakdown.solved,
                deviceName = breakdown.Device.name
            };

            return CreatedAtRoute("DefaultApi", new { id = breakdown.breakdownId }, breakdown);
        }

        // DELETE: api/Breakdowns/5
        [ResponseType(typeof(Breakdown))]
        public IHttpActionResult DeleteBreakdown(int id)
        {
            Breakdown breakdown = db.Breakdowns.Find(id);
            if (breakdown == null)
            {
                return NotFound();
            }

            db.Breakdowns.Remove(breakdown);
            db.SaveChanges();

            return Ok(breakdown);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BreakdownExists(int id)
        {
            return db.Breakdowns.Count(e => e.breakdownId == id) > 0;
        }
    }
}