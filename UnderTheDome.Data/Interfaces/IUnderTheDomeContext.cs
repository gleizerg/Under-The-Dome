using System;
using System.Collections.Generic;
using System.Text;
using UnderTheDome.Data.Models;

namespace UnderTheDome.Data.Interfaces
{
    public interface IUnderTheDomeContext
    {
        public List<Actor> Actors { get; set; }

    }
}
