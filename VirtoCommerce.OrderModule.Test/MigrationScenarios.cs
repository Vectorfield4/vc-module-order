using System.Linq;
using VirtoCommerce.OrderModule.Data.Migrations;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Testing.Bases;
using Xunit;

namespace VirtoCommerce.OrderModule.Test
{
    public class MigrationScenarios : MigrationsTestBase
    {
        [Fact]
        public void Can_create_order_new_database()
        {
            DropDatabase();

            var migrator = CreateMigrator<Configuration>();

            using (var context = CreateContext<OrderRepositoryImpl>())
            {
                context.Database.CreateIfNotExists();
                new SetupDatabaseInitializer<OrderRepositoryImpl, Configuration>().InitializeDatabase(context);
                Assert.Equal(0, context.CustomerOrders.Count());
            }

            // remove all migrations
            migrator.Update("0");
            Assert.False(TableExists("OrderLineItem"));
            var existTables = Info.Tables.Any();
            Assert.False(existTables);

            DropDatabase();
        }
    }
}
