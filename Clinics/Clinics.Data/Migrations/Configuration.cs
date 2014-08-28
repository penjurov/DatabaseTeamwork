namespace Clinics.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<ClinicsDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
            ContextKey = "Clinics.Data.ClinicsDBContex";
        }

        protected override void Seed(ClinicsDBContext context)
        {
        }
    }
}

