using IndustryWench.Exceptions;
using IndustryWench.Test.Common;
using NUnit.Framework;

namespace IndustryWench.Test.NoContextNorPersistance {
    /// <summary>
    /// Remember, remeber the 5th of November
    /// </summary>
    [TestFixture]
    public class RememberGimmeNoPersistance {

        #region mock_data
        private const string _jennasFirstName = "Jenna";
        private const string _jennasLastName = "Jameson";
        private const string _jennasChangedLastName = "Haze";

        private const string _petersFirstName = "Peter";
        private const string _petersLastName = "North";

        private const string _banana = "Banana";
        private const string _apple = "Apple";
        private const string _red = "red";
        private const string _yellow = "yellow";

        #endregion

        #region helpers
        private User RememberAndGimme(string firstName, string lastName) {
            IndustryWench.Remember(() => new User {
                FirstName = firstName,
                LastName = lastName
            });
            return IndustryWench.Gimme<User>(firstName);
        }
        #endregion

        [SetUp]
        public void ResetConfiguration() {
            IndustryWench.Forget();
            IndustryWench.Config<User>(m => { m.SetId(u => u.FirstName); });
        }

        [Test]
        public void RemebersMultipleItemsOfTheSameType() {
            var johnny = RememberAndGimme(_jennasFirstName, _jennasLastName);

            Assert.That(johnny.FirstName, Is.EqualTo(_jennasFirstName));
            Assert.That(johnny.LastName, Is.EqualTo(_jennasLastName));

            var mary = RememberAndGimme(_petersFirstName, _petersLastName);

            Assert.That(mary.FirstName, Is.EqualTo(_petersFirstName));
            Assert.That(mary.LastName, Is.EqualTo(_petersLastName));
        }

        [Test]
        public void RemebersMultipleItemsOfDifferentTypesWithConfig() {
            var johnny = RememberAndGimme(_jennasFirstName, _jennasLastName);

            IndustryWench.Config<Fruit>(m => { m.SetId(u => u.Name); });
            IndustryWench.Remember(() => new Fruit { Name = _banana });
            var banana = IndustryWench.Gimme<Fruit>(_banana);

            Assert.That(johnny.FirstName, Is.EqualTo(_jennasFirstName));
            Assert.That(johnny.LastName, Is.EqualTo(_jennasLastName));
            Assert.That(banana.Name, Is.EqualTo(_banana));
        }

        [Test]
        public void RememberUnconfiguredType() {
            IndustryWench.Remember(() => new Fruit { Name = _banana, Color = _yellow });
            IndustryWench.Remember(() => new Fruit { Name = _apple, Color = _red });

            // since we did'nt configured the type the Wench will assume that the first declared property is the key
            var banana = IndustryWench.Gimme<Fruit>("A " + _yellow + " " + _banana);
            var apple = IndustryWench.Gimme<Fruit>("A " + _red + " " + _apple);

            Assert.That(banana.ToString(), Is.EqualTo("A " + _yellow + " " + _banana));
            Assert.That(apple.ToString(), Is.EqualTo("A " + _red + " " + _apple));
        }

        [Test]
        public void CannotAskForUnknownType() {
            Assert.Throws<NeverHeardAboutThisClassException>(() => { IndustryWench.Gimme<User>(_jennasFirstName); });
        }

        [Test]
        public void CannotAskForUnknownKey() {
            IndustryWench.Remember(() => new User {
                FirstName = _jennasFirstName,
                LastName = _jennasChangedLastName
            });
            Assert.Throws<NeverHeardAboutThisKeyException>(() => { IndustryWench.Gimme<User>(_petersFirstName); });
        }
    }
}
