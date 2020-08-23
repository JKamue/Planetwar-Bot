using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetwarApi.objects
{
    public class Owner
    {
        public string id;
        public string name;

        public Owner(string id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
