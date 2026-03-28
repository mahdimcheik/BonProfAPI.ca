using BonProfCa.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BonProfCa.Models;

public class StudentDetails 
{
    [Required]
    public Guid Id { get; set; }
    public UserMinimalDetails User { get; set; }

    public StudentDetails(Student student)
    {
        Id = student.Id;
        if (student.User is not null)
        {
            User = new UserMinimalDetails
            {
                Id = student.User.Id,
                FirstName = student.User.FirstName,
                LastName = student.User.LastName,
                Email = student.User.Email
            };
        }
    }

}

public class StudentCreate
{
    public StudentCreate() { }
}

public class StudentUpdate 
{
    public void UpdateStudent(Student student)
    {
    }
}
