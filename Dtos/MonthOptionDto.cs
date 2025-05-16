namespace Memofy.Dtos
{
    public class MonthOptionDto
    {
        public int Number { get; set; }      // 1–12
        public required string Name { get; set; }     // "Január", "Február" stb.
    }
}
