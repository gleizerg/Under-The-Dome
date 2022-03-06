using System;
using System.Collections.Generic;
using System.Text;
using UnderTheDome.Data.Interfaces;
using UnderTheDome.Data.Models;

namespace UnderTheDome.Data.Repositories
{
    public class ActorRepository : IActorRepository
    {

        private IUnderTheDomeContext _underTheDomeContext;
        private INoteContext _noteContext;

        public ActorRepository(IUnderTheDomeContext underTheDomeContext, INoteContext noteContext)
        {
            _underTheDomeContext = underTheDomeContext;
            _noteContext = noteContext;
        }

        public List<Actor> GetAllActors() 
        { 
            return _underTheDomeContext.Actors;
        }

        public Actor GetActor(int id)
        {
            return _underTheDomeContext.Actors.Find(x => x.Id == id);
        }

        public void AddNoteForActor(Note note)
        {
            _noteContext.AddNoteForActor(note);
            GetActor(note.ActorId).Note = note;
        }

        public Note GetNoteForActor(int id)
        {
            return (GetActor(id)).Note;
        }

        public bool DeleteActor(int id)
        {
            Actor actor = GetActor(id);
            if(actor != null)
            {
                _underTheDomeContext.Actors.Remove(actor);
                _noteContext.RemoveNoteFromFile(actor.Note);
                return true;
            }
            return false;
        }
    }
}
