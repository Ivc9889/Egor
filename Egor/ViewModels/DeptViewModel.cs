using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Egor.Models;

namespace Egor.ViewModels
{
    public class DeptViewModel
    {
        public IEnumerable<Profile> Profiles { get; set; }
        public IEnumerable<TypeProgram> TypesProgram { get; set; }
        public IEnumerable<Discipline> Disciplines { get; set; }
    }
}
