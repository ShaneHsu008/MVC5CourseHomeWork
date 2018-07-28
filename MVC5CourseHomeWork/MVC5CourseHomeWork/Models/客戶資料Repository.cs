using System;
using System.Linq;
using System.Collections.Generic;
using MVC5CourseHomeWork.ViewModels;
using System.Web.Security;

namespace MVC5CourseHomeWork.Models
{
    public class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(a => a.是否已刪除 == false);
        }

        public 客戶資料 Find(int id)
        {
            return this.All().FirstOrDefault(a => a.Id == id);
        }

        public override void Delete(客戶資料 entity)
        {
            entity.是否已刪除 = true;
        }

        internal IQueryable<客戶資料> Search(string name, string classification)
        {
            var 客戶資料 = this.All();
            if (!string.IsNullOrEmpty(name))
            {
                客戶資料 = 客戶資料.Where(a => a.客戶名稱.Contains(name));
            }
            if (!string.IsNullOrEmpty(classification))
            {
                客戶資料 = 客戶資料.Where(a => a.客戶分類 == classification);
            }
            return 客戶資料;
        }

        internal bool CheckUser(LoginViewModel model)
        {
            string passworld = FormsAuthentication.HashPasswordForStoringInConfigFile(model.密碼, "SHA1");

            var 客戶資料 = this.All().FirstOrDefault(a => a.帳號 == model.帳號 && a.密碼 == passworld);

            if (客戶資料 != null)
            {
                return true;
            }

            return false;
        }
    }

    public interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}