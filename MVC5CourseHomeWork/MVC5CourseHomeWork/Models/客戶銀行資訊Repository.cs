using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5CourseHomeWork.Models
{
    public class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
    {
        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(a => a.是否已刪除 == false);
        }

        internal IQueryable<客戶銀行資訊> Search(string bankName)
        {
            var 客戶銀行資訊 = this.All();
            if (!string.IsNullOrEmpty(bankName))
            {
                客戶銀行資訊 = 客戶銀行資訊.Where(a => a.銀行名稱.Contains(bankName));
            }
            return 客戶銀行資訊;
        }
    }

    public interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
    {

    }
}