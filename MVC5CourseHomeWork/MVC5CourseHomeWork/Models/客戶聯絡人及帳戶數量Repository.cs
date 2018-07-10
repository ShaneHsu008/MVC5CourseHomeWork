using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5CourseHomeWork.Models
{   
	public  class 客戶聯絡人及帳戶數量Repository : EFRepository<客戶聯絡人及帳戶數量>, I客戶聯絡人及帳戶數量Repository
	{

	}

	public  interface I客戶聯絡人及帳戶數量Repository : IRepository<客戶聯絡人及帳戶數量>
	{

	}
}