using AutoMapper;
using FloralFusion.Application.Models;
using FloralFusion.Application.Models.ModelsForOrders;
using FloralFusion.Application.Models.ModelsForUserAndAdminPanel;
using FloralFusion.Domain.Entities;

namespace FloralFusion.Application.Mapper
{
    public class AutoMapper:Profile
    {
        public AutoMapper()
        {
            CreateMap<UserModel,User>().ReverseMap();
            CreateMap<DeliveryOptionModel,DeliveryOption>().ReverseMap();
            CreateMap<OrderItemModel,OrderItem>().ReverseMap();
            CreateMap<OrderModel,Order>().ReverseMap();
            CreateMap<WishlistItemModel,WishlistItem>().ReverseMap();
            CreateMap<WishlistModel,Wishlist>().ReverseMap();
            CreateMap<FlowerModel,Flower>().ReverseMap();
            CreateMap<NotificationModel,Notification>().ReverseMap();
            CreateMap<UserNotificationModel,UserNotification>().ReverseMap();
            CreateMap<PaymentModel,PaymentMethod>().ReverseMap();
            CreateMap<SalesReportModel,SalesReport>().ReverseMap();
            CreateMap<ReviewModel, Reviews>().ReverseMap();
        }
    }
}
