using AutoMapper;
using DataAccess.Entities;
using BusinessLogic.Domain;
using DataAccess.Elasticsearch.Documents;

namespace BusinessLogic
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Group, GroupDb>().ReverseMap();
            CreateMap<Lecture, LectureDb>().ReverseMap();
            CreateMap<StudentDb, Student>().ReverseMap();
            CreateMap<Attendance, AttendanceDb>().ReverseMap();
            CreateMap<StudentDb, StudentDocument>()
                .ForMember(sd => sd.GroupName, m => m.MapFrom(sdb => sdb.Group.Name));
            CreateMap<Student, StudentDocument>()
                .ForMember(sd => sd.GroupName, m => m.MapFrom(s => s.Group.Name))
                .ReverseMap();
            CreateMap(typeof(DataAccess.Page<>), typeof(Domain.Page<>))
                .ReverseMap();
            CreateMap(typeof(DataAccess.PageParams), typeof(Domain.PageParams))
                .ReverseMap();
        }
    }
}
