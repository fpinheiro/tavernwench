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
    public class RememberTest {

        [SetUp]
        public void ResetConfiguration() {
            IndustryWench.Forget();
        }

        [Test]
        public void RememberReturnsActorImmediately() {
            IndustryWench.Config<ConfigTest.User>(m => { m.SetId(u => u.FirstName); });

            var johnny = IndustryWench.Remember(() => new ConfigTest.User { FirstName = "Johnny",
                                                                            LastName = "Goode" });

            Assert.That(johnny.FirstName, Is.EqualTo("Johnny"));
        }
    }
}
