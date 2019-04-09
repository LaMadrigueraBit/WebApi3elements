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

namespace WebApi3elements.Controllers
{
    public class MeasuresController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Measures
        public IQueryable<Measure> GetMeasures()
        {
            return db.Measures;
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
        [ResponseType(typeof(Measure))]
        public IHttpActionResult PostMeasure(Measure measure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Measures.Add(measure);
            db.SaveChanges();

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