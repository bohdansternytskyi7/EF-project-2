namespace TripCompany.DAL
{
    public partial class Country
    {
        public Country()
        {
            Trips = new HashSet<Trip>();
        }

        public int IdCountry { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Trip> Trips { get; set; }
    }
}
