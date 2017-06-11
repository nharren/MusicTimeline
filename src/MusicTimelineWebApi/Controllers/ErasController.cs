using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ClassicalMusicDbService.Models;

namespace ClassicalMusicDbService.Controllers
{
    public class ErasController : ApiController
    {
        private ClassicalMusicEntities db = new ClassicalMusicEntities();

        // GET: api/Eras
        public IQueryable<MusicTimelineWebApi.Models.Era> GetEras()
        {
            var eras = from e in db.Eras
                       select new MusicTimelineWebApi.Models.Era()
                       {
                           Id = e.EraId,
                           Name = e.Name,
                           Dates = e.Dates,
                       };

            return eras;
        }

        // GET: api/Eras/5
        [ResponseType(typeof(Era))]
        public async Task<IHttpActionResult> GetEra(int id)
        {
            var era = await db.Eras
                .Select(e => new MusicTimelineWebApi.Models.Era()
                {
                    Id = e.EraId,
                    Name = e.Name,
                    Dates = e.Dates,
                })
                .SingleOrDefaultAsync(c => c.Id == id);

            if (era == null)
            {
                return NotFound();
            }

            return Ok(era);
        }

        //// PUT: api/Eras/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutEra(int id, Era era)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != era.EraId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(era).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EraExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Eras
        //[ResponseType(typeof(Era))]
        //public async Task<IHttpActionResult> PostEra(Era era)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Eras.Add(era);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = era.EraId }, era);
        //}

        //// DELETE: api/Eras/5
        //[ResponseType(typeof(Era))]
        //public async Task<IHttpActionResult> DeleteEra(int id)
        //{
        //    Era era = await db.Eras.FindAsync(id);
        //    if (era == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Eras.Remove(era);
        //    await db.SaveChangesAsync();

        //    return Ok(era);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EraExists(int id)
        {
            return db.Eras.Count(e => e.EraId == id) > 0;
        }
    }
}