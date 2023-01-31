using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.DataTransferObjects.Attendance
{
    public record AttendanceDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Lecture")]
        public string LectureName { get; set; }

        [Display(Name = "Date")]
        public DateTime LectureDate { get; set; }

        [Display(Name = "Presence")]
        public bool Presence { get; set; }

        [Display(Name = "Hometask done")]
        public bool HometaskDone { get; set; }
    }
}
