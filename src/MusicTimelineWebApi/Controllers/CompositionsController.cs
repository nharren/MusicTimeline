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
    public class CompositionsController : ApiController
    {
        private ClassicalMusicEntities db = new ClassicalMusicEntities();

        // GET: api/Compositions
        public IQueryable<Composition> GetCompositions()
        {
            return db.Compositions;
        }

        // GET: api/Compositions/5
        [ResponseType(typeof(Composition))]
        public async Task<IHttpActionResult> GetComposition(int id)
        {
            Composition composition = await db.Compositions.FindAsync(id);
            if (composition == null)
            {
                return NotFound();
            }

            return Ok(composition);
        }

        //// PUT: api/Compositions/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutComposition(int id, Composition composition)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != composition.CompositionId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(composition).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CompositionExists(id))
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

        //// POST: api/Compositions
        //[ResponseType(typeof(Composition))]
        //public async Task<IHttpActionResult> PostComposition(Composition composition)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Compositions.Add(composition);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = composition.CompositionId }, composition);
        //}

        //// DELETE: api/Compositions/5
        //[ResponseType(typeof(Composition))]
        //public async Task<IHttpActionResult> DeleteComposition(int id)
        //{
        //    Composition composition = await db.Compositions.FindAsync(id);
        //    if (composition == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Compositions.Remove(composition);
        //    await db.SaveChangesAsync();

        //    return Ok(composition);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompositionExists(int id)
        {
            return db.Compositions.Count(e => e.CompositionId == id) > 0;
        }
    }
}