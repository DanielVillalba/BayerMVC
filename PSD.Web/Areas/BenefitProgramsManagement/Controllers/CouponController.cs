using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Model;
using PSD.Security;
using PSD.Web.Areas.BenefitProgramsManagement.Models;

namespace PSD.Web.Areas.BenefitProgramsManagement.Controllers
{
    [Authorization(allowedRoles: Controller.UserRole.AppAdmin)]
    public class CouponController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.BenefitPrograms.Coupon.CouponManagementController controller;
        public CouponController()
            : base("CouponController")
        {
            controller = new Controller.BenefitPrograms.Coupon.CouponManagementController(Configurations);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DiscountsDetail()
        {
            return View(Configurations.BenefitProgram_Coupon_Discount);
        }
        public ActionResult DiscountsEdit()
        {
            if (!Configurations.BenefitProgram_Coupon_IsOpen || (!Configurations.BenefitProgram_Coupon_S1_IsOpen && !Configurations.BenefitProgram_Coupon_S2_IsOpen))
            {
                NotifyUser(messageError: "Ha concluido la temporada de captura de compras");
                return RedirectToAction("DiscountsDetail");
            }

            return View(Configurations.BenefitProgram_Coupon_Discount);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DiscountsEdit(string[][] model)
        {
            if (model == null)
            {
                NotifyUser(messageDebug: controllerTraceId + "DistributorCreate[Post].111 No model was received");
                return RedirectToAction("DiscountsDetail");
            }

            int[,] auxModel = new int[10,5];
            for (int a = 0; a < 10; a++)
            {
                for (int b = 0; b < 5; b++)
                {
                    try { auxModel[a, b] = Convert.ToInt32(model[a][b]); }
                    catch { auxModel[a, b] = 0; }
                }
            }

            if (controller.DiscountsUpdate(auxModel) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(resultManager: controller.ResultManager);
                return RedirectToAction("DiscountsDetail");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(Configurations.BenefitProgram_Coupon_Discount);
        }
        public ActionResult PromotionsDetail()
        {
            return View(Configurations.BenefitProgram_Coupon_Promotion);
        }
        public ActionResult PromotionsEdit()
        {
            if (!Configurations.BenefitProgram_Coupon_IsOpen || (!Configurations.BenefitProgram_Coupon_S1_IsOpen && !Configurations.BenefitProgram_Coupon_S2_IsOpen))
            {
                NotifyUser(messageError: "Ha concluido la temporada de captura de compras");
                return RedirectToAction("DiscountsDetail");
            }

            return View(Configurations.BenefitProgram_Coupon_Promotion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PromotionsEdit(string[][] model)
        {
            if (model == null)
            {
                NotifyUser(messageDebug: controllerTraceId + "DistributorCreate[Post].111 No model was received");
                RedirectToAction("PromotionsDetail");
            }

            int[,] auxModel = new int[12, 2];
            for (int a = 0; a < 12; a++)
            {
                for (int b = 0; b < 2; b++)
                {
                    try { auxModel[a, b] = Convert.ToInt32(model[a][b]); }
                    catch { auxModel[a, b] = 0; }
                }
            }

            if (controller.PromotionsUpdate(auxModel) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(resultManager: controller.ResultManager);
                return RedirectToAction("PromotionsDetail");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(Configurations.BenefitProgram_Coupon_Promotion);
        }

        public ActionResult CouponConfigurations()
        {
            CouponConfigurationsModel model = new CouponConfigurationsModel();
            model.IsOpen = Configurations.BenefitProgram_Coupon_IsOpen;
            model.S1IsOpen = Configurations.BenefitProgram_Coupon_S1_IsOpen;
            model.S2IsOpen = Configurations.BenefitProgram_Coupon_S2_IsOpen;
            model.IsCalculated = Configurations.BenefitProgram_Coupon_IsCalculated;
            model.S1IsCalculated = Configurations.BenefitProgram_Coupon_S1_IsCalculated;
            model.S2IsCalculated = Configurations.BenefitProgram_Coupon_S2_IsCalculated;
            return View(model);
        }
        public ActionResult CouponConfigurations_S1IsOpen_TurnOff()
        {

            if (controller.S1IsOpen_TurnOff() && controller.ResultManager.IsCorrect)
            {
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("CouponConfigurations");
        }
        public ActionResult CouponConfigurations_S1IsOpen_TurnOn()
        {

            if (controller.S1IsOpen_TurnOn() && controller.ResultManager.IsCorrect)
            {
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("CouponConfigurations");
        }
        public ActionResult CouponConfigurations_S2IsOpen_TurnOff()
        {

            if (controller.S2IsOpen_TurnOff() && controller.ResultManager.IsCorrect)
            {
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("CouponConfigurations");
        }
        public ActionResult CouponConfigurations_S2IsOpen_TurnOn()
        {

            if (controller.S2IsOpen_TurnOn() && controller.ResultManager.IsCorrect)
            {
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("CouponConfigurations");
        }


        public ActionResult CouponProcessing()
        {
            CouponConfigurationsModel model = new CouponConfigurationsModel();
            model.IsOpen = Configurations.BenefitProgram_Coupon_IsOpen;
            model.S1IsOpen = Configurations.BenefitProgram_Coupon_S1_IsOpen;
            model.S2IsOpen = Configurations.BenefitProgram_Coupon_S2_IsOpen;
            model.IsCalculated = Configurations.BenefitProgram_Coupon_IsCalculated;
            model.S1IsCalculated = Configurations.BenefitProgram_Coupon_S1_IsCalculated;
            model.S2IsCalculated = Configurations.BenefitProgram_Coupon_S2_IsCalculated;
            return View(model);
        }

        public ActionResult CouponProcessing_ProcessS1()
        {
            if (controller.ProcessS1() && controller.ResultManager.IsCorrect)
            {
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("CouponProcessing");
        }
        public ActionResult CouponProcessing_ProcessS2()
        {
            if (controller.ProcessS2() && controller.ResultManager.IsCorrect)
            {
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("CouponProcessing");
        }

        public ActionResult CouponProcessing_ReportS1()
        {
            List<ContractSubdistributor> currentContracts = controller.RetrieveActiveContracts();
            if (currentContracts.Count > 0 && controller.ResultManager.IsCorrect)
            {
                return View(currentContracts);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("CouponProcessing");
        }

        public ActionResult CouponProcessing_ReportS2()
        {
            List<ContractSubdistributor> currentContracts = controller.RetrieveActiveContracts();
            if (currentContracts.Count > 0 && controller.ResultManager.IsCorrect)
            {
                return View(currentContracts);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("CouponProcessing");
        }
        public ActionResult CouponProcessing_Restart()
        {
            if (controller.StartNewCycle() && controller.ResultManager.IsCorrect)
            {
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("CouponProcessing");
        }

    }
}