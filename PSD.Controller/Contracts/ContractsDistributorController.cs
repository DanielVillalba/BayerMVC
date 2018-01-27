using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSD.Model;
using PSD.Model.Filters.PipelineAndFilters.ContractDistributorFilters;
using PSD.Security;

namespace PSD.Controller.Contracts
{
    public class ContractsDistributorController : _BaseController
    {
        public ContractsDistributorController(IAppConfiguration configurations)
            : base("ContractsDistributorController.", configurations)
        {

        }

        /// <summary>
        /// This method provides the required object for the Pipeline filtering
        /// </summary>
        /// <returns></returns>
        public IQueryable<ContractDistributor> RetrieveAllToFilter()
        {
            return Repository.ContractsDistributor.GetAllToFilter();
        }

        /// <summary>
        /// Provides a list of Contract Distributor as a result of filtering the DB on specific filters
        /// </summary>
        /// <returns></returns>
        public List<ContractDistributor> FilteredItems()
        {
            ResultManager.IsCorrect = false;
            List<ContractDistributor> items = new List<ContractDistributor>();
            try
            {
                // implementing filtering pipeline
                var contractDistributorList = RetrieveAllToFilter();
                ContractDistributorFilteringPipeline pipeline = new ContractDistributorFilteringPipeline();

                if (Identity.CurrentUser.IsInRole(UserRole.EmployeeManagerOperation + "," + UserRole.EmployeeManagerView + "," + UserRole.EmployeeRTVOperation + "," + UserRole.EmployeeRTVView))
                {
                    EmployeeController employee = new EmployeeController(Configurations);
                    List<string> zones = employee.GetBayerEmployeeZones(CurrentUser.Id);
                    pipeline.Register(new RegisteredZoneNameListFilter(zones));
                    //pipeline.Register(new AssignedGRVFilter(CurrentUser.Id));
                }

                items = pipeline.Process(contractDistributorList).ToList();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Index.311: Error while retrieving ContractDistributor list from DB: " + ex.Message);
            }
            return items;
        }

        public List<ContractDistributor> RetrieveAll()
        {
            ResultManager.IsCorrect = false;
            List<ContractDistributor> auxList;
            try
            {
                auxList = (List<ContractDistributor>)Repository.ContractsDistributor.GetAll();
                ResultManager.IsCorrect = true;
                return auxList;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "RetrieveAll.511 Excepción al obtener el listado de convenios Bayer-Distribuidor: " + ex.Message);
            }

            return null;
        }

