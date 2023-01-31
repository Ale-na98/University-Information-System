﻿using System.ComponentModel.DataAnnotations;

namespace Presentation.DataTransferObjects.Students
{
    public record StudentDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Group")]
        public string GroupName { get; set; }
    }
}