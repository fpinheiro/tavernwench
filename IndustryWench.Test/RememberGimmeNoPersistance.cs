using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using IndustryWench.Exceptions;

namespace IndustryWench.Test {
    /// <summary>
    /// Remember, remeber the 5th of November
    /// </summary>
    [TestFixture]
    public class RememberGimmeNoPersistance {

        #region mock_data

        private const string _johnnysFirstName = "Johnny";
        private const string _johnnysLastName = "Goode";
        private const string _johnnysChangedLastName = "Knoxville";

        private const string _marysFirstName = "Mary";
        private const string _marysLastName = "Jane";

        private const string _banana = "Banana";

        #endregion

        #region helpers
        private User RememberAndGimme(string firstName, string lastName) {
            IndustryWench.Remember(() => new User { FirstName = firstName,
                                                    LastName = lastName });
            return IndustryWench.Gimme<User>(firstName);
        }
        #endregion

        [SetUp]
        public void ResetConfiguration() {
            IndustryWench.Forget();
            IndustryWench.Config<User>(m => { m.SetId(u => u.FirstName); });
            IndustryWench.Config<Fruit>(m => { m.SetId(u => u.Name); });
        }

        [Test]
        public void RemebersMultipleItemsOfTheSameType() {
            var johnny = RememberAndGimme(_johnnysFirstName, _johnnysLastName);

            Assert.That(johnny.FirstName, Is.EqualTo(_johnnysFirstName));
            Assert.That(johnny.LastName, Is.EqualTo(_johnnysLastName));

            var mary = RememberAndGimme(_marysFirstName, _marysLastName);

            Assert.That(mary.FirstName, Is.EqualTo(_marysFirstName));
            Assert.That(mary.LastName, Is.EqualTo(_marysLastName));
        }

        [Test]
        public void RemebersMultipleItemsOfDifferentTypes() {
            var johnny = RememberAndGimme(_johnnysFirstName, _johnnysLastName);

            IndustryWench.Remember(() => new Fruit { Name = _banana });
            var banana = IndustryWench.Gimme<Fruit>(_banana);

            Assert.That(johnny.FirstName, Is.EqualTo(_johnnysFirstName));
            Assert.That(johnny.LastName, Is.EqualTo(_johnnysLastName));
            Assert.That(banana.Name, Is.EqualTo(_banana));
        }

        [Test]
        public void CannotAskForUnknownType() {
            Assert.Throws<NeverHeardAboutThisClassException>(() => { IndustryWench.Gimme<User>(_johnnysFirstName); });
        }

        [Test]
        public void CannotAskForUnknownKey() {
            IndustryWench.Remember(() => new User { FirstName = _johnnysFirstName,
                                                    LastName = _johnnysChangedLastName});
            Assert.Throws<NeverHeardAboutThisKeyException>(() => { IndustryWench.Gimme<User>(_marysFirstName); });
        }

    }
}
