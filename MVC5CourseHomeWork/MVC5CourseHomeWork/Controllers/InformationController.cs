﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ClosedXML.Excel;
using MVC5CourseHomeWork.Models;
using MVC5CourseHomeWork.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using X.PagedList;

namespace MVC5CourseHomeWork.Controllers
{
    public class InformationController : Controller
    {
        private 客戶資料Repository repo;
        private 客戶聯絡人及帳戶數量Repository repoCount;
        private int pageSize = 1;

        public InformationController()
        {
            repo = RepositoryHelper.Get客戶資料Repository();
            repoCount = RepositoryHelper.Get客戶聯絡人及帳戶數量Repository(repo.UnitOfWork);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index(string sortName, string sortOrder, int page = 1)
        {
            var 客戶資料 = repo.All();
            switch (sortName)
            {
                case "客戶名稱":
                    if (sortOrder == "ASC")
                        客戶資料 = 客戶資料.OrderBy(a => a.客戶名稱);
                    else
                        客戶資料 = 客戶資料.OrderByDescending(a => a.客戶名稱);
                    break;
                case "統一編號":
                    if (sortOrder == "ASC")
                        客戶資料 = 客戶資料.OrderBy(a => a.統一編號);
                    else
                        客戶資料 = 客戶資料.OrderByDescending(a => a.統一編號);
                    break;
                case "電話":
                    if (sortOrder == "ASC")
                        客戶資料 = 客戶資料.OrderBy(a => a.電話);
                    else
                        客戶資料 = 客戶資料.OrderByDescending(a => a.電話);
                    break;
                case "傳真":
                    if (sortOrder == "ASC")
                        客戶資料 = 客戶資料.OrderBy(a => a.傳真);
                    else
                        客戶資料 = 客戶資料.OrderByDescending(a => a.傳真);
                    break;
                case "地址":
                    if (sortOrder == "ASC")
                        客戶資料 = 客戶資料.OrderBy(a => a.地址);
                    else
                        客戶資料 = 客戶資料.OrderByDescending(a => a.地址);
                    break;
                case "Email":
                    if (sortOrder == "ASC")
                        客戶資料 = 客戶資料.OrderBy(a => a.Email);
                    else
                        客戶資料 = 客戶資料.OrderByDescending(a => a.Email);
                    break;
                case "客戶分類":
                    if (sortOrder == "ASC")
                        客戶資料 = 客戶資料.OrderBy(a => a.客戶分類);
                    else
                        客戶資料 = 客戶資料.OrderByDescending(a => a.客戶分類);
                    break;
                default:
                    客戶資料 = 客戶資料.OrderBy(a => a.客戶名稱);
                    break;
            }

            ViewBag.sortName = sortName;
            ViewBag.sortOrder = sortOrder;

            return View(客戶資料.ToPagedList(page, pageSize));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Search(string name, string classification)
        {
            var 客戶資料 = repo.Search(name, classification);

            return View("Index", 客戶資料);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Count()
        {
            return View(repoCount.All().ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CountJson()
        {
            var countList = repoCount.All().ToList();

            string result = "";

            if (countList == null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            result = JsonConvert.SerializeObject(countList);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Information/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);

            if (客戶資料 == null)
            {
                return HttpNotFound();
            }

            return View(客戶資料);
        }

        // GET: Information/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Information/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);
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
        [Authorize(Roles = "Admin")]
        [HandleError(ExceptionType = typeof(DbEntityValidationException), View = "Error_DbEntityValidationException")]
        public ActionResult Edit(AdminEdit客戶資料VM model)
        {
            var 客戶資料 = repo.Find(model.Id);
            if (TryUpdateModel(客戶資料, new string[] { "客戶名稱", "統一編號", "電話", "傳真", "地址", "Email", "客戶分類" })
                && ModelState.IsValid)
            {
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: Information/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: Information/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            repo.Delete(repo.Find(id));
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "一般使用者")]
        public ActionResult UserEdit()
        {
            string user = HttpContext.User.Identity.Name;
            var result = repo.GetUserData(user);

            return View(result);
        }

        [HttpPost]
        [Authorize(Roles = "一般使用者")]
        public ActionResult UserEdit(UserEdit客戶資料VM model)
        {
            var 客戶資料 = repo.Find(model.Id);

            if (TryUpdateModel(客戶資料, new string[] { "電話", "傳真", "地址", "Email", "密碼" })
                && ModelState.IsValid)
            {
                客戶資料.密碼 = FormsAuthentication.HashPasswordForStoringInConfigFile(model.密碼, "SHA1");
                repo.UnitOfWork.Commit();

                FormsAuthentication.SignOut();

                return RedirectToAction("Login", "Account");
            }

            return View(客戶資料);
        }
    }
}
