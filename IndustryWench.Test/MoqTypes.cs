using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndustryWench.Test {
    public class User {
        public int Id { get; set; }
        public string FirstName;
        public virtual string LastName { get; set; }

        public string TheMethod() { return "mehotd"; }
    }

    public class Fruit {
        public string Name { get; set; }
    }
}
