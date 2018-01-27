using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class CatCropCategoryRepository : Repository<Cat_CropCategory>, ICatCropCategoryRepository
    {
        public CatCropCategoryRepository(PSDContext context)
            : base(context)
        {
        }
    }
}