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

        internal IQueryable<客戶聯絡人> Search(string name)
        {
            var 客戶聯絡人 = this.All();
            if (!string.IsNullOrEmpty(name))
            {
                客戶聯絡人 = 客戶聯絡人.Where(a => a.姓名.Contains(name));
            }
            return 客戶聯絡人;
        }
    }

    public interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
    {

    }
}