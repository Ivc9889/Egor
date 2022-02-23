using Egor.Models;

namespace Egor.ViewModels
{
    public class ShowProgramViewModel
    {
        public IEnumerable<TypeProgram> TypesProgram { get; set; }
        public IEnumerable<Discipline> Disciplines { get; set; }
    }
}
