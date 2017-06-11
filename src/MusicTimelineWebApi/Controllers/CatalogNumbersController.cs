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
    public class CatalogNumbersController : ApiController
    {
        private ClassicalMusicEntities db = new ClassicalMusicEntities();

        // GET: api/CatalogNumbers
        public IQueryable<CatalogNumber> GetCatalogNumbers()
        {
            return db.CatalogNumbers;
        }

        // GET: api/CatalogNumbers/5
        [ResponseType(typeof(CatalogNumber))]
        public async Task<IHttpActionResult> GetCatalogNumber(int id)
        {
            CatalogNumber catalogNumber = await db.CatalogNumbers.FindAsync(id);
            if (catalogNumber == null)
            {
                return NotFound();
            }

            return Ok(catalogNumber);
        }

        //// PUT: api/CatalogNumbers/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutCatalogNumber(int id, CatalogNumber catalogNumber)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != catalogNumber.CatalogNumberId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(catalogNumber).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CatalogNumberExists(id))
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

        //// POST: api/CatalogNumbers
        //[ResponseType(typeof(CatalogNumber))]
        //public async Task<IHttpActionResult> PostCatalogNumber(CatalogNumber catalogNumber)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.CatalogNumbers.Add(catalogNumber);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = catalogNumber.CatalogNumberId }, catalogNumber);
        //}

        //// DELETE: api/CatalogNumbers/5
        //[ResponseType(typeof(CatalogNumber))]
        //public async Task<IHttpActionResult> DeleteCatalogNumber(int id)
        //{
        //    CatalogNumber catalogNumber = await db.CatalogNumbers.FindAsync(id);
        //    if (catalogNumber == null)
        //    {
        //        return NotFound();
        //    }

        //    db.CatalogNumbers.Remove(catalogNumber);
        //    await db.SaveChangesAsync();

        //    return Ok(catalogNumber);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CatalogNumberExists(int id)
        {
            return db.CatalogNumbers.Count(e => e.CatalogNumberId == id) > 0;
        }
    }
}