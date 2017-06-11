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
    public class NationalitiesController : ApiController
    {
        private ClassicalMusicEntities db = new ClassicalMusicEntities();

        // GET: api/Nationalities
        public IQueryable<Nationality> GetNationalities()
        {
            return db.Nationalities;
        }

        // GET: api/Nationalities/5
        [ResponseType(typeof(Nationality))]
        public async Task<IHttpActionResult> GetNationality(int id)
        {
            Nationality nationality = await db.Nationalities.FindAsync(id);
            if (nationality == null)
            {
                return NotFound();
            }

            return Ok(nationality);
        }

        //// PUT: api/Nationalities/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutNationality(int id, Nationality nationality)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != nationality.NationalityId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(nationality).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!NationalityExists(id))
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

        //// POST: api/Nationalities
        //[ResponseType(typeof(Nationality))]
        //public async Task<IHttpActionResult> PostNationality(Nationality nationality)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Nationalities.Add(nationality);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = nationality.NationalityId }, nationality);
        //}

        //// DELETE: api/Nationalities/5
        //[ResponseType(typeof(Nationality))]
        //public async Task<IHttpActionResult> DeleteNationality(int id)
        //{
        //    Nationality nationality = await db.Nationalities.FindAsync(id);
        //    if (nationality == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Nationalities.Remove(nationality);
        //    await db.SaveChangesAsync();

        //    return Ok(nationality);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NationalityExists(int id)
        {
            return db.Nationalities.Count(e => e.NationalityId == id) > 0;
        }
    }
}