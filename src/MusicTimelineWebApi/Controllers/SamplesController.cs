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
    public class SamplesController : ApiController
    {
        private ClassicalMusicEntities db = new ClassicalMusicEntities();

        // GET: api/Samples
        public IQueryable<Sample> GetSamples()
        {
            return db.Samples;
        }

        // GET: api/Samples/5
        [ResponseType(typeof(Sample))]
        public async Task<IHttpActionResult> GetSample(int id)
        {
            Sample sample = await db.Samples.FindAsync(id);
            if (sample == null)
            {
                return NotFound();
            }

            return Ok(sample);
        }

        //// PUT: api/Samples/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutSample(int id, Sample sample)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != sample.SampleId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(sample).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SampleExists(id))
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

        //// POST: api/Samples
        //[ResponseType(typeof(Sample))]
        //public async Task<IHttpActionResult> PostSample(Sample sample)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Samples.Add(sample);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = sample.SampleId }, sample);
        //}

        //// DELETE: api/Samples/5
        //[ResponseType(typeof(Sample))]
        //public async Task<IHttpActionResult> DeleteSample(int id)
        //{
        //    Sample sample = await db.Samples.FindAsync(id);
        //    if (sample == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Samples.Remove(sample);
        //    await db.SaveChangesAsync();

        //    return Ok(sample);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SampleExists(int id)
        {
            return db.Samples.Count(e => e.SampleId == id) > 0;
        }
    }
}