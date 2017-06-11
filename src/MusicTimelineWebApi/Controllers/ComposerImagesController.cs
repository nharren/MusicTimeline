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
    public class ComposerImagesController : ApiController
    {
        private ClassicalMusicEntities db = new ClassicalMusicEntities();

        // GET: api/ComposerImages
        public IQueryable<ComposerImage> GetComposerImages()
        {
            return db.ComposerImages;
        }

        // GET: api/ComposerImages/5
        [ResponseType(typeof(ComposerImage))]
        public async Task<IHttpActionResult> GetComposerImage(int id)
        {
            ComposerImage composerImage = await db.ComposerImages.FindAsync(id);
            if (composerImage == null)
            {
                return NotFound();
            }

            return Ok(composerImage);
        }

        //// PUT: api/ComposerImages/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutComposerImage(int id, ComposerImage composerImage)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != composerImage.ComposerImageId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(composerImage).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ComposerImageExists(id))
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

        //// POST: api/ComposerImages
        //[ResponseType(typeof(ComposerImage))]
        //public async Task<IHttpActionResult> PostComposerImage(ComposerImage composerImage)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.ComposerImages.Add(composerImage);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = composerImage.ComposerImageId }, composerImage);
        //}

        //// DELETE: api/ComposerImages/5
        //[ResponseType(typeof(ComposerImage))]
        //public async Task<IHttpActionResult> DeleteComposerImage(int id)
        //{
        //    ComposerImage composerImage = await db.ComposerImages.FindAsync(id);
        //    if (composerImage == null)
        //    {
        //        return NotFound();
        //    }

        //    db.ComposerImages.Remove(composerImage);
        //    await db.SaveChangesAsync();

        //    return Ok(composerImage);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ComposerImageExists(int id)
        {
            return db.ComposerImages.Count(e => e.ComposerImageId == id) > 0;
        }
    }
}