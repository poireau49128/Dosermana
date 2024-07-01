using Dosermana.Domain.Abstract;
using Dosermana.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dosermana.Domain.Concrete
{
    public class EFNewsRepository : INewsRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<News> News
        {
            get { return context.News; }
        }

        public void SaveNews(News news)
        {
            if (news.NewsId == 0)
                context.News.Add(news);
            else
            {
                News dbEntry = context.News.Find(news.NewsId);
                if (dbEntry != null)
                {
                    dbEntry.NewsTitle = news.NewsTitle;
                    dbEntry.Date = news.Date;
                    dbEntry.FileName = news.FileName;
                    dbEntry.Content = news.Content;
                }
            }
            context.SaveChanges();
        }

        public News DeleteNews(int NewsId)
        {
            News dbEntry = context.News.Find(NewsId);
            if (dbEntry != null)
            {
                context.News.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
