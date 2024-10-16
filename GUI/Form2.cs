using BUS;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class Form2 : Form
    {
        private Model1 context = new Model1();

        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        private readonly MajorService majorService = new MajorService();
        public Form2()
        {
            InitializeComponent();
            LoadFaculties();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn chuyên ngành trước khi đăng ký.");
                return;
            }

            // Lấy ID chuyên ngành từ ComboBox2
            int majorId = context.Majors
                .Where(m => m.Name == comboBox2.SelectedItem.ToString())
                .Select(m => m.MajorID)
                .FirstOrDefault();

            // Kiểm tra xem có sinh viên nào được chọn không
            bool hasRegistered = false;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Kiểm tra nếu hàng không phải là hàng mới
                if (!row.IsNewRow)
                {
                    // Lấy giá trị của CheckBox
                    bool isSelected = Convert.ToBoolean(row.Cells["IsSelected"].Value);

                    // Nếu sinh viên được chọn
                    if (isSelected)
                    {
                        // Lấy thông tin sinh viên
                        int studentId = Convert.ToInt32(row.Cells["StudentID"].Value);

                        // Cập nhật thông tin sinh viên
                        var student = context.Students.Find(studentId);
                        if (student != null)
                        {
                            student.MajorID = majorId; // Cập nhật chuyên ngành
                            hasRegistered = true; // Đánh dấu đã đăng ký
                        }
                    }
                }
            }

            // Lưu thay đổi vào cơ sở dữ liệu nếu có sinh viên được đăng ký
            if (hasRegistered)
            {
                context.SaveChanges(); // Lưu thay đổi
                MessageBox.Show("Đăng ký chuyên ngành thành công cho sinh viên đã chọn.");
                LoadStudents(comboBox1.SelectedIndex); // Tải lại danh sách sinh viên
            }
            else
            {
                MessageBox.Show("Không có sinh viên nào được chọn để đăng ký.");
            }

        }
        private void LoadFaculties()
        {
            var faculties = context.Faculties.ToList();
            foreach (var faculty in faculties)
            {
                comboBox1.Items.Add(faculty.FacultyName);
            }
        }
        private void LoadMajors(int facultyID)
        {
            var majors = context.Majors.Where(m => m.FacultyID == facultyID).ToList();
            foreach (var major in majors)
            {
                comboBox2.Items.Add(major.Name);
            }
        }
        private void LoadStudents(int facultyID)
        {
            var students = context.Students
                .Where(s => s.FacultyID == facultyID && s.MajorID == null)
                .ToList();

            // Ràng buộc danh sách sinh viên lên DataGridView
            dataGridView1.DataSource = students;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Kiểm tra nếu hàng không phải là hàng mới
                if (!row.IsNewRow)
                {
                    // Lấy giá trị của CheckBox
                    bool isSelected = Convert.ToBoolean(row.Cells["IsSelected"].Value);

                    // Nếu sinh viên được chọn
                    if (isSelected)
                    {
                        // Lấy thông tin sinh viên
                        int studentId = Convert.ToInt32(row.Cells["StudentID"].Value);
                        // Lấy ID chuyên ngành đã chọn từ ComboBox2
                        int? majorId = comboBox2.SelectedItem != null
                            ? context.Majors
                                .Where(m => m.Name == comboBox2.SelectedItem.ToString())
                                .Select(m => m.MajorID)
                                .FirstOrDefault()
                            : (int?)null;

                        // Cập nhật thông tin sinh viên
                        var student = context.Students.Find(studentId);
                        if (student != null)
                        {
                            student.MajorID = majorId; // Cập nhật chuyên ngành
                            context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                        }
                    }
                }
            }

            // Thông báo đăng ký thành công
            MessageBox.Show("Đăng ký chuyên ngành thành công cho sinh viên đã chọn.");

            // Cập nhật lại DataGridView
            LoadStudents(comboBox1.SelectedIndex); // Tải lại danh sách sinh viên
        }
    }
    }

