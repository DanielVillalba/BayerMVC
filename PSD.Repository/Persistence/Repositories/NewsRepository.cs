using PSD.Model;
using PSD.Repository.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Repository.Persistence.Repositories
{
    public class NewsRepository : Repository<News>, INewsRepository
    {
        public NewsRepository(PSDContext context)
            : base(context)
        {

        }
    }
}
