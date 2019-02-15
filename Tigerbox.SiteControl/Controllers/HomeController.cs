using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Tigerbox.Objects;
using Tigerbox.Services;
using Tigerbox.SiteControl.Services;
using Tigerbox.Spec;

namespace Tigerbox.SiteControl.Controllers
{
    public class HomeController : Controller
    {
        private HomeService homeService;

        public HomeController()
        {
            homeService = new HomeService(new ConfigurationService(), new JsonService());
        }                

        public ActionResult Index()
        {
            Session["AdminUser"] = homeService.IsAdminUser(Request.UserHostName);
            Session["NoImage"] = homeService.GetNoImageBase64();
            Session["MusicIcon"] = homeService.GetMusicIconBase64();
            Session["VideoIcon"] = homeService.GetVideoIconBase64();
            return View();
        }

        public PartialViewResult Musics()
        { 
            string firstFolderName = homeService.GetFirstFolderName();
            return GetMusicList(firstFolderName);
        }

        [HttpPost]
        public PartialViewResult GetMusicsByFolder(string folderName)
        {
            return GetMusicList(folderName);
        }

        private PartialViewResult GetMusicList(string folderName)
        {

            var medias = homeService.ListMedias(folderName);

            return PartialView("_musics", medias);
        }

        public PartialViewResult Folders()
        {
            ViewData["ImageUrlBase"] = homeService.GetIMageUrlBase();

            var folders = homeService.ListFolders().Take(30);

            var result = new List<TigerFolder>();

            if (folders.Any())
            {
                result = folders.Take(30).ToList();
            }
            return PartialView("_folders", result);
        }

        [HttpPost]
        public JsonResult SelectMedia(string folderName, string musicName)
        {
            try
            {
                homeService.SendMusicToTigerBox(folderName.Trim(), musicName.Trim());

                return Json(new { Success = true, Message = $"Media '{musicName}' adicionada à lista com sucesso." }, new JsonRequestBehavior());
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = $"Desculpe, houve um erro ao adicionar sua música.<br/> Detalhes: {ex.Message}" }, new JsonRequestBehavior());
            }

        }

        [HttpPost]
        public PartialViewResult Search(string text)
        {
            return PartialView("_folders", homeService.ListFolders(text));
        }

        [HttpPost]
        public JsonResult SetTigerBoxAction(string action)
        {
            try
            {
                homeService.SendActionToTigerBox(action);
                return Json(new { Success = true, Message = string.Empty });
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = ex.Message });
            }
            
        }

        
    }
}