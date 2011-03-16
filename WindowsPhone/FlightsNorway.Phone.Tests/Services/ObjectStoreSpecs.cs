using System.IO;
using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.Services;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightsNorway.Tests.Services
{
    [TestClass]
    public class ObjectStoreSpecs
    {
        [TestMethod, Tag(Tags.Services)]
        public void Can_persist_a_simple_object()
        {
            var airport = new Airport("LKL", "Lakselv");

            _objectStore.Save(airport, Airport.SelectedAirportFilename);
            var airportRead = _objectStore.Load<Airport>(Airport.SelectedAirportFilename);
           
            Assert.AreEqual(airport.Code, airportRead.Code);
            Assert.AreEqual(airport.Name, airportRead.Name);
        }

        [TestMethod, Tag(Tags.Services), ExpectedException(typeof(FileNotFoundException))]
        public void Can_delete_an_object()
        {
            var airport = new Airport("LKL", "Lakselv");
            _objectStore.Save(airport, Airport.SelectedAirportFilename);
            _objectStore.Delete(Airport.SelectedAirportFilename);
            _objectStore.Load<Airport>(Airport.SelectedAirportFilename);
        }

        [TestMethod, Tag(Tags.Services)]
        public void Can_verify_that_an_object_do_not_exists()
        {
            Assert.IsFalse(_objectStore.FileExists("some_file"));
        }

        [TestMethod, Tag(Tags.Services)]
        public void Can_verify_that_an_object_do_exists()
        {
            _objectStore.Save(new Airport("LKL", "Lakselv"), "some_file");
            bool exists = _objectStore.FileExists("some_file");
            _objectStore.Delete("some_file");
            Assert.IsTrue(exists);
        }

        [TestInitialize]
        public void Setup()
        {
            _objectStore = new ObjectStore();
        }

        private ObjectStore _objectStore;
    }
}
