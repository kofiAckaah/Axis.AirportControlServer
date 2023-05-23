using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.DAL.DbContexts
{
    public class AircraftAPIDbContextFactory : IDesignTimeDbContextFactory<AircraftAPIDbContext>
    {
        //TODO: change this to use constring configuration in shared
        const string connectionString = "Data Source=.;Initial Catalog=ControlServerDb;Integrated Security=True;MultipleActiveResultSets=True";
        public AircraftAPIDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AircraftAPIDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AircraftAPIDbContext(optionsBuilder.Options, null);
        }
    }
}
