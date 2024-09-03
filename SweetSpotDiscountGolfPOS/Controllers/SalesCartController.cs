using SweetSpotDiscountGolfPOS.OB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace SweetSpotDiscountGolfPOS
{
    public class SalesCartController : Controller
    {

        public async Task<ActionResult> Index(int invoice, int customer)
        {
            // Check if the Invoice object exists in ViewBag
            if (ViewBag.Invoice == null)
            {
                if(invoice== -10)
                {
                    ViewBag.Invoice = await CreateInvoiceAsync(invoice);
                }
                else
                {
                    // If not, retrieve it asynchronously from the database
                    ViewBag.Invoice = await GetInvoiceAsync(invoice);
                }
            }
            // Perform other actions as needed, such as passing data to the view
            return View();
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}