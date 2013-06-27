using NUnit.Framework;
using TavernWench.Exceptions;
using TavernWench.Test.Common;

namespace TavernWench.Test.NoPersistence {
    /// <summary>
    /// Tests over the TavernWench's configuration
    /// </summary>
    [TestFixture]
    public class Config {

        [SetUp]
        public void ResetConfiguration() {
            TavernWench.Forget();
        }

        // -------------------------------------------------------
        // Configuring Id
        // -------------------------------------------------------

        [Test]
        public void ConfigFieldAsId() {
            TavernWench.Config<User>(m => { m.SetKey(u => u.FirstName); });
            Assert.That(TavernWench.Config<User>().Key, Is.EqualTo("FirstName"));
        }

        [Test]
        public void ConfigPropertyAsId() {
            TavernWench.Config<User>(m => { m.SetKey(u => u.Id); });
            Assert.That(TavernWench.Config<User>().Key, Is.EqualTo("Id"));
        }

        [Test]
        public void ConfigVirtualPropertyAsId() {
            TavernWench.Config<User>(m => { m.SetKey(u => u.LastName); });
            Assert.That(TavernWench.Config<User>().Key, Is.EqualTo("LastName"));
        }

        [Test]
        public void ConfigMethodAsId() {
            TavernWench.Config<User>(m => { m.SetKey(u => u.TheMethod()); });
            Assert.That(TavernWench.Config<User>().Key, Is.EqualTo("TheMethod"));
        }

        [Test]
        public void CannotMapAMethodWithParameterAsKey() {
            Assert.Throws<CantUseMethodWithParametersAsKeyException>(() => {
                TavernWench.Config<User>(m => { m.SetKey(u => u.TheMethod("TheParameter")); });
            });
        }

        // -------------------------------------------------------
        // Configuring Persistance
        // -------------------------------------------------------
        [Test]
        public void ByDefaultDoNotOverwritePreviousConfigs() {
            TavernWench.Config<User>(m => { m.Persist = true; });
            TavernWench.Config<User>(m => { m.TableName = "tbl_User"; });
            TavernWench.Config<User>(m => { m.SetDatabasePk(u => u.LastName); });

            Assert.That(TavernWench.Config<User>().Persist, Is.True);
            Assert.That(TavernWench.Config<User>().TableName, Is.EqualTo("tbl_User"));
            Assert.That(TavernWench.Config<User>().DatabasePk, Is.EqualTo("LastName"));
            Assert.That(TavernWench.Config<User>().Key, Is.Null);
        }

        [Test]
        public void OverwritePreviousConfigIfITellSo() {
            TavernWench.Config<User>(m => { m.Persist = true; });
            TavernWench.Config<User>(m => { m.TableName = "tbl_User"; }, false);
            
            Assert.That(TavernWench.Config<User>().Persist, Is.Null);
            Assert.That(TavernWench.Config<User>().TableName, Is.EqualTo("tbl_User"));
        }

        // -------------------------------------------------------
        // Misc
        // -------------------------------------------------------
        [Test]
        public void OverwriteConfig() {
            TavernWench.Config<User>(m => { m.SetKey(u => u.LastName);
                                              m.Persist = true; });
            Assert.That(TavernWench.Config<User>().Key, Is.EqualTo("LastName"));
            Assert.That(TavernWench.Config<User>().Persist, Is.True);

            TavernWench.Config<User>(m => { m.SetKey(u => u.FirstName);
                                              m.Persist = false;});
            Assert.That(TavernWench.Config<User>().Key, Is.EqualTo("FirstName"));
            Assert.That(TavernWench.Config<User>().Persist, Is.False);
        }

        [Test]
        public void CannotAskForNonExistentConfig() {
            Assert.Throws<NoConfigurationFoundForThisClassException>(() => { 
                var a = TavernWench.Config<User>().Key; });
        }

    }
}
