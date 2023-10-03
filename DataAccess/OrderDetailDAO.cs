using BusinessObject;
using DataAccess.DTO.Order;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderDetailDAO
    {

        public static List<OrderDetail> GetOrderDetails()
        {
            var orderDetails = new List<OrderDetail>();
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    orderDetails = dbContext.OrderDetails.Include(x => x.Product).Include(x => x.Order).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orderDetails;
        }

        public static List<OrderDetail> FindOrderDetailByOrderId(int oId)
        {
            var orderDetails = new List<OrderDetail>();
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    orderDetails = dbContext.OrderDetails.Where(x => x.OrderId == oId).Include(x => x.Product).Include(x => x.Order).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orderDetails;
        }

        public static void AddOrderDetail(OrderDetailRequest orderDetail, int orderId)
        {
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    var newOrderDetail = new OrderDetail()
                    {
                        OrderId = orderId,
                        ProductId = orderDetail.ProductId,
                        Quantity = orderDetail.Quantity,
                        UnitPrice = orderDetail.UnitPrice
                    };
                    var product = dbContext.Products.SingleOrDefault(x => x.ProductId == orderDetail.ProductId);
                    product.UnitsInStock -= orderDetail.Quantity;
                    if (product.UnitsInStock < 0)
                    {
                        dbContext.Orders.Remove(dbContext.Orders.SingleOrDefault(x => x.OrderId == orderId));
                        throw new Exception("Out of stock");
                    }
                    dbContext.OrderDetails.Add(newOrderDetail);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteOrderDetail(int orderDetailId)
        {
            throw new NotImplementedException();
        }

        public static List<OrderDetail> GetOrderDetailByOrderId(int orderId)
        {
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    var result = dbContext.OrderDetails.Where(x => x.OrderId == orderId).Include(x => x.Product).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateOrderDetail(OrderDetailRequest orderDetail)
        {
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    var orderDetailToUpdate = dbContext.OrderDetails.SingleOrDefault(x => x.OrderId == orderDetail.OrderId && x.ProductId == orderDetail.ProductId);
                    if (orderDetailToUpdate != null)
                    {
                        orderDetailToUpdate.OrderId = orderDetail.OrderId;
                        orderDetailToUpdate.ProductId = orderDetail.ProductId;
                        orderDetailToUpdate.Quantity = orderDetail.Quantity;
                        orderDetailToUpdate.UnitPrice = orderDetail.UnitPrice;
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
