using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BackEnd.DAL.DbContexts
{
    public class ControlServerContextFactory : IDesignTimeDbContextFactory<ControlServerDbContext>
    {
        //TODO: change this to use constring configuration in shared
        const string connectionString = "Data Source=.;Initial Catalog=WebAppDb;Integrated Security=True;MultipleActiveResultSets=True";
        public ControlServerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ControlServerDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ControlServerDbContext(optionsBuilder.Options, null);
        }
    }
}
