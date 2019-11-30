using System.Collections.Generic;
using AutoMapper;
using Billing.API.Models;
using Billing.API.Models.ViewModels;
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
        private readonly IMapper _mapper;

        public ICostCalculationService CostCalculationService { get; }

        public IProductsService ProductsService { get; }

        public BillingController(ICostCalculationService costCalculationService, IProductsService productsService, IMapper mapper)
        {
            CostCalculationService = costCalculationService;
            ProductsService = productsService;
            _mapper = mapper;
        }


        // GET api/billing/cost/
        [HttpGet("cost")]
        public ActionResult<IEnumerable<PurchaseViewModel>> Cost([FromBody] string[] skus)
        {
            var purchases = CostCalculationService.CalculateCost(skus);
            var purchaseViewModel = _mapper.Map<IEnumerable<PurchaseViewModel>>(purchases);
            return new ActionResult<IEnumerable<PurchaseViewModel>>(purchaseViewModel);
        }

        // GET api/billing/products/
        [HttpGet("products")]
        public ActionResult<IEnumerable<ProductViewModel>> ProductsBySku([FromBody] string[] skus)
        {
            var product = ProductsService.Get(skus);
            var productViewModel = _mapper.Map<IEnumerable<ProductViewModel>>(product);
            return new ActionResult<IEnumerable<ProductViewModel>>(productViewModel);
        }
    }
}