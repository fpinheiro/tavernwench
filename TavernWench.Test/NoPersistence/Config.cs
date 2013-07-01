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
            TavernWench.Config<Actor>(m => { m.SetKey(u => u.FirstName); });
            Assert.That(TavernWench.Config<Actor>().Key, Is.EqualTo("FirstName"));
        }

        [Test]
        public void ConfigPropertyAsId() {
            TavernWench.Config<Actor>(m => { m.SetKey(u => u.Id); });
            Assert.That(TavernWench.Config<Actor>().Key, Is.EqualTo("Id"));
        }

        [Test]
        public void ConfigVirtualPropertyAsId() {
            TavernWench.Config<Actor>(m => { m.SetKey(u => u.LastName); });
            Assert.That(TavernWench.Config<Actor>().Key, Is.EqualTo("LastName"));
        }

        [Test]
        public void ConfigMethodAsId() {
            TavernWench.Config<Actor>(m => { m.SetKey(u => u.TheMethod()); });
            Assert.That(TavernWench.Config<Actor>().Key, Is.EqualTo("TheMethod"));
        }

        [Test]
        public void CannotMapAMethodWithParameterAsKey() {
            Assert.Throws<CantUseMethodWithParametersAsKeyException>(() => {
                TavernWench.Config<Actor>(m => { m.SetKey(u => u.TheMethod("TheParameter")); });
            });
        }

        // -------------------------------------------------------
        // Configuring Persistance
        // -------------------------------------------------------
        [Test]
        public void ByDefaultDoNotOverwritePreviousConfigs() {
            TavernWench.Config<Actor>(m => { m.Persist = true; });
            TavernWench.Config<Actor>(m => { m.TableName = "tbl_User"; });
            TavernWench.Config<Actor>(m => { m.SetDatabasePk(u => u.LastName); });

            Assert.That(TavernWench.Config<Actor>().Persist, Is.True);
            Assert.That(TavernWench.Config<Actor>().TableName, Is.EqualTo("tbl_User"));
            Assert.That(TavernWench.Config<Actor>().DatabasePk, Is.EqualTo("LastName"));
            Assert.That(TavernWench.Config<Actor>().Key, Is.EqualTo("ToString"));
        }

        [Test]
        public void OverwritePreviousConfigIfITellSo() {
            TavernWench.Config<Actor>(m => { m.Persist = true; });
            TavernWench.Config<Actor>(m => { m.TableName = "tbl_User"; }, false);
            
            Assert.That(TavernWench.Config<Actor>().Persist, Is.Null);
            Assert.That(TavernWench.Config<Actor>().TableName, Is.EqualTo("tbl_User"));
        }

        [Test]
        public void OnlyPropertyCanBePK() {
            Assert.Throws<DatabasePKMustBeProperty>(() => {
                TavernWench.Config<Actor>(m => { m.SetDatabasePk(u => u.FirstName); });
            });

            Assert.Throws<DatabasePKMustBeProperty>(() => {
                TavernWench.Config<Actor>(m => { m.SetDatabasePk(u => u.TheMethod()); });
            });

            Assert.DoesNotThrow(() => {
                TavernWench.Config<Actor>(m => { m.SetDatabasePk(u => u.Id); });
            });
        }

        // -------------------------------------------------------
        // Misc
        // -------------------------------------------------------
        [Test]
        public void OverwriteConfig() {
            TavernWench.Config<Actor>(m => { m.SetKey(u => u.LastName);
                                              m.Persist = true; });
            Assert.That(TavernWench.Config<Actor>().Key, Is.EqualTo("LastName"));
            Assert.That(TavernWench.Config<Actor>().Persist, Is.True);

            TavernWench.Config<Actor>(m => { m.SetKey(u => u.FirstName);
                                              m.Persist = false;});
            Assert.That(TavernWench.Config<Actor>().Key, Is.EqualTo("FirstName"));
            Assert.That(TavernWench.Config<Actor>().Persist, Is.False);
        }
    }
}
