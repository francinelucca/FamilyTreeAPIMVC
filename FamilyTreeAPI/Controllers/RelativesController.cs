using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using FamilyTreeAPI.DATA;

namespace FamilyTreeAPI.Controllers
{
    public class RelativesController : ApiController
    {
        private FamilyTreeEntities db = new FamilyTreeEntities();

        // GET: api/Relatives
        public IQueryable<Relative> GetRelatives()
        {
            return db.Relatives.Include(r => r.Relationship).Include(r => r.RelatedTo);
        }

        // GET: api/Relatives/5
        [ResponseType(typeof(Relative))]
        public IHttpActionResult GetRelative(int id)
        {
            Relative relative = db.Relatives.Find(id);
            if (relative == null)
            {
                return NotFound();
            }

            return Ok(relative);
        }

        // PUT: api/Relatives/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRelative(int id, Relative relative)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != relative.id)
            {
                return BadRequest();
            }

            try
            {
                var currentRelative = db.Relatives.FirstOrDefault(p => p.id == relative.id);
                if (currentRelative == null)
                    return NotFound();
                currentRelative.RelationshipId = relative.RelationshipId;
                currentRelative.RelativeId = relative.RelativeId;
                currentRelative.updatedAt = DateTime.Now;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RelativeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Relatives
        [ResponseType(typeof(Relative))]
        public IHttpActionResult PostRelative(Relative relative)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            relative.createdAt = DateTime.Now;
            db.Relatives.Add(relative);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = relative.id }, relative);
        }

        // DELETE: api/Relatives/5
        [ResponseType(typeof(Relative))]
        public IHttpActionResult DeleteRelative(int id)
        {
            Relative relative = db.Relatives.Find(id);
            if (relative == null)
            {
                return NotFound();
            }

            db.Relatives.Remove(relative);
            db.SaveChanges();

            return Ok(relative);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RelativeExists(int id)
        {
            return db.Relatives.Any(e => e.id == id);
        }
    }
}