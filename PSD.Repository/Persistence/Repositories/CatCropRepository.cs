using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class CatCropRepository : Repository<Cat_Crop>, ICatCropRepository
    {
        public CatCropRepository(PSDContext context)
            : base(context)
        {
        }
        
        public new IEnumerable<Cat_Crop> GetAll()
        {
            return PSDContext.Crops.OrderBy(x => x.CropCategory.Name).ThenBy(x => x.Name).ToList();
        }
    }
}