using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5CourseHomeWork.Models
{
    public class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(a => a.是否已刪除 == false);
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
    }

    public interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}