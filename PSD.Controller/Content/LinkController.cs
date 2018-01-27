using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSD.Model;

namespace PSD.Controller.Content
{
    public class LinkController : _BaseController
    {
        public LinkController(IAppConfiguration configurations)
            : base("LinkController.", configurations)
        {

        }

        #region Addresses

        public List<ContentLink> RetrieveAll()
        {
            ResultManager.IsCorrect = false;
            List<ContentLink> auxList = null;
            try
            {
                auxList = Repository.ContentLinks.GetAll().ToList();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "RetrieveAll.511 Excepción al obtener el listado: " + ex.Message);
            }

            return auxList;
        }
        public ContentLink Retrieve(int id = -1)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no id was received
                ResultManager.Add(ErrorDefault, Trace + "Retrieve.131 No se recibio el id del cultivo");
                return null;
            }

            ContentLink auxItem = null;
            try
            {
                auxItem = Repository.ContentLinks.Get(id);
                if (auxItem == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "Retrieve.511 No se encontró un item con id '" + id + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Retrieve.511 Excepción al obtener el item: " + ex.Message);
            }

            return auxItem;
        }

        public bool Add(ContentLink item)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if(item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "Add.111 No se recibio el objeto del cultivo a editar");
                return false;
            }
            //-business validations
            if (string.IsNullOrWhiteSpace(item.DisplayName))
            {
                ResultManager.Add("El texto a mostrar no puede estar vacío");
                return false;
            }
            if (string.IsNullOrWhiteSpace(item.Url))
            {
                ResultManager.Add("La url no puede estar vacía");
                return false;
            }
            
            //insert new item
            try
            {
                //ContentLink auxNew = new ContentLink();
                //auxNew.DisplayName = item.DisplayName;
                //auxNew.Url = item.Url;
                Repository.ContentLinks.Add(item);
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Add.511 Excepción al agregar el link con id '" + item.Id + "': " + ex.Message);
            }
            return false;
        }
        public bool Update(ContentLink item)
        {
            ResultManager.IsCorrect = false;
            
            //initial validations
            //-sys validations
            if (item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "Update.111 No se recibio el objeto del item a editar");
                return false;
            }
            if (item.Id == -1 || item.Id == 0)
            {//no id was received
                ResultManager.Add(ErrorDefault, Trace + "Update.131 No se recibio el id del item a editar");
                return false;
            }
            //-business validations
            if (string.IsNullOrWhiteSpace(item.DisplayName))
            {
                ResultManager.Add("El texto a mostrar no puede estar vacío");
                return false;
            }
            if (string.IsNullOrWhiteSpace(item.Url))
            {
                ResultManager.Add("La url no puede estar vacía");
                return false;
            }

            //update item
            try
            {
                ContentLink auxItem = Repository.ContentLinks.Get(item.Id);
                if(auxItem == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "Update.311 item con id '" + item.Id + "' no encontrado en bd");
                    return false;
                }
                auxItem.DisplayName= item.DisplayName;
                auxItem.Url = item.Url;
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Update.511 Excepción al editar el item con id '" + item.Id + "': " + ex.Message);
            }
            return false;
        }
        public bool Delete(int id = -1)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no id was received
                ResultManager.Add(ErrorDefault, Trace + "Delete.131 No se recibio el id del item a editar");
                return false;
            }
            
            //delete item
            try
            {
                Repository.ContentLinks.Remove(id);
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Delete.511 Excepción al eliminar el item con id '" + id + "': " + ex.Message);
            }
            return false;
        }

        #endregion

    }
}
