using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSD.Model;

namespace PSD.Controller.BenefitPrograms.Coupon
{
    public class CouponManagementController : _BaseController
    {
        public CouponManagementController(IAppConfiguration configurations)
            : base("CouponManagementController.", configurations)
        {

        }

        public bool DiscountsUpdate(int[,] model)
        {
            ResultManager.IsCorrect = false;

            int nRows = 10;
            int nCols = 5;

            //initial validations
            //-sys validations
            if (model == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "DiscountsUpdate.111 No se recibio el modelo");
                return false;
            }
            if (model.Length != nRows * nCols)
            {
                ResultManager.Add(ErrorDefault, Trace + "DiscountsUpdate.121 El modelo no tiene la cantidad de elementos esperada. Se esperaban " + (nCols * nRows) + "(=" + nCols + " * " + nRows + ") elementos, pero contiene '" + model.Length + "'");
                return false;
            }

            //update item
            try
            {
                //-business validations
                for (int i = 0; i < nRows; i++)
                {
                    for (int j = 0; j < nCols; j++)
                    {
                        if (j > 1 && (model[i, j] < 0 || model[i, j] > 100))//2 first cols are for limits so they can be more than 100%, the other three has to be between 0 and 100
                        {
                            ResultManager.Add("El porcentaje en '[" + (i + 1) + "," + (j + 1) + "]' no es válido (" + model[i, j] + "%), debe ser un valor entre 0 y 100");
                            return false;
                        }
                    }
                }

                StringBuilder newValue = new StringBuilder();
                for (int i = 0; i < nRows; i++)
                {
                    for (int j = 0; j < nCols; j++)
                    {
                        newValue.Append(model[i, j]);
                        if (j < nCols - 1) newValue.Append(":");
                    }
                    if (i < nRows - 1) newValue.Append("|");
                }

                AppConfigurationController configurationController = new AppConfigurationController(Configurations);
                ResultManager.IsCorrect = configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_Discount, newValue.ToString());
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "DiscountsUpdate.511 Excepción al crear el nuevo elemento" + ex.Message);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("La tabla de promociones ha sido actualizada");
            }

            return ResultManager.IsCorrect;
        }

        public bool PromotionsUpdate(int[,] model)
        {
            ResultManager.IsCorrect = false;

            int nRows = 2;
            int nCols = 12;

            //initial validations
            //-sys validations
            if (model == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "PromotionsUpdate.111 No se recibio el modelo");
                return false;
            }
            if (model.Length != nRows * nCols)
            {
                ResultManager.Add(ErrorDefault, Trace + "PromotionsUpdate.121 El modelo no tiene la cantidad de elementos esperada. Se esperaban " + (nCols * nRows) + "(=" + nCols + " * " + nRows + ") elementos, pero contiene '" + model.Length + "'");
                return false;
            }

            //update item
            try
            {
                //-business validations
                bool hasPercentage = false, hasAmount = false;
                for (int i = 0; i < nCols; i++)
                {
                    hasPercentage = hasAmount = false;
                    for (int j = 0; j < nRows; j++)
                    {
                        switch (j)
                        {
                            case 0: //percentage
                                if (model[i, j] < 0 || model[i, j] > 100)
                                {
                                    ResultManager.Add("El 'porcentaje' en '[" + (i + 1) + "," + (j + 1) + "]' no es válido (" + model[i, j] + "%), debe ser un valor entre 0 y 100");
                                    return false;
                                }
                                if (model[i, j] > 0) hasPercentage = true;
                                break;
                            case 1: //minimal amount
                                if (model[i, j] < 0)
                                {
                                    ResultManager.Add("El 'monto mínimo de compra' en '[" + (i + 1) + "," + (j + 1) + "]' no es válido (" + model[i, j] + "%), no puede ser menor a cero ");
                                    return false;
                                }
                                if (model[i, j] > 0) hasAmount = true;

                                if (hasAmount && !hasPercentage)
                                {
                                    ResultManager.Add("Se indicó un 'monto mínimo de compra' en '[" + (i + 1) + "," + (j + 1) + "]' ($" + model[i, j] + "), pero no se indicó un valor en porcentaje");
                                    return false;
                                }
                                if (!hasAmount && hasPercentage)
                                {
                                    ResultManager.Add("Se indicó un 'porcentage' en '[" + (i + 1) + "," + (j + 1) + "]' ($" + model[i, j] + "), pero no se indicó un valor en 'monto mínimo de compra'");
                                    return false;
                                }
                                break;
                        }
                    }
                }

                StringBuilder newValue = new StringBuilder();
                for (int i = 0; i < nCols; i++)
                {
                    for (int j = 0; j < nRows; j++)
                    {
                        newValue.Append(model[i, j]);
                        if (j < nRows - 1) newValue.Append(":");
                    }
                    if (i < nCols - 1) newValue.Append("|");
                }

                AppConfigurationController configurationController = new AppConfigurationController(Configurations);
                ResultManager.IsCorrect = configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_Promotion, newValue.ToString());
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "PromotionsUpdate.511 Excepción al crear el nuevo elemento" + ex.Message);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("La tabla de promociones ha sido actualizada");
            }

            return ResultManager.IsCorrect;
        }

        public int GetDiscountPercentage(decimal purchasePercentage, int discountType = 0)
        {
            int vf = 0;
            int[,] auxDiscounts = Configurations.BenefitProgram_Coupon_Discount;

            for (int i = 0; i < 10; i++)
            {
                if (purchasePercentage >= auxDiscounts[i, 0] && (purchasePercentage < auxDiscounts[i, 1] || auxDiscounts[i, 1] == 0))//between min value and (max value or max value is zero meanning there's no max value)
                {
                    switch (discountType)
                    {
                        case 2: vf = auxDiscounts[i, 4]; break;
                        case 1: vf = auxDiscounts[i, 3]; break;
                        default: vf = auxDiscounts[i, 2]; break;
                    }
                    break;
                }
            }
            return vf;
        }

        public int GetPromotionPercentage(decimal purchaseAmount, int purchaseMonth)
        {
            if (purchaseAmount >= Configurations.BenefitProgram_Coupon_Promotion[purchaseMonth - 1, 1])
            {
                return Configurations.BenefitProgram_Coupon_Promotion[purchaseMonth - 1, 0];
            }
            return 0;
        }

        public bool S1IsOpen_TurnOff()
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations

            //update item
            try
            {
                //-business validations

                AppConfigurationController configurationController = new AppConfigurationController(Configurations);
                ResultManager.IsCorrect = configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S1_IsOpen, "false");
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "S1IsOpen_TurnOff.511 Excepción al actualizar la configuración" + ex.Message);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("El semestre 1 ha sido bloqueado");
            }

            return ResultManager.IsCorrect;
        }

        public bool S1IsOpen_TurnOn()
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations

            //update item
            try
            {
                //-business validations

                AppConfigurationController configurationController = new AppConfigurationController(Configurations);
                ResultManager.IsCorrect = configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S1_IsOpen, "true");
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "S1IsOpen_TurnOff.511 Excepción al actualizar la configuración" + ex.Message);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("El semestre 1 ha sido desbloqueado");
            }

            return ResultManager.IsCorrect;
        }

        public bool S2IsOpen_TurnOff()
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations

            //update item
            try
            {
                //-business validations

                AppConfigurationController configurationController = new AppConfigurationController(Configurations);
                ResultManager.IsCorrect = configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S2_IsOpen, "false");
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "S2IsOpen_TurnOff.511 Excepción al actualizar la configuración" + ex.Message);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("El semestre 2 ha sido bloqueado");
            }

            return ResultManager.IsCorrect;
        }

        public bool S2IsOpen_TurnOn()
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations

            //update item
            try
            {
                //-business validations

                AppConfigurationController configurationController = new AppConfigurationController(Configurations);
                ResultManager.IsCorrect = configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S2_IsOpen, "true");
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "S2IsOpen_TurnOff.511 Excepción al actualizar la configuración" + ex.Message);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("El semestre 2 ha sido desbloqueado");
            }

            return ResultManager.IsCorrect;
        }

        public bool ProcessS1()
        {
            ResultManager.Clear();

            //initial validations
            //-sys validations
            if (false)
            {
                ResultManager.Add(ErrorDefault, Trace + "ProcessS1.111 ");
                return false;
            }

            //process contracts for semester1
            try
            {
                //ContractSubdistributor contractController = new ContractSubdistributor(Configurations);
                List<ContractSubdistributor> currentContracts = Repository.ContractsSubdistributor.GetAllActive();

                foreach (ContractSubdistributor item in currentContracts)
                {
                    if (true)//item.SubdistributorDiscountCoupon.HasCouponS1)
                    {
                        foreach (DistributorPurchasesXContractSubdistributor purchaseDistributor in item.DistributorPurchases)
                        {
                            purchaseDistributor.CouponSharePercentageS1 = item.PurchaseTotalS1 <= 0 ? 0 : (purchaseDistributor.PurchaseTotalS1 / item.PurchaseTotalS1) * 100;
                            purchaseDistributor.CouponShareAmountS1 = item.SubdistributorDiscountCoupon.CouponAmountS1 * (purchaseDistributor.CouponSharePercentageS1 / 100);
                        }
                    }

                    item.SubdistributorDiscountCoupon.IsCalculatedS1 = true;
                    item.SubdistributorPromotionCoupon.IsCalculatedS1 = true;
                }

                Repository.Complete();

                AppConfigurationController configurationController = new AppConfigurationController(Configurations);
                if (configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S1_IsCalculated, "true")
                    && configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S1_IsOpen, "false"))
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ProcessS1.511 Excepción al procesar el semestre 1, la tabla de configuraciones podría haberse corrompido", ex);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("El semestre 1 ha sido procesado correctamente");
            }

            return ResultManager.IsCorrect;
        }
        public bool ProcessS2()
        {
            ResultManager.Clear();

            //initial validations
            //-sys validations
            if (false)
            {
                ResultManager.Add(ErrorDefault, Trace + "ProcessS2.111 ");
                return false;
            }

            //process contracts for semester1
            try
            {
                //ContractSubdistributor contractController = new ContractSubdistributor(Configurations);
                List<ContractSubdistributor> currentContracts = Repository.ContractsSubdistributor.GetAllActive();

                foreach (ContractSubdistributor item in currentContracts)
                {
                    if (true)//item.SubdistributorDiscountCoupon.HasCouponS1)
                    {
                        foreach (DistributorPurchasesXContractSubdistributor purchaseDistributor in item.DistributorPurchases)
                        {
                            purchaseDistributor.CouponSharePercentageS2 = item.PurchaseTotalS2 <= 0 ? 0 : (purchaseDistributor.PurchaseTotalS2 / item.PurchaseTotalS2) * 100;
                            purchaseDistributor.CouponShareAmountS2 = item.SubdistributorDiscountCoupon.CouponAmountS2 * (purchaseDistributor.CouponSharePercentageS2 / 100);

                            purchaseDistributor.CouponSharePercentage = item.PurchaseTotal <= 0 ? 0 : (purchaseDistributor.PurchaseTotal / item.PurchaseTotal) * 100;
                            purchaseDistributor.CouponShareAmount = item.SubdistributorDiscountCoupon.CouponAmount * (purchaseDistributor.CouponSharePercentage / 100);
                        }
                    }

                    item.SubdistributorDiscountCoupon.IsCalculatedS2 = true;
                    item.SubdistributorDiscountCoupon.IsCalculated = true;
                    item.SubdistributorPromotionCoupon.IsCalculatedS2 = true;
                    item.SubdistributorPromotionCoupon.IsCalculated = true;

                }

                Repository.Complete();

                AppConfigurationController configurationController = new AppConfigurationController(Configurations);
                if (configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S2_IsCalculated, "true")
                    && configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_IsCalculated, "true")
                    && configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S2_IsOpen, "false")
                    && configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_IsOpen, "false")
                    )
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ProcessS1.511 Excepción al procesar el semestre 1, la tabla de configuraciones podría haberse corrompido", ex);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("El semestre 1 ha sido procesado correctamente");
            }

            return ResultManager.IsCorrect;
        }

        public List<ContractSubdistributor> RetrieveActiveContracts()
        {
            ResultManager.Clear();

            //initial validations
            //-sys validations
            if (false)
            {
                ResultManager.Add(ErrorDefault, Trace + "ProcessS2.111 ");
                return new List<ContractSubdistributor>();
            }

            List<ContractSubdistributor> currentContracts = new List<ContractSubdistributor>();
            try
            {
                currentContracts = Repository.ContractsSubdistributor.GetAllActive();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ProcessS1.511 Excepción al procesar el semestre 1, la tabla de configuraciones podría haberse corrompido", ex);
            }

            if (ResultManager.IsCorrect)
            {
            }

            return currentContracts; ;
        }

        public bool StartNewCycle()
        {
            ResultManager.Clear();

            //initial validations
            //-sys validations
            if (false)
            {
                ResultManager.Add(ErrorDefault, Trace + "StartNewCycle.111 ");
                return false;
            }

            //process contracts for semester1
            try
            {
                //ContractSubdistributor contractController = new ContractSubdistributor(Configurations);
                List<ContractSubdistributor> currentContracts = Repository.ContractsSubdistributor.GetAllActive();

                foreach (ContractSubdistributor item in currentContracts)
                {
                    item.ContractSubdistributorStatusId = 2;//expired
                    item.Subdistributor.CurrentContract = null;
                }

                Repository.Complete();

                AppConfigurationController configurationController = new AppConfigurationController(Configurations);
                if (configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S1_IsCalculated, "false")
                    && configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S2_IsCalculated, "false")
                    && configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_IsCalculated, "false")
                    && configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S1_IsOpen, "true")
                    && configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_S2_IsOpen, "true")
                    && configurationController.Update(PSD.Model.Keys.AppConfigurationKey.BenefitProgram_Coupon_IsOpen, "true")
                    )
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "StartNewCycle.511 Excepción al iniciar nuevo ciclo, la tabla de configuraciones podría haberse corrompido", ex);
            }

            if (ResultManager.IsCorrect)
            {
                ResultManager.Add("Ciclo reiniciado, todos los convenios has sido caducados");
            }

            return ResultManager.IsCorrect;
        }

    }
}
