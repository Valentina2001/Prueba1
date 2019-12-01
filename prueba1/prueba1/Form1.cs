using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prueba1
{
    public partial class Form1 : Form
    {
        private StudentManager _Manager;
        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _Manager = new StudentManager();
        }

        public void LimpiarTexto()
        {
            foreach(Control ctrl in this.Controls)
            {
                if(ctrl is TextBox)
                {
                    TextBox text = ctrl as TextBox;
                    text.Clear();
                }
            }
        }

        private void btnReadStudent_Click(object sender, EventArgs e)
        {
            listStudent.Items.Clear();
            List<Student> students = _Manager.GetStudents();
            foreach(Student student in students)
            {
                ListViewItem item = listStudent.Items.Add(student.ID.ToString());
                item.SubItems.Add(student.Name);
                item.SubItems.Add(student.LastName);
                item.SubItems.Add(student.Address);
                item.SubItems.Add(student.Phone);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnWriteStudent_Click(object sender, EventArgs e)
        {
            Student student = new Student()
            {
                Name = txtName.Text,
                LastName = txtLastName.Text,
                Address = txtAddress.Text,
                Phone = textBox1.Text,
            };
            _Manager.Write(student);
            btnReadStudent_Click(this, new EventArgs());
            LimpiarTexto();

        }

        private void btnUpdateStudent_Click(object sender, EventArgs e)
        {
            Student student = new Student()
            {
                ID = int.Parse(txtID.Text),
                Name = txtName.Text,
                LastName = txtLastName.Text,
                Address = txtAddress.Text,
                Phone = textBox1.Text,
            };
            _Manager.Update(student);
            btnReadStudent_Click(this, new EventArgs());
            LimpiarTexto();
        }

        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            _Manager.Delete(int.Parse(txtID.Text));
            btnReadStudent_Click(this, new EventArgs());
            LimpiarTexto();
        }
    }
}
