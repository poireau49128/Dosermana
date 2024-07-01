using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dosermana.Domain.Entities
{
    public class News
    {
        public int NewsId { get; set; }
        [Display(Name = "Название")]
        public string NewsTitle { get; set; }
        public DateTime Date { get; set; }
        public string FileName { get; set; }
        [Display(Name = "Текст")]
        public string Content { get; set; }
    }
}
