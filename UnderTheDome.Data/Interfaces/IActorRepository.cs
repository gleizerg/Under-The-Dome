using System;
using System.Collections.Generic;
using System.Text;
using UnderTheDome.Data.Models;

namespace UnderTheDome.Data.Interfaces
{
    public interface IActorRepository
    {
        public List<Actor> GetAllActors();
        public Actor GetActor(int id);
        public void AddNoteForActor(Note note);
        public Note GetNoteForActor(int id);
        public bool DeleteActor(int id);

    }
}
