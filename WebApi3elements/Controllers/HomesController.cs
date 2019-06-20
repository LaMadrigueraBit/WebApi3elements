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
    public class HomesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private static readonly Expression<Func<Home, HomeDto>> AsHomeDto =
          x => new HomeDto
          {
              homeId = x.homeId,
              userId = x.userId,
              userName = x.ApplicationUser.UserName
          };

        // GET: api/Homes
        public IQueryable<HomeDto> GetHomes()
        {
            return db.Homes.Include(b => b.ApplicationUser).Select(AsHomeDto);
        }

        // GET: api/Homes/5
        [ResponseType(typeof(Home))]
        public IHttpActionResult GetHome(string id)
        {
            Home home = db.Homes.Find(id);
            if (home == null)
            {
                return NotFound();
            }

            return Ok(home);
        }

        // PUT: api/Homes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHome(string id, Home home)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != home.homeId)
            {
                return BadRequest();
            }

            db.Entry(home).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomeExists(id))
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

        // POST: api/Homes
        [ResponseType(typeof(HomeDto))]
        public IHttpActionResult PostHome(Home home)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Homes.Add(home);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (HomeExists(home.homeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = home.homeId }, home);
        }

        // DELETE: api/Homes/5
        [ResponseType(typeof(Home))]
        public IHttpActionResult DeleteHome(string id)
        {
            Home home = db.Homes.Find(id);
            if (home == null)
            {
                return NotFound();
            }

            db.Homes.Remove(home);
            db.SaveChanges();

            return Ok(home);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HomeExists(string id)
        {
            return db.Homes.Count(e => e.homeId == id) > 0;
        }
    }
}