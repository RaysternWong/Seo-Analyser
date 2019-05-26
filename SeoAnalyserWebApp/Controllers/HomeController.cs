using SeoAnalyserWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SeoAnalyserWebApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            AnalyseModel model = new AnalyseModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(AnalyseModel model)
        {
            string content = string.Empty;
            model.InvalidMessage = string.Empty;

            #region Validate input
            if (model.IsAnyOptionSelected == false)
            {
                model.InvalidMessage = "Please check at least one of the checkbox";
            }
            else if (string.IsNullOrWhiteSpace(model.Input))
            {
                model.InvalidMessage = "Please enter text or url";
            }
            else if(Uri.TryCreate(model.Input, UriKind.Absolute, out Uri uri) )
            {
                content = GetHtmlText(uri);

                if(string.IsNullOrEmpty(content))
                {
                    model.InvalidMessage = "The URL you enter could not be access";
                }
            }
            else
            {
                content = model.Input;
            }
            #endregion

            if (string.IsNullOrEmpty(model.InvalidMessage))
            {
                model.IsValid = true;

                if (model.FilterStopsWordsFlag)
                {
                    content = model.RemoveStopWords(content);
                }

                if (model.CalNumOfOccuranceOfWordsFlag)
                {
                    model.OccuranceOfWordsTable = model.GetOccuranceWordTable(HttpUtility.HtmlEncode(content));
                }

                if (model.CalNumOfOccuranceOfWordsListedInMetaTagsFlag)
                {
                    model.OccuranceOfWordsInMetaTagsTable = model.GetOccuranceWordInMetaTagTable(content);
                }

                if (model.CalNumOfOccuranceOfExternalLinksFlag)
                {
                    model.OccuranceOfExternalLinksTable = model.GetOccuranceExternalLinkTable(content);
                }
            }
  
            return View(model);
        }

        private string GetHtmlText(Uri uri)
        {
            var result = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(uri).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;                     
                    }
                }
            }
            catch(Exception ex)
            {
                result = string.Empty;
            }

            return result;          
        }
    }
}