using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.Models
{

    public class CityTest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
        public virtual State State { get; set; }
    }

    public class StateTest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual List<City> City { get; set; }
    }

    public class TreeViewNode
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
    }

}
