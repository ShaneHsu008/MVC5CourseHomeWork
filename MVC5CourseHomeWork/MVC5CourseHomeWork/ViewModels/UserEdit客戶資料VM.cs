using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5CourseHomeWork.ViewModels
{
    public class UserEdit客戶資料VM
    {
        [Required]
        public int Id { get; set; }

        [ReadOnly(true)]
        public string 客戶名稱 { get; set; }

        [ReadOnly(true)]
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

        [ReadOnly(true)]
        public string 帳號 { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "密碼不得大於 20 個字元")]
        public string 密碼 { get; set; }
    }
}