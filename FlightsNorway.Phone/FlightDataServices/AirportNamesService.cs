using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

using FlightsNorway.Phone.Model;

namespace FlightsNorway.Phone.FlightDataServices
{
    public class AirportNamesService : IGetAirports
    {        
        private readonly Uri _serviceUrl;

        public AirportNamesService()
        {
            _serviceUrl = new Uri("http://flydata.avinor.no/airportNames.asp");    
        }

        public IObservable<IEnumerable<Airport>> GetAirports()
        {
            return WebRequestFactory.GetData(_serviceUrl, ParseAirportXml);
        }

        private static IEnumerable<Airport> ParseAirportXml(XmlReader reader)
        {
            var xml = XDocument.Load(reader);

            return from airportNames in xml.Elements("airportNames")
                   from airport in airportNames.Elements("airportName")
                   select new Airport
                              {
                                  Code = airport.Attribute("code").Value,
                                  Name = airport.Attribute("name").Value
                              };
        }

        public IEnumerable<Airport> GetNorwegianAirports()
        {
            yield return new Airport("ALF", "Alta");
            yield return new Airport("ANX", "Andøya");
            yield return new Airport("BDU", "Bardufoss");
            yield return new Airport("BGO", "Bergen");
            yield return new Airport("BVG", "Berlevåg");
            yield return new Airport("BOO", "Bodø");
            yield return new Airport("BNN", "Brønnøysund");
            yield return new Airport("BJF", "Båtsfjord");
            yield return new Airport("DLD", "Dagali");
            yield return new Airport("VDB", "Fagernes");
            yield return new Airport("FAN", "Farsund");
            yield return new Airport("FRO", "Florø");
            yield return new Airport("FDE", "Førde");
            yield return new Airport("HMR", "Hamar");
            yield return new Airport("HFT", "Hammerfest");
            yield return new Airport("EVE", "Harstad/Narvik");
            yield return new Airport("HAA", "Hasvik");
            yield return new Airport("HAU", "Haugesund");
            yield return new Airport("HVG", "Honningsvåg");
            yield return new Airport("KKN", "Kirkenes");
            yield return new Airport("KRS", "Kristiansand");
            yield return new Airport("KSU", "Kristiansund");
            yield return new Airport("LKL", "Lakselv");
            yield return new Airport("LKN", "Leknes");
            yield return new Airport("MEH", "Mehamn");
            yield return new Airport("MQN", "Mo i Rana");
            yield return new Airport("MOL", "Molde");
            yield return new Airport("MJF", "Mosjøen");
            yield return new Airport("RYG", "Moss");
            yield return new Airport("OSY", "Namsos");
            yield return new Airport("NTB", "Notodden");
            yield return new Airport("OSL", "Oslo");
            yield return new Airport("RRS", "Røros");
            yield return new Airport("RVK", "Rørvik");
            yield return new Airport("RET", "Røst");
            yield return new Airport("SDN", "Sandane");
            yield return new Airport("TRF", "Sandefjord");
            yield return new Airport("SSJ", "Sandnessjøen");
            yield return new Airport("SKE", "Skien");
            yield return new Airport("SOG", "Sogndal");
            yield return new Airport("SVG", "Stavanger");
            yield return new Airport("SKN", "Stokmarknes");
            yield return new Airport("SRP", "Stord");
            yield return new Airport("LYR", "Svalbard");
            yield return new Airport("SVJ", "Svolvær");
            yield return new Airport("SOJ", "Sørkjosen");
            yield return new Airport("TOS", "Tromsø");
            yield return new Airport("TRD", "Trondheim");
            yield return new Airport("VDS", "Vadsø");
            yield return new Airport("VAW", "Vardø");
            yield return new Airport("VRY", "Værøy");
            yield return new Airport("OLA", "Ørland");
            yield return new Airport("HOV", "Ørsta/Volda");
            yield return new Airport("AES", "Ålesund");
        }
    }
}