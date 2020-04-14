using FamilyFinace.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.Repository
{
    public class RepositoryContext : DbContext
    {
        private readonly IHashGenerator hashGenerator;
        public RepositoryContext(IHashGenerator hashGenerator, DbContextOptions<RepositoryContext> options) : base(options)
        {
            this.hashGenerator = hashGenerator;
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
