using System;
using System.Collections.Generic;
using System.Linq;
using Billing.API.Models;
using Billing.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Billing.API.Controllers
{
    /// <summary>
    /// API Controller
    /// </summary>
    [Route("api/billing")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        public BillingController(ICostCalculationService costCalculationService)
        {
            CostCalculationService = costCalculationService;
        }

        public ICostCalculationService CostCalculationService { get; }

        // GET api/billing/cost/
        [HttpGet("cost")]
        public ActionResult<IEnumerable<PurchaseViewModel>> Cost(string[] skus)
        {
            var results = CostCalculationService.CalculateCost(skus);
            throw new NotImplementedException();
        }

    }
}