using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class PersonDbContext : DbContext
	{
        public PersonDbContext(DbContextOptions options) :base(options)
        {
            
        }
        public DbSet<Country> Countries { get; set; }
		public DbSet<Person> Persons { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Country>().ToTable("Countries");
			modelBuilder.Entity<Person>().ToTable("Persons");

			//seed to Countries
			string CountryJson = System.IO.File.ReadAllText("countries.json");
			List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(CountryJson);

			foreach(Country country in countries)
				modelBuilder.Entity<Country>().HasData(country);

			//seed to Persons
			string PersonJson = System.IO.File.ReadAllText("persons.json");
			List<Person> Persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(PersonJson);

			foreach (Person person in Persons)
				modelBuilder.Entity<Person>().HasData(person);

		}
	}
}
