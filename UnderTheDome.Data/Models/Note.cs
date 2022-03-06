using System;
using System.Collections.Generic;
using System.Text;

namespace UnderTheDome.Data.Models
{
    public class Note
    {
        public int ActorId { get; set; }
        public string ActorNote { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Note other = obj as Note;
            if (other == null) return false;
            else return Equals(other);
        }
        public bool Equals(Note other)
        {
            if (other == null) return false;
            return (this.ActorId.Equals(other.ActorId));
        }

        public override int GetHashCode()
        {
            return (ActorId + 12345678);
        }
    }
}
