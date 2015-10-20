using HRM.Data.BaseRepository;
using HRM.Data.Entity.Initializers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Data.Entity
{
    public class RepositoryInitializer : IRepositoryInitializer
    {
        public RepositoryInitializer()
        {
            // Sets the default database initialization code for working with Sql Server Compact databases
            Database.SetInitializer<VnrHrmDataContext>(new VnRDbContextDropCreateDatabaseAlways());
        }

        public void Initialize()
        {

        }
    }
}
