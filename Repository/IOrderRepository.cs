using DataAccess.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IOrderRepository
    {
        int AddOrder(OrderRequest order);
        void DeleteOrder(int id);
        IEnumerable<OrderResponse> GetAllOrder(string from = null, string to = null);
        OrderResponse GetOrderById(int id);
        void UpdateOrder(OrderRequest order);
        IEnumerable<OrderResponse> GetOrdersByMemberId(int memberId);
    }
}
