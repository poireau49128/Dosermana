using Dosermana.Domain.Abstract;
using Dosermana.Domain.Concrete;
using Dosermana.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dosermana.WebUI.Controllers
{
    public class NewsController : Controller
    {
        private INewsRepository _repository;
        public NewsController()
        {
            _repository = new EFNewsRepository();
        }
        public ActionResult List()
        {
            IEnumerable<News> newsList = _repository.News;
            return View(newsList);
        }

        public ViewResult Create()
        {
            var news = new News
            {
                Date = DateTime.Now,
            };
            return View("Edit", news);
        }

        [HttpPost]
        public ActionResult Delete(int NewsId)
        {
            News deletedNews = _repository.DeleteNews(NewsId);
            if (deletedNews != null)
            {
                TempData["message"] = string.Format("Новость \"{0}\" была удалена",
                    deletedNews.NewsTitle);
            }
            return RedirectToAction("List");
        }

        public ViewResult Edit(int NewsId)
        {
            News news = _repository.News
                .FirstOrDefault(p => p.NewsId == NewsId);
            return View(news);
        }

        [HttpPost]
        public ActionResult Edit(News news, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    news.FileName = UploadImage(image);
                }
                else
                {
                    News existingNews = _repository.News.FirstOrDefault(n => n.NewsId == news.NewsId);
                    if (existingNews != null)
                    {
                        news.FileName = existingNews.FileName;
                    }
                }

                _repository.SaveNews(news);
                TempData["message"] = string.Format("Изменения в \"{0}\" были сохранены", news.NewsTitle);
                return RedirectToAction("List");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(news);
            }
        }

        public ActionResult ClearImage(int NewsId)
        {
            News news = _repository.News
                .FirstOrDefault(p => p.NewsId == NewsId);
            if (news != null)
            {
                news.FileName = null;
                _repository.SaveNews(news);
            }
            return RedirectToAction("Edit", new { NewsId = NewsId });
            //return RedirectToAction("List");
        }

        public string UploadImage(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Server.MapPath("~/Images/"), fileName);
                file.SaveAs(filePath);

                return fileName;
            }

            return null;
        }
    }
}