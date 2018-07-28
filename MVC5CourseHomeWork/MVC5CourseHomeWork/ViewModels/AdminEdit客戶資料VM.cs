using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5CourseHomeWork.ViewModels
{
    public class AdminEdit客戶資料VM
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string 客戶名稱 { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "欄位長度不得大於 8 個字元")]
        public string 統一編號 { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string 傳真 { get; set; }

        [StringLength(100, ErrorMessage = "欄位長度不得大於 100 個字元")]
        public string 地址 { get; set; }

        [StringLength(250, ErrorMessage = "欄位長度不得大於 250 個字元")]
        public string Email { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "欄位長度不得大於 250 個字元")]
        public string 客戶分類 { get; set; }
    }
}