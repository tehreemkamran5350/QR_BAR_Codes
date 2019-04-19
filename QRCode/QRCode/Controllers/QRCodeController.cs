using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using OnBarcode.Barcode.BarcodeScanner;
using ZXing;
using QRCode;



namespace QRCode.Controllers
{
    public class QRCodeController : Controller
    {

        // GET: /BarCode/  
        public ActionResult QRCode()
        {
            return View("QRCode");
        }

        [HttpPost]
        public ActionResult GenerateQRCode(string qrcode)
        {
            string QRCodeImage;
            byte[] QRCode;
            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            var result = barcodeWriter.Write(qrcode);
            var qrcodeBitmap = new Bitmap(result);
            using (MemoryStream Mmst = new MemoryStream())
            {
                qrcodeBitmap.Save(Mmst, ImageFormat.Png);
                QRCode = Mmst.GetBuffer();
                QRCodeImage = QRCode != null ? "data:image/jpg;base64," + Convert.ToBase64String((byte[])QRCode) : "";
                ViewBag.QRImage = QRCodeImage;
            }         
          
            return View("QRCode");
        }

        public ActionResult QRCodeRead()
        {
            return View("QRCodeRead");
        }

        [HttpPost]
        public ActionResult QRCodeReader(HttpPostedFileBase qrUpload)
        {
            String localSavePath = "~/UploadFiles/";
            string str = string.Empty;
            if (qrUpload != null)
            {
                String fileName = qrUpload.FileName;
                localSavePath += fileName;
                qrUpload.SaveAs(Server.MapPath(localSavePath));

                Bitmap bitmap = null;
                try
                {
                    bitmap = new Bitmap(qrUpload.InputStream);
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
                    ViewBag.QRImage = "http://localhost:" + Request.Url.Port + "/UploadFiles/" + fileName;
                    var barcodeReader = new BarcodeReader();

                    var qrcode = barcodeReader.Decode(bitmap);
                    ViewBag.QRCode = qrcode.Text;
                }

            }
            return View("QRCodeRead");
        }

    }
}