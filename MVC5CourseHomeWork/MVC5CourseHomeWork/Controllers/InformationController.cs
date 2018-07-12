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
    public class InformationController : Controller
    {
        private 客戶資料Repository repo;
        private 客戶聯絡人及帳戶數量Repository repoCount;

        public InformationController()
        {
            repo = RepositoryHelper.Get客戶資料Repository();
            repoCount = RepositoryHelper.Get客戶聯絡人及帳戶數量Repository(repo.UnitOfWork);
        }
        // GET: Information
        public ActionResult Index()
        {
            return View(repo.All().ToList());
        }
        [HttpGet]
        public ActionResult Search(string name,string classification)
        {
            var 客戶資料 = repo.Search(name, classification);

            return View("Index", 客戶資料);
        }

        public ActionResult Count()
        {
            return View(repoCount.All().ToList());
        }

        // GET: Information/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.All().FirstOrDefault(a => a.Id == id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: Information/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Information/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶資料);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: Information/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.All().FirstOrDefault(a => a.Id == id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: Information/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                var db = repo.UnitOfWork.Context;
                db.Entry(客戶資料).State = EntityState.Modified;
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: Information/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.All().FirstOrDefault(a => a.Id == id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: Information/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = repo.All().FirstOrDefault(a => a.Id == id);
            客戶資料.是否已刪除 = true;
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult GetExcel()
        {
            List<客戶資料> model = repo.All().ToList();

            //將List轉成Json格式
            var exportSource = GetExportList(model);
            //再將json格式反序列化轉換成資料表
            var dt = JsonConvert.DeserializeObject<DataTable>(exportSource.ToString());

            string fileName = string.Concat("客戶資料", DateTime.Now.ToString("_yyyyMMddHHmmss"), ".xlsx");

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

        private JArray GetExportList(List<客戶資料> model)
        {
            JArray objects = new JArray();

            if (model.Count > 0)
            {
                foreach (var item in model)
                {
                    var jo = new JObject();
                    jo.Add("ID", item.Id);
                    jo.Add("客戶名稱", item.客戶名稱);
                    jo.Add("統一編號", item.統一編號);
                    jo.Add("電話", item.電話);
                    jo.Add("傳真", item.傳真);
                    jo.Add("地址", item.地址);
                    jo.Add("Email", item.Email);
                    jo.Add("客戶分類", item.客戶分類);
                    objects.Add(jo);
                }
            }
            else
            {
                var jo = new JObject();
                jo.Add("ID", string.Empty);
                jo.Add("客戶名稱", string.Empty);
                jo.Add("統一編號", string.Empty);
                jo.Add("電話", string.Empty);
                jo.Add("傳真", string.Empty);
                jo.Add("地址", string.Empty);
                jo.Add("Email", string.Empty);
                jo.Add("客戶分類", string.Empty);
                objects.Add(jo);
            }

            return objects;
        }
    }
}
