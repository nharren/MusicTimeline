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
    public class LinksController : ApiController
    {
        private ClassicalMusicEntities db = new ClassicalMusicEntities();

        // GET: api/Links
        public IQueryable<Link> GetLinks()
        {
            return db.Links;
        }

        // GET: api/Links/5
        [ResponseType(typeof(Link))]
        public async Task<IHttpActionResult> GetLink(int id)
        {
            Link link = await db.Links.FindAsync(id);
            if (link == null)
            {
                return NotFound();
            }

            return Ok(link);
        }

        //// PUT: api/Links/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutLink(int id, Link link)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != link.LinkId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(link).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LinkExists(id))
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

        //// POST: api/Links
        //[ResponseType(typeof(Link))]
        //public async Task<IHttpActionResult> PostLink(Link link)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Links.Add(link);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = link.LinkId }, link);
        //}

        //// DELETE: api/Links/5
        //[ResponseType(typeof(Link))]
        //public async Task<IHttpActionResult> DeleteLink(int id)
        //{
        //    Link link = await db.Links.FindAsync(id);
        //    if (link == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Links.Remove(link);
        //    await db.SaveChangesAsync();

        //    return Ok(link);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LinkExists(int id)
        {
            return db.Links.Count(e => e.LinkId == id) > 0;
        }
    }
}