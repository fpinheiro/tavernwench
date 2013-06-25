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

        public class IUser {
            string MiddleName { get;set; }
        }

        public class User  {
            public int Id { get; set; }
            public string FirstName;
            public virtual string LastName { get;set; }
        }

        [Test]
        public void RegisterFieldAsId() {
            IndustryWench.Config<User>(m => { m.SetId(u => u.FirstName); });
            Assert.That(IndustryWench.Config<User>().Id, Is.EqualTo("FirstName"));
        }

        [Test]
        public void RegisterPropertyAsId() {
            IndustryWench.Config<User>(m => { m.SetId(u => u.Id); });
            Assert.That(IndustryWench.Config<User>().Id, Is.EqualTo("Id"));
        }

        [Test]
        public void RegisterPropertyFromInterface() {
            IndustryWench.Config<User>(m => { m.SetId(u => u.LastName); });
            Assert.That(IndustryWench.Config<User>().Id, Is.EqualTo("LastName"));
        }

        [Test]
        public void OverwriteConfig() {
            IndustryWench.Config<User>(m => { m.SetId(u => u.LastName); });
            Assert.That(IndustryWench.Config<User>().Id, Is.EqualTo("LastName"));

            IndustryWench.Config<User>(m => { m.SetId(u => u.FirstName); });
            Assert.That(IndustryWench.Config<User>().Id, Is.EqualTo("FirstName"));
        }

        [Test]
        public void AskForNonExistentConfig() {
            Assert.Throws<UnmappedClassException>(() => { var a = IndustryWench.Config<User>().Id; });
        }

        [SetUp]
        public void ResetConfiguration() {
            IndustryWench.Clear();
        }

    }
}
