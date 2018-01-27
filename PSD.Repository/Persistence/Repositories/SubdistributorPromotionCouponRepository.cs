using System.Data.Entity;
using System.Linq;
using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;

namespace PSD.Repository.Persistence.Repositories
{
    public class SubdistributorPromotionCouponRepository : Repository<SubdistributorPromotionCoupon>, ISubdistributorPromotionCouponRepository
    {
        public SubdistributorPromotionCouponRepository(PSDContext context)
            : base(context)
        {
        }
    }
}