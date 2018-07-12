using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using MVC5CourseHomeWork.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MVC5CourseHomeWork.Controllers
{
    public class ContactPersonController : Controller
    {
        private 客戶聯絡人Repository repo;
        private 客戶資料Repository repoInformation;

        public ContactPersonController()
        {
            repo = RepositoryHelper.Get客戶聯絡人Repository();
            repoInformation = RepositoryHelper.Get客戶資料Repository(repo.UnitOfWork);
        }
        // GET: ContactPerson
        public ActionResult Index()
        {
            var 客戶聯絡人 = repo.All().Include(客 => 客.客戶資料);
            return View(客戶聯絡人.ToList());
        }
        [HttpGet]
        public ActionResult Search(string name, string jobTitle)
        {
            var 客戶聯絡人 = repo.Search(name, jobTitle);

            return View("Index", 客戶聯絡人);
        }

        // GET: ContactPerson/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: ContactPerson/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(repoInformation.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: ContactPerson/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                if (repo.IsRepeatEmail(客戶聯絡人))
                {
                    //為了避免造成錯誤：具有索引鍵 '客戶Id' 的 ViewData 項目為 'System.Int32' 型別，但必須是 'IEnumerable<SelectListItem>' 型別
                    ViewBag.客戶Id = new SelectList(repoInformation.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
                    ModelState.AddModelError("Email", "同客戶下的聯絡人Email不可重複!");
                    return View(客戶聯絡人);
                }

                repo.Add(客戶聯絡人);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(repoInformation.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: ContactPerson/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(repoInformation.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: ContactPerson/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                var db = repo.UnitOfWork.Context;
                db.Entry(客戶聯絡人).State = EntityState.Modified;
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(repoInformation.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: ContactPerson/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: ContactPerson/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repo.Delete(repo.Find(id));
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult GetExcel()
        {
            List<客戶聯絡人> model = repo.All().ToList();

            //將List轉成Json格式
            var exportSource = GetExportList(model);
            //再將json格式反序列化轉換成資料表
            var dt = JsonConvert.DeserializeObject<DataTable>(exportSource.ToString());

            string fileName = string.Concat("客戶聯絡人", DateTime.Now.ToString("_yyyyMMddHHmmss"), ".xlsx");

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Sheet1");
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "	application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        private JArray GetExportList(List<客戶聯絡人> model)
        {
            JArray objects = new JArray();

            if (model.Count > 0)
            {
                foreach (var item in model)
                {
                    var jo = new JObject();
                    jo.Add("ID", item.Id);
                    jo.Add("客戶Id", item.客戶Id);
                    jo.Add("職稱", item.職稱);
                    jo.Add("姓名", item.姓名);
                    jo.Add("Email", item.Email);
                    jo.Add("手機", item.手機);
                    jo.Add("電話", item.電話);
                    objects.Add(jo);
                }
            }
            else
            {
                var jo = new JObject();
                jo.Add("ID", string.Empty);
                jo.Add("客戶Id", string.Empty);
                jo.Add("職稱", string.Empty);
                jo.Add("姓名", string.Empty);
                jo.Add("Email", string.Empty);
                jo.Add("手機", string.Empty);
                jo.Add("電話", string.Empty);
                objects.Add(jo);
            }

            return objects;
        }
    }
}
