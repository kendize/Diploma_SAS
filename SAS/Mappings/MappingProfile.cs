using AutoMapper;
using SAS.DTO;

namespace SAS.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDTO, DashboardTableResponseDTO>();
            CreateMap<UserCourseDTO, UserCourseResponseDTO>();
        }
    }
}
