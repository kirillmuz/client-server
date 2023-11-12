namespace WebAPI.Models.DTO
{
    public class WorkExperienceRequestDto
    {
        public int EmployeeId { get; set; }
        public int WorkedYears { get; set; } = 0;
        public string? Description { get; set; } = null;
    }
}
