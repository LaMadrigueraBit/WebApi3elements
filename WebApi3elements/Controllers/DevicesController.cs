using System;
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
    public class DevicesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private static readonly Expression<Func<Device, DeviceDto>> AsDeviceDto =
            x => new DeviceDto
            {
                deviceId = x.deviceId,
                name = x.name,
                on = x.on,
                homeId = x.homeId
            };

        // GET: api/Devices
        public IQueryable<DeviceDto> GetDevices()
        {
            return db.Devices.Select(AsDeviceDto);
        }

        // GET: api/Devices/5
        [ResponseType(typeof(DeviceDto))]
        public IQueryable<DeviceDto> GetDevice(string id)
        {
            return db.Devices.Where(b => (b.deviceId == id)).Select(AsDeviceDto);
        }

        // GET: api/Devices?deviceName=value
        [ResponseType(typeof(DeviceDto))]
        public IQueryable<DeviceDto> GetDeviceByName([FromUri]string deviceName)
        {
            return db.Devices.Where(b => (b.name == deviceName)).Select(AsDeviceDto);
        }

        // GET: api/Devices?homeId=value
        [ResponseType(typeof(DeviceDto))]
        public IQueryable<DeviceDto> GetDeviceByHome([FromUri]string homeId)
        {
            return db.Devices.Where(b => (b.homeId == homeId)).Select(AsDeviceDto);
        }
        // PUT: api/Devices/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDevice(string id, Device device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != device.deviceId)
            {
                return BadRequest();
            }

            db.Entry(device).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
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

        // POST: api/Devices
        [ResponseType(typeof(DeviceDto))]
        public IHttpActionResult PostDevice(Device device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Devices.Add(device);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DeviceExists(device.deviceId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            var dto = new DeviceDto()
            {
                deviceId = device.deviceId,
                name = device.name,
                on = device.on,
                homeId = device.homeId
            };
            return CreatedAtRoute("DefaultApi", new { id = device.deviceId }, device);
        }

        // DELETE: api/Devices/5
        [ResponseType(typeof(Device))]
        public IHttpActionResult DeleteDevice(string id)
        {
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return NotFound();
            }

            db.Devices.Remove(device);
            db.SaveChanges();

            return Ok(device);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeviceExists(string id)
        {
            return db.Devices.Count(e => e.deviceId == id) > 0;
        }
    }
}