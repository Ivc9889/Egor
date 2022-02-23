namespace Egor.Models
{
    public class Discipline
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int TypeProgramId { get; set; }
        public string? Content { get; set; }
    }
}
