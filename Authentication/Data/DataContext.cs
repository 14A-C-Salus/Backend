namespace Authentication.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseSqlServer("Server=SalusDB.mssql.somee.com;Database=SalusDB;User Id=AnoBlade_SQLLogin_1;Password=hmw5xto7f8;");
        }

        public DbSet<Auth> auths => Set<Auth>();
    }
}
