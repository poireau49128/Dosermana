using Dosermana.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dosermana.Domain.Abstract
{
    public interface INewsRepository
    {
        IEnumerable<News> News { get; }
        void SaveNews(News news);
        News DeleteNews(int NewsId);
    }
}