        public ContractDistributor Retrieve(int id)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no id was received
                ResultManager.Add(ErrorDefault, Trace + "Retrieve.131 No se recibio el id del convenio Bayer-Distribuidor");
                return null;
            }

            ContractDistributor auxItem = null;
            try
            {
                auxItem = Repository.ContractsDistributor.Get(id);
                if (auxItem == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "Retrieve.511 No se encontró un convenio Bayer-Distribuidor con id '" + id + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Retrieve.511 Excepción al obtener el convenio Bayer-Distribuidor a editar: " + ex.Message);
            }

            return auxItem;
        }
        public bool Create(ContractDistributor model, bool sendToDistributorReview)
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
            if (model.DistributorId == 0 || model.DistributorId == -1)
            {
                ResultManager.Add("Se debe seleccionar un distribuidor");
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
            /*deprecated: it can be some of these are not available
            if (model.AmountGoalQ1Pre == 0 || model.AmountGoalQ1Pre == -1)
            {
                ResultManager.Add("El monto Q1 no puede estar vacio");
                return false;
            }
            if (model.AmountGoalQ2Pre == 0 || model.AmountGoalQ2Pre == -1)
            {
                ResultManager.Add("El monto Q2 no puede estar vacio");
                return false;
            }
            if (model.AmountGoalQ3Pre == 0 || model.AmountGoalQ3Pre == -1)
            {
                ResultManager.Add("El monto Q3 no puede estar vacio");
                return false;
            }
            if (model.AmountGoalQ4Pre == 0 || model.AmountGoalQ4Pre == -1)
            {
                ResultManager.Add("El monto Q4 no puede estar vacio");
                return false;
            }
            */
            if (model.AmountGoalTotalPre == 0 || model.AmountGoalTotalPre == -1)
            {
                ResultManager.Add("El total de monto meta no puede estar vacio");
                return false;
            }

            //insert new item
            try
            {
                ContractDistributor newContract = new ContractDistributor();

                newContract.IdB = Repository.AppConfigurations.IdBCounterGetNextContractDistributor();
                newContract.GRVBayerEmployeeId = CurrentUser.Id;
                newContract.RTVBayerEmployeeId = CurrentUser.Id;
                newContract.RegisteredZoneName = model.RegisteredZoneName;
                newContract.RegisteredRegionName = model.RegisteredRegionName;
                newContract.Year = model.Year;
                newContract.AmountGoalQ1 = newContract.AmountGoalQ1Pre = model.AmountGoalQ1Pre;
                newContract.AmountGoalQ2 = newContract.AmountGoalQ2Pre = model.AmountGoalQ2Pre;
                newContract.AmountGoalQ3 = newContract.AmountGoalQ3Pre = model.AmountGoalQ3Pre;
                newContract.AmountGoalQ4 = newContract.AmountGoalQ4Pre = model.AmountGoalQ4Pre;
                newContract.AmountGoalTotalPre = newContract.AmountGoalTotal = model.AmountGoalTotalPre;
                                
                //default values
                newContract.ContractDate = Common.Dates.Today;
                newContract.ContractFilePath = string.Empty;
                
                if (sendToDistributorReview)
                {
                    newContract.ContractDistributorStatusId = 4;//4:revision distribuidor
                }
                else
                {
                    newContract.ContractDistributorStatusId = 3;//3:revision bayer
                }

                //relate contract as current distributor contract
                newContract.DistributorId = model.DistributorId;
                newContract.Distributor = Repository.Distributors.Get(newContract.DistributorId);
                if (newContract.Distributor.CurrentContract == null)
                {//first contract
                    newContract.DiscountType = 0;
                }
                else
                {//contract renovation
                    newContract.DiscountType = 1;
                }
                newContract.Distributor.CurrentContract = newContract;
                //distributorController = null;//release so we can call main Repository.Complete
                
                Repository.ContractsDistributor.Add(newContract);
                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Create.511 Excepción al crear el nuevo elemento", ex);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("El convenio ha sido actualizado", "");
                try
                {
                    //send email to subdistributor
                    DistributorEmployee auxDistributorEmployee = Repository.Distributors.Get(model.DistributorId).DistributorUsers.FirstOrDefault();
                    if (SendEmailToDistributorRequestApproval(auxDistributorEmployee.NameDisplay, auxDistributorEmployee.EMail, auxDistributorEmployee.Distributor.CurrentContract.Id, auxDistributorEmployee.Distributor.CurrentContract.IdB))
                    {
                        ResultManager.Add("Se ha enviado un correo al distribuidor", "");
                    }
                    else
                    {
                        ResultManager.Add("No se pudo enviar actualización por correo al distribuidor");
                    }
                }
                catch (Exception ex)
                {
                    ResultManager.Add(ErrorDefault, Trace + "Create.511 Excepción al crear el convenio: " + ex.Message);
                }
            }

            return ResultManager.IsCorrect;
        }
        public bool Update(ContractDistributor model, bool sendToDistributorReview)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (model == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "Update.111 No se recibio el modelo");
                return false;
            }
            //-business validations
            /*if (string.IsNullOrWhiteSpace(model.IdB))
            {
                ResultManager.Add("El id del convenio no puede estar vacio");
                return false;
            }
            if (model.DistributorId == 0 || model.DistributorId == -1)
            {
                ResultManager.Add("El nombre del cultivo no puede estar vacio");
                return false;
            }
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
            if (model.AmountGoalQ1Pre == 0 || model.AmountGoalQ1Pre == -1)
            {
                ResultManager.Add("El monto Q1 no puede estar vacio");
                return false;
            }
            if (model.AmountGoalQ2Pre == 0 || model.AmountGoalQ2Pre == -1)
            {
                ResultManager.Add("El monto Q2 no puede estar vacio");
                return false;
            }
            if (model.AmountGoalQ3Pre == 0 || model.AmountGoalQ3Pre == -1)
            {
                ResultManager.Add("El monto Q3 no puede estar vacio");
                return false;
            }
            if (model.AmountGoalQ4Pre == 0 || model.AmountGoalQ4Pre == -1)
            {
                ResultManager.Add("El monto Q4 no puede estar vacio");
                return false;
            }*/
            if (model.AmountGoalTotalPre == 0 || model.AmountGoalTotalPre == -1)
            {
                ResultManager.Add("El total de monto meta no puede estar vacio");
                return false;
            }

            //update item
            try
            {
                ContractDistributor auxContract = Repository.ContractsDistributor.Get(model.Id);

                auxContract.AmountGoalQ1 = auxContract.AmountGoalQ1Pre = model.AmountGoalQ1Pre;
                auxContract.AmountGoalQ2 = auxContract.AmountGoalQ2Pre = model.AmountGoalQ2Pre;
                auxContract.AmountGoalQ3 = auxContract.AmountGoalQ3Pre = model.AmountGoalQ3Pre;
                auxContract.AmountGoalQ4 = auxContract.AmountGoalQ4Pre = model.AmountGoalQ4Pre;
                auxContract.AmountGoalTotalPre = auxContract.AmountGoalTotal = model.AmountGoalTotalPre;

                //auxContract.ContractFilePath = string.Empty;

                if (sendToDistributorReview)
                {
                    auxContract.ContractDistributorStatusId = 4;
                }

                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Update.511 Excepción al crear el nuevo elemento" + ex.Message);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("El convenio ha sido actualizado", "");
                if (sendToDistributorReview)
                {
                    DistributorEmployee auxDistributorEmployee = Repository.Distributors.Get(model.DistributorId).DistributorUsers.FirstOrDefault();
                    if (SendEmailToDistributorContractUpdated(auxDistributorEmployee.NameDisplay, auxDistributorEmployee.EMail, model.Id, model.IdB))
                    {
                        ResultManager.Add("Se ha enviado un correo al distribuidor", "");
                    }
                    else
                    {
                        ResultManager.Add("No se pudo enviar actualización por correo al distribuidor");
                    }
                }
            }

            return ResultManager.IsCorrect;
        }

        public bool Delete(int contractId)
        {
            ResultManager.IsCorrect = false;

            // validate bayer contract associated
            ContractDistributor item = Repository.ContractsDistributor.Get(contractId);

            if (item.ContractDistributorStatusId == 1 || item.ContractDistributorStatusId == 2)
            {
                ResultManager.Add(ErrorDefault, Trace + "DeleteContractDistributor.### El Convenio con Distribuidor seleccionado no se puede eliminar, su estatus es activo / vencido.");
                return false;
            }

            // now proceed with the delete operation
            try
            {
                Repository.ContractsDistributor.Remove(contractId);

                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ErrorManager.Add(Trace + "", ErrorDefault, "Exception while deleting distributor contract ", ex);
                ResultManager.IsCorrect = false;
            }

            return ResultManager.IsCorrect;
        }

        public bool Approve(ContractDistributor model)
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
            ContractDistributor auxContract = null;
            try
            {
                auxContract = Repository.ContractsDistributor.Get(model.Id);

                auxContract.ContractDistributorStatusId = 1;

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
                DistributorEmployee auxDistributorEmployee = Repository.Distributors.Get(model.DistributorId).DistributorUsers.FirstOrDefault();
                if (SendEmailToDistributorContractApproved(auxDistributorEmployee.NameDisplay, auxDistributorEmployee.EMail, auxContract.Id, auxContract.IdB))
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
        public bool Approve(int id)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (id == -1)
            {
                ResultManager.Add(ErrorDefault, Trace + "Approve.111 No se recibio el parámetro id");
                return false;
            }
            //-business validations

            ContractDistributor auxContract = null;
            try
            {
                auxContract = Repository.ContractsDistributor.Get(id);
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Approve.511 Excepción al obtener la información del contrato id '" + id + "'" + ex.Message);
                return false;
            }

            return Approve(auxContract);
        }
        public bool SendToDistributor(int id)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            //-business validations
            if (id < 1)
            {
                ResultManager.Add(ErrorDefault, Trace + "SendToDistributor.111 No se recibio el parámetro id");
                return false;
            }

            //update status to 4: review-distributor
            ContractDistributor auxContract = null;
            try
            {
                auxContract = Repository.ContractsDistributor.Get(id);
                auxContract.ContractDistributorStatusId = 4;

                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "SendToDistributor.511 Excepción al actualizar el elemento" + ex.Message);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("El convenio ha sido actualizado", "");
                try
                {
                    //send email to subdistributor
                    DistributorEmployee auxDistributorEmployee = Repository.Distributors.Get(id).DistributorUsers.FirstOrDefault();
                    if (SendEmailToDistributorRequestApproval(auxDistributorEmployee.NameDisplay, auxDistributorEmployee.EMail, auxDistributorEmployee.Distributor.CurrentContract.Id, auxDistributorEmployee.Distributor.CurrentContract.IdB))
                    {
                        ResultManager.Add("Se ha enviado un correo al distribuidor", "");
                    }
                    else
                    {
                        ResultManager.Add("No se pudo enviar actualización por correo al distribuidor");
                    }
                }
                catch (Exception ex)
                {
                    ResultManager.Add(ErrorDefault, Trace + "Create.511 Excepción al crear el convenio: " + ex.Message);
                }
            }

            return ResultManager.IsCorrect;
        }
        public bool RequestApprovalGRV(int id)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            //-business validations
            if (id < 1)
            {
                ResultManager.Add(ErrorDefault, Trace + "SendToDistributor.111 No se recibio el parámetro id");
                return false;
            }

            //retrieve contract details
            ContractDistributor auxContract = null;
            try
            {
                auxContract = Repository.ContractsDistributor.Get(id);
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "SendToDistributor.511 Excepción al actualizar el elemento" + ex.Message);
            }

            if (auxContract != null)
            {                
                if (SendEmailToApproveGRV(auxContract.GRVBayerEmployee.NameDisplay, auxContract.GRVBayerEmployee.EMail, id, auxContract.IdB))
                {
                    ResultManager.Add("Se ha enviado un correo al GRV solicitando la aprobación del convenio", "");
                    ResultManager.IsCorrect = true;
                }
                else
                {
                    ResultManager.Add("No se pudo enviar la solicitud por correo al GRV");
                }
            }

            return ResultManager.IsCorrect;
        }

        private bool SendEmailToDistributorRequestApproval(string toName, string toEMail, int contractId, string contractIdB)
        {
            string urlContractDetails = Configurations.HostUrl + "Contracts/MyDistributorContracts/Detail/" + contractId;
            Dictionary<string, string> emailParams = new Dictionary<string, string>() {
                { "userName", toName },
                { "message", "Por favor verifique la información y apruebe el convenio @contractIdB dando click en la liga de abajo:" },
                { "urlContractDetails ", urlContractDetails },
                { "contractIdB", contractIdB}
                };
            string subject = Configurations.LayoutEmailContractDistributor_SendTodistributor_Subject;
            string messageBody = Configurations.LayoutEmailContractDistributor_SendTodistributor_Body;
            if (SendEmail(toEMail, subject, messageBody, emailParams))
            {
                return true;
            }
            else
            {
                ErrorManager.Add(Trace + "SendEmailToDistributorRequestApproval.911", "Error while sending email to '" + toEMail + "'");
                return false;
            }
        }
        private bool SendEmailToDistributorContractUpdated(string toName, string toEMail, int contractId, string contractIdB)
        {
            string urlContractDetails = Configurations.HostUrl + "Contracts/MyDistributorContracts/Detail/" + contractId;
            Dictionary<string, string> emailParams = new Dictionary<string, string>() {
                { "userName", toName },
                { "message", "Hemos actualizado su convenio, de click en la liga de abajo para ver lo detalles:" },
                { "urlContractDetails ", urlContractDetails },
                { "contractIdB", contractIdB}
                };
            string subject = "Actualización de convenio: @contractIdB - Mi Portal Bayer";
            string messageBody = "<h2>Actualización de convenio: @contractIdB</h2><h3>Mi Portal Bayer</h3><p>@userName</p><p>@message</p><p><a href='@urlContractDetails '>Ver convenio</a></p>";
            if (SendEmail(toEMail, subject, messageBody, emailParams))
            {
                return true;
            }
            else
            {
                ErrorManager.Add(Trace + "SendEmailToDistributorContractUpdated.911", "Error while sending email to '" + toEMail + "'");
                return false;
            }
        }
        private bool SendEmailToDistributorContractApproved(string toName, string toEMail, int contractId, string contractIdB)
        {
            string urlContractDetails = Configurations.HostUrl + "Contracts/MyDistributorContracts/Detail/" + contractId;
            Dictionary<string, string> emailParams = new Dictionary<string, string>() {
                { "userName", toName },
                { "message", "Su convenio ha sido aprobado, de click en la liga de abajo para ver lo detalles:" },
                { "urlContractDetails ", urlContractDetails },
                { "contractIdB", contractIdB}
                };
            string subject = "Convenio aprobado: @contractIdB - Mi Portal Bayer";
            string messageBody = "<h2>Convenio aprobado: @contractIdB</h2><h3>Mi Portal Bayer</h3><p>@userName</p><p>@message</p><p><a href='@urlContractDetails '>Ver convenio</a></p>";
            if (SendEmail(toEMail, subject, messageBody, emailParams))
            {
                return true;
            }
            else
            {
                ErrorManager.Add(Trace + "SendEmailToDistributorContractApproved.911", "Error while sending email to '" + toEMail + "'");
                return false;
            }
        }
        private bool SendEmailToBayerEmployeeContractApproved(string toName, string toEMail, int contractId, string contractIdB)
        {
            string urlContractDetails = Configurations.HostUrl + "Contracts/BayerToDistributor/Detail/" + contractId;
            Dictionary<string, string> emailParams = new Dictionary<string, string>() {
                { "userName", toName },
                { "message", "El convenio ha sido aprobado, de click en la liga de abajo para ver lo detalles:" },
                { "urlContractDetails ", urlContractDetails },
                { "contractIdB", contractIdB}
                };
            string subject = "Convenio aprobado: @contractIdB - Mi Portal Bayer";
            string messageBody = "<h2>Convenio aprobado: @contractIdB</h2><h3>Mi Portal Bayer</h3><p>@userName</p><p>@message</p><p><a href='@urlContractDetails '>Ver convenio</a></p>";
            if (SendEmail(toEMail, subject, messageBody, emailParams))
            {
                return true;
            }
            else
            {
                ErrorManager.Add(Trace + "SendEmailToBayerEmployeeContractApproved.911", "Error while sending email to '" + toEMail + "'");
                return false;
            }
        }
        private bool SendEmailContractUpdated(string toName, string toEMail, int contractId, string contractIdB)
        {
            string urlContractDetails = Configurations.HostUrl + "Contracts/MyDistributorContracts/Detail/" + contractId;
            Dictionary<string, string> emailParams = new Dictionary<string, string>() {
                { "userName", toName },
                { "message", "Hemos actualizado los detalles de su convenio, de click en la liga de abajo para ver el convenio:" },
                { "urlContractDetails ", urlContractDetails },
                { "contractIdB", contractIdB}
                };
            string messageBody = "<h2>Actualización de convenio: @contractIdB</h2><h3>Mi Portal Bayer</h3><p>@userName</p><p>@message</p><p><a href='@urlContractDetails '>Ver convenio</a></p>";
            string subject = "Actualización de convenio: @contractIdB - Mi Portal Bayer";
            if (SendEmail(toEMail, subject, messageBody, emailParams))
            {
                return true;
            }
            else
            {
                ErrorManager.Add(Trace + "SendEmailContractUpdated.911", "Error while sending email to '" + toEMail + "'");
                return false;
            }
        }
        private bool SendEmailContractApprovedDistributor(string toName, string toEMail, int contractId, string contractIdB)
        {
            string urlContractDetails = Configurations.HostUrl + "Contracts/MyDistributorContracts/Detail/" + contractId;
            Dictionary<string, string> emailParams = new Dictionary<string, string>() {
                { "userName", toName },
                { "message", "Hemos actualizado los detalles de su convenio, de click en la liga de abajo para ver el convenio:" },
                { "urlContractDetails ", urlContractDetails },
                { "contractIdB", contractIdB}                
                };
            string messageBody = "<h2>Actualización de convenio: @contractIdB</h2><h3>Mi Portal Bayer</h3><p>@userName</p><p>@message</p><p><a href='@urlContractDetails '>Ver convenio</a></p>";
            string subject = "Actualización de convenio: @contractIdB - Mi Portal Bayer";
            if (SendEmail(toEMail, subject, messageBody, emailParams))
            {
                return true;
            }
            else
            {
                ErrorManager.Add(Trace + "SendEmailContractApprovedDistributor.911", "Error while sending email to '" + toEMail + "'");
                return false;
            }
        }
        private bool SendEmailToReviewDistributor(string toName, string toEMail, int contractId, string contractIdB)
        {
            string urlContractDetails = Configurations.HostUrl + "Contracts/MyDistributorContracts/Detail/" + contractId;
            Dictionary<string, string> emailParams = new Dictionary<string, string>() {
                { "userName", toName },
                { "message", "Hemos actualizado los detalles de su convenio, de click en la liga de abajo para ver el convenio:" },
                { "urlContractDetails ", urlContractDetails },
                { "contractIdB", contractIdB}
                };
            string messageBody = "<h2>Actualización de convenio: @contractIdB</h2><h3>Mi Portal Bayer</h3><p>@userName</p><p>@message</p><p><a href='@urlContractDetails '>Ver convenio</a></p>";
            string subject = "Actualización de convenio: @contractIdB - Mi Portal Bayer";
            if (SendEmail(toEMail, subject, messageBody, emailParams))
            {
                return true;
            }
            else
            {
                ResultManager.Add(ErrorDefault, Trace + "SendEmailToReviewDistributor.911 Error al enviar el mensaje de correo a '" + toEMail + "'");
                return false;
            }
        }
        private bool SendEmailToApproveGRV(string toName, string toEMail, int contractId, string contractIdB)
        {
            string urlContractDetails = Configurations.HostUrl + "Contracts/BayerToDistributor/Detail/" + contractId;
            Dictionary<string, string> emailParams = new Dictionary<string, string>() {
                { "userName", toName },
                { "message", "Se ha solicitado su apoyo para realizar la aprobación de un convenio, de click en la liga de abajo para ver el convenio:" },
                { "urlContractDetails ", urlContractDetails },
                { "contractIdB", contractIdB}
                };
            string subject = "Solicitud de aprobación de convenio: @contractIdB - Mi Portal Bayer";
            string messageBody = @"<h2>Solicitud de aprobación de convenio: @contractIdB</h2><h3>Mi Portal Bayer</h3><p>@userName</p><p>@message</p><p><a href='@urlContractDetails '>Ver convenio</a></p>";
            if (SendEmail(toEMail, subject, messageBody, emailParams))
            {
                return true;
            }
            else
            {
                ResultManager.Add(ErrorDefault, Trace + "SendEmailToApproveGRV.911 Error al enviar el mensaje de correo a '" + toEMail + "'");
                return false;
            }
        }
    }
}
