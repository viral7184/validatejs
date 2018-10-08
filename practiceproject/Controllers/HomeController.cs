using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using practiceproject.Controllers;
using practiceproject.Models;
using System.Data;
using System.Configuration;
using System.IO;


namespace practiceproject.Controllers
{
   

    public class HomeController : Controller
    {
        practiceDatabaseEntities1 db = new practiceDatabaseEntities1();
        public ActionResult customerorder()
        {
            var list = db.tbl_customer.ToList();
            ViewBag.listitem = list;
            return View();
        }
        [HttpPost]
        public ActionResult customerorder(tbl_customer data, FormCollection data1, HttpPostedFileBase[] FILE_UPLOAD)
        {
            string commase = "";
            var multiplefile = Request.Form["FILE_UPLOAD"];
            if (data.FILE_UPLOAD != null)
            {

                Models.tbl_customer imgs = new tbl_customer();  
                imgs.FILE_UPLOAD = data.FILE_UPLOAD;
                HttpFileCollectionBase file = Request.Files;
                for (int i = 0; i < file.Count; i++)
                {
                    HttpPostedFileBase Image = file[i];
                    var filename = "";
                    if (Image != null)
                    {
                        filename = Path.GetFileName(Image.FileName);
                        var path3 = Path.Combine(Server.MapPath("~/UploadedFiles"), filename);
                        Image.SaveAs(path3);
                        data.FILE_UPLOAD = filename;
                        commase += Image.FileName;
                        //conf.IMAGE = data.IMAGE;
                    }
                    commase += ",";
                }

            }
            //if (ModelState.IsValid)
            //{   //iterating through multiple file collection   
            //    foreach (HttpPostedFileBase file in FILE_UPLOAD)
            //    {

            //        //Checking file is available to save.  
            //        if (file != null)
            //        {                       
            //            var InputFileName = Path.GetFileName(file.FileName);
            //           var commaseparate = file.FileName;

            //            var ServerSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/") + InputFileName);
            //            //Save file to server folder  



            //            file.SaveAs(ServerSavePath);

            //            //assigning file uploaded status to ViewBag for showing message to user.  
            //            ViewBag.UploadStatus = FILE_UPLOAD.Count().ToString() + " files uploaded successfully.";
            //        }

            //    }
            //}
            //if(FILE_UPLOAD.Length > 0)
            //{
            //    HttpFileCollectionBase files = Request.Files;
            //    DataTable dt = new DataTable { Columns = { new DataColumn("path") } };
            //    for(int i=0; i< files.Count;i++)
            //    {
            //        HttpPostedFileBase file = files[i];
            //        string path = Server.MapPath("~") + "\\UploadedFiles\\" + file.FileName;
            //        dt.Rows.Add(file.FileName);
            //        file.SaveAs(path);

            //    }
            //    ViewData.Model = dt.AsEnumerable();
            //}

            data.DATE = DateTime.Now;
            data.FILE_UPLOAD = commase;
           var abc = db.tbl_customer.Add(data);

           var name = Request.Form["NAME"];
           var price = Request.Form["PRICE"];
           var quantity = Request.Form["QUANTITY"];
            var amount = Request.Form["AMOUNT"];
          string[] name1 = name.Split(',');
           string[] price1 = price.Split(',');
           string[] quantity1 = quantity.Split(',');
            string[] amount1 = amount.Split(',');
            tbl_order order = new tbl_order();
            for(var i = 0; i < name1.Length; i++)
            {
                order.NAME = name1[i];
                order.PRICE = price1[i];
                order.QUANTITY = quantity1[i];
                order.AMOUNT = amount1[i];
                order.CUSTOMER_ID = data.ID;
                db.tbl_order.Add(order);
                db.SaveChanges();
            }                                             
           
            return Redirect("customerorder");
            
        }
        
        public ActionResult updatecustomer(int? id)
        {
            var customer = db.tbl_customer.Where(i=>i.ID==id).FirstOrDefault();
            var order = db.tbl_order.Where(i=>i.CUSTOMER_ID==id).ToList();
            ViewBag.orderlist = order;
            return View(customer);
        }
        [HttpPost]
       public ActionResult updatecustomer(tbl_customer cust,tbl_order ord, FormCollection file)
        {
            var name = Request.Form["NAME"];
            var price = Request.Form["PRICE"];
            var quantity = Request.Form["QUANTITY"];
            var amount = Request.Form["AMOUNT"];
            string[] name1 = name.Split(',');
            string[] price1 = price.Split(',');
            string[] quantity1 = quantity.Split(',');
            string[] amount1 = amount.Split(',');

            var customer = db.tbl_customer.Where(i => i.ID == cust.ID).FirstOrDefault();
            if(customer !=null)
            {
                customer.CUSTOMER_NAME = cust.CUSTOMER_NAME;
                customer.DELIVERY_DATE = cust.DELIVERY_DATE;
                customer.BROKER = cust.BROKER;
                customer.TOTAL = cust.TOTAL;
                db.SaveChanges();
            }
            var order = db.tbl_order.Where(i => i.CUSTOMER_ID == cust.ID).Select(i => new
            {
                i.ID
            }).ToArray();

            var arraylength = name1.Count();
            var ordlength = order.Count();

            var a = 0;
            foreach (var item in order)
            {

                var ordid = item.ID;
                var ord2 = db.tbl_order.Where(i => i.ID == ordid).FirstOrDefault();
                if(a<arraylength)
                { 
                ord2.NAME = name1[a];
                ord2.PRICE = price1[a];
                ord2.QUANTITY = quantity1[a];
                ord2.AMOUNT = amount1[a];
                db.SaveChanges();
                a++;
                }
                else
                {
                    db.tbl_order.Remove(ord2);
                    db.SaveChanges();
                }
            }
          
            for(var j=ordlength;j<arraylength;j++)
            {
                ord.NAME = name1[j];
                ord.PRICE = price1[j];
                ord.QUANTITY = quantity1[j];
                ord.AMOUNT = amount1[j];
                ord.CUSTOMER_ID = cust.ID;
                db.tbl_order.Add(ord);
                db.SaveChanges();
            }

            return RedirectToAction("customerorder");
        }

        public ActionResult delete(int? id)
        {
            var delete = db.tbl_customer.Where(i => i.ID == id).FirstOrDefault();
            db.tbl_customer.Remove(delete);
            db.SaveChanges();
            return RedirectToAction("customerorder");
        }

        public ActionResult validatejs()
        {
            return View();
        }

    }
}