namespace Memofy.Dtos
{
    public class PersonDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Birthday { get; set; }
        public string? Nameday { get; set; }
    }
}
