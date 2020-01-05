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
    public class PersonasController : ApiController
    {
        private FamilyTreeEntities db = new FamilyTreeEntities();

        // GET: api/Personas
        public IHttpActionResult GetPersonas()
        {
            return Ok(db.Personas.Select(p => new {
                p.id,
                p.FirstName,
                p.LastName,
               Gender =  p.Gender == Persona.FEMALE_GENDER_VALUE ? Persona.FEMALE_GENDER_STRING : Persona.MALE_GENDER_STRING,
                p.BirthDay
            }));
        }

        // GET: api/Personas/5
        [ResponseType(typeof(Persona))]
        public IHttpActionResult GetPersona(int id)
        {
            Persona persona = db.Personas.Find(id);
            if (persona == null)
            {
                return NotFound();
            }

            return Ok(persona);
        }

        // PUT: api/Personas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPersona(int id, Persona persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != persona.id)
            {
                return BadRequest();
            }

            try
            {
                var currentPersona = db.Personas.FirstOrDefault(p => p.id == persona.id);
                if (currentPersona == null)
                    return NotFound();
                currentPersona.Gender = persona.Gender;
                currentPersona.LastName = persona.LastName;
                currentPersona.FirstName = persona.FirstName;
                currentPersona.BirthDay = persona.BirthDay;
                currentPersona.updatedAt = DateTime.Now;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonaExists(id))
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

        // POST: api/Personas
        [ResponseType(typeof(Persona))]
        public IHttpActionResult PostPersona(Persona persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            persona.createdAt = DateTime.Now;
            db.Personas.Add(persona);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = persona.id }, persona);
        }

        // DELETE: api/Personas/5
        [ResponseType(typeof(Persona))]
        public IHttpActionResult DeletePersona(int id)
        {
            Persona persona = db.Personas.Find(id);
            if (persona == null)
            {
                return NotFound();
            }
            if (db.Relatives.Any(r => r.RelativeId == persona.id))
            {
                return BadRequest("Can't Remove Persona Because is Being Used as Other Personas' Relative");

            }
            db.Relatives.RemoveRange(persona.Relatives);
            db.Personas.Remove(persona);
            db.SaveChanges();

            return Ok(persona);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonaExists(int id)
        {
            return db.Personas.Any(e => e.id == id);
        }
    }
}