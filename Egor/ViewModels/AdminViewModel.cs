using Egor.Models;

namespace Egor.ViewModels
{
    public class AdminViewModel
    {
        public IEnumerable<Dept> Depts { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
