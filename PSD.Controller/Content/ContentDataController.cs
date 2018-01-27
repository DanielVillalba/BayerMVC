using PSD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Controller.Content
{
    public class ContentDataController : _BaseController
    {
        /// <summary>
        /// Centralized way to handle ContentData keys
        /// </summary>
        public Dictionary<ContentTypeKey, string> Keys
        {
            get
            {
                return contentKeys;
            }
        }
        private Dictionary<ContentTypeKey, string> contentKeys = new Dictionary<ContentTypeKey, string>
        {
            { ContentTypeKey.StartPageTitle, "StartPageTitle" },
            { ContentTypeKey.StartPageSubtitle, "StartPageSubtitle" },
            { ContentTypeKey.StartPageParagraph, "StartPageParagraph" },
            { ContentTypeKey.StartPageButton, "StartPageButton" },
            { ContentTypeKey.ContactPageContent, "ContactPageContent" }
        };

        public ContentDataController(IAppConfiguration configurations)
            : base ("ContentDataController.", configurations)
        {

        }

        public ContentData GetContentDataByKey(ContentTypeKey key)
        {
            ResultManager.IsCorrect = false;

            ContentData item = null;
            try
            {
                string lookupKey = Keys.ContainsKey(key)
                                ? Keys[key]
                                : string.Empty;

                item = Repository.ContentData.GetAll().Where(x => string.Equals(x.Key, lookupKey)).FirstOrDefault();
            }
            catch(Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Retrieve.511 Excepción al obtener el contenido con llave " + key + ": " + ex.Message);
            }

            return item;
        }

        public bool Update(ContentTypeKey key, string value)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (value == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "ContenDataEdit.111 Value del contenido a editar no puede ser nulo");
                return false;
            }

            //update item
            try
            {

                string lookupKey = Keys.ContainsKey(key)
                                ? Keys[key]
                                : string.Empty;

                ContentData item = Repository.ContentData.GetAll().Where(x => string.Equals(x.Key, lookupKey)).FirstOrDefault();
                bool isNewContentKey = false;
                if (item == null)
                {
                    isNewContentKey = true;
                     item = new ContentData();
                }

                item.Key = Keys[key];
                item.Value = value;

                if(isNewContentKey)
                    Repository.ContentData.Add(item);

                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ContenDataEdit.511 Excepción al editar el contenido con key '" + key + "': " + ex.Message);
            }
            return false;
        }

        #region Contact Page

        public bool SendContactMail(string name, string mail, string message)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            //-business validations
            if (string.IsNullOrEmpty(name))
            {
                ResultManager.Add(ErrorDefault, Trace + "SendContactMail.111 No se recibio el nombre del contacto");
                return false;
            }
            if (string.IsNullOrEmpty(mail))
            {
                ResultManager.Add(ErrorDefault, Trace + "SendContactMail.111 No se recibio el e mail del contacto");
                return false;
            }
            if (string.IsNullOrEmpty(message))
            {
                ResultManager.Add(ErrorDefault, Trace + "SendContactMail.111 No se recibio el mensaje del contacto");
                return false;
            }

            if (SendContactEmail(name, mail, message))
            {
                ResultManager.IsCorrect = true;
                ResultManager.Add("Se ha enviado un correo con la informacion de contacto", "");
            }
            else
            {
                ResultManager.Add("No se pudo enviar informacion de contacto");
            }

            return ResultManager.IsCorrect;
        }
        #endregion

        #region Helpers
        private bool SendContactEmail(string contactName, string contactMail, string contactMessage)
        {
            Dictionary<string, string> emailParams = new Dictionary<string, string>() {
                { "contactName", contactName },
                { "contactMail", contactMail },
                { "contactMessage", contactMessage }
            };
            string subject = Configurations.LayoutEmailContactPage_Subject;
            string messageBody = Configurations.LayoutEmailContactPage_Body;
            string mailAddress = Configurations.MailServerContactPageMailTo;
            if (SendEmail(mailAddress, subject, messageBody, emailParams))
            {
                return true;
            }
            else
            {
                ErrorManager.Add(Trace + "SendContactEmail.911", "Error while sending contact email to '" + mailAddress + "' from '" + contactMail + "' ");
                return false;
            }
        }
        #endregion
    }

    /// <summary>
    /// enumeration to provide control the ContentData keys
    /// </summary>
    public enum ContentTypeKey
    {
        StartPageTitle,
        StartPageSubtitle,
        StartPageParagraph,
        StartPageButton,
        ContactPageContent
    }
}
