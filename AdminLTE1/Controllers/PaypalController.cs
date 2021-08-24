using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE1.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminLTE1.Controllers
{
    public class PaypalController : Controller
    {
      
        private readonly AppDbContext _context;
        public PaypalController(AppDbContext context)
        {
            this._context = context;
        }



        //[HttpPost]
        //public JsonResult Transaction(test model)
        //{
        //    return Json("Transaction is saved successfully");
        //}


        [HttpPost]
        public JsonResult StoreTransaction(PaypalData model)
        {
            Transaction tranObj = new Transaction();
            tranObj.TransactionId = model.transactionId;
            tranObj.Status = model.transactionStatus;
            tranObj.CurrencyCode= model.CurrencyCode;
            tranObj.Amount= model.transactionAmount;
            tranObj.PayerName= model.PayerName;
            tranObj.MerchantId= model.MerchantId;
            tranObj.PayerId= model.PayerId;
            tranObj.PayerEmail= model.PayerEmail;
            tranObj.PostalCode= model.PostalCode;
            tranObj.Address= model.Address;

            _context.Transaction.Add(tranObj);
            _context.SaveChanges();

            return Json("Saved Successfully!");

        }


    }
}