using CodeExam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeExam.Areas.Admin.Controllers
{
    public class DataTypeController : Controller
    {
        CodeWarDbContext db = new CodeWarDbContext();
        // GET: Admin/DataType
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public JsonResult GetAll()
        {
            return Json(db.DataTypes, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDataTypeById(int id)
        {
            return Json(db.DataTypes.FirstOrDefault(f=>f.DataTypeId == id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreateDataType(DataType data)
        {
            db.DataTypes.Add(data);
            if (db.SaveChanges() == 1)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(DataType data)
        {
            var item = db.DataTypes.FirstOrDefault(f => f.DataTypeId == data.DataTypeId);
            if (item != null)
            {
                item.DataTypeName = data.DataTypeName;
                item.DisplayName = data.DisplayName;
                if (db.SaveChanges() == 1)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int id)
        {
            var itemToDel = db.DataTypes.FirstOrDefault(f => f.DataTypeId == id);
            if (itemToDel != null)
            {
                itemToDel.DataTypeStatus = Constant.Status.Deactive;
                if (db.SaveChanges() == 1)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}