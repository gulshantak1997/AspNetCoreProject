using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE1.Models;
using AdminLTE1.PayPalHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AdminLTE1.Controllers
{
  
    public class CartController : Controller
    {
        private IConfiguration configuration { get; }

        private readonly AppDbContext _context;

        public CartController(IConfiguration _configuration, AppDbContext context)
        {
            configuration = _configuration;
            _context = context;
        }


        public IActionResult Index()
        {
            var productModel = new ProductModel();
            ViewBag.products = productModel.FindAll();
            ViewBag.total = productModel.FindAll().Sum(p => p.Price * p.Quantity);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(double total)
        {
            var payPalAPI = new PayPalAPI(configuration);
            string url = await payPalAPI.GetRedirectURLToPayPal(total, "USD");

            return Redirect(url);
        }

        public async Task<IActionResult> Success([FromQuery(Name = "paymentId")] string paymentId, [FromQuery(Name = "PayerId")] string payerID)
        {
            var payPalAPI = new PayPalAPI(configuration);
            PayPalPaymentExecutedResponse result = await payPalAPI.ExecutedPayment(paymentId, payerID);
            Debug.WriteLine("Transaction Details");
            Debug.WriteLine("cart: " + result.cart);
            Debug.WriteLine("create_time: " + result.create_time.ToLongDateString());
            Debug.WriteLine("id: " + result.id);
            Debug.WriteLine("intent: " + result.intent);
            Debug.WriteLine("link 0 - href: " + result.links[0].href);
            Debug.WriteLine("link 0 - method: " + result.links[0].method);
            Debug.WriteLine("link 0 - rel: " + result.links[0].rel);
            Debug.WriteLine("payer_info - first_name: " + result.payer.payer_info.first_name);
            Debug.WriteLine("payer_info - last_name: " + result.payer.payer_info.last_name);
            Debug.WriteLine("payer_info - email: " + result.payer.payer_info.email);
            Debug.WriteLine("payer_info - billing_address: " + result.payer.payer_info.billing_address);
            Debug.WriteLine("payer_info - country_code: " + result.payer.payer_info.country_code);
            Debug.WriteLine("payer_info - shipping_address: " + result.payer.payer_info.shipping_address);
            Debug.WriteLine("payer_info - payer_id: " + result.payer.payer_info.payer_id);
            Debug.WriteLine("state: " + result.state);


            PayPalAPITransaction apiTransaction = new PayPalAPITransaction();
            apiTransaction.TransactionId = result.id;
            apiTransaction.Intent = result.intent;
            apiTransaction.CartId = result.cart;
            apiTransaction.TransactionState = result.state;
            apiTransaction.CreateTime = result.create_time;

            _context.PayPalAPITransaction.Add(apiTransaction);
            _context.SaveChanges();

            ViewBag.result = result;
            return View("Success");
        }
    }
}