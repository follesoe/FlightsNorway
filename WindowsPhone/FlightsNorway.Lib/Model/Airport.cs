namespace FlightsNorway.Lib.Model
{
    public class Airport
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Code, Name);
        }
    }

}
