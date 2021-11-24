using AutoMapper;
using DataAccess;

namespace BusinessLogic
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<StudentDto, StudentDb>().ReverseMap();
            CreateMap<TeacherDto, TeacherDb>().ReverseMap();
            CreateMap<LectureDto, LectureDb>().ReverseMap();
            CreateMap<HometaskDto, HometaskDb>().ReverseMap();
            CreateMap<AttendanceDto, AttendanceDb>().ReverseMap();
            CreateMap<AttendanceReportDto, AttendanceDb>().ReverseMap();
        }
    }
}