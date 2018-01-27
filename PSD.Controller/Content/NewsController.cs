using PSD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Controller.Content
{
    public class NewsController : _BaseController
    {
        public NewsController(IAppConfiguration configurations)
            : base("NewsController.", configurations)
        {

        }

        public List<News> RetrieveAll()
        {
            ResultManager.IsCorrect = false;
            List<News> auxList = null;
            try
            {
                auxList = Repository.News.GetAll().ToList();
                ResultManager.IsCorrect = true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "RetrieveAll.511 Excepción al obtener el listado de Aviso/Promocion: " + ex.Message);
            }

            return auxList;
        }

        public News Retrieve(int id = -1)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no id was received
                ResultManager.Add(ErrorDefault, Trace + "Retrieve.131 No se recibio el id del Aviso/Promocion");
                return null;
            }

            News auxItem = null;
            try
            {
                auxItem = Repository.News.Get(id);
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

        public bool Add(News item)
        {
            // the RetrieveAll method will set the IsCorrect control flag to true, thats why its executed before is set to False
            bool IsDuplicatedURLId = RetrieveAll().Any(x => string.Equals(x.URLId, item.URLId));

            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "Add.111 No se recibio el objeto del Aviso/Promocion a crear");
                return false;
            }
            //-business validations
            if (string.IsNullOrWhiteSpace(item.URLId))
            {
                ResultManager.Add("El URL id no puede estar vacío");
                return false;
            }
            if (IsDuplicatedURLId)
            {
                ResultManager.Add("El url id a mostrar ya existe, este dato debe ser unico para ser usado como parte del URL");
                ResultManager.Add(ErrorDefault, Trace + "NewsAdd.311 Aviso/Evento con URL Id '" + item.URLId + "' ya existe en bd");
                return false;
            }
            if (!Uri.IsWellFormedUriString(item.URLId, UriKind.RelativeOrAbsolute))
            {
                ResultManager.Add("El url id a mostrar no es correcto para ser usado como parte del URL");
                return false;
            }

            if (string.IsNullOrWhiteSpace(item.Title))
            {
                ResultManager.Add("El titulo no puede estar vacío");
                return false;
            }
            if(item.Title.Length > 50)
            {
                ResultManager.Add("El titulo a mostrar no puede ser mayor a 50 caracteres");
            }

            if (string.IsNullOrWhiteSpace(item.Subtitle))
            {
                ResultManager.Add("El subtitulo no puede estar vacío");
                return false;
            }
            if (item.Title.Length > 200)
            {
                ResultManager.Add("El subtitulo a mostrar no puede ser mayor a 200 caracteres");
            }

            if (string.IsNullOrWhiteSpace(item.Paragraph))
            {
                ResultManager.Add("El parrafo principal no puede estar vacío");
                return false;
            }

            if (string.IsNullOrWhiteSpace(item.Image))
            {
                ResultManager.Add("La imagen principal no puede estar vacío");
                return false;
            }

            if (string.IsNullOrWhiteSpace(item.Author))
            {
                ResultManager.Add("El autor no puede estar vacio");
                return false;
            }

            //insert new item
            try
            {
                Repository.News.Add(item);
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Add.511 Excepción al agregar el Aviso/Promocion: " + ex.Message);
            }
            return false;
        }

        public bool Update(News item)
        {
            // the RetrieveAll method will set the IsCorrect control flag to true, thats why its executed before is set to False
            IEnumerable<News> test = RetrieveAll().Where(x => string.Equals(x.URLId, item.URLId));
            bool IsDuplicatedURLId = test.Count() > 0
                                    ? test.FirstOrDefault().Id != item.Id
                                    : false;


            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "Add.111 No se recibio el objeto del Aviso/Promocion a editar");
                return false;
            }
            //-business validations
            if (string.IsNullOrWhiteSpace(item.URLId))
            {
                ResultManager.Add("El URL id no puede estar vacío");
                return false;
            }
            if (IsDuplicatedURLId)
            {
                ResultManager.Add("El url id a mostrar ya existe, este dato debe ser unico para ser usado como parte del URL");
                ResultManager.Add(ErrorDefault, Trace + "NewsAdd.311 Aviso/Evento con URL Id '" + item.URLId + "' ya existe en bd");
                return false;
            }
            if (!Uri.IsWellFormedUriString(item.URLId, UriKind.RelativeOrAbsolute))
            {
                ResultManager.Add("El url id a mostrar no es correcto para ser usado como parte del URL");
                return false;
            }

            if (string.IsNullOrWhiteSpace(item.Title))
            {
                ResultManager.Add("El titulo no puede estar vacío");
                return false;
            }
            if (item.Title.Length > 50)
            {
                ResultManager.Add("El titulo a mostrar no puede ser mayor a 50 caracteres");
            }

            if (string.IsNullOrWhiteSpace(item.Subtitle))
            {
                ResultManager.Add("El subtitulo no puede estar vacío");
                return false;
            }
            if (item.Title.Length > 200)
            {
                ResultManager.Add("El subtitulo a mostrar no puede ser mayor a 200 caracteres");
            }

            if (string.IsNullOrWhiteSpace(item.Paragraph))
            {
                ResultManager.Add("El parrafo principal no puede estar vacío");
                return false;
            }

            if (string.IsNullOrWhiteSpace(item.Image))
            {
                ResultManager.Add("La imagen principal no puede estar vacío");
                return false;
            }

            if (string.IsNullOrWhiteSpace(item.Author))
            {
                ResultManager.Add("El autor no puede estar vacio");
                return false;
            }

            //update item
            try
            {
                News auxNews = Repository.News.Get(item.Id);
                if (auxNews == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "NewsEdit.311 Aviso/Evento con id '" + item.Id + "' no encontrado en bd");
                    return false;
                }

                // first remove all the news sections, they will be re associated with the iformation from the item
                var sectionIdsToDelete = auxNews.NewsSections.Select(x => x.Id).ToList();
                foreach(int sectionId in sectionIdsToDelete)
                {
                    Repository.NewsSection.Remove(sectionId);
                }

                auxNews.URLId = item.URLId;
                auxNews.Title = item.Title;
                auxNews.Subtitle = item.Subtitle;
                auxNews.Paragraph = item.Paragraph;
                auxNews.Image = item.Image;
                auxNews.ImageFooter = item.ImageFooter;
                auxNews.Author = item.Author;

                auxNews.IsDistributorVisible = item.IsDistributorVisible;
                auxNews.IsSubdistributorVisible = item.IsSubdistributorVisible;
                auxNews.IsFarmerVisible = item.IsFarmerVisible;
                auxNews.IsPublished = item.IsPublished;
                auxNews.PublishDate = item.PublishDate;
                auxNews.IsNonAuthenticatedVisible = item.IsNonAuthenticatedVisible;

                auxNews.NewsSections = item.NewsSections;

                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "NewsEdit.511 Excepción al editar el Aviso/Evento con id '" + item.Id + "': " + ex.Message);
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
                ResultManager.Add(ErrorDefault, Trace + "Delete.131 No se recibio el id del Aviso/Evento a eliminar");
                return false;
            }

            //delete item
            try
            {
                News auxNews = Repository.News.Get(id);
                if (auxNews == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "NewsEdit.311 Aviso/Evento con id '" + id + "' no encontrado en bd");
                    return false;
                }

                // first remove all the news sections, they will be re associated with the iformation from the item
                var sectionIdsToDelete = auxNews.NewsSections.Select(x => x.Id).ToList();
                foreach (int sectionId in sectionIdsToDelete)
                {
                    Repository.NewsSection.Remove(sectionId);
                }

                Repository.News.Remove(id);
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "Delete.511 Excepción al eliminar el Aviso/Evento con id '" + id + "': " + ex.Message);
            }
            return false;
        }
    }
}
