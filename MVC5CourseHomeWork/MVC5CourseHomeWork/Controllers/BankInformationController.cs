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
using X.PagedList;

namespace MVC5CourseHomeWork.Controllers
{
    [ActionTimer]
    public class BankInformationController : Controller
    {
        客戶銀行資訊Repository repo;
        客戶資料Repository repoInformation;
        private int pageSize = 1;

        public BankInformationController()
        {
            repo = RepositoryHelper.Get客戶銀行資訊Repository();
            repoInformation = RepositoryHelper.Get客戶資料Repository(repo.UnitOfWork);
        }

        // GET: BankInformation
        public ActionResult Index(int page = 1)
        {
            var 客戶銀行資訊 = repo.All().Include(客 => 客.客戶資料).OrderBy(銀 => 銀.客戶Id);
            return View(客戶銀行資訊.ToPagedList(page, pageSize));
        }

        [HttpGet]
        public ActionResult Search(string bankName)
        {
            var 客戶銀行資訊 = repo.Search(bankName);

            return View("Index", 客戶銀行資訊);
        }

        // GET: BankInformation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var 客戶銀行資訊 = repo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: BankInformation/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(repoInformation.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: BankInformation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶銀行資訊);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(repoInformation.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: BankInformation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var 客戶銀行資訊 = repo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(repoInformation.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: BankInformation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                var db = repo.UnitOfWork.Context;
                db.Entry(客戶銀行資訊).State = EntityState.Modified;
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(repoInformation.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: BankInformation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var 客戶銀行資訊 = repo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: BankInformation/Delete/5
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
            List<客戶銀行資訊> model = repo.All().ToList();

            //將List轉成Json格式
            var exportSource = GetExportList(model);
            //再將json格式反序列化轉換成資料表
            var dt = JsonConvert.DeserializeObject<DataTable>(exportSource.ToString());

            string fileName = string.Concat("客戶銀行資訊", DateTime.Now.ToString("_yyyyMMddHHmmss"), ".xlsx");

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

        private JArray GetExportList(List<客戶銀行資訊> model)
        {
            JArray objects = new JArray();

            if (model.Count > 0)
            {
                foreach (var item in model)
                {
                    var jo = new JObject();
                    jo.Add("ID", item.Id);
                    jo.Add("客戶Id", item.客戶Id);
                    jo.Add("銀行名稱", item.銀行名稱);
                    jo.Add("銀行代碼", item.銀行代碼);
                    jo.Add("分行代碼", item.分行代碼);
                    jo.Add("帳戶名稱", item.帳戶名稱);
                    jo.Add("帳戶號碼", item.帳戶號碼);
                    objects.Add(jo);
                }
            }
            else
            {
                var jo = new JObject();
                jo.Add("ID", string.Empty);
                jo.Add("客戶Id", string.Empty);
                jo.Add("銀行名稱", string.Empty);
                jo.Add("銀行代碼", string.Empty);
                jo.Add("分行代碼", string.Empty);
                jo.Add("帳戶名稱", string.Empty);
                jo.Add("帳戶號碼", string.Empty);
                objects.Add(jo);
            }

            return objects;
        }
    }
}
