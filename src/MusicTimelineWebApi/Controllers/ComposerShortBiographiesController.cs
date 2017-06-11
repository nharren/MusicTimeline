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
    public class ComposerShortBiographiesController : ApiController
    {
        private ClassicalMusicEntities db = new ClassicalMusicEntities();

        // GET: api/ComposerShortBiographies
        public IQueryable<ComposerShortBiography> GetComposerShortBiographies()
        {
            return db.ComposerShortBiographies;
        }

        // GET: api/ComposerShortBiographies/5
        [ResponseType(typeof(ComposerShortBiography))]
        public async Task<IHttpActionResult> GetComposerShortBiography(int id)
        {
            ComposerShortBiography composerShortBiography = await db.ComposerShortBiographies.FindAsync(id);
            if (composerShortBiography == null)
            {
                return NotFound();
            }

            return Ok(composerShortBiography);
        }

        //// PUT: api/ComposerShortBiographies/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutComposerShortBiography(int id, ComposerShortBiography composerShortBiography)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != composerShortBiography.ComposerId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(composerShortBiography).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ComposerShortBiographyExists(id))
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

        //// POST: api/ComposerShortBiographies
        //[ResponseType(typeof(ComposerShortBiography))]
        //public async Task<IHttpActionResult> PostComposerShortBiography(ComposerShortBiography composerShortBiography)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.ComposerShortBiographies.Add(composerShortBiography);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = composerShortBiography.ComposerId }, composerShortBiography);
        //}

        //// DELETE: api/ComposerShortBiographies/5
        //[ResponseType(typeof(ComposerShortBiography))]
        //public async Task<IHttpActionResult> DeleteComposerShortBiography(int id)
        //{
        //    ComposerShortBiography composerShortBiography = await db.ComposerShortBiographies.FindAsync(id);
        //    if (composerShortBiography == null)
        //    {
        //        return NotFound();
        //    }

        //    db.ComposerShortBiographies.Remove(composerShortBiography);
        //    await db.SaveChangesAsync();

        //    return Ok(composerShortBiography);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ComposerShortBiographyExists(int id)
        {
            return db.ComposerShortBiographies.Count(e => e.ComposerId == id) > 0;
        }
    }
}