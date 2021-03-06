﻿using System;
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
    public class RelationshipsController : ApiController
    {
        private FamilyTreeEntities db = new FamilyTreeEntities();

        // GET: api/Relationships
        public IQueryable<Relationship> GetRelationships()
        {
            return db.Relationships;
        }

        // GET: api/Relationships/5
        [ResponseType(typeof(Relationship))]
        public IHttpActionResult GetRelationship(int id)
        {
            Relationship relationship = db.Relationships.Find(id);
            if (relationship == null)
            {
                return NotFound();
            }

            return Ok(relationship);
        }

        // PUT: api/Relationships/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRelationship(int id, Relationship relationship)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != relationship.id)
            {
                return BadRequest();
            }

            try
            {
                var currentRelationship = db.Relationships.FirstOrDefault(p => p.id == relationship.id);
                if (currentRelationship == null)
                    return NotFound();
                currentRelationship.Relationship1 = relationship.Relationship1;
                currentRelationship.updatedAt = DateTime.Now;
                db.SaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RelationshipExists(id))
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

        // POST: api/Relationships
        [ResponseType(typeof(Relationship))]
        public IHttpActionResult PostRelationship(Relationship relationship)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            relationship.createdAt = DateTime.Now;
            db.Relationships.Add(relationship);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = relationship.id }, relationship);
        }

        // DELETE: api/Relationships/5
        [ResponseType(typeof(Relationship))]
        public IHttpActionResult DeleteRelationship(int id)
        {
            Relationship relationship = db.Relationships.Find(id);
            if (relationship == null)
            {
                return NotFound();
            }
            if (db.Relatives.Any(r => r.RelationshipId == id))
            {
                return BadRequest("Cannot erase relationship because it is being used in some personas relatives");
            }
            db.Relationships.Remove(relationship);
            db.SaveChanges();

            return Ok(relationship);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RelationshipExists(int id)
        {
            return db.Relationships.Any(e => e.id == id);
        }
    }
}