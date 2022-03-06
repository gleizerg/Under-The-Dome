using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;
using UnderTheDome.Data.Interfaces;
using UnderTheDome.Data.Models;
using UnderTheDome.Data.Repositories;

namespace UnderTheDomeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnderTheDomeController : ControllerBase
    {
        private IActorRepository _actorRepository;

        public UnderTheDomeController(IActorRepository actors)
        {
            _actorRepository = actors;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Actor>> GetAllActors()
        {
            return _actorRepository.GetAllActors();
        }

        [HttpGet("{id}")]
        public ActionResult<Actor> GetActor(int id)
        {
            Actor actor = _actorRepository.GetActor(id);
            if (actor == null)
                return NotFound();
            return actor;
        }

        [HttpPost]
        public ActionResult<Note> PostNote( [FromForm] Note note)
        {
            _actorRepository.AddNoteForActor(note);
            
            return note;
            
        }

        [HttpDelete]
        [Route("actor/{id}")]
        public ActionResult<IEnumerable<Actor>> DeleteActor(int id)
        {
            if (_actorRepository.DeleteActor(id))
            {
                return _actorRepository.GetAllActors();
            }
            return NotFound();
        }

        [HttpPut]
        public ActionResult<Note> UpdateNote( [FromForm] Note note)
        {
            _actorRepository.AddNoteForActor(note);
            return note;
        }
    }
}
