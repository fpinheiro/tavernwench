using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using IndustryWench.Exceptions;

namespace IndustryWench.Test {
    /// <summary>
    /// Summary description for RegiterTest
    /// </summary>
    [TestFixture]
    public class ConfigTest {

        public class User  {
            public int Id { get; set; }
            public string FirstName;
            public virtual string LastName { get;set; }

            public string TheMethod() { return "method"; }
        }

        [SetUp]
        public void ResetConfiguration() {
            IndustryWench.Forget();
        }

        // -------------------------------------------------------
        // Configuring Id
        // -------------------------------------------------------

        [Test]
        public void ConfigFieldAsId() {
            IndustryWench.Config<User>(m => { m.SetId(u => u.FirstName); });
            Assert.That(IndustryWench.Config<User>().Id, Is.EqualTo("FirstName"));
        }

        [Test]
        public void ConfigPropertyAsId() {
            IndustryWench.Config<User>(m => { m.SetId(u => u.Id); });
            Assert.That(IndustryWench.Config<User>().Id, Is.EqualTo("Id"));
        }

        [Test]
        public void ConfigVirtualPropertyAsId() {
            IndustryWench.Config<User>(m => { m.SetId(u => u.LastName); });
            Assert.That(IndustryWench.Config<User>().Id, Is.EqualTo("LastName"));
        }

        [Test]
        public void CannotMapAMethodAsId() {
            Assert.Throws<IdMustBeAPropertyOrFieldException>(() => {
                IndustryWench.Config<User>(m => { m.SetId(u => u.TheMethod()); });
            });
        }

        [Test]
        public void OverwriteConfig() {
            IndustryWench.Config<User>(m => { m.SetId(u => u.LastName); });
            Assert.That(IndustryWench.Config<User>().Id, Is.EqualTo("LastName"));

            IndustryWench.Config<User>(m => { m.SetId(u => u.FirstName); });
            Assert.That(IndustryWench.Config<User>().Id, Is.EqualTo("FirstName"));
        }

        [Test]
        public void CannotAskForNonExistentConfig() {
            Assert.Throws<UnmappedClassException>(() => { var a = IndustryWench.Config<User>().Id; });
        }

        // -------------------------------------------------------
        // Configuring Persistance
        // -------------------------------------------------------
      

    }
}
