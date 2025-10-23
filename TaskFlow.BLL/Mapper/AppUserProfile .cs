using AutoMapper;
using TaskFlow.DAL.Entites;
using TaskFlow.BLL.ModelVM.AppUser;

namespace TaskFlow.BLL.Mapper.AppUserMapping
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            // Read mappings
            CreateMap<AppUser, AppUserReadDto>()
                .ForMember(m => m.FullName, o => o.MapFrom(a => $"{a.FirstName} {a.LastName}"))
                .ForMember(m => m.RoleDisplay, o => o.MapFrom(a => a.UserRole.ToString()));

            // Create mapping using constructor
            CreateMap<AppUserCreateDto, AppUser>()
                .ConstructUsing(dto => new AppUser(
                    dto.FirstName,
                    dto.LastName,
                    dto.Email,
                    dto.UserRole,
                    dto.CreatedByUserId
                ));

            // Update mapping with ReverseMap
            CreateMap<AppUserUpdateDto, AppUser>().ReverseMap();
        }
    }
}