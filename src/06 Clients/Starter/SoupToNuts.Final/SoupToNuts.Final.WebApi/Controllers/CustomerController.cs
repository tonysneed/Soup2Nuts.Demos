using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SoupToNuts.Final.Entities.Service.Net45;
using SoupToNuts.Final.Service.Persistence.UnitsOfWork;

namespace SoupToNuts.Final.WebApi.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly INorthwindUnitOfWork _unitOfWork;

        public CustomerController(INorthwindUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/Customer
        [ResponseType(typeof(IEnumerable<Customer>))]
        public async Task<IHttpActionResult> GetCustomers()
        {
            IEnumerable<Customer> entities = await _unitOfWork.CustomerRepository.GetCustomers();
            return Ok(entities);
        }

        // GET api/Customer/ABCD
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> GetCustomer(string id)
        {
            Customer entity = await _unitOfWork.CustomerRepository.GetCustomer(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
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
