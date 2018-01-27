using System.Data.Entity;
using System.Linq;
using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;

namespace PSD.Repository.Persistence.Repositories
{
    public class SubdistributorDiscountCouponRepository : Repository<SubdistributorDiscountCoupon>, ISubdistributorDiscountCouponRepository
    {
        public SubdistributorDiscountCouponRepository(PSDContext context)
            : base(context)
        {
        }
    }
}