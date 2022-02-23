using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Egor.Models
{
	public class EgorContext : IdentityDbContext<User>
	{
		public DbSet<Discipline> Disciplines { get; set; }
		public DbSet<TypeProgram> TypesProgram { get; set; }
		public DbSet<Profile> Profiles { get; set; }
		public DbSet<Dept> Depts { get; set; }
		public EgorContext(DbContextOptions<EgorContext> options)
			: base(options)
		{
			Database.EnsureCreated();   // создаем базу данных при первом обращении
		}
	}
}
