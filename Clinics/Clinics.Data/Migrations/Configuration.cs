namespace Clinics.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<ClinicsDBContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = false;
            this.ContextKey = "Clinics.Data.ClinicsDBContex";
        }

        protected override void Seed(ClinicsDBContext context)
        {
        }
    }
}