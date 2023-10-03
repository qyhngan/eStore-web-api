using BusinessObject;
using DataAccess.DTO.Category;
using DataAccess.DTO.Member;
using DataAccess.DTO.Order;
using DataAccess.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapper
{
    public class Mapper
    {
        public static MemberResponse MapMemberToDTO(Member member, string role = null)
        {
            if (member == null) return null;
            if (role == "admin")
            {
                return new MemberResponse
                {
                    //MemberId = 0,
                    Email = member.Email,
                    //CompanyName = "",
                    //City = "",
                    //Country = "",
                    Role = role,
                };
            }
            return new MemberResponse
            {
                MemberId = member.MemberId,
                Email = member.Email,
                CompanyName = member.CompanyName,
                City = member.City,
                Country = member.Country,
                Role = role,
            };
        }

        public static ProductResponse MapProductToDTO(Product product)
        {
            if (product == null) return null;
            return new ProductResponse
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Weight = product.Weight,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                CategoryName = product.Category.CategoryName,
                CategoryId = product.Category.CategoryId,
            };
        }

        public static CategoryResponse MapCategoryToDTO(Category category)
        {
            if (category == null) return null;
            return new CategoryResponse
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
            };
        }

        public static OrderResponse MapOrderToDTO(Order order, decimal total)
        {

            if (order == null) return null;
            return new OrderResponse
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                RequiredDate = order.RequiredDate,
                ShippedDate = order.ShippedDate,
                Freight = order.Freight,
                MemberId = order.MemberId,
                MemberEmail = order.Member.Email,
                Total = total,
            };
        }

        public static OrderDetailResponse MapOrderDetailToDTO(OrderDetail orderDetail)
        {
            if (orderDetail == null) return null;
            return new OrderDetailResponse
            {
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                UnitPrice = orderDetail.UnitPrice,
                Quantity = orderDetail.Quantity,
                Discount = orderDetail.Discount,
                ProductName = orderDetail.Product.ProductName,
            };
        }
    }
}
