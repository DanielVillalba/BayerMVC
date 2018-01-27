using PSD.Repository.Core;
using PSD.Repository.Core.Repositories;
using PSD.Repository.Persistence.Repositories;

namespace PSD.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PSDContext _context;
        private bool isDisposed;

        public UnitOfWork(PSDContext context)
        {
            isDisposed = false;
            _context = context;

            AppConfigurations = new AppConfigurationRepository(_context);
    
            //catalogs
            UserRoles = new CatUserRoleRepository(_context);
            UserStatuses = new CatUserStatusRepository(_context);
            Zones = new CatZoneRepository(_context);
            DistributorBranchContactAreas = new CatDistributorBranchContactAreaRepository(_context);
            DistributorContactAreas = new CatDistributorContactAreaRepository(_context);
            SubdistributorContactAreas = new CatSubdistributorContactAreaRepository(_context);
            CropCategories = new CatCropCategoryRepository(_context);
            Crops = new CatCropRepository(_context);
            ContractDistributorStatuses = new CatContractDistributorStatusRepository(_context);
            
            RolesXUser = new RolesXUserRepository(_context);
            Users = new UserRepository(_context);
            Persons = new PersonRepository(_context);

            Addresses = new AddressRepository(_context);
            AddressColonies = new AddressColonyRepository(_context);
            AddressPostalCodes = new AddressPostalCodeRepository(_context);
            AddressMunicipalities = new AddressMunicipalityRepository(_context);
            AddressStates = new AddressStateRepository(_context);

            BayerEmployees = new BayerEmployeeRepository(_context);
            MunicipalitiesXEmployee = new MunicipalitiesXEmployeeRepository(_context);
            
            DistributorBranchContacts = new DistributorBranchContactRepository(_context);
            DistributorBranches = new DistributorBranchRepository(_context);
            DistributorContacts = new DistributorContactRepository(_context);
            Distributors = new DistributorRepository(_context);
            DistributorEmployees = new DistributorEmployeeRepository(_context);
            DistributorCropsXMunicipality = new DistributorCropsXMunicipalityRepository(_context);

            Subdistributors = new SubdistributorRepository(_context);
            SubdistributorEmployees = new SubdistributorEmployeeRepository(_context);
            SubdistributorCropsXMunicipality = new SubdistributorCropsXMunicipalityRepository(_context);
            SubdistributorContacts = new SubdistributorContactRepository(_context);
            SubdistributorCommercialNames = new SubdistributorCommercialNameRepository(_context);
            AddressesXSubdistributor = new AddressesXSubdistributorRepository(_context);
            SubdistributorDiscountCoupons = new SubdistributorDiscountCouponRepository(_context);
            SubdistributorPromotionCoupons = new SubdistributorPromotionCouponRepository(_context);

            ContractsDistributor = new ContractDistributorRepository(_context);
            ContractsSubdistributor = new ContractSubdistributorRepository(_context);
            DistributorPurchasesXContractSubdistributors = new DistributorPurchasesXContractSubdistributorRepository(_context);

            //Content
            ContentLinks = new ContentLinkRepository(_context);
            News = new NewsRepository(_context);
            NewsSection = new NewsSectionRepository(_context);
            ContentData = new ContentDatarepository(_context);
        }

        public IAppConfigurationRepository AppConfigurations { get; private set; }

        #region Catalogs

        public ICatCropRepository Crops { get; private set; }
        public ICatDistributorBranchContactAreaRepository DistributorBranchContactAreas { get; private set; }
        public ICatDistributorContactAreaRepository DistributorContactAreas { get; private set; }
        public ICatSubdistributorContactAreaRepository SubdistributorContactAreas { get; private set; }
        public ICatUserRoleRepository UserRoles { get; private set; }
        public ICatUserStatusRepository UserStatuses { get; private set; }
        public ICatZoneRepository Zones { get; private set; }
        public ICatContractDistributorStatusRepository ContractDistributorStatuses { get; private set; }
        public ICatCropCategoryRepository CropCategories { get; private set; }

        #endregion

        public IAddressRepository Addresses { get; private set; }
        public IAddressColonyRepository AddressColonies { get; private set; }
        public IAddressPostalCodeRepository AddressPostalCodes { get; private set; }
        public IAddressMunicipalityRepository AddressMunicipalities { get; private set; }
        public IAddressStateRepository AddressStates { get; private set; }
        
        public IBayerEmployeeRepository BayerEmployees { get; private set; }
        public IMunicipalitiesXEmployeeRepository MunicipalitiesXEmployee { get; private set; }
        
        public IRolesXUserRepository RolesXUser { get; private set; }
        public IUserRepository Users { get; private set; }
        public IPersonRepository Persons { get; private set; }

        public IDistributorRepository Distributors { get; private set; }
        public IDistributorEmployeeRepository DistributorEmployees { get; private set; }
        public IDistributorBranchContactRepository DistributorBranchContacts { get; private set; }
        public IDistributorBranchRepository DistributorBranches { get; private set; }
        public IDistributorContactRepository DistributorContacts { get; private set; }
        public IDistributorCropsXMunicipalityRepository DistributorCropsXMunicipality { get; private set; }

        public ISubdistributorRepository Subdistributors { get; private set; }
        public ISubdistributorEmployeeRepository SubdistributorEmployees { get; private set; }
        public ISubdistributorContactRepository SubdistributorContacts { get; private set; }
        public ISubdistributorCropsXMunicipalityRepository SubdistributorCropsXMunicipality { get; private set; }
        public ISubdistributorCommercialNameRepository SubdistributorCommercialNames { get; private set; }
        public IAddressesXSubdistributorRepository AddressesXSubdistributor { get; private set; }
        public ISubdistributorDiscountCouponRepository SubdistributorDiscountCoupons { get; private set; }
        public ISubdistributorPromotionCouponRepository SubdistributorPromotionCoupons { get; private set; }

        public IContractDistributorRepository ContractsDistributor { get; private set; }
        public IContractSubdistributorRepository ContractsSubdistributor { get; private set; }
        public IDistributorPurchasesXContractSubdistributorRepository DistributorPurchasesXContractSubdistributors { get; private set; }

        #region Content
        public IContentLinkRepository ContentLinks { get; private set; }
        public INewsRepository News { get; private set; }
        public INewsSectionRepository NewsSection { get; private set; }
        public IContentDataRepository ContentData { get; set; }
        #endregion

        public int Complete()
        {
            ///TODO:remove int if not used and make direct return
            int vf = _context.SaveChanges();
            Dispose();
            //_context = new PSDContext();
            return vf;
        }

        public void Dispose()
        {
            isDisposed = true;
            _context.Dispose();
        }

        public bool IsDisposed { get { return isDisposed; } }
    }
}
