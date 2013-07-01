
using TavernWench.Exceptions;
using TavernWench.Test.Common;
using NUnit.Framework;

namespace TavernWench.Test.NoPersistence {

    using PetaPoco;

    /// <summary>
    /// Remember, remeber the 5th of November
    /// </summary>
    [TestFixture]
    public class RememberGimmePersistence {

        private Database _createDatabaseConn;
        private Database _testDatabaseConn;

        [TestFixtureSetUp]
        public void CreateDatabaseAndStuff() {
            //creating the database
            _createDatabaseConn = new Database("create_db_conn");
            _createDatabaseConn.Execute("CREATE DATABASE TavernWenchTestDb");

            //creating test tables
            _testDatabaseConn = new Database("test_db_conn");
            _testDatabaseConn.Execute(@"CREATE TABLE Fruit (
                                              Name VARCHAR(60),
                                              Color VARCHAR(60)
                                        );");

            _testDatabaseConn.Execute(@"CREATE TABLE Actor (
                                              Id INT IDENTITY(1,1) NOT NULL,
                                              LastName VARCHAR(60)
                                        );");
        }

        [SetUp]
        public void ResetInstances() {
            TavernWench.Forget();
        }

        [Test]
        public void PersistWithExplicitConfig() {
            TavernWench.Config<Fruit>(m => { m.SetKey(u => u.Name);
                                             m.Persist = true;
                                             m.TableName = "Fruit";
                                             m.SetDatabasePk(u => u.Name);
            });

            TavernWench.Remember<Fruit>(() => new Fruit { Name = "Banana",
                                                          Color = "Yellow"});
            Fruit banana = null;
            Assert.DoesNotThrow(() => {
                banana = _testDatabaseConn.Single<Fruit>("select * from Fruit where Name = 'Banana'");
            });

            Assert.That(banana.Name, Is.EqualTo("Banana"));
            Assert.That(banana.Color, Is.EqualTo("Yellow"));
        }

        [Test]
        public void PersistWithImplicitConfig() {
            TavernWench.Config<Actor>(m => m.Persist = true);

            TavernWench.Remember<Actor>(() => new Actor { LastName = "Jenna" });

            Actor user = null;
            Assert.DoesNotThrow(() => {
                user = _testDatabaseConn.Single<Actor>("select * from Actor where LastName = 'Jenna'");
            });

            Assert.That(user.Id, Is.Not.Null);
        }

        [TestFixtureTearDown]
        public void DropDatabaseAndStuff() {
            _testDatabaseConn.Dispose();

            _createDatabaseConn.Execute("ALTER DATABASE TavernWenchTestDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            _createDatabaseConn.Execute("DROP DATABASE TavernWenchTestDb");

            _createDatabaseConn.Dispose();
        }
    }
}
