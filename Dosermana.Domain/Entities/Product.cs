using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Dosermana.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int ProductId { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название продукта")]
        public string Name { get; set; }

        [Display(Name = "Цвет")]
        [Required(ErrorMessage = "Пожалуйста, введите цвет продукта")]
        public string Color { get; set; }

        [Display(Name = "Количество на складе")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Пожалуйста, введите неотрицательное значение для количества")]
        public int Quantity { get; set; }

        [Display(Name = "Цена (руб)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для цены")]
        public decimal Price { get; set; }

        [Display(Name = "Имя картинки")]
        public string PictureName { get; set; }

        [Display(Name = "Размеры")]
        public string Sizes { get; set; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Пожалуйста, введите категорию продукта")]
        public string Category { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}
