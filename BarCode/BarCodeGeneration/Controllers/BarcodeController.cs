using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using OnBarcode.Barcode.BarcodeScanner;




namespace BarCodeGeneration.Controllers
{
    public class BarCodeController : Controller
    {
       
            // GET: /BarCode/  
            public ActionResult BarCode()
            {
                return View();
            }

            [HttpPost]
            public ActionResult GenerateBarCode(string barcode)
            {
                byte[] BarCode;
            string BarCodeImage;
            Bitmap objBitmap = new Bitmap(barcode.Length * 100, 100);
            using (Graphics graphic = Graphics.FromImage(objBitmap))
            {
                Font newFont = new Font("IDAutomationHC39M Free Version", 18, FontStyle.Regular);
                PointF point = new PointF(2, 2);
                SolidBrush balck = new SolidBrush(Color.Black);
                SolidBrush white = new SolidBrush(Color.White);
                graphic.FillRectangle(white, 0, 0, objBitmap.Width, objBitmap.Height);
                graphic.DrawString("*" + barcode + "*", newFont, balck, point);
            }
            using (MemoryStream Mmst = new MemoryStream())
            {
                objBitmap.Save(Mmst, ImageFormat.Png);
                BarCode = Mmst.GetBuffer();
                BarCodeImage = BarCode != null ? "data:image/jpg;base64," + Convert.ToBase64String((byte[])BarCode) : "";
                ViewBag.BarcodeImage=BarCodeImage;
            }                                                        
                return View("BarCode");
            }

            public ActionResult barCodeRead()
            {
                return View();
            }

            [HttpPost]
            public ActionResult BarCodeRead(HttpPostedFileBase barCodeUpload)
            {
                String localSavePath = "~/UploadFiles/";
                string str = string.Empty;
                string strImage = string.Empty;
                string strBarCode = string.Empty;
                if (barCodeUpload != null)
                {
                    String fileName = barCodeUpload.FileName;
                    localSavePath += fileName;
                    barCodeUpload.SaveAs(Server.MapPath(localSavePath));

                    Bitmap bitmap = null;
                    try
                    {
                        bitmap = new Bitmap(barCodeUpload.InputStream);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                    if (bitmap == null)
                    {
                        str = "Your file is not an image";
                    }
                    else
                    {
                        strImage = "http://localhost:" + Request.Url.Port + "/UploadFiles/" + fileName;
                        strBarCode = ReadBarcodeFromFile(Server.MapPath(localSavePath));
                    }
                }
                else
                {
                    str = "Please upload the bar code Image.";
                }
                ViewBag.ErrorMessage = str;
                ViewBag.BarCode = strBarCode;
                ViewBag.BarImage = strImage;
                return View();
            }
            private String ReadBarcodeFromFile(string _Filepath)
            {
                String[] barcodes = BarcodeScanner.Scan(_Filepath, BarcodeType.Code39);
                return barcodes[0];
            }  
	}
}
