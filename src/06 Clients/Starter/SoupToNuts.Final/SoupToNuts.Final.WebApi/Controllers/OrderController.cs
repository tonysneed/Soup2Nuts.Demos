using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TrackableEntities.Common;
using SoupToNuts.Final.Entities.Service.Net45;
using SoupToNuts.Final.Service.Persistence.Exceptions;
using SoupToNuts.Final.Service.Persistence.UnitsOfWork;

namespace SoupToNuts.Final.WebApi.Controllers
{
    public class OrderController : ApiController
    {
        private readonly INorthwindUnitOfWork _unitOfWork;

        public OrderController(INorthwindUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/Order
        [ResponseType(typeof(IEnumerable<Order>))]
        public async Task<IHttpActionResult> GetOrders()
        {
            IEnumerable<Order> entities = await _unitOfWork.OrderRepository.GetOrders();
            return Ok(entities);
        }

        // GET api/Order?customerId=ABCD
        [ResponseType(typeof(IEnumerable<Order>))]
        public async Task<IHttpActionResult> GetOrders(string customerId)
        {
            IEnumerable<Order> orders = await _unitOfWork.OrderRepository.GetOrders(customerId);
            return Ok(orders);
        }

        // GET api/Order/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrder(int id)
        {
            Order entity = await _unitOfWork.OrderRepository.GetOrder(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        // POST api/Order
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrder(Order entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.OrderRepository.Insert(entity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (UpdateException)
            {
                if (_unitOfWork.OrderRepository.Find(entity.OrderId) == null)
                {
                    return Conflict();
                }
                throw;
            }

            await _unitOfWork.OrderRepository.LoadRelatedEntitiesAsync(entity);
            entity.AcceptChanges();

            return CreatedAtRoute("DefaultApi", new { id = entity.OrderId }, entity);
        }

        // PUT api/Order
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PutOrder(Order entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.OrderRepository.Update(entity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (UpdateConcurrencyException)
            {
                if (_unitOfWork.OrderRepository.Find(entity.OrderId) == null)
                {
                    return Conflict();
                }
                throw;
            }

            await _unitOfWork.OrderRepository.LoadRelatedEntitiesAsync(entity);
            entity.AcceptChanges();
            return Ok(entity);
        }

        // DELETE api/Order/5
        public async Task<IHttpActionResult> DeleteOrder(int id)
        {
            bool result = await _unitOfWork.OrderRepository.DeleteOrder(id);
            if (!result) return Ok();

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (UpdateConcurrencyException)
            {
                if (_unitOfWork.OrderRepository.Find(id) == null)
                {
                    return Conflict();
                }
                throw;
            }

            return Ok();
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
