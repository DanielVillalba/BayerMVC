using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSD.Model;
using PSD.Security;
using PSD.Model.Filters.PipelineAndFilters.ContractDistributorFilters;

namespace PSD.Controller.Contracts
{
    public class MyDistributorContractsController : _BaseController
    {
        public MyDistributorContractsController(IAppConfiguration configurations)
            : base("MyDistributorContractsController.", configurations)
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
                EmployeeController employee = new EmployeeController(Configurations);
                int distributorId = employee.GetDistributorId(CurrentUser.Id);
                pipeline.Register(new DistributorIdFilter(distributorId));
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
                    ResultManager.Add(ErrorDefault, Trace + "ContractRetrieve.511 No se encontró un convenio Bayer-Distribuidor con id '" + id + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ContractRetrieve.511 Excepción al obtener el convenio Bayer-Distribuidor a editar: " + ex.Message);
            }

            return auxItem;
        }

        public bool UpdateAndApprove(ContractDistributor model)
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
            ContractDistributor auxContract = null;
            BayerEmployee auxRTV = new BayerEmployee();
            try
            {
                auxContract = Repository.ContractsDistributor.Get(model.Id);

                auxContract.AmountGoalQ1 = model.AmountGoalQ1;
                auxContract.AmountGoalQ2 = model.AmountGoalQ2;
                auxContract.AmountGoalQ3 = model.AmountGoalQ3;
                auxContract.AmountGoalQ4 = model.AmountGoalQ4;
                auxContract.AmountGoalTotal = model.AmountGoalTotal;

                auxContract.ContractDistributorStatusId = 3;

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
            int auxDistributorId = -1;
            ContractDistributor auxContract = null;
            try
            {
                auxContract = Repository.ContractsDistributor.Get(contractId);
                auxContract.ContractDistributorStatusId = 3;

                auxDistributorId = auxContract.DistributorId;

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
                DistributorEmployee auxDistributorEmployee = Repository.Distributors.Get(auxDistributorId).DistributorUsers.FirstOrDefault();
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

        private bool SendEmailContractApproved(string toName, string toEMail, int contractId, string contractIdB)
        {
            string urlContractDetails = Configurations.HostUrl + "Contracts/BayerToDistributor/Detail/" + contractId;
            Dictionary<string, string> emailParams = new Dictionary<string, string>() {
                { "userName", toName },
                { "message", "Convenio aprobado por distribuidor, de click en la liga de abajo para ver el convenio:" },
                { "urlContractDetails ", urlContractDetails },
                { "contractIdB", contractIdB}
                };
            string subject = "Convenio aprobado por distribuidor: @contractIdB - Bayer Portal de Servicios al Distribuidor";
            string messageBody = @"<h2>Convenio: @contractIdB</h2><h3>Bayer - Portal de Servicios al Distribuidor</h3><p>@userName</p><p>@message</p><p><a href='@urlContractDetails '>Ver convenio</a></p>";
            if (SendEmail(toEMail, subject, messageBody, emailParams))
            {
                return true;
            }
            else
            {
                ResultManager.Add(ErrorDefault, Trace + "SendEmailContractApproved.911 Error al enviar el mensaje de correo a '" + toEMail + "'");
                return false;
            }
        }
        
    }
}
