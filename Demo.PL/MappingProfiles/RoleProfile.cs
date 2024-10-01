using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.MappingProfiles
{
    public class RoleProfile : Profile
    {

        public RoleProfile() { CreateMap<IdentifyRole, RoleViewModel>().ForMember(d => d.RoleName, o => o.MapFrom(s => s.Name)).ReverseMap(); }
    }
  

}
