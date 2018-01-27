using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSD.Security;
using PSD.Model;
using PSD.Model.Filters.PipelineAndFilters.ContractSubdistributorFilters;
using PSD.Model.Filters.PipelineAndFilters.SubdistributorFilters;

namespace PSD.Controller.Contracts
{
    public class MyCollaboratorContractsController : _BaseController
    {
        public MyCollaboratorContractsController(IAppConfiguration configurations)
            : base("MyCollaboratorContractsController.", configurations)
        {

        }
        /// <summary>
        /// This method provides the required object for the Pipeline filtering
        /// </summary>
        /// <returns></returns>
        public IQueryable<Subdistributor> RetrieveAllToFilter()
        {
            return Repository.Subdistributors.GetAllToFilter();
        }
        public List<Subdistributor> RetrieveAll()
        {
            // applying filtering pipeline
            List<Subdistributor> subdistributorFilteredList = null;
            try
            {
                // implementing filtering pipeline
                var subdistributorList = RetrieveAllToFilter();
                SubdistributorFilteringPipeline pipeline = new SubdistributorFilteringPipeline();

                //TODO: implement pipeline.Register(new RelatedByActiveContractDistributorIdFilter(CurrentUser.Id));
            
                subdistributorFilteredList = pipeline.Process(subdistributorList).ToList();

                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "RetrieveAll.511 Excepción al obtener el listado de convenios Bayer-Subdistribuidor: " + ex.Message);
            }
            return subdistributorFilteredList;
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

        public bool RegisterPurchase(ContractSubdistributor model, int selectedMonth = -1, decimal purchaseAmount = 0)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (model == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "RegisterPurchase.111 No se recibio el modelo");
                return false;
            }
            //-business validations
            if(selectedMonth < 1 || selectedMonth > 12)
            {
                ResultManager.Add(ErrorDefault, Trace + "RegisterPurchase.311 Valor inesperado para mes seleccionado, se esperaba numero entre 1 y 12, se recibio '" + selectedMonth + "'");
                return false;
            }
            
