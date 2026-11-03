using Microsoft.Reporting.WinForms;
using StudentRegistration.Repositories;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace StudentRegistration
{
    public partial class StudentReportForm : Form
    {
        private readonly StudentRepository studentRepository;
        public StudentReportForm(StudentRepository studentRepository)
        {
            InitializeComponent();
            this.studentRepository = studentRepository;
        }

        private void StudentReportForm_Load(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void LoadReport()
        {
            try
            {
                DataTable students = studentRepository.GetStudentsForReport();

                // Set the report file path
                string reportPath = Path.Combine(
                    Application.StartupPath,
                    "Reports",
                    "StudentReport.rdlc"
                );

                // Configure ReportViewer
                reportViewer1.ProcessingMode = ProcessingMode.Local;
                reportViewer1.LocalReport.ReportPath = reportPath;

                reportViewer1.ShowToolBar = true;
                reportViewer1.ShowPrintButton = true;
                reportViewer1.ShowPrintButton = true;

                // Clear existing data sources
                reportViewer1.LocalReport.DataSources.Clear();

                // Connect the DataTable to the RDLC dataset
                ReportDataSource dataSource = new ReportDataSource(
                    "StudentCourseDataset",
                    students
                );

                reportViewer1.LocalReport.DataSources.Add(dataSource);

                // Display the report
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.ToString(),
                    "Report Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}