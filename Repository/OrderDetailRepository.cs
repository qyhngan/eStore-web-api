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
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void AddOrderDetail(OrderDetailRequest orderDetail, int orderId) => OrderDetailDAO.AddOrderDetail(orderDetail, orderId);

        public void DeleteOrderDetail(int orderDetailId) => OrderDetailDAO.DeleteOrderDetail(orderDetailId);

        public IEnumerable<OrderDetailResponse> GetOrderDetailByOrderId(int orderId) => OrderDetailDAO.GetOrderDetailByOrderId(orderId).Select(item => Mapper.MapOrderDetailToDTO(item));

        public void UpdateOrderDetail(OrderDetailRequest orderDetail) => OrderDetailDAO.UpdateOrderDetail(orderDetail);
    }
}
