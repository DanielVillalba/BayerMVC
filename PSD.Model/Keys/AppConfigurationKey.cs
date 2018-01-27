using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model.Keys
{
    public class AppConfigurationKey
    {
        public const string

            IsDebugEnabled = "isdebugenabled",

            //password management
            PasswordValidFormatPattern = "userpassword-validformat",
            PasswordInvalidFormatMessage = "userpassword-msginvalidformat",
            PasswordExpireDays = "userpassword-expiredays",

            //mail server
            MailServerSmtpAddress = "mailserver-smtp-address",
            MailServerSmtpPort = "mailserver-smtp-port",
            MailServerFromEmail = "mailserver-from-email",
            MailServerFromPassword = "mailserver-from-password",
            MailServerFromDisplayName = "mailserver-from-displayname",
            MailServerMailIsBodyHtml = "mailserver-mail-isbodyhtml",
            MailServerMailUseSSL = "mailserver-mail-usessl",
            MailServerMailToCC = "mailserver-mail-tocc",
            MailServerMailToCCO = "mailserver-mail-tocco",
            MailServerContactPageMailTo = "mailserver-contactpage-mail-to",

            //idb counters
            IdBCounterDistributor = "idbcounter-distributor",
            IdBCounterDistributorPrefix = "idbcounterprefix-distributor",
            IdBCounterSubdistributor = "idbcounter-subdistributor",
            IdBCounterSubdistributorPrefix = "idbcounterprefix-subdistributor",
            IdBCounterContractDistributor = "idbcounter-contract-distributor",
            IdBCounterContractDistributorPrefix = "idbcounterprefix-contract-distributor",
            IdBCounterContractSubdistributor = "idbcounter-contract-subdistributor",
            IdBCounterContractSubdistributorPrefix = "idbcounterprefix-contract-subdistributor",
            
            //Layout emails
            LayoutEmailContractDistributor_SendTodistributor_Subject = "layoutemail-contractdistributor-sendtodistributor-subject",
            LayoutEmailContractDistributor_SendTodistributor_Body = "layoutemail-contractdistributor-sendtodistributor-body",
            LayoutEmailContactPage_Subject = "layoutemail-contactpage-subject",
            LayoutEmailContactPage_Body = "layoutemail-contactpage-body",

            //Benefit programs
            BenefitProgram_Coupon_Discount = "benefitprogram-coupon-discount",
            BenefitProgram_Coupon_Promotion = "benefitprogram-coupon-promotion",
            BenefitProgram_Coupon_S1_IsOpen = "benefitprogram-coupon-s1-isopen",
            BenefitProgram_Coupon_S2_IsOpen = "benefitprogram-coupon-s2-isopen",
            BenefitProgram_Coupon_IsOpen = "benefitprogram-coupon-isopen",
            BenefitProgram_Coupon_S1_IsCalculated = "benefitprogram-coupon-s1-iscalculated",
            BenefitProgram_Coupon_S2_IsCalculated = "benefitprogram-coupon-s2-iscalculated",
            BenefitProgram_Coupon_IsCalculated = "benefitprogram-coupon-iscalculated",

            //News management
            NewsImagesStoragePath = "news-Images-Storage-Path"
            ;


    }
}
