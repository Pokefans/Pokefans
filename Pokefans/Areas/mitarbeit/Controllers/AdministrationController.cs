using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Pokefans.Data;
using Pokefans.Areas.mitarbeit.Models;

namespace Pokefans.Areas.mitarbeit.Controllers
{
    [Authorize(Roles="superadmin")]
    public class AdministrationController : Controller
    {
        Entities db;

        public AdministrationController(Entities ents) 
        {
            db = ents;
        }

        public ActionResult TermsOfService() 
        {
            var tos = db.DsgvoComplianceInfos.ToList();

            return View("~/Areas/mitarbeit/Views/Administration/TermsOfService.cshtml", tos);            
        }

        [HttpGet]
        public ActionResult NewTos() 
        {
            NewTosViewModel viewModel = new NewTosViewModel();

            int privacyid = int.Parse(ConfigurationManager.AppSettings["DataProtectionId"]);
            int tosid = int.Parse(ConfigurationManager.AppSettings["TermsOfServiceId"]);
            int forumrulesid = int.Parse(ConfigurationManager.AppSettings["ForumRulesId"]);

            viewModel.PrivacyStatement = db.Contents.Where(x => x.Id == privacyid).First();
            viewModel.TermsOfService = db.Contents.Where(x => x.Id == tosid).First();
            viewModel.ForumRules = db.Contents.Where(x => x.Id == forumrulesid).First();

            return View("~/Areas/mitarbeit/Views/Administration/NewTos.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewTos(bool publish)
        {
            if (!publish)
                return NewTos();

            NewTosViewModel viewModel = new NewTosViewModel();

            int privacyid = int.Parse(ConfigurationManager.AppSettings["DataProtectionId"]);
            int tosid = int.Parse(ConfigurationManager.AppSettings["TermsOfServiceId"]);
            int forumrulesid = int.Parse(ConfigurationManager.AppSettings["ForumRulesId"]);

            viewModel.PrivacyStatement = db.Contents.Where(x => x.Id == privacyid).First();
            viewModel.TermsOfService = db.Contents.Where(x => x.Id == tosid).First();
            viewModel.ForumRules = db.Contents.Where(x => x.Id == forumrulesid).First();

            DsgvoComplianceInfo info = new DsgvoComplianceInfo();

            info.ForumRules = viewModel.ForumRules.ParsedContent;
            info.DataProtectionStatement = viewModel.PrivacyStatement.ParsedContent;
            info.TermsOfUsage = viewModel.TermsOfService.ParsedContent;

            info.EffectiveTime = DateTime.Now;

            db.DsgvoComplianceInfos.Add(info);
            db.SaveChanges();

            return RedirectToAction("TermsOfService");
        }

        public ActionResult ViewTos(int id) 
        {
            var tos = db.DsgvoComplianceInfos.FirstOrDefault(x => x.Id == id);

            if (tos == null)
                return HttpNotFound();

            return View("~/Areas/mitarbeit/Views/Administration/ViewTos.cshtml", tos);
        }
    }
}
