using System;

namespace AppointmentService.API.DTOs
{
    public class DoctorDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string Qualifications { get; set; } = string.Empty;
        public decimal ConsultationFee { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateDoctorDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string Qualifications { get; set; } = string.Empty;
        public decimal ConsultationFee { get; set; }
    }
}
