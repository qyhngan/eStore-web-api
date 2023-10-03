using DataAccess;
using DataAccess.DTO.Order;
using DataAccess.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        public int AddOrder(OrderRequest order) => OrderDAO.SaveOrder(order);

        public void DeleteOrder(int id) => OrderDAO.DeleteOrder(id);

        public IEnumerable<OrderResponse> GetAllOrder(string from = null, string to = null) => OrderDAO.GetOrders(from, to).Select(item => Mapper.MapOrderToDTO(item, 0));

        public OrderResponse GetOrderById(int id) => Mapper.MapOrderToDTO(OrderDAO.FindOrderById(id), 0);

        public IEnumerable<OrderResponse> GetOrdersByMemberId(int memberId) => OrderDAO.GetOrdersByMemberId(memberId).Select(item => Mapper.MapOrderToDTO(item, 0));

        public void UpdateOrder(OrderRequest order) => OrderDAO.UpdateOrder(order);
    }
}
