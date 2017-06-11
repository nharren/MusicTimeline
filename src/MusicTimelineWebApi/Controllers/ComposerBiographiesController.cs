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
    public class ComposerBiographiesController : ApiController
    {
        private ClassicalMusicEntities db = new ClassicalMusicEntities();

        // GET: api/ComposerBiographies
        public IQueryable<ComposerBiography> GetComposerBiographies()
        {
            return db.ComposerBiographies;
        }

        // GET: api/ComposerBiographies/5
        [ResponseType(typeof(ComposerBiography))]
        public async Task<IHttpActionResult> GetComposerBiography(int id)
        {
            ComposerBiography composerBiography = await db.ComposerBiographies.FindAsync(id);
            if (composerBiography == null)
            {
                return NotFound();
            }

            return Ok(composerBiography);
        }

        //// PUT: api/ComposerBiographies/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutComposerBiography(int id, ComposerBiography composerBiography)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != composerBiography.ComposerId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(composerBiography).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ComposerBiographyExists(id))
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

        //// POST: api/ComposerBiographies
        //[ResponseType(typeof(ComposerBiography))]
        //public async Task<IHttpActionResult> PostComposerBiography(ComposerBiography composerBiography)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.ComposerBiographies.Add(composerBiography);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = composerBiography.ComposerId }, composerBiography);
        //}

        //// DELETE: api/ComposerBiographies/5
        //[ResponseType(typeof(ComposerBiography))]
        //public async Task<IHttpActionResult> DeleteComposerBiography(int id)
        //{
        //    ComposerBiography composerBiography = await db.ComposerBiographies.FindAsync(id);
        //    if (composerBiography == null)
        //    {
        //        return NotFound();
        //    }

        //    db.ComposerBiographies.Remove(composerBiography);
        //    await db.SaveChangesAsync();

        //    return Ok(composerBiography);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ComposerBiographyExists(int id)
        {
            return db.ComposerBiographies.Count(e => e.ComposerId == id) > 0;
        }
    }
}