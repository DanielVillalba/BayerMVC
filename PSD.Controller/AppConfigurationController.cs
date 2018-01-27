using PSD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Controller
{
    public class AppConfigurationController : _BaseController
    {
        public AppConfigurationController(IAppConfiguration configurations)
            : base("AppConfigurationController.", configurations)
        {

        }

        public bool Update(string key, string newValue)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (string.IsNullOrWhiteSpace(key))
            {
                ResultManager.Add(ErrorDefault, Trace + "Update.111 No se recibio el parámetro 'key'");
                return false;
            }
            
            //update item
            try
            {
                AppConfiguration auxConfiguration = Repository.AppConfigurations.GetByKey(key);
                auxConfiguration.Value = newValue;

                Repository.Complete();

                Configurations = RetrieveConfigurations(Configurations);
                
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Update.511 Excepción al actualizar la configuración '" + key + "' con el nuevo valor '" + newValue + "'", ex);
            }
                        
            return ResultManager.IsCorrect;
        }
    }
}