            //register purchase
            ContractSubdistributor auxContract = null;
            DistributorPurchasesXContractSubdistributor auxDistributorPurchase = null;
            BayerEmployee auxRTV = new BayerEmployee();
            try
            {
                auxContract = Repository.ContractsSubdistributor.Get(model.Id);
                int purchaseId = 0;
                decimal auxTotalMonth = 0;
                foreach(DistributorPurchasesXContractSubdistributor item in auxContract.DistributorPurchases)
                {
                    if (item.Distributor.IdB == CurrentUser.EmployeeId) { purchaseId = item.Id; }
                }
                auxDistributorPurchase = Repository.DistributorPurchasesXContractSubdistributors.Get(purchaseId);

                switch(selectedMonth)
                {
                    case 1:
                        auxContract.PurchaseTotalJan += purchaseAmount;
                        auxTotalMonth = auxContract.PurchaseTotalJan;
                        auxContract.PurchaseTotalS1 += purchaseAmount;
                        auxDistributorPurchase.PurchaseJan += purchaseAmount;
                        auxDistributorPurchase.PurchaseTotalS1 += purchaseAmount;
                        break;
                    case 2:
                        auxContract.PurchaseTotalFeb += purchaseAmount;
                        auxTotalMonth = auxContract.PurchaseTotalFeb;
                        auxContract.PurchaseTotalS1 += purchaseAmount;
                        auxDistributorPurchase.PurchaseFeb += purchaseAmount;
                        auxDistributorPurchase.PurchaseTotalS1 += purchaseAmount;
                        break;
                    case 3:
                        auxContract.PurchaseTotalMar += purchaseAmount;
                        auxTotalMonth = auxContract.PurchaseTotalMar;
                        auxContract.PurchaseTotalS1 += purchaseAmount;
                        auxDistributorPurchase.PurchaseMar += purchaseAmount;
                        auxDistributorPurchase.PurchaseTotalS1 += purchaseAmount;
                        break;
                    case 4:
                        auxContract.PurchaseTotalApr += purchaseAmount;
                        auxTotalMonth = auxContract.PurchaseTotalApr;
                        auxContract.PurchaseTotalS1 += purchaseAmount;
                        auxDistributorPurchase.PurchaseApr += purchaseAmount;
                        auxDistributorPurchase.PurchaseTotalS1 += purchaseAmount;
                        break;
                    case 5:
                        auxContract.PurchaseTotalMay += purchaseAmount;
                        auxTotalMonth = auxContract.PurchaseTotalMay;
                        auxContract.PurchaseTotalS1 += purchaseAmount;
                        auxDistributorPurchase.PurchaseMay += purchaseAmount;
                        auxDistributorPurchase.PurchaseTotalS1 += purchaseAmount;
                        break;
                    case 6:
                        auxContract.PurchaseTotalJun += purchaseAmount;
                        auxTotalMonth = auxContract.PurchaseTotalJun;
                        auxContract.PurchaseTotalS1 += purchaseAmount;
                        auxDistributorPurchase.PurchaseJun += purchaseAmount;
                        auxDistributorPurchase.PurchaseTotalS1 += purchaseAmount;
                        break;
                    case 7:
                        auxContract.PurchaseTotalJul += purchaseAmount;
                        auxTotalMonth = auxContract.PurchaseTotalJul;
                        auxContract.PurchaseTotalS2 += purchaseAmount;
                        auxDistributorPurchase.PurchaseJul += purchaseAmount;
                        auxDistributorPurchase.PurchaseTotalS2 += purchaseAmount;
                        break;
                    case 8:
                        auxContract.PurchaseTotalAgo += purchaseAmount;
                        auxTotalMonth = auxContract.PurchaseTotalAgo;
                        auxContract.PurchaseTotalS2 += purchaseAmount;
                        auxDistributorPurchase.PurchaseAgo += purchaseAmount;
                        auxDistributorPurchase.PurchaseTotalS2 += purchaseAmount;
                        break;
                    case 9:
                        auxContract.PurchaseTotalSep += purchaseAmount;
                        auxTotalMonth = auxContract.PurchaseTotalSep;
                        auxContract.PurchaseTotalS2 += purchaseAmount;
                        auxDistributorPurchase.PurchaseSep += purchaseAmount;
                        auxDistributorPurchase.PurchaseTotalS2 += purchaseAmount;
                        break;
                    case 10:
                        auxContract.PurchaseTotalOct += purchaseAmount;
                        auxTotalMonth = auxContract.PurchaseTotalOct;
                        auxContract.PurchaseTotalS2 += purchaseAmount;
                        auxDistributorPurchase.PurchaseOct += purchaseAmount;
                        auxDistributorPurchase.PurchaseTotalS2 += purchaseAmount;
                        break;
                    case 11:
                        auxContract.PurchaseTotalNov += purchaseAmount;
                        auxTotalMonth = auxContract.PurchaseTotalNov;
                        auxContract.PurchaseTotalS2 += purchaseAmount;
                        auxDistributorPurchase.PurchaseNov += purchaseAmount;
                        auxDistributorPurchase.PurchaseTotalS2 += purchaseAmount;
                        break;
                    case 12:
                        auxContract.PurchaseTotalDic += purchaseAmount;
                        auxTotalMonth = auxContract.PurchaseTotalDic;
                        auxContract.PurchaseTotalS2 += purchaseAmount;
                        auxDistributorPurchase.PurchaseDic += purchaseAmount;
                        auxDistributorPurchase.PurchaseTotalS2 += purchaseAmount;
                        break;
                }

                auxContract.PurchaseTotal += purchaseAmount;
                auxDistributorPurchase.PurchaseTotal += purchaseAmount;

                //update coupon discount
                decimal auxDiscountPercentage = 0;
                PSD.Controller.BenefitPrograms.Coupon.CouponManagementController couponManagementContoller = new BenefitPrograms.Coupon.CouponManagementController(Configurations);
                auxDiscountPercentage = couponManagementContoller.GetDiscountPercentage(auxContract.GoalTotalPercentage, auxContract.DiscountType);
                if(auxDiscountPercentage > 0)
                {
                    auxContract.SubdistributorDiscountCoupon.HasCoupon = true;
                    auxContract.SubdistributorDiscountCoupon.CouponAmount = (auxDiscountPercentage / 100) * auxContract.PurchaseTotal;
                    
                    if (selectedMonth <= 6)
                    {
                        auxContract.SubdistributorDiscountCoupon.HasCouponS1 = true;
                        auxContract.SubdistributorDiscountCoupon.CouponAmountS1 = (auxDiscountPercentage / 100) * auxContract.PurchaseTotalS1;
                    }
                    else
                    {
                        auxContract.SubdistributorDiscountCoupon.HasCouponS2 = true;
                        auxContract.SubdistributorDiscountCoupon.CouponAmountS2 = (auxDiscountPercentage / 100) * auxContract.PurchaseTotalS2;
                    }
                }

                /*deprecated, value set for coupon S1 and S2 is wrong, it should be updated with the sum of all months promotions, not only current one (thru promotion loop?)
                //update coupon promotion
                decimal auxPromotionPercentage = 0;
                auxPromotionPercentage = couponManagementContoller.GetPromotionPercentage(auxTotalMonth, selectedMonth);
                if (auxPromotionPercentage > 0)
                {
                    auxContract.SubdistributorPromotionCoupon.HasCoupon = true;
                    auxContract.SubdistributorPromotionCoupon.CouponAmount = (auxPromotionPercentage / 100) * auxContract.PurchaseTotal;

                    if (selectedMonth <= 6)
                    {
                        auxContract.SubdistributorPromotionCoupon.HasCouponS1 = true;
                        auxContract.SubdistributorPromotionCoupon.CouponAmountS1 = (auxPromotionPercentage / 100) * auxTotalMonth;
                    }
                    else
                    {
                        auxContract.SubdistributorPromotionCoupon.HasCouponS2 = true;
                        auxContract.SubdistributorPromotionCoupon.CouponAmountS2 = (auxPromotionPercentage / 100) * auxTotalMonth;
                    }
                }
                */

                //update coupon promotion
                //-clear previous amounts
                auxContract.SubdistributorPromotionCoupon.HasCoupon = false;
                auxContract.SubdistributorPromotionCoupon.HasCouponS1 = false;
                auxContract.SubdistributorPromotionCoupon.HasCouponS2 = false;
                auxContract.SubdistributorPromotionCoupon.CouponAmount = 0;
                auxContract.SubdistributorPromotionCoupon.CouponAmountS1 = 0;
                auxContract.SubdistributorPromotionCoupon.CouponAmountS2 = 0;

                //set new purchases into aux for looping
                decimal[] auxPurchaseXMonth = new decimal[13];
                auxPurchaseXMonth[1] = auxContract.PurchaseTotalJan;
                auxPurchaseXMonth[2] = auxContract.PurchaseTotalFeb;
                auxPurchaseXMonth[3] = auxContract.PurchaseTotalMar;
                auxPurchaseXMonth[4] = auxContract.PurchaseTotalApr;
                auxPurchaseXMonth[5] = auxContract.PurchaseTotalMay;
                auxPurchaseXMonth[6] = auxContract.PurchaseTotalJun;
                auxPurchaseXMonth[7] = auxContract.PurchaseTotalJul;
                auxPurchaseXMonth[8] = auxContract.PurchaseTotalAgo;
                auxPurchaseXMonth[9] = auxContract.PurchaseTotalSep;
                auxPurchaseXMonth[10] = auxContract.PurchaseTotalOct;
                auxPurchaseXMonth[11] = auxContract.PurchaseTotalNov;
                auxPurchaseXMonth[12] = auxContract.PurchaseTotalDic;

                //loop thu each month and update coupon
                decimal auxPromotionPercentage = 0;
                for (int a = 1; a <= 12; a++)
                {
                    auxPromotionPercentage = couponManagementContoller.GetPromotionPercentage(auxPurchaseXMonth[a], a);
                    if (auxPromotionPercentage > 0)
                    {
                        auxContract.SubdistributorPromotionCoupon.HasCoupon = true;
                        auxContract.SubdistributorPromotionCoupon.CouponAmount += (auxPromotionPercentage / 100) * auxPurchaseXMonth[a];

                        if (a <= 6)
                        {
                            auxContract.SubdistributorPromotionCoupon.HasCouponS1 = true;
                            auxContract.SubdistributorPromotionCoupon.CouponAmountS1 += (auxPromotionPercentage / 100) * auxPurchaseXMonth[a];
                        }
                        else
                        {
                            auxContract.SubdistributorPromotionCoupon.HasCouponS2 = true;
                            auxContract.SubdistributorPromotionCoupon.CouponAmountS2 += (auxPromotionPercentage / 100) * auxPurchaseXMonth[a];
                        }
                    }
                }


                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "UpdateAndApprove.511 Excepción al crear el nuevo elemento" + ex.Message);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("La compra ha sido registrada", "");

