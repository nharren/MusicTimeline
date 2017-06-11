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
    public class KeysController : ApiController
    {
        private ClassicalMusicEntities db = new ClassicalMusicEntities();

        // GET: api/Keys
        public IQueryable<Key> GetKeys()
        {
            return db.Keys;
        }

        // GET: api/Keys/5
        [ResponseType(typeof(Key))]
        public async Task<IHttpActionResult> GetKey(int id)
        {
            Key key = await db.Keys.FindAsync(id);
            if (key == null)
            {
                return NotFound();
            }

            return Ok(key);
        }

        //// PUT: api/Keys/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutKey(int id, Key key)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != key.KeyId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(key).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!KeyExists(id))
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

        //// POST: api/Keys
        //[ResponseType(typeof(Key))]
        //public async Task<IHttpActionResult> PostKey(Key key)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Keys.Add(key);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = key.KeyId }, key);
        //}

        //// DELETE: api/Keys/5
        //[ResponseType(typeof(Key))]
        //public async Task<IHttpActionResult> DeleteKey(int id)
        //{
        //    Key key = await db.Keys.FindAsync(id);
        //    if (key == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Keys.Remove(key);
        //    await db.SaveChangesAsync();

        //    return Ok(key);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KeyExists(int id)
        {
            return db.Keys.Count(e => e.KeyId == id) > 0;
        }
    }
}