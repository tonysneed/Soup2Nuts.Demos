using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SoupToNuts.Final.Entities.Service.Net45;
using SoupToNuts.Final.Service.Persistence.UnitsOfWork;

namespace SoupToNuts.Final.WebApi.Controllers
{
    public class ProductController : ApiController
    {
        private readonly INorthwindUnitOfWork _unitOfWork;

        public ProductController(INorthwindUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/Product
        [ResponseType(typeof(IEnumerable<Product>))]
        public async Task<IHttpActionResult> GetProducts()
        {
            IEnumerable<Product> entities = await _unitOfWork.ProductRepository.GetProducts();
            return Ok(entities);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var disposable = _unitOfWork as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
