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

        [Display(Name = "Остаток на складе в Гродно")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Пожалуйста, введите неотрицательное значение для количества")]
        public int Quantity_Grodno { get; set; }

        [Display(Name = "Остаток на складе в Гродно")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Пожалуйста, введите неотрицательное значение для количества")]
        public int Quantity_Moscow { get; set; }

        [Display(Name = "Цена (руб)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для цены")]
        public decimal Price { get; set; }

        [Display(Name = "Размеры")]
        public string Sizes { get; set; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Пожалуйста, введите категорию продукта")]
        public string Category { get; set; }

        [Display(Name = "Подкатегория")]
        [Required(ErrorMessage = "Пожалуйста, введите подкатегорию продукта")]
        public string SubCategory { get; set; }
        public string FileName { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}
