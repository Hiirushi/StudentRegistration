
using StudentRegistration.Repositories;
using StudentRegistration.Models;
using System;

namespace StudentRegistration.Services
{
    public class StudentService
    {
        private readonly StudentRepository studentRepository;

        public StudentService()
        {
            studentRepository = new StudentRepository();
        }

        public void RegisterStudent(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.FirstName))
            {
                throw new Exception("First Name is Required");
            }

            if (string.IsNullOrWhiteSpace(student.LastName))
            {
                throw new Exception("Last Name is Required");
            }

            if (student.CourseId <= 0)
            {
                throw new Exception("Please Select a Course");
            }

            if (student.FirstName.Length > 50)
            {
                throw new Exception("First Name cannot exceed 50 characters");
            }

            if (student.LastName.Length > 50)
            {
                throw new Exception("Last Name cannot exceed 50 characters");
            }

            if (student.Address != null && student.Address.Length > 255)
            {
                throw new Exception("Address cannot exceed 255 characters");
            }

            if (student.DateOfBirth.HasValue && student.DateOfBirth.Value.Date > DateTime.Today)
            {
                throw new Exception("Date of Birth cannot be in the future");
            }

            studentRepository.AddStudent(student);
        }

        public Student GetStudentByID(int studentID)
        {
            return studentRepository.GetStudentByID(studentID);
        }

        public void UpdateStudent(Student student)
        {
            studentRepository.UpdateStudent(student);
        }

        public void DeleteStudent(int studentID)
        {
            studentRepository.DeleteStudent(studentID);
        }
    }
}
