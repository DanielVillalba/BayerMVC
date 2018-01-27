using PSD.Model;
using PSD.Security;
using PSD.Web.Areas.Content.Models;
using PSD.Web.Areas.ContentManagement.Models;
using PSD.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSD.Web.Areas.ContentManagement.Controllers
{
    [Authorization(allowedRoles: "appadmin")]
    public class NewsController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.Content.NewsController controller;
        public NewsController()
            : base("NewsController")
        {
            controller = new Controller.Content.NewsController(Configurations);
        }
        // GET: ContentManagement/News
        public ActionResult Index()
        {
            List<News> newsList = controller.RetrieveAll();
            if (controller.ResultManager.IsCorrect)
            {
                return View(newsList);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        // GET: ContentManagement/News/Details/this_is_a_test
        public ActionResult Detail(string titleId = "")
        {
            var newsFeed = controller.RetrieveAll().ToList();
            News newsDetail = newsFeed.Where(x => x.URLId.Equals(titleId)).FirstOrDefault();

            var previous = Utils.GetPrevious<News>(newsFeed, newsDetail);
            var next = Utils.GetNext<News>(newsFeed, newsDetail);

            DetailViewModel model = new DetailViewModel()
            {
                DetailNews = newsDetail,
                PreviousNews = previous.Id != newsDetail.Id ? previous : null,  //providing different news only
                NextNews = next.Id != newsDetail.Id && next.Id != previous.Id ? next : null  //providing different news only  
            };
            return View(model);
        }

        // GET: ContentManagement/News/Create
        public ActionResult Create()
        {
            NewsViewModel model = new NewsViewModel
            {
                // this is required for summernote rich text editor handling, otherwise it couses focus issues
                Paragraph = "<p><br></p>",
                S1Paragraph = "<p><br></p>",
                S2Paragraph = "<p><br></p>",
                S3Paragraph = "<p><br></p>",
                S4Paragraph = "<p><br></p>",
                S5Paragraph = "<p><br></p>"
            };
            return View(model);
        }

        // POST: ContentManagement/News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsViewModel model)
        {
            var validImageTypes = new string[]  //TODO: Send to webconfig
            {
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
            };

            List<string> modelImageErrors = ValidateModelImages(model, validImageTypes);
            foreach(string imageError in modelImageErrors)
            {
                ModelState.AddModelError("ImageUpload", imageError);
            }

            if (ModelState.IsValid)
            {
                // validate models main news data
                News newsData = new News
                {
                    URLId = model.URLId,
                    Title = model.Title,
                    Subtitle = model.Subtitle,
                    Paragraph = HttpUtility.HtmlEncode(model.Paragraph),
                    Author = model.Author,

                    // configuration
                    IsDistributorVisible = model.IsDistributorVisible,
                    IsSubdistributorVisible = model.IsSubdistributorVisible,
                    IsFarmerVisible = model.IsFarmerVisible,
                    IsNonAuthenticatedVisible = model.IsNonAuthenticatedVisible,
                    IsPublished = model.IsPublished,
                    PublishDate = model.IsPublished ? DateTime.Now : (DateTime?)null
                };

                // main image
                if (model.Image != null && model.Image.ContentLength > 0)
                {
                    newsData.Image = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                    newsData.ImageFooter = model.ImageFooter ?? string.Empty;
                    string _path = Path.Combine(Server.MapPath(ImageStoragePath), newsData.Image);
                    model.Image.SaveAs(_path);
                    model.ImageName = newsData.Image;
                }

                // validate and add sections
                model.S1ImageName = ValidateAndProcessSection(model.S1Paragraph, model.S1ImageFooter, model.S1Image, newsData);
                model.S2ImageName = ValidateAndProcessSection(model.S2Paragraph, model.S2ImageFooter, model.S2Image, newsData);
                model.S3ImageName = ValidateAndProcessSection(model.S3Paragraph, model.S3ImageFooter, model.S3Image, newsData);
                model.S4ImageName = ValidateAndProcessSection(model.S4Paragraph, model.S4ImageFooter, model.S4Image, newsData);
                model.S5ImageName = ValidateAndProcessSection(model.S5Paragraph, model.S5ImageFooter, model.S5Image, newsData);

                // PSD pattern
                if (controller.Add(newsData) || controller.ResultManager.IsCorrect)
                {
                    NotifyUser(messageOk: "Aviso/Promocion agregado correctamente");
                    return RedirectToAction("Index");
                }

                NotifyUser(resultManager: controller.ResultManager);
                return View(model);
            }

            return View(model);
        }

        // GET: ContentManagement/News/Edit/5
        public ActionResult Edit(int id)
        {
            News itemToEdit = controller.Retrieve(id);
            TempData["OriginalNewsData"] = itemToEdit;

            // populate the view model out of the model
            NewsViewModel model = new NewsViewModel()
            {
                Id = itemToEdit.Id,
                URLId = itemToEdit.URLId,
                Title = itemToEdit.Title,
                Subtitle = itemToEdit.Subtitle,
                Paragraph = HttpUtility.HtmlDecode(itemToEdit.Paragraph),
                ImageName = itemToEdit.Image,
                ImageFooter = itemToEdit.ImageFooter,
                Author = itemToEdit.Author,
                IsDistributorVisible = itemToEdit.IsDistributorVisible,
                IsSubdistributorVisible = itemToEdit.IsSubdistributorVisible,
                IsFarmerVisible = itemToEdit.IsFarmerVisible,
                IsNonAuthenticatedVisible = itemToEdit.IsNonAuthenticatedVisible,
                IsPublished = itemToEdit.IsPublished,

                // this is required for summernote rich text editor handling, otherwise it couses focus issues
                S1Paragraph = "<p><br></p>",
                S2Paragraph = "<p><br></p>",
                S3Paragraph = "<p><br></p>",
                S4Paragraph = "<p><br></p>",
                S5Paragraph = "<p><br></p>"
            };

            foreach(NewsSection section in itemToEdit.NewsSections)
            {
                if (IsSectionAvailable(model.S1Paragraph, model.S1ImageName, model.S1ImageFooter))
                {
                    model.S1Id = section.Id;
                    model.S1Paragraph = HttpUtility.HtmlDecode(section.Paragraph);
                    model.S1ImageName = section.Image;
                    model.S1ImageFooter = section.ImageFooter;
                    continue;
                }

                if (IsSectionAvailable(model.S2Paragraph, model.S2ImageName, model.S2ImageFooter))
                {
                    model.S2Id = section.Id;
                    model.S2Paragraph = HttpUtility.HtmlDecode(section.Paragraph);
                    model.S2ImageName = section.Image;
                    model.S2ImageFooter = section.ImageFooter;
                    continue;
                }

                if (IsSectionAvailable(model.S3Paragraph, model.S3ImageName, model.S3ImageFooter))
                {
                    model.S3Id = section.Id;
                    model.S3Paragraph = HttpUtility.HtmlDecode(section.Paragraph);
                    model.S3ImageName = section.Image;
                    model.S3ImageFooter = section.ImageFooter;
                    continue;
                }

                if (IsSectionAvailable(model.S4Paragraph, model.S4ImageName, model.S4ImageFooter))
                {
                    model.S4Id = section.Id;
                    model.S4Paragraph = HttpUtility.HtmlDecode(section.Paragraph);
                    model.S4ImageName = section.Image;
                    model.S4ImageFooter = section.ImageFooter;
                    continue;
                }

                if (IsSectionAvailable(model.S5Paragraph, model.S5ImageName, model.S5ImageFooter))
                {
                    model.S5Id = section.Id;
                    model.S5Paragraph = HttpUtility.HtmlDecode(section.Paragraph);
                    model.S5ImageName = section.Image;
                    model.S5ImageFooter = section.ImageFooter;
                    continue;
                }
            }

            if (controller.ResultManager.IsCorrect)
            {
                return View(model);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }

        // POST: ContentManagement/News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsViewModel model)
        {
            News itemToEdit = TempData["OriginalNewsData"] as News;
            var validImageTypes = new string[]  //TODO: Send to webconfig
            {
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
            };

            List<string> modelImageErrors = ValidateModelImagesToEdit(model, validImageTypes, itemToEdit);
            foreach (string imageError in modelImageErrors)
            {
                ModelState.AddModelError("ImageUpload", imageError);
            }

            if (ModelState.IsValid)
            {
                // validate models main news data
                News newsData = new News
                {
                    Id = itemToEdit.Id,
                    URLId = model.URLId,
                    Title = model.Title,
                    Subtitle = model.Subtitle,
                    Paragraph = HttpUtility.HtmlEncode(model.Paragraph),
                    ImageFooter = model.ImageFooter ?? string.Empty,
                    Author = model.Author,

                    // configuration
                    IsDistributorVisible = model.IsDistributorVisible,
                    IsSubdistributorVisible = model.IsSubdistributorVisible,
                    IsFarmerVisible = model.IsFarmerVisible,
                    IsNonAuthenticatedVisible = model.IsNonAuthenticatedVisible,
                    IsPublished = model.IsPublished,
                    PublishDate = model.IsPublished ? DateTime.Now : (DateTime?)null
                };

                // main image
                if (model.IsImageDirty && model.Image != null && model.Image.ContentLength > 0)
                {
                    // delete previous image
                    DeleteFile(itemToEdit.Image);

                    // save new image
                    newsData.Image = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                    string _path = Path.Combine(Server.MapPath(ImageStoragePath), newsData.Image);
                    model.Image.SaveAs(_path);
                    model.ImageName = newsData.Image;
                }
                else
                {
                    newsData.Image = itemToEdit.Image;
                    model.ImageName = itemToEdit.Image;
                }

                // validate and update sections
                model.S1ImageName = ValidateAndProcessEditSection(model.IsS1ImageDirty, model.S1Id, model.S1Paragraph, model.S1ImageFooter, model.S1Image, newsData, itemToEdit);
                model.S2ImageName = ValidateAndProcessEditSection(model.IsS2ImageDirty, model.S2Id, model.S2Paragraph, model.S2ImageFooter, model.S2Image, newsData, itemToEdit);
                model.S3ImageName = ValidateAndProcessEditSection(model.IsS3ImageDirty, model.S3Id, model.S3Paragraph, model.S3ImageFooter, model.S3Image, newsData, itemToEdit);
                model.S4ImageName = ValidateAndProcessEditSection(model.IsS4ImageDirty, model.S4Id, model.S4Paragraph, model.S4ImageFooter, model.S4Image, newsData, itemToEdit);
                model.S5ImageName = ValidateAndProcessEditSection(model.IsS5ImageDirty, model.S5Id, model.S5Paragraph, model.S5ImageFooter, model.S5Image, newsData, itemToEdit);

                // PSD pattern
                if (controller.Update(newsData) || controller.ResultManager.IsCorrect)
                {
                    NotifyUser(messageOk: "Aviso/Promocion editado correctamente");
                    return RedirectToAction("Index");
                }

                NotifyUser(resultManager: controller.ResultManager);
                return View(model);
            }

            return View(model);
        }

        // GET: ContentManagement/News/Delete/5
        public ActionResult Delete(int id)
        {
            News itemToEdit = controller.Retrieve(id);
            List<string> filesToDelete = ImageFromThisNews(itemToEdit);

            if (controller.Delete(id) || controller.ResultManager.IsCorrect)
            {
                // delete image files
                foreach (string fileName in filesToDelete)
                {
                    DeleteFile(fileName);
                }

                NotifyUser(messageOk: "Aviso/Promocion eliminado correctamente");
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }


        #region Helper Methods

        /// <summary>
        /// Validates the parameters required to populate a NewsSection entity and assign it to the News if required
        /// </summary>
        /// <param name="sectionParagraph"></param>
        /// <param name="sectionImageFooter"></param>
        /// <param name="sectionImage"></param>
        /// <param name="newsData"></param>
        /// <returns>A string with the assigned name to the saved image</returns>
        private string ValidateAndProcessSection(string sectionParagraph, string sectionImageFooter, HttpPostedFileBase sectionImage, News newsData)
        {
            string newSectionImageName = string.Empty;
            bool isEmptyParagraph = string.Equals(sectionParagraph, "<p><br></p>");
            if (!isEmptyParagraph || sectionImage != null)
            {
                NewsSection section = new NewsSection
                {
                    Paragraph = HttpUtility.HtmlEncode(sectionParagraph)
                };

                if (sectionImage != null && sectionImage.ContentLength > 0)
                {
                    section.Image = Guid.NewGuid().ToString() + Path.GetExtension(sectionImage.FileName);
                    section.ImageFooter = sectionImageFooter ?? string.Empty;
                    string _path = Path.Combine(Server.MapPath(ImageStoragePath), section.Image);
                    sectionImage.SaveAs(_path);
                    newSectionImageName = section.Image;
                }
                newsData.NewsSections.Add(section);
            }

            return newSectionImageName;
        }

        private string ValidateAndProcessEditSection(bool isImageDirty, int sectionIdFromDB, string sectionParagraph, string sectionImageFooter, HttpPostedFileBase sectionImage, News newsData, News currentNewsData)
        {
            string newSectionImageName = string.Empty;

            var currentSectionFromDB = currentNewsData.NewsSections.Where(x => x.Id == sectionIdFromDB).FirstOrDefault();

            if (currentSectionFromDB != null)    // section already exist on DB
            {
                NewsSection section = new NewsSection
                {
                    Paragraph = HttpUtility.HtmlEncode(sectionParagraph)
                };

                if (isImageDirty && sectionImage != null && sectionImage.ContentLength > 0)
                {
                    // delete previous image
                    if (currentSectionFromDB != null)
                    {
                        string fileNameToDelete = currentSectionFromDB.Image;
                        DeleteFile(fileNameToDelete);
                    }

                    // save new image
                    section.Image = Guid.NewGuid().ToString() + Path.GetExtension(sectionImage.FileName);
                    string _path = Path.Combine(Server.MapPath(ImageStoragePath), section.Image);
                    sectionImage.SaveAs(_path);
                    newSectionImageName = section.Image;
                }
                else
                {
                    section.Image = currentSectionFromDB.Image;
                    newSectionImageName = section.Image;
                }

                section.ImageFooter = sectionImageFooter ?? string.Empty; ;
                newsData.NewsSections.Add(section);
            }
            else    // is not an update, check if it qualifies as new section
            {
                bool isEmptyParagraph = string.Equals(sectionParagraph, "<p><br></p>");
                if (!isEmptyParagraph || sectionImage != null)
                {
                    NewsSection section = new NewsSection
                    {
                        Paragraph = HttpUtility.HtmlEncode(sectionParagraph)
                    };

                    if (sectionImage != null && sectionImage.ContentLength > 0)
                    {
                        // save new image
                        section.Image = Guid.NewGuid().ToString() + Path.GetExtension(sectionImage.FileName);
                        section.ImageFooter = sectionImageFooter ?? string.Empty;
                        string _path = Path.Combine(Server.MapPath(ImageStoragePath), section.Image);
                        sectionImage.SaveAs(_path);
                        newSectionImageName = section.Image;
                    }

                    section.Image = section.Image ?? string.Empty;
                    section.ImageFooter = section.ImageFooter ?? string.Empty;

                    newsData.NewsSections.Add(section);
                }
            }
            return newSectionImageName;
        }

        /// <summary>
        /// Validate model's received images
        /// </summary>
        /// <param name="model"></param>
        /// <param name="validImageTypes"></param>
        private List<string> ValidateModelImages(NewsViewModel model, string[] validImageTypes)
        {
            List<string> modelBindingErrors = new List<string>();
            if (model.Image == null) modelBindingErrors.Add("Imagen principal no puede ser null");
            if (model.Image.ContentLength <= 0) modelBindingErrors.Add("Imagen principal sin contenido");
            if (!validImageTypes.Contains(model.Image.ContentType)) modelBindingErrors.Add("Imagen principal no puede ser un archivo tipo: " + model.Image.ContentType);
            if (model.S1Image != null && !validImageTypes.Contains(model.S1Image.ContentType)) modelBindingErrors.Add("Imagen seccion 1 no puede ser un archivo tipo: " + model.S1Image.ContentType);
            if (model.S2Image != null && !validImageTypes.Contains(model.S2Image.ContentType)) modelBindingErrors.Add("Imagen seccion 2 no puede ser un archivo tipo: " + model.S2Image.ContentType);
            if (model.S3Image != null && !validImageTypes.Contains(model.S3Image.ContentType)) modelBindingErrors.Add("Imagen seccion 3 no puede ser un archivo tipo: " + model.S3Image.ContentType);
            if (model.S4Image != null && !validImageTypes.Contains(model.S4Image.ContentType)) modelBindingErrors.Add("Imagen seccion 4 no puede ser un archivo tipo: " + model.S4Image.ContentType);
            if (model.S5Image != null && !validImageTypes.Contains(model.S5Image.ContentType)) modelBindingErrors.Add("Imagen seccion 5 no puede ser un archivo tipo: " + model.S5Image.ContentType);

            return modelBindingErrors;
        }

        /// <summary>
        /// Validate only if images has been modified 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="validImageTypes"></param>
        /// <param name="modelFromDB"></param>
        /// <returns></returns>
        private List<string> ValidateModelImagesToEdit(NewsViewModel model,string[] validImageTypes, News modelFromDB)
        {
            List<string> modelBindingErrors = new List<string>();
            if (model.IsImageDirty)
            {
                if (model.Image == null) modelBindingErrors.Add("Imagen principal no puede ser null");
                if (model.Image.ContentLength <= 0) modelBindingErrors.Add("Imagen principal sin contenido");
                if (!validImageTypes.Contains(model.Image.ContentType)) modelBindingErrors.Add("Imagen principal no puede ser un archivo tipo: " + model.Image.ContentType);
            }
            if (model.IsS1ImageDirty && model.S1Image != null && !validImageTypes.Contains(model.S1Image.ContentType)) modelBindingErrors.Add("Imagen seccion 1 no puede ser un archivo tipo: " + model.S1Image.ContentType);
            if (model.IsS2ImageDirty && model.S2Image != null && !validImageTypes.Contains(model.S2Image.ContentType)) modelBindingErrors.Add("Imagen seccion 2 no puede ser un archivo tipo: " + model.S2Image.ContentType);
            if (model.IsS3ImageDirty && model.S3Image != null && !validImageTypes.Contains(model.S3Image.ContentType)) modelBindingErrors.Add("Imagen seccion 3 no puede ser un archivo tipo: " + model.S3Image.ContentType);
            if (model.IsS4ImageDirty && model.S4Image != null && !validImageTypes.Contains(model.S4Image.ContentType)) modelBindingErrors.Add("Imagen seccion 4 no puede ser un archivo tipo: " + model.S4Image.ContentType);
            if (model.IsS5ImageDirty && model.S5Image != null && !validImageTypes.Contains(model.S5Image.ContentType)) modelBindingErrors.Add("Imagen seccion 5 no puede ser un archivo tipo: " + model.S5Image.ContentType);

            return modelBindingErrors;
        }

        /// <summary>
        /// This will allow us to identify which of the NewsViewModel section area is available to store a given section from News Model
        /// </summary>
        /// <param name="sectionParagraph"></param>
        /// <param name="sectionImageNama"></param>
        /// <param name="sectionImageFooter"></param>
        /// <returns></returns>
        private bool IsSectionAvailable(string sectionParagraph, string sectionImageName, string sectionImageFooter)
        {
            return string.Equals(sectionParagraph, "<p><br></p>") && string.IsNullOrEmpty(sectionImageName) && string.IsNullOrEmpty(sectionImageFooter);
        }

        /// <summary>
        /// Returns a list of file names related to a specific News item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private List<string> ImageFromThisNews(News item)
        {
            List<string> filesToRemove = new List<string>();
            filesToRemove.Add(item.Image);
            foreach (NewsSection section in item.NewsSections)
                filesToRemove.Add(section.Image);

            return filesToRemove;
        }

        /// <summary>
        /// This will try to delete a file with the file name provided
        /// </summary>
        /// <param name="fileName"></param>
        private void DeleteFile(string fileName)
        {
            var fullPathToDelete = Path.Combine(Server.MapPath(ImageStoragePath), fileName);
            if (System.IO.File.Exists(fullPathToDelete))
            {
                System.IO.File.Delete(fullPathToDelete);
            }
        }

        #endregion
    }
}
