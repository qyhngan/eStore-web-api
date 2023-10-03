using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Repository;
using DataAccess.DTO.Order;
using System.Security.Claims;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository = new OrderRepository();
        private readonly IOrderDetailRepository _orderDetailRepository = new OrderDetailRepository();

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders(string from = null, string to = null)
        {
            string role = User.FindFirst(ClaimTypes.Role).Value;

            if (role != "admin")
            {
                return Unauthorized();
            }
            List<OrderResponse> orderResponse = null;
            try
            {
                orderResponse = _orderRepository.GetAllOrder(from, to).ToList();

                if (orderResponse == null)
                {
                    return NotFound("List is empty");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Ok(orderResponse);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<OrderDetailResponse>>> GetOrderDetails(int id)
        {
            List<OrderDetailResponse> orderDetailResponse = null;
            try
            {
                orderDetailResponse = _orderDetailRepository.GetOrderDetailByOrderId(id).ToList();

                if (orderDetailResponse == null)
                {
                    return NotFound("List is empty");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Ok(orderDetailResponse);
        }

        // GET: api/Orders/History
        [HttpGet("History")]
        public async Task<ActionResult<List<OrderResponse>>> GetOrderByMemberId()
        {
            string role = User.FindFirst(ClaimTypes.Role).Value;

            if (role != "member")
            {
                return Unauthorized();
            }
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<OrderResponse> orderDetailResponse = null;
            try
            {
                orderDetailResponse = _orderRepository.GetOrdersByMemberId(id).ToList();

                if (orderDetailResponse == null)
                {
                    return NotFound("List is empty");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Ok(orderDetailResponse);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            //if (id != order.OrderId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(order).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!OrderExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderRequest orderRequest)
        {
            try
            {
                string role = User.FindFirst(ClaimTypes.Role).Value;

                if (role != "member")
                {
                    return Unauthorized();
                }

                int orderId = _orderRepository.AddOrder(orderRequest);
                orderRequest.OrderDetails.ForEach(x => _orderDetailRepository.AddOrderDetail(x, orderId));

            }
            catch (DbUpdateException)
            {
                //if (OrderExists(order.OrderId))
                //{
                //    return Conflict();
                //}
                //else
                //{
                //    throw;
                //}
            }

            return Ok();
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            //var order = await _context.Orders.FindAsync(id);
            //if (order == null)
            //{
            //    return NotFound();
            //}

            //_context.Orders.Remove(order);
            //await _context.SaveChangesAsync();

            return NoContent();
        }

        //private bool OrderExists(int id)
        //{
        //    return _context.Orders.Any(e => e.OrderId == id);
        //}
    }
}
