using AutoMapper;
using Standard_Solution.Domain.DTOs.Request;
using Standard_Solution.Domain.DTOs.Response;
using Standard_Solution.Domain.Models;
namespace Standard_Solution.Infra.AutoMapper;

public class Standard_APIProfile : Profile
{
    public Standard_APIProfile()
    {
        CreateMap<User, UserExistsEmailResponse>()
            .ReverseMap();
        CreateMap<User, EditUserRequest>()
            .ReverseMap();
        CreateMap<User, UserGetResponse>()
            .ForMember(request => request.Email, opt => opt.MapFrom(model => model.UserName))
            .ReverseMap();
    }
}
