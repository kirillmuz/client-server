using WebAPI.Models.Users;

namespace WebAPI.Models.Employees
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MidleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Education> Educations { get; set; }
        public List<WorkExperience> WorkExperience { get; set; }
        public List<UserFile> UserFiles { get; set; }
    }
}
