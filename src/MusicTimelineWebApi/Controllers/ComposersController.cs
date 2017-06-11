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
using MusicTimelineWebApi.Models;

namespace ClassicalMusicDbService.Controllers
{
    public class ComposersController : ApiController
    {
        private ClassicalMusicEntities db = new ClassicalMusicEntities();

        // GET: api/Composers
        public IEnumerable<MusicTimelineWebApi.Models.Composer> GetComposers()
        {
            return from c in db.Composers
                   select new MusicTimelineWebApi.Models.Composer()
                   {
                       Id = c.ComposerId,
                       Name = c.Name,
                       Dates = c.Dates,
                       Thumbnail = "/Images/" + c.ComposerImages.FirstOrDefault().ComposerImageId + ".thumb.jpg",
                       Nationalities = c.Nationalities.Select(n => n.Name),
                       Eras = c.Eras.Select(e => e.EraId)
                   };
        }

        // GET: api/Composers/5
        [ResponseType(typeof(ComposerDetail))]
        public async Task<IHttpActionResult> GetComposer(int id)
        {
            var composer = await db.Composers
                .Include("Compositions")
                .Include("Influences")
                .Include("ComposerBiography")
                .Include("Links")
                .Include("Eras")
                .Include("Samples")
                .Select(c => new ComposerDetail()
                {
                    Id = c.ComposerId,
                    Name = c.Name,
                    Dates = c.Dates,
                    Thumbnail = "/Images/" + c.ComposerImages.FirstOrDefault().ComposerImageId + ".thumb.jpg",
                    Nationalities = c.Nationalities.Select(n => n.Name),
                    Eras = c.Eras.Select(e => e.EraId),
                    BirthLocation = c.BirthLocation.Name,
                    DeathLocation = c.DeathLocation.Name,
                    Compositions = c.Compositions.Select(x => new MusicTimelineWebApi.Models.Composition()
                                                 {
                                                     Id = x.CompositionId,
                                                     Name = x.Name,
                                                     Dates = x.Dates,
                                                     Key = x.Key.Name,
                                                     CatalogNumbers = x.CatalogNumbers.Select(p => p.Catalog.Prefix + " " + p.Value)
                                                 }),
                    Biography = c.ComposerBiography.Text,
                    Images = c.ComposerImages.Select(i => "/Images/" + i.ComposerImageId + ".jpg"),
                    Influences = c.Influences.Select(i => new MusicTimelineWebApi.Models.Composer()
                                             {
                                                 Id = i.ComposerId,
                                                 Name = i.Name,
                                                 Dates = i.Dates,
                                                 Thumbnail = "/Images/" + i.ComposerImages.FirstOrDefault().ComposerImageId + ".thumb.jpg",
                                                 Nationalities = i.Nationalities.Select(n => n.Name),
                                                 Eras = i.Eras.Select(e => e.EraId)
                                             }),
                    Influenced = c.Influenced.Select(i => new MusicTimelineWebApi.Models.Composer()
                                             {
                                                 Id = i.ComposerId,
                                                 Name = i.Name,
                                                 Dates = i.Dates,
                                                 Thumbnail = "/Images/" + i.ComposerImages.FirstOrDefault().ComposerImageId + ".thumb.jpg",
                                                 Nationalities = i.Nationalities.Select(n => n.Name),
                                                 Eras = i.Eras.Select(e => e.EraId)
                                             }),
                    Samples = c.Samples.Select(s => new MusicTimelineWebApi.Models.Sample()
                    {
                        Title = s.Title,
                        Artist = s.Artists,
                        Url = "/Samples/" + s.SampleId + ".mp3"
                    }),
                    Links = c.Links.Select(l => l.Url)
                })
                .SingleOrDefaultAsync(c => c.Id == id);


            if (composer == null)
            {
                return NotFound();
            }

            return Ok(composer);
        }

        //// PUT: api/Composers/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutComposer(int id, Composer composer)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != composer.ComposerId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(composer).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ComposerExists(id))
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

        //// POST: api/Composers
        //[ResponseType(typeof(Composer))]
        //public async Task<IHttpActionResult> PostComposer(Composer composer)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Composers.Add(composer);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = composer.ComposerId }, composer);
        //}

        //// DELETE: api/Composers/5
        //[ResponseType(typeof(Composer))]
        //public async Task<IHttpActionResult> DeleteComposer(int id)
        //{
        //    Composer composer = await db.Composers.FindAsync(id);
        //    if (composer == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Composers.Remove(composer);
        //    await db.SaveChangesAsync();

        //    return Ok(composer);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ComposerExists(int id)
        {
            return db.Composers.Count(e => e.ComposerId == id) > 0;
        }
    }
}