using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5CourseHomeWork.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "帳號不得大於 20 個字元")]
        public string 帳號 { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "密碼不得大於 20 個字元")]
        public string 密碼 { get; set; }
    }
}