using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrudOperationInMVC_Bool_Image_Calendar_.Models;

namespace CrudOperationInMVC_Bool_Image_Calendar_.Controllers
{
    public class StudentController : Controller
    {
        private DBExampleEntities db = new DBExampleEntities();

        // GET: Student
        public ActionResult Index()
        {
            var tblStudents = db.tblStudents.Include(t => t.tblDepartment);
            return View(tblStudents.ToList());
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStudent tblStudent = db.tblStudents.Find(id);
            if (tblStudent == null)
            {
                return HttpNotFound();
            }
            return View(tblStudent);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.tblDepartments, "DepartmentID", "DeparttmentName");
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblStudent s)
        {
            string FileName = Path.GetFileNameWithoutExtension(s.ImageFile.FileName);
            string Extension = Path.GetExtension(s.ImageFile.FileName);
            HttpPostedFileBase PostedFile = s.ImageFile;
            int lenght = PostedFile.ContentLength;
            if (Extension.ToLower() == ".jpg" || Extension.ToLower() == ".png" || Extension.ToLower() == ".jpeg")
            {
                if (lenght <= 100000)
                {
                    FileName = FileName + Extension;
                    s.StudentImage = "~/Image/" + FileName;
                    FileName = Path.Combine(Server.MapPath("~/Image/"), FileName);
                    s.ImageFile.SaveAs(FileName);
                    db.tblStudents.Add(s);
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["CreateMessage"] = "<script> alert('Successfully Created') </script>";
                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["CreateMessage"] = "<script> alert('Failed!!') </script>";
                    }
                }
                else
                {
                    TempData["SizeMessage"] = "<script> alert('Over size of Photo, Pls add a photo exceeding 1 MB.') </script>";
                }
            }
            else
            {
                TempData["ExtensionMessage"] = "<script> alert('Format Not supported. Please get JPG, PNG or JPEG Format.') </script>";
            }

            ViewBag.DepartmentId = new SelectList(db.tblDepartments, "DepartmentID", "DeparttmentName", s.DepartmentId);
            return View();
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStudent tblStudent = db.tblStudents.Find(id);
            Session["Image"] = tblStudent.StudentImage;
            if (tblStudent == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.tblDepartments, "DepartmentID", "DeparttmentName", tblStudent.DepartmentId);
            return View(tblStudent);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblStudent s)
        {
            if (ModelState.IsValid)
            {
                if (s.ImageFile != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(s.ImageFile.FileName);
                    string Extension = Path.GetExtension(s.ImageFile.FileName);
                    HttpPostedFileBase PostedFile = s.ImageFile;
                    int lenght = PostedFile.ContentLength;
                    if (Extension.ToLower() == ".jpg" || Extension.ToLower() == ".png" || Extension.ToLower() == ".jpeg")
                    {
                        if (lenght <= 100000)
                        {
                            FileName = FileName + Extension;
                            s.StudentImage = "~/Image/" + FileName;
                            FileName = Path.Combine(Server.MapPath("~/Image/"), FileName);
                            s.ImageFile.SaveAs(FileName);
                            db.Entry(s).State = EntityState.Modified;
                            int a = db.SaveChanges();
                            if (a > 0)
                            {
                                TempData["UpdateMessage"] = "<script> alert('Başarıyla Güncellendi.') </script>";
                                ModelState.Clear();
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["UpdateMessage"] = "<script> alert('Bir hata oluştu') </script>";
                            }
                        }
                        else
                        {
                            TempData["SizeMessage"] = "<script> alert('Boyutu çok yüksek bir foto, 1m boyutlu ekleyiniz.') </script>";
                        }
                    }
                    else
                    {
                        TempData["ExtensionMessage"] = "<script> alert('Biçim Desteklenmemektedir. Lütfen JPG PNG veya JPEG Formatında olsun.') </script>";
                    }
                }
                else
                {
                    s.StudentImage = Session["Image"].ToString();
                    db.Entry(s).State = EntityState.Modified;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["UpdateMessage"] = "<script> alert('Başarıyla Güncellendi.') </script>";
                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["UpdateMessage"] = "<script> alert('Bir hata oluştu') </script>";
                    }
                }
            }
                ViewBag.DepartmentId = new SelectList(db.tblDepartments, "DepartmentID", "DeparttmentName", s.DepartmentId);
            return View();
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStudent tblStudent = db.tblStudents.Find(id);
            if (tblStudent == null)
            {
                return HttpNotFound();
            }
            return View(tblStudent);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblStudent tblStudent = db.tblStudents.Find(id);
            db.tblStudents.Remove(tblStudent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
