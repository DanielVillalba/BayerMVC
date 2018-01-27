using PSD.Model;
using PSD.Repository.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Repository.Persistence.Repositories
{
    public class ContentDatarepository : Repository<ContentData>, IContentDataRepository
    {
        public ContentDatarepository(PSDContext context)
            : base(context)
        {
        }
    }
}
