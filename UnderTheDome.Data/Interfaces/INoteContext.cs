using System;
using System.Collections.Generic;
using System.Text;
using UnderTheDome.Data.Models;

namespace UnderTheDome.Data.Interfaces
{
    public interface INoteContext
    {
        public bool AddNoteForActor(Note note);
        public Note GetNoteForActor(int actorId);
        public void RemoveNoteFromFile(Note note);
    }
}
