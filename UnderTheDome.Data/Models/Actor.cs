using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace UnderTheDome.Data.Models
{
    public class Actor : IEquatable<Actor>
    {
        public int Id { get; set; }
        public string ActorName { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }
        public Note Note { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Actor other = obj as Actor;
            if (other == null) return false;
            else return Equals(other);
        }
        public bool Equals(Actor other)
        {
            if (other == null) return false;
            return (this.Id.Equals(other.Id));
        }

        public override int GetHashCode()
        {
            return (Id+12345678);
        }
    }
}
