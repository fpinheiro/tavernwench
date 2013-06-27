
namespace TavernWench.Test.Common {
    public class User {
        public int Id { get; set; }
        public string FirstName;
        public virtual string LastName { get; set; }

        public string TheMethod(string theParameter) { return "method"; }
        public string TheMethod() { return this.FirstName + " " + this.LastName; }
    }

    public class Fruit {
        public string Name;
        public string Color { get; set; }

        public override string ToString() {
            return string.Format("A {0} {1}", this.Color, this.Name);
        }
    }
}
