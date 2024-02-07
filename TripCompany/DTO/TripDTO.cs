namespace TripCompany.DTO
{
    public class TripDTO
    {
        public TripDTO()
        {
            Clients = new HashSet<ClientDTO>();
            Countries = new HashSet<CountryDTO>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }

        public virtual ICollection<ClientDTO> Clients { get; set; }

        public virtual ICollection<CountryDTO> Countries { get; set; }
    }
}
