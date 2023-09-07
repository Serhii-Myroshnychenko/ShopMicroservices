using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mapper;

public class DicountProfile : Profile
{
    public DicountProfile()
    {
        CreateMap<Coupon, CouponModel>().ReverseMap();
    }
}
