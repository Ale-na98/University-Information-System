using AutoMapper;
using BusinessLogic.Domain;
using Presentation.DataTransferObjects.Students;
using Presentation.DataTransferObjects.Attendance;

namespace Presentation
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Student, CreateStudentForm>()
                .ForMember(csf => csf.GroupId, m => m.MapFrom(student => student.Group.Id))
                .ReverseMap();

            CreateMap<Student, StudentDetailsViewModel>()
                .ForMember(sdvm => sdvm.GroupName, m => m.MapFrom(student => student.Group.Name));

            CreateMap<Student, EditStudentForm>()
                .ForMember(esf => esf.GroupId, m => m.MapFrom(student => student.Group.Id))
                .ReverseMap();

            CreateMap<Student, StudentResponse>()
                .ForMember(sr => sr.GroupName, m => m.MapFrom(student => student.Group.Name))
                .ReverseMap();

            CreateMap<Attendance, AttendanceDetailsViewModel>()
                .ForMember(advm => advm.LectureName, m => m.MapFrom(attendance => attendance.Lecture.Name))
                .ReverseMap();
        }
    }
}
