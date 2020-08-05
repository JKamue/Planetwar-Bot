using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.Objects.Responses
{
    public class Error
    {
        public int id;
        public string title;
        public string description;

        public Error(int id, string title, string description)
        {
            this.id = id;
            this.title = title;
            this.description = description;
        }
    }
}
