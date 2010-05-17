using System.IO;
using FlightsNorway.Phone.Model;
using FlightsNorway.Phone.Services;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Phone.Tests.Services
{
    [TestClass]
    public class ObjectStoreSpecs
    {
        [TestMethod, Tag(Tags.Services)]
        public void Can_persist_a_simple_object()
        {
            var airport = new Airport("LKL", "Lakselv");

            objectStore.Save(airport, "selected_airport");
            var airportRead = objectStore.Load<Airport>("selected_airport");
           

            Assert.AreEqual(airport.Code, airportRead.Code);
            Assert.AreEqual(airport.Name, airportRead.Name);
        }

        [TestMethod, Tag(Tags.Services), ExpectedException(typeof(FileNotFoundException))]
        public void Can_delete_an_object()
        {
            var airport = new Airport("LKL", "Lakselv");
            objectStore.Save(airport, "selected_airport");
            objectStore.Delete("selected_airport");
            objectStore.Load<Airport>("selected_airport");
        }

        [TestMethod, Tag(Tags.Services)]
        public void Can_verify_that_an_object_do_not_exists()
        {
            Assert.IsFalse(objectStore.FileExists("some_file"));
        }

        [TestMethod, Tag(Tags.Services)]
        public void Can_verify_that_an_object_do_exists()
        {
            objectStore.Save(new Airport("LKL", "Lakselv"), "some_file");
            bool exists = objectStore.FileExists("some_file");
            objectStore.Delete("some_file");
            Assert.IsTrue(exists);
        }

        [TestInitialize]
        public void Setup()
        {
            objectStore = new ObjectStore();
        }

        private ObjectStore objectStore;
    }
}
