using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi3elements.Models;

using System.Linq.Expressions;
using WebApi3elements.DTOs;

namespace WebApi3elements.Controllers
{
    public class MeasuresController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private static readonly Expression<Func<Measure, MeasureDto>> AsMeasureDto =
           x => new MeasureDto
           {
               date = x.date,
               consumption = x.consumption,
               type = x.type,
               deviceName = x.Device.name
           };

        // GET: api/Measures
        public IQueryable<MeasureDto> GetMeasures()
        {
            return db.Measures.Include(b => b.Device).Select(AsMeasureDto);
        }

        // GET: api/Measures/5
        [ResponseType(typeof(Measure))]
        public IHttpActionResult GetMeasure(int id)
        {
            Measure measure = db.Measures.Find(id);
            if (measure == null)
            {
                return NotFound();
            }

            return Ok(measure);
        }

        // GET: api/Measures?startDate=value&endDate=value
        [ResponseType(typeof(MeasureDto))]
        public IQueryable<MeasureDto> GetMeasuresByDate([FromUri]DateTime startDate, [FromUri]DateTime endDate)
        {
            return db.Measures.Include(b => b.Device).Where(b => (DbFunctions.TruncateTime(b.date) >= DbFunctions.TruncateTime(startDate)) && (DbFunctions.TruncateTime(b.date) <= DbFunctions.TruncateTime(endDate))).Select(AsMeasureDto);

        }

        // GET: api/Measures?deviceId=value
        [ResponseType(typeof(MeasureDto))]
        public IQueryable<MeasureDto> GetMeasuresByDeviceId([FromUri]string deviceId)
        {
            return db.Measures.Include(b => b.Device).Where(b => (b.deviceId == deviceId)).Select(AsMeasureDto);
        }

        // GET: api/Measures?deviceName=value
        [ResponseType(typeof(MeasureDto))]
        public IQueryable<MeasureDto> GetMeasuresByDeviceName([FromUri]string deviceName)
        {
            return db.Measures.Include(b => b.Device).Where(b => (b.Device.name == deviceName)).Select(AsMeasureDto);
        }

        // PUT: api/Measures/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMeasure(int id, Measure measure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != measure.measureId)
            {
                return BadRequest();
            }

            db.Entry(measure).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeasureExists(id))
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

        // POST: api/Measures
        [ResponseType(typeof(MeasureDto))]
        public IHttpActionResult PostMeasure(Measure measure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Measures.Add(measure);
            db.SaveChanges();


            // Cargar datos de Device en la variable measure
            db.Entry(measure).Reference(x => x.Device).Load();
            var dto = new MeasureDto()
            {
                date = measure.date,
                consumption = measure.consumption,
                type = measure.type,
                deviceName = measure.Device.name
            };
            return CreatedAtRoute("DefaultApi", new { id = measure.measureId }, measure);
        }

        // DELETE: api/Measures/5
        [ResponseType(typeof(Measure))]
        public IHttpActionResult DeleteMeasure(int id)
        {
            Measure measure = db.Measures.Find(id);
            if (measure == null)
            {
                return NotFound();
            }

            db.Measures.Remove(measure);
            db.SaveChanges();

            return Ok(measure);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MeasureExists(int id)
        {
            return db.Measures.Count(e => e.measureId == id) > 0;
        }
    }
}