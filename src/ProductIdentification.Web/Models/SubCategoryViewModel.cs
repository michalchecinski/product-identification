﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProductIdentification.Web.Models
{
    public class SubCategoryViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [DisplayName("Subcategory Name")]
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Required")]
        public string CategoryName { get; set; }
    }
}
