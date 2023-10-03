using DataAccess.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IOrderDetailRepository
    {
        public IEnumerable<OrderDetailResponse> GetOrderDetailByOrderId(int orderId);
        public void AddOrderDetail(OrderDetailRequest orderDetail, int orderId);
        public void UpdateOrderDetail(OrderDetailRequest orderDetail);
        public void DeleteOrderDetail(int orderDetailId);
    }
}
