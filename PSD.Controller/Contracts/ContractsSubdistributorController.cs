using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSD.Model;
using PSD.Model.Filters.PipelineAndFilters.ContractSubdistributorFilters;
using PSD.Security;

namespace PSD.Controller.Contracts
{
    public class ContractsSubdistributorController : _BaseController
    {
        public ContractsSubdistributorController(IAppConfiguration configurations)
            : base("ContractsSubdistributorController.", configurations)
        {

        }

        /// <summary>
        /// This method provides the required object for the Pipeline filtering
        /// </summary>
        /// <returns></returns>
        public IQueryable<ContractSubdistributor> RetrieveAllToFilter()
        {
            return Repository.ContractsSubdistributor.GetAllToFilter();
        }

        /// <summary>
        /// Provides a list of Contract Subdistributor as a result of filtering the DB on specific filters
        /// </summary>
        /// <returns></returns>
        public List<ContractSubdistributor> FilteredItems()
        {
            ResultManager.IsCorrect = false;
            List<ContractSubdistributor> items = new List<ContractSubdistributor>();
            try
            {
                // implementing filtering pipeline
                var contractSubistributorList = RetrieveAllToFilter();
                ContractSubdistributorFilteringPipeline pipeline = new ContractSubdistributorFilteringPipeline();

                if (Identity.CurrentUser.IsInRole(UserRole.EmployeeManagerOperation + "," + UserRole.EmployeeManagerView + "," + UserRole.EmployeeRTVOperation + "," + UserRole.EmployeeRTVView))
                {
                    EmployeeController employee = new EmployeeController(Configurations);
                    List<string> zones = employee.GetBayerEmployeeZones(CurrentUser.Id);
                    pipeline.Register(new RegisteredZoneNameListFilter(zones));
                    //pipeline.Register(new AssignedGRVFilter(CurrentUser.Id));
                }

                items = pipeline.Process(contractSubistributorList).ToList();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Index.311: Error while retrieving ContractSubdistributor list from DB: " + ex.Message);
            }
            return items;
        }

        public List<ContractSubdistributor> RetrieveAll()
        {
            ResultManager.IsCorrect = false;
            List<ContractSubdistributor> auxList;
            try
            {
                auxList = (List<ContractSubdistributor>)Repository.ContractsSubdistributor.GetAll();
                ResultManager.IsCorrect = true;
                return auxList;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "RetrieveAll.511 Excepción al obtener el listado de convenios: " + ex.Message);
            }

            return null;
        }

