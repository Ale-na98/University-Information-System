using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.DataTransferObjects.Attendance
{
    public record SearchAttendanceForm
    {
        [Display(Name = "Student")]
        public int? StudentId { get; set; }

        [Display(Name = "Lecture")]
        public int? LectureId { get; set; }

        [Display(Name = "Date from")]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "Date to")]
        public DateTime? DateTo { get; set; }
    }
}
