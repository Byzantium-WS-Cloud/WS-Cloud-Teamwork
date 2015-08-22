namespace FacebookSystem.Data.Migrations
{
    using System;
    using System.Linq;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    using FacebookSystem.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<FacebookDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(FacebookDbContext context)
        {
            
        }
    }
}