        public ContractSubdistributor Retrieve(int id)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no id was received
                ResultManager.Add(ErrorDefault, Trace + "Retrieve.131 No se recibio el parametro id");
                return null;
            }

            ContractSubdistributor auxItem = null;
            try
            {
                auxItem = Repository.ContractsSubdistributor.Get(id);
                if (auxItem == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "Retrieve.511 No se encontró un convenio con id '" + id + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Retrieve.511 Excepción al obtener el convenio a editar: " + ex.Message);
            }

            return auxItem;
        }
        public ContractSubdistributor RetrieveLastContract(int subdistributorId)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (subdistributorId < 1)
            {//no id was received
                ResultManager.Add(ErrorDefault, Trace + "RetrieveLastContract.131 No se recibio el parametro id");
                return null;
            }

            ContractSubdistributor auxItem = null;
            try
            {
                auxItem = Repository.ContractsSubdistributor.GetLastSubdistributorContract(subdistributorId);
                if (auxItem == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "RetrieveLastContract.511 No se encontró un convenio con id '" + subdistributorId + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "RetrieveLastContract.511 Excepción al obtener el convenio: " + ex.Message);
            }

            return auxItem;
        }
        
        /// <summary>
        /// Retrieve a new instance of a subdistributor contract, setting the initial information (discount type, last contract amounts, etc)
        /// </summary>
        /// <param name="id">Subdistributor id</param>
        /// <returns></returns>
        public ContractSubdistributor InitializeNew(int id)
        {
            ResultManager.Clear();
            ContractSubdistributor item = null;
            try
            {
                PSD.Controller.SubdistributorController subdistributorController = new Controller.SubdistributorController(Configurations);
            
                item = ContractSubdistributor.NewEmpty();
                item.Subdistributor = subdistributorController.RetrieveSubdistributor(id);
                item.SubdistributorId = id;
                item.RegisteredRegionName = item.Subdistributor.BNAddress.AddressColony.AddressPostalCode.AddressMunicipality.Zone.RegionName;
                item.RegisteredZoneName = item.Subdistributor.BNAddress.AddressColony.AddressPostalCode.AddressMunicipality.Zone.Name;

                //set discount type (for benefits program->coupons->discountTable)
                ContractSubdistributor auxLastContract = Repository.ContractsSubdistributor.GetPastYearSubdistributorContract(id);
                if (auxLastContract == null)
                {//first contract
                    item.DiscountType = 0;
                }
                else
                {//contract renovation
                    item.DiscountType = auxLastContract.DiscountType + 1;
                }

                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "InitializeNew.511 Excepción al inicializar el nuevo elemento" + ex.Message);
            }
            return item;
        }
        public bool Create(ContractSubdistributor model, bool sendToDistributorReview = true)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (model == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "Create.111 No se recibio el modelo");
                return false;
            }
            //-business validations
            if (model.SubdistributorId == 0 || model.SubdistributorId == -1)
            {
                ResultManager.Add("Se debe seleccionar un subdistribuidor");
                return false;
            }
            if (model.DistributorPurchases == null || model.DistributorPurchases.Count == 0)
            {
                ResultManager.Add("Se debe seleccionar al menos un distribuidor");
                return false;
            }
            /*
            if (model.GRVBayerEmployeeId == 0 || model.GRVBayerEmployeeId == -1)
            {
                ResultManager.Add("Se debe tener asignado un GRV");
                return false;
            }
            if (model.RTVBayerEmployeeId == 0 || model.RTVBayerEmployeeId == -1)
            {
                ResultManager.Add("Se debe tener asignado un RTV");
                return false;
            }
            */
            if (model.Year == 0 || model.Year == -1)
            {
                ResultManager.Add("Se debe indicar el año del convenio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.RegisteredZoneName))
            {
                ResultManager.Add("Debe haber una zona asignada");
                return false;
            }
            if (model.AmountGoalS1Pre == 0 || model.AmountGoalS1Pre == -1)
            {
                ResultManager.Add("El monto S1 no puede estar vacio");
                return false;
            }
            if (model.AmountGoalS2Pre == 0 || model.AmountGoalS2Pre == -1)
            {
                ResultManager.Add("El monto S2 no puede estar vacio");
                return false;
            }
            if (model.AmountGoalTotalPre == 0 || model.AmountGoalTotalPre == -1)
            {
                ResultManager.Add("El total de monto meta no puede estar vacio");
                return false;
            }
            if (model.Subdistributor.Type == "Subdistribuidor" && model.AmountGoalTotalPre < 75000)
            {
                ResultManager.Add("El total de monto meta no puede ser menor a $75,000 MXN para un subdistribuidor");
                return false;
            }
            if (model.Subdistributor.Type == "Agricultor" && model.AmountGoalTotalPre < 50000)
            {
                ResultManager.Add("El total de monto meta no puede ser menor a $50,000 MXN para un agricultor");
                return false;
            }

            //insert new item
            try
            {
                ContractSubdistributor newContract = new ContractSubdistributor();

                newContract.IdB = Repository.AppConfigurations.IdBCounterGetNextContractSubdistributor();
                newContract.SubdistributorId = model.SubdistributorId;
                newContract.GRVBayerEmployeeId = CurrentUser.Id;//model.GRVBayerEmployeeId;
                newContract.RTVBayerEmployeeId = CurrentUser.Id;//model.RTVBayerEmployeeId;
                newContract.RegisteredRegionName = model.RegisteredRegionName;
                newContract.RegisteredZoneName = model.RegisteredZoneName;
                newContract.Year = model.Year;
                newContract.AmountGoalS1 = newContract.AmountGoalS1Pre = model.AmountGoalS1Pre;
                newContract.AmountGoalS2 = newContract.AmountGoalS2Pre = model.AmountGoalS2Pre;
                newContract.AmountGoalTotalPre = newContract.AmountGoalTotal = model.AmountGoalTotalPre;
                
                foreach (DistributorPurchasesXContractSubdistributor item in model.DistributorPurchases)
                {
                    newContract.DistributorPurchases.Add(
                        new DistributorPurchasesXContractSubdistributor()
                        {
                            ContractSubdistributorId = newContract.Id,
                            DistributorId = item.DistributorId
                        }
                        );
                }
                                
                //set initial contract status
                if (sendToDistributorReview)
                {
                    newContract.ContractSubdistributorStatusId = 4;//4:revision subdistribuidor
                }
                else
                {
                    newContract.ContractSubdistributorStatusId = 3;//3:revision bayer
                }

                //update current contract at subdistributor
                Subdistributor auxSubdistributor = Repository.Subdistributors.Get(newContract.SubdistributorId);
                if (auxSubdistributor == null)
                {
                    throw new Exception("No se encontro el subdistribuidor con id '" + newContract.SubdistributorId + "' para actualizar el convenio actual");
                }
                auxSubdistributor.CurrentContract = newContract;

                //create discount coupon entry
                SubdistributorDiscountCoupon newDiscountCoupon = new SubdistributorDiscountCoupon();
                newDiscountCoupon.HasCoupon =
                    newDiscountCoupon.HasCouponS1 =
                    newDiscountCoupon.HasCouponS2 =
                    newDiscountCoupon.IsCalculated =
                    newDiscountCoupon.IsCalculatedS1 =
                    newDiscountCoupon.IsCalculatedS2 =
                    false;
                newContract.SubdistributorDiscountCoupon = newDiscountCoupon;

                //create promotion coupon entry
                SubdistributorPromotionCoupon newPromotionCoupon = new SubdistributorPromotionCoupon();
                newPromotionCoupon.HasCoupon =
                    newPromotionCoupon.HasCouponS1 =
                    newPromotionCoupon.HasCouponS2 =
                    newPromotionCoupon.IsCalculated =
                    newPromotionCoupon.IsCalculatedS1 =
                    newPromotionCoupon.IsCalculatedS2 =
                    false;
                newContract.SubdistributorPromotionCoupon = newPromotionCoupon;

                //set default values
                newContract.ContractDate = Common.Dates.Today;
                newContract.ContractFilePath = string.Empty; 
                
                Repository.ContractsSubdistributor.Add(newContract);
                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Create.511 Excepción al crear el nuevo elemento" + ex.Message);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("El convenio ha sido creado", "");
                try
                {
                    //send email to subdistributor
                    SubdistributorEmployee auxSubdistributorEmployee = Repository.Subdistributors.Get(model.SubdistributorId).SubdistributorEmployees.FirstOrDefault();
                    if (SendEmailToSubdistributorContractCreated(auxSubdistributorEmployee.NameDisplay, auxSubdistributorEmployee.EMail, auxSubdistributorEmployee.Subdistributor.CurrentContract.Id, auxSubdistributorEmployee.Subdistributor.CurrentContract.IdB))
                    {
                        ResultManager.Add("Se ha enviado una notificación por correo electrónico al subdistribuidor", "");
                    }
                    else
                    {
                        ResultManager.Add("No se pudo notificar por correo electrónico al subdistribuidor");
                    }
                }
                catch (Exception ex)
                {
                    ResultManager.Add(ErrorDefault, Trace + "Create.511 Excepción al crear el convenio: " + ex.Message);
                }
            }

            return ResultManager.IsCorrect;
        }

        public bool Delete(int contractId)
        {
            ResultManager.Clear();

            // validate bayer contract associated
            ContractSubdistributor item = Repository.ContractsSubdistributor.Get(contractId);

            if (item.ContractSubdistributorStatusId == 1 || item.ContractSubdistributorStatusId == 2)
            {
                if (!CurrentUser.IsInRole(UserRole.AppAdmin))
                {
                    ResultManager.Add("Este convenio no puede ser eliminado en el estatus actual, solo puede eliminarse por el administrado", Trace + "Delete.111 Error por Regla de negocio: no se puede eliminar un convenio con estatus activo/vencido, a menos que sea por el administrador.");
                    return false;
                }
            }

            // now proceed with the delete operation
            try
            {
                IEnumerable<int> purchaseIds = Repository.DistributorPurchasesXContractSubdistributors.GetAll().Where(x => x.ContractSubdistributorId == contractId).Select(y => y.Id);
                foreach (int purchaseId in purchaseIds)
                {
                    Repository.DistributorPurchasesXContractSubdistributors.Remove(purchaseId);
                }

                Repository.SubdistributorDiscountCoupons.Remove(item.SubdistributorDiscountCoupon.Id);
                Repository.SubdistributorPromotionCoupons.Remove(item.SubdistributorPromotionCoupon.Id);

                Repository.ContractsSubdistributor.Remove(contractId);

                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ErrorManager.Add(Trace + "", ErrorDefault, "Exception while deleting subdistributor contract ", ex);
                ResultManager.IsCorrect = false;
            }

            return ResultManager.IsCorrect;
        }
        /*
        public bool Approve(ContractSubdistributor model)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (model == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "Approve.111 No se recibio el modelo");
                return false;
            }
            //-business validations

            //approve
            ContractSubdistributor auxContract = null;
            try
            {
                auxContract = Repository.ContractsSubdistributor.Get(model.Id);

                auxContract.ContractSubdistributorStatusId = 1;

                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Approve.511 Excepción al crear el nuevo elemento" + ex.Message);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("El convenio ha sido actualizado", "");
                SubdistributorEmployee auxSubdistributorEmployee = Repository.Subdistributors.Get(model.SubdistributorId).SubdistributorEmployees.FirstOrDefault();
                if (SendEmailToSubdistributorContractApproved(auxSubdistributorEmployee.NameDisplay, auxSubdistributorEmployee.EMail, auxContract.Id, auxContract.IdB))
                {
                    ResultManager.Add("Se ha enviado un correo al distribuidor", "");
                }
                else
                {
                    ResultManager.Add("No se pudo enviar actualización por correo al distribuidor");
                }
            }

            return ResultManager.IsCorrect;
        }
        */

        private bool SendEmailToSubdistributorContractCreated(string toName, string toEMail, int contractId, string contractIdB)
        {
            string urlContractDetails = Configurations.HostUrl + "Contracts/MySubdistributorContracts/Detail/" + contractId;
            Dictionary<string, string> emailParams = new Dictionary<string, string>() {
                { "userName", toName },
                { "message", "Por favor verifique la información y apruebe el convenio @contractIdB dando click en la liga de abajo:" },
                { "urlContractDetails ", urlContractDetails },
                { "contractIdB", contractIdB}
                };
            string subject = "Convenio listo para su aprobación: @contractIdB - Mi Portal Bayer";
            string messageBody = "<h2>Convenio listo para su aprobación: @contractIdB</h2><h3>Mi Portal Bayer</h3><p>@userName</p><p>@message</p><p><a href='@urlContractDetails '>Ver convenio</a></p>";
            if (SendEmail(toEMail, subject, messageBody, emailParams))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
