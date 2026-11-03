using System;
using StudentRegistration.Repositories;
using System.Windows.Forms;
using StudentRegistration.Models;
using StudentRegistration.Services;

namespace StudentRegistration
{
    public partial class StudentRegistration : Form
    {
        private int selectedStudentID = 0;
        private readonly StudentService studentService;
        private readonly StudentRepository studentRepository;
        private readonly CourseRespository courseRespository;
        public StudentRegistration(StudentService studentService, StudentRepository studentRepository, CourseRespository courseRespository)
        {
            this.studentService = studentService;
            this.studentRepository = studentRepository;
            this.courseRespository = courseRespository;
            InitializeComponent();
        }

        private void StudentRegistration_Load(object sender, EventArgs e)
        {
            LoadCourses();
            LoadStudents();
            ButtonUpdate.Enabled = false;
            buttonDelete.Enabled = false;
        }

        private void LoadCourses()
        {
            comboBoxCourse.DataSource = courseRespository.GetAllCourses();
            comboBoxCourse.DisplayMember = "CourseName";
            comboBoxCourse.ValueMember = "CourseID";
            comboBoxCourse.SelectedIndex = -1; // No selection by default
        }

        private void LoadStudents()
        {
            dataGridViewStudentDetails.DataSource = studentService.GetAllStudents();

            dataGridViewStudentDetails.Columns["StudentID"].Visible = false;

            dataGridViewStudentDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewStudentDetails.ReadOnly = true;
            dataGridViewStudentDetails.AllowUserToAddRows = false;
            dataGridViewStudentDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void ClearFields()
        {
            textBoxFirstName.Clear();
            textBoxLastName.Clear();
            textBoxAddress.Clear();
            dateTimePickerDateOfBirth.Checked = false;
            comboBoxCourse.SelectedIndex = -1;
            textBoxFirstName.Focus();
            selectedStudentID = 0;
            dataGridViewStudentDetails.ClearSelection();
            ButtonUpdate.Enabled = false;
            buttonDelete.Enabled = false;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void buttonGenerateReport_Click_(object sender, EventArgs e)
        {
            StudentReportForm reportForm = new StudentReportForm(studentRepository);

            reportForm.ShowDialog();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                Student student = new Student();
                student.FirstName = textBoxFirstName.Text.Trim();
                student.LastName = textBoxLastName.Text.Trim();
                student.Address = textBoxAddress.Text.Trim();
                student.DateOfBirth = dateTimePickerDateOfBirth.Checked
                        ? dateTimePickerDateOfBirth.Value.Date : (DateTime?)null;
                student.CourseId = comboBoxCourse.SelectedValue == null ? 0 : Convert.ToInt32(comboBoxCourse.SelectedValue);

                studentService.RegisterStudent(student);

                MessageBox.Show(
                    "Student registered successfully!",
                    "Sucess",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadStudents();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void dataGridViewStudentDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int studentID = Convert.ToInt32(dataGridViewStudentDetails.Rows[e.RowIndex].Cells["StudentID"].Value);

            Student student = studentService.GetStudentByID(studentID);

            if (student == null)
            {
                MessageBox.Show("Student not found");
                return;
            }

            selectedStudentID = student.StudentId;
            textBoxFirstName.Text = student.FirstName;
            textBoxLastName.Text = student.LastName;
            textBoxAddress.Text = student.Address;

            if (student.DateOfBirth.HasValue)
            {
                dateTimePickerDateOfBirth.Value = student.DateOfBirth.Value;
            }

            comboBoxCourse.SelectedValue = student.CourseId;

            ButtonUpdate.Enabled = true;
            buttonDelete.Enabled = true;

        }

        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedStudentID == 0)
                {
                    MessageBox.Show("Please select a student to update");
                    return;
                }

                Student existingStudent = studentService.GetStudentByID(selectedStudentID);

                Student student = new Student
                {
                    StudentId = selectedStudentID,
                    FirstName = textBoxFirstName.Text.Trim(),
                    LastName = textBoxLastName.Text.Trim(),
                    Address = textBoxAddress.Text.Trim(),
                    DateOfBirth = dateTimePickerDateOfBirth.Checked
                        ? dateTimePickerDateOfBirth.Value.Date
                        : existingStudent.DateOfBirth,
                    CourseId = comboBoxCourse.SelectedValue == null
                        ? 0
                        : Convert.ToInt32(comboBoxCourse.SelectedValue)
                };

                studentService.UpdateStudent(student);

                MessageBox.Show(
                    "Student updated successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadStudents();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Update Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (selectedStudentID == 0)
            {
                MessageBox.Show("Please select a student to delete");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this student?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                studentService.DeleteStudent(selectedStudentID);
                MessageBox.Show("Student deleted successfully");

                LoadStudents();
                ClearFields();
            }
        }
    }
}
