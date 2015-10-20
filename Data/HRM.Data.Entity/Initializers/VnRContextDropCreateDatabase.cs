using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Data.Entity.Initializers
{
    public class VnRDbContextDropCreateDatabaseAlways : DropCreateDatabaseAlways<VnrHrmDataContext>
    {

        protected override void Seed(VnrHrmDataContext context)
        {

            ISeedDatabase seeder = context as ISeedDatabase;
            if (seeder != null)
            {
                seeder.Seed();
            }
        }
    }

    public class VnRDbContextDropCreateDatabaseIfModelChanges : DropCreateDatabaseIfModelChanges<VnrHrmDataContext>
    {

        protected override void Seed(VnrHrmDataContext context)
        {

            ISeedDatabase seeder = context as ISeedDatabase;
            if (seeder != null)
            {
                seeder.Seed();
            }
        }
    }

    public class VnRDbContextCreateDatabaseIfNotExists : CreateDatabaseIfNotExists<VnrHrmDataContext>
    {

        protected override void Seed(VnrHrmDataContext context)
        {

            ISeedDatabase seeder = context as ISeedDatabase;
            if (seeder != null)
            {
                seeder.Seed();
            }
        }
    }
}