                if (true)//SendEmailContractApproved(auxRTV.Name, auxRTV.EMail, auxContract.Id, auxContract.IdB))
                {
                    //ResultManager.Add("Se ha enviado un correo al RTV para la aprobación final", "");
                }
                else
                {
                    //ResultManager.Add("No se pudo enviar actualización por correo al RTV");
                }
            }

            return ResultManager.IsCorrect;
        }

        public bool Edit(DistributorPurchasesXContractSubdistributor model)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (model == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "Edit.111 No se recibio el modelo");
                return false;
            }
            if (model.Id <= 0)
            {
                ResultManager.Add(ErrorDefault, Trace + "Edit.111 No se recibio el Id del DistributorPurchasesXContractSubdistributor a editar en el modelo");
                return false;
            }
            if (model.ContractSubdistributorId <= 0)
            {
                ResultManager.Add(ErrorDefault, Trace + "Edit.111 No se recibio el Id del contrato a editar en el modelo");
                return false;
            }
            //-business validations
            
            ContractSubdistributor auxContract = null;
            DistributorPurchasesXContractSubdistributor auxDistributorPurchase = null;
            BayerEmployee auxRTV = new BayerEmployee();
            decimal auxAmountdif = 0;
            try
            {
                auxContract = Repository.ContractsSubdistributor.Get(model.ContractSubdistributorId);
                auxDistributorPurchase = Repository.DistributorPurchasesXContractSubdistributors.Get(model.Id);

                if (model.PurchaseJan != auxDistributorPurchase.PurchaseJan)
                {
                    auxAmountdif = model.PurchaseJan - auxDistributorPurchase.PurchaseJan;
                    auxContract.PurchaseTotalJan += auxAmountdif;
                    auxContract.PurchaseTotalS1 += auxAmountdif;
                    auxDistributorPurchase.PurchaseJan += auxAmountdif;
                    auxDistributorPurchase.PurchaseTotalS1 += auxAmountdif;
                }
                if (model.PurchaseFeb != auxDistributorPurchase.PurchaseFeb)
                {
                    auxAmountdif = model.PurchaseFeb - auxDistributorPurchase.PurchaseFeb;
                    auxContract.PurchaseTotalFeb += auxAmountdif;
                    auxContract.PurchaseTotalS1 += auxAmountdif;
                    auxDistributorPurchase.PurchaseFeb += auxAmountdif;
                    auxDistributorPurchase.PurchaseTotalS1 += auxAmountdif;
                }
                if (model.PurchaseMar != auxDistributorPurchase.PurchaseMar)
                {
                    auxAmountdif = model.PurchaseMar - auxDistributorPurchase.PurchaseMar;
                    auxContract.PurchaseTotalMar += auxAmountdif;
                    auxContract.PurchaseTotalS1 += auxAmountdif;
                    auxDistributorPurchase.PurchaseMar += auxAmountdif;
                    auxDistributorPurchase.PurchaseTotalS1 += auxAmountdif;
                }
                if (model.PurchaseApr != auxDistributorPurchase.PurchaseApr)
                {
                    auxAmountdif = model.PurchaseApr - auxDistributorPurchase.PurchaseApr;
                    auxContract.PurchaseTotalApr += auxAmountdif;
                    auxContract.PurchaseTotalS1 += auxAmountdif;
                    auxDistributorPurchase.PurchaseApr += auxAmountdif;
                    auxDistributorPurchase.PurchaseTotalS1 += auxAmountdif;
                }
                if (model.PurchaseMay != auxDistributorPurchase.PurchaseMay)
                {
                    auxAmountdif = model.PurchaseMay - auxDistributorPurchase.PurchaseMay;
                    auxContract.PurchaseTotalMay += auxAmountdif;
                    auxContract.PurchaseTotalS1 += auxAmountdif;
                    auxDistributorPurchase.PurchaseMay += auxAmountdif;
                    auxDistributorPurchase.PurchaseTotalS1 += auxAmountdif;
                }
                if (model.PurchaseJun != auxDistributorPurchase.PurchaseJun)
                {
                    auxAmountdif = model.PurchaseJun - auxDistributorPurchase.PurchaseJun;
                    auxContract.PurchaseTotalJun += auxAmountdif;
                    auxContract.PurchaseTotalS1 += auxAmountdif;
                    auxDistributorPurchase.PurchaseJun += auxAmountdif;
                    auxDistributorPurchase.PurchaseTotalS1 += auxAmountdif;
                }
                if (model.PurchaseJul != auxDistributorPurchase.PurchaseJul)
                {
                    auxAmountdif = model.PurchaseJul - auxDistributorPurchase.PurchaseJul;
                    auxContract.PurchaseTotalJul += auxAmountdif;
                    auxContract.PurchaseTotalS2 += auxAmountdif;
                    auxDistributorPurchase.PurchaseJul += auxAmountdif;
                    auxDistributorPurchase.PurchaseTotalS2 += auxAmountdif;
                }
                if (model.PurchaseAgo != auxDistributorPurchase.PurchaseAgo)
                {
                    auxAmountdif = model.PurchaseAgo - auxDistributorPurchase.PurchaseAgo;
                    auxContract.PurchaseTotalAgo += auxAmountdif;
                    auxContract.PurchaseTotalS2 += auxAmountdif;
                    auxDistributorPurchase.PurchaseAgo += auxAmountdif;
                    auxDistributorPurchase.PurchaseTotalS2 += auxAmountdif;
                }
                if (model.PurchaseSep != auxDistributorPurchase.PurchaseSep)
                {
                    auxAmountdif = model.PurchaseSep - auxDistributorPurchase.PurchaseSep;
                    auxContract.PurchaseTotalSep += auxAmountdif;
                    auxContract.PurchaseTotalS2 += auxAmountdif;
                    auxDistributorPurchase.PurchaseSep += auxAmountdif;
                    auxDistributorPurchase.PurchaseTotalS2 += auxAmountdif;
                }
                if (model.PurchaseOct != auxDistributorPurchase.PurchaseOct)
                {
                    auxAmountdif = model.PurchaseOct - auxDistributorPurchase.PurchaseOct;
                    auxContract.PurchaseTotalOct += auxAmountdif;
                    auxContract.PurchaseTotalS2 += auxAmountdif;
                    auxDistributorPurchase.PurchaseOct += auxAmountdif;
                    auxDistributorPurchase.PurchaseTotalS2 += auxAmountdif;
                }
                if (model.PurchaseNov != auxDistributorPurchase.PurchaseNov)
                {
                    auxAmountdif = model.PurchaseNov - auxDistributorPurchase.PurchaseNov;
                    auxContract.PurchaseTotalNov += auxAmountdif;
                    auxContract.PurchaseTotalS2 += auxAmountdif;
                    auxDistributorPurchase.PurchaseNov += auxAmountdif;
                    auxDistributorPurchase.PurchaseTotalS2 += auxAmountdif;
                }
                if (model.PurchaseDic != auxDistributorPurchase.PurchaseDic)
                {
                    auxAmountdif = model.PurchaseDic - auxDistributorPurchase.PurchaseDic;
                    auxContract.PurchaseTotalDic += auxAmountdif;
                    auxContract.PurchaseTotalS2 += auxAmountdif;
                    auxDistributorPurchase.PurchaseDic += auxAmountdif;
                    auxDistributorPurchase.PurchaseTotalS2 += auxAmountdif;
                }

                auxContract.PurchaseTotal += auxAmountdif;
                auxDistributorPurchase.PurchaseTotal += auxAmountdif;

                //update coupon discount
                //*clear coupon discount values
                auxContract.SubdistributorDiscountCoupon.CouponAmount = 0;
                auxContract.SubdistributorDiscountCoupon.CouponAmountS1 = 0;
                auxContract.SubdistributorDiscountCoupon.CouponAmountS2 = 0;
                auxContract.SubdistributorDiscountCoupon.HasCoupon = false;
                auxContract.SubdistributorDiscountCoupon.HasCouponS1 = false;
                auxContract.SubdistributorDiscountCoupon.HasCouponS2 = false;
                //*calculate new values
                decimal auxDiscountPercentage = 0;
                PSD.Controller.BenefitPrograms.Coupon.CouponManagementController couponManagementContoller = new BenefitPrograms.Coupon.CouponManagementController(Configurations);
                auxDiscountPercentage = couponManagementContoller.GetDiscountPercentage(auxContract.GoalTotalPercentage, auxContract.DiscountType);
                if (auxDiscountPercentage > 0)
                {
                    //update total
                    auxContract.SubdistributorDiscountCoupon.HasCoupon = true;
                    auxContract.SubdistributorDiscountCoupon.CouponAmount = (auxDiscountPercentage / 100) * auxContract.PurchaseTotal;
                    
                    //update semester1
                    //auxDiscountPercentage = couponManagementContoller.GetDiscountPercentage(auxContract.GoalTotalS1Percentage, auxContract.DiscountType);
                    if (auxDiscountPercentage > 0)
                    {
                        auxContract.SubdistributorDiscountCoupon.HasCouponS1 = true;
                        auxContract.SubdistributorDiscountCoupon.CouponAmountS1 = (auxDiscountPercentage / 100) * auxContract.PurchaseTotalS1;
                    }

                    //update semester2
                    //auxDiscountPercentage = couponManagementContoller.GetDiscountPercentage(auxContract.GoalTotalS2Percentage, auxContract.DiscountType);
                    if (auxDiscountPercentage > 0)
                    {
                        auxContract.SubdistributorDiscountCoupon.HasCouponS2 = true;
                        auxContract.SubdistributorDiscountCoupon.CouponAmountS2 = (auxDiscountPercentage / 100) * auxContract.PurchaseTotalS2;
                    }
                }

                //update coupon promotion
                //-clear previous amounts
                auxContract.SubdistributorPromotionCoupon.HasCoupon = false;
                auxContract.SubdistributorPromotionCoupon.HasCouponS1 = false;
                auxContract.SubdistributorPromotionCoupon.HasCouponS2 = false;
                auxContract.SubdistributorPromotionCoupon.CouponAmount = 0;
                auxContract.SubdistributorPromotionCoupon.CouponAmountS1 = 0;
                auxContract.SubdistributorPromotionCoupon.CouponAmountS2 = 0;

                //set new purchases into aux for looping
                decimal[] auxPurchaseXMonth = new decimal[13];
                auxPurchaseXMonth[1] = auxContract.PurchaseTotalJan;
                auxPurchaseXMonth[2] = auxContract.PurchaseTotalFeb;
                auxPurchaseXMonth[3] = auxContract.PurchaseTotalMar;
                auxPurchaseXMonth[4] = auxContract.PurchaseTotalApr;
                auxPurchaseXMonth[5] = auxContract.PurchaseTotalMay;
                auxPurchaseXMonth[6] = auxContract.PurchaseTotalJun;
                auxPurchaseXMonth[7] = auxContract.PurchaseTotalJul;
                auxPurchaseXMonth[8] = auxContract.PurchaseTotalAgo;
                auxPurchaseXMonth[9] = auxContract.PurchaseTotalSep;
                auxPurchaseXMonth[10] = auxContract.PurchaseTotalOct;
                auxPurchaseXMonth[11] = auxContract.PurchaseTotalNov;
                auxPurchaseXMonth[12] = auxContract.PurchaseTotalDic;

                //loop thu each month and update coupon
                decimal auxPromotionPercentage = 0;
                for (int a = 1; a <= 12; a++)
                {
                    auxPromotionPercentage = couponManagementContoller.GetPromotionPercentage(auxPurchaseXMonth[a], a);
                    if (auxPromotionPercentage > 0)
                    {
                        auxContract.SubdistributorPromotionCoupon.HasCoupon = true;
                        auxContract.SubdistributorPromotionCoupon.CouponAmount += (auxPromotionPercentage / 100) * auxPurchaseXMonth[a];

                        if (a <= 6)
                        {
                            auxContract.SubdistributorPromotionCoupon.HasCouponS1 = true;
                            auxContract.SubdistributorPromotionCoupon.CouponAmountS1 += (auxPromotionPercentage / 100) * auxPurchaseXMonth[a];
                        }
                        else
                        {
                            auxContract.SubdistributorPromotionCoupon.HasCouponS2 = true;
                            auxContract.SubdistributorPromotionCoupon.CouponAmountS2 += (auxPromotionPercentage / 100) * auxPurchaseXMonth[a];
                        }
                    }
                }

                Repository.Complete();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Edit.511 Excepción al editar montos de compras" + ex.Message);
            }

            if (ResultManager.IsCorrect)
            {
                //ResultManager.Add("La compra ha sido registrada", "");
            }

            return ResultManager.IsCorrect;
        }

    }
}
