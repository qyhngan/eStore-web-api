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
    public class OrderDAO
    {
        public static Order FindOrderById(int oId)
        {
            Order order = new Order();
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    order = dbContext.Orders.Include(x => x.Member).Include(x => x.OrderDetails).SingleOrDefault(x => x.OrderId == oId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return order;
        }

        public static int SaveOrder(OrderRequest order)
        {
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    int id = 0;
                    if (dbContext.Orders.Count() > 0)
                    {
                        id = dbContext.Orders.Max(x => x.OrderId);
                    }

                    Order order1 = new Order
                    {
                        OrderDate = order.OrderDate,
                        MemberId = order.MemberId,
                        Freight = order.Freight,
                        RequiredDate = order.RequiredDate,
                        ShippedDate = order.ShippedDate,
                        OrderId = id + 1 
                    };
                    dbContext.Orders.Add(order1);
                    dbContext.SaveChanges();
                    return order1.OrderId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateOrder(OrderRequest order)
        {
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    var order1 = dbContext.Orders.SingleOrDefault(x => x.OrderId == order.OrderId);
                    order1.OrderDate = order.OrderDate;
                    order1.MemberId = order.MemberId;
                    order1.Freight = order.Freight;
                    order1.RequiredDate = order.RequiredDate;
                    order1.ShippedDate = order.ShippedDate;
                    dbContext.Entry<Order>(order1).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteOrder(int oId)
        {
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    var order = dbContext.Orders.SingleOrDefault(x => x.OrderId == oId);
                    dbContext.Orders.Remove(order);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static IEnumerable<Order> GetOrders(string from = null, string to = null)
        {
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    if (from == null && to != null)
                    {
                        return dbContext.Orders.Where(x => x.OrderDate <= DateTime.Parse(to)).Include(x => x.Member).ToList();
                    }
                    else if (from != null && to == null)
                    {
                        return dbContext.Orders.Where(x => x.OrderDate >= DateTime.Parse(from)).Include(x => x.Member).ToList();
                    }
                    else if (from != null && to != null)
                    {
                        return dbContext.Orders.Where(x => x.OrderDate >= DateTime.Parse(from) && x.OrderDate <= DateTime.Parse(to)).Include(x => x.Member).ToList();
                    }
                    return dbContext.Orders.Include(x => x.Member).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static IEnumerable<Order> GetOrdersByMemberId(int memberId)
        {
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    return dbContext.Orders.Where(x => x.MemberId == memberId).Include(x=>x.OrderDetails).Include(x => x.Member).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
