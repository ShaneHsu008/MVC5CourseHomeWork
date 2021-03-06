using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5CourseHomeWork.Models
{
    public class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
    {
        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(a => a.是否已刪除 == false);
        }

        public 客戶聯絡人 Find(int id)
        {
            return this.All().FirstOrDefault(a => a.Id == id);
        }

        public override void Delete(客戶聯絡人 entity)
        {
            entity.是否已刪除 = true;
        }

        public bool IsRepeatEmail(客戶聯絡人 entity)
        {
            var chack = this.All().FirstOrDefault(a => a.客戶Id == entity.客戶Id && a.Email == entity.Email);
            if (chack != null)
                return true;

            return false;
        }

        internal IQueryable<客戶聯絡人> Search(string name, string jobTitle)
        {
            var 客戶聯絡人 = this.All();
            if (!string.IsNullOrEmpty(name))
            {
                客戶聯絡人 = 客戶聯絡人.Where(a => a.姓名.Contains(name));
            }
            if (!string.IsNullOrEmpty(jobTitle))
            {
                客戶聯絡人 = 客戶聯絡人.Where(a => a.職稱.Contains(jobTitle));
            }
            return 客戶聯絡人;
        }        
    }

    public interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
    {

    }
}