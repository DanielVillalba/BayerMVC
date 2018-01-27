using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSD.Model;
using PSD.Security;
using PSD.Model.Filters.PipelineAndFilters.ContractSubdistributorFilters;

namespace PSD.Controller.Contracts
{
    public class MySubdistributorContractsController : _BaseController
    {
        public MySubdistributorContractsController(IAppConfiguration configurations)
            : base("MySubdistributorContractsController.", configurations)
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
                EmployeeController employee = new EmployeeController(Configurations);
                int subdistributorId = employee.GetSubdistributorId(CurrentUser.Id);
                pipeline.Register(new SubdistributorIdFilter(subdistributorId));
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
                ResultManager.Add(ErrorDefault, Trace + "RetrieveAll.511 Excepción al obtener el listado de convenios Bayer-Distribuidor: " + ex.Message);
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
                ResultManager.Add(ErrorDefault, Trace + "Retrieve.131 No se recibio el id del convenio Bayer-Distribuidor");
                return null;
            }

            ContractSubdistributor auxItem = null;
            try
            {
                auxItem = Repository.ContractsSubdistributor.Get(id);
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

        /*
        public bool UpdateAndApprove(ContractSubdistributor model)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (model == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "UpdateAndApprove.111 No se recibio el modelo");
                return false;
            }
            //-business validations

            //approve
            ContractSubdistributor auxContract = null;
            BayerEmployee auxRTV = new BayerEmployee();
            try
            {
                auxContract = Repository.ContractsSubdistributor.Get(model.Id);

                auxContract.AmountGoalS1 = model.AmountGoalS1;
                auxContract.AmountGoalS2 = model.AmountGoalS2;
                auxContract.AmountGoalTotal = model.AmountGoalTotal;

                auxContract.ContractSubdistributorStatusId = 3;

                //set rtv info for email send (will be lost from auxContract after Repository.Complete())
                auxRTV.EMail = auxContract.RTVBayerEmployee.EMail;
                auxRTV.Name = auxContract.RTVBayerEmployee.NameDisplay;

                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "UpdateAndApprove.511 Excepción al crear el nuevo elemento" + ex.Message);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("El convenio ha sido actualizado", "");

                if (SendEmailContractApproved(auxRTV.Name, auxRTV.EMail, auxContract.Id, auxContract.IdB))
                {
                    ResultManager.Add("Se ha enviado un correo al RTV para la aprobación final", "");
                }
                else
                {
                    ResultManager.Add("No se pudo enviar actualización por correo al RTV");
                }
            }

            return ResultManager.IsCorrect;
        }
        */
        
        /*
        public bool Approve(int contractId)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (contractId == -1)
            {
                ResultManager.Add(ErrorDefault, Trace + "Approve.111 No se recibio el parámetro id");
                return false;
            }
            //-business validations

            //approve
            int auxSubdistributorId = -1;
            ContractSubdistributor auxContract = null;
            try
            {
                auxContract = Repository.ContractsSubdistributor.Get(contractId);
                auxContract.ContractSubdistributorStatusId = 3;

                auxSubdistributorId = auxContract.SubdistributorId;

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
                DistributorEmployee auxDistributorEmployee = Repository.Distributors.Get(auxSubdistributorId).DistributorUsers.FirstOrDefault();
                if (SendEmailContractApproved(auxDistributorEmployee.NameDisplay, auxDistributorEmployee.EMail, contractId, auxContract.IdB))
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

            ContractSubdistributor auxContract = null;
            string auxSubdistributorDisplayName = string.Empty;
            string auxSubdistributorEmail = string.Empty;
            string auxRtvDisplayName = string.Empty;
            string auxRtvEmail = string.Empty;
            string auxContractIdB = string.Empty;
            try
            {
                //retrieve contract to update
                auxContract = Repository.ContractsSubdistributor.Get(id);

                //set new values
                auxContract.ContractSubdistributorStatusId = 1;//1:active

                //save aux informationto send emails
                SubdistributorEmployee auxSubdistributorEmployee = auxContract.Subdistributor.SubdistributorEmployees.FirstOrDefault();
                auxSubdistributorDisplayName = auxSubdistributorEmployee.NameDisplay;
                auxSubdistributorEmail = auxSubdistributorEmployee.EMail;
                auxRtvDisplayName = auxContract.RTVBayerEmployee.NameDisplay;
                auxRtvEmail = auxContract.RTVBayerEmployee.EMail;
                auxContractIdB = auxContract.IdB;

                //save changes
                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Approve.311 Excepción al actualizar la información del contrato id '" + id + "'" + ex.Message);
                return false;
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("El convenio ha sido actualizado", "");
                try
                {
                    //send email to subdistributor
                    if (SendEmailToSubdistributorContractApproved(auxSubdistributorDisplayName, auxSubdistributorEmail, id, auxContract.IdB))
                    {
                        ResultManager.Add("Se ha enviado un correo al distribuidor", "");
                    }
                    else
                    {
                        ResultManager.Add("No se pudo enviar actualización por correo al distribuidor");
                    }

                    //send email to rtv
                    if (SendEmailToBayerEmployeeContractApproved(auxRtvDisplayName, auxRtvEmail, id, auxContract.IdB))
                    {
                        ResultManager.Add("Se ha enviado un correo al rtv", "");
                    }
                    else
                    {
                        ResultManager.Add("No se pudo enviar actualización por correo al rtv");
                    }
                }
                catch (Exception ex)
                {
                    ResultManager.Add("Error al enviar notificaciones de correo", Trace + "Create.711 Excepción al enviar notificación de correo del convenio: " + ex.Message);
                }
            }

            return ResultManager.IsCorrect;
        }

        private bool SendEmailToSubdistributorContractApproved(string toName, string toEMail, int contractId, string contractIdB)
        {
            string urlContractDetails = Configurations.HostUrl + "Contracts/MySubdistributorContracts/Detail/" + contractId;
            Dictionary<string, string> emailParams = new Dictionary<string, string>() {
                { "userName", toName },
                { "message", "Su convenio ha sido aprobado y ahora esta activo, de click en la liga de abajo para ver los detalles:" },
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
            string urlContractDetails = Configurations.HostUrl + "Contracts/ToSubdistributor/Detail/" + contractId;
            Dictionary<string, string> emailParams = new Dictionary<string, string>() {
                { "userName", toName },
                { "message", "El convenio ahora esta activo. Puede ver el detalle del convenio dando click en la liga de abajo:" },
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
                return false;
            }
        }
    }
}
