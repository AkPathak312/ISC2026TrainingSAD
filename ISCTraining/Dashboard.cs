using ISCTraining.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISCTraining
{
    public partial class Dashboard : Form
    {
        String username;
        AmioncDbContext db;
        int selectedUser = 0;
        public Dashboard(String username)
        {
            InitializeComponent();
            this.username = username;
            db = new AmioncDbContext();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            label1.Text = $"Hi {username}";
            LoadCombobox();
            LoadUsers(0);
        }

        private void LoadUsers(int v)
        {
            var users = db.Users.Where(x => v == 0 || x.OfficeId == v).Select(x => new
            {
                Id = x.Id,
                Name = x.FirstName,
                LastName = x.LastName,
                Age = DateTime.Now.Year - ((DateOnly)x.Birthdate).Year,
                UserRole = x.Role.Title,
                Email = x.Email,
                Office = x.Office.Title,
                Active = x.Active
            }).ToList();
            dataGridView1.DataSource = users;
            //Setting the columns to invisible which we dont want on UI but want its backend value
            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.Columns["Active"].Visible = false;

            //Chaging headrer text with space as while building object spaces are not allowed.
            dataGridView1.Columns["LastName"].HeaderText = "Last Name";
            dataGridView1.Columns["UserRole"].HeaderText = "User Role";

            //Making sure rows is slected instead of cell.
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void LoadCombobox()
        {
            List<Office> offices = new List<Office>();
            offices = db.Offices.ToList();
            Office alloffice = new Office();
            alloffice.Id = 0;
            alloffice.Title = "All Offices";
            offices.Insert(0, alloffice);
            comboBox1.DisplayMember = "Title";
            comboBox1.ValueMember = "Id";
            comboBox1.DataSource = offices;

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = dataGridView1.Rows[e.RowIndex];
            bool isActive = (bool)row.Cells["Active"].Value;
            dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = isActive ? Color.White : Color.Red;
            dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = isActive ? Color.Black : Color.White;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUsers((int)comboBox1.SelectedValue);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //When application load sleected row is none so handling that cindition
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                return;
            }
            DataGridViewRow row = dataGridView1.SelectedRows[0];
            bool isActive = (bool)row.Cells["Active"].Value;
            selectedUser = (int)row.Cells["Id"].Value;
            button1.Text = isActive ? "Disable User" : "Enable User";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User u = db.Users.Where(x => x.Id == selectedUser).FirstOrDefault();
            u.Active = !u.Active;
            db.SaveChanges();
            LoadUsers((int)comboBox1.SelectedValue);
        }
    }
}
