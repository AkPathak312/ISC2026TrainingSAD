using ISCTraining.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISCTraining
{
    public partial class AddUser : Form
    {
        AmioncDbContext db;
        int userId;
        public AddUser(int userId)
        {
            InitializeComponent();
            db = new AmioncDbContext();
            this.userId = userId;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            List<Office> offices = db.Offices.ToList();
            comboBox1.DisplayMember = "Title";
            comboBox1.ValueMember = "Id";
            comboBox1.DataSource = offices;

            //Setting min date for picker
            dateTimePicker1.MaxDate = DateTime.Now - new TimeSpan(365, 0, 0, 0);

            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            MessageBox.Show(userId.ToString());
            if(userId != 0)
            {
                PopulateData(userId);
            }

        }

        private void PopulateData(int userId)
        {
           User u = db.Users.Where(x=> x.Id == userId).FirstOrDefault();
            textBox1.Text = u.Email;
            textBox2.Text = u.FirstName;
            textBox3.Text  = u.LastName;
            textBox4.Text = u.Password;
            comboBox1.SelectedItem = u.Office;
            DateOnly birthDate = (DateOnly)u.Birthdate;
            DateTime birthdateactual = new DateTime(birthDate.Year, birthDate.Month, birthDate.Day);
            dateTimePicker1.Value = birthdateactual;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in this.Controls)
            {
                if(ctrl is TextBox txt)
                {
                    if(txt.Text.IsNullOrEmpty())
                    {
                        errorProvider1.SetError(txt, "This field is Required");
                    }
                }
            }

            if (!errorProvider1.HasErrors)
            {
                
                int insertedUserId = db.Users.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                User u;
                if(userId == 0)
                {
                    u = new User();
                    u.Id = insertedUserId + 1;
                    u.Active = true;
                    u.RoleId = 2;
                }
                else
                {
                    u = db.Users.Where(x => x.Id == userId).FirstOrDefault();
                }
                u.Email = textBox1.Text;
                u.FirstName = textBox2.Text;
                u.LastName = textBox3.Text;
                u.Password = GlobalUtil.HashPassword(textBox4.Text);
                u.OfficeId = (int)comboBox1.SelectedValue;
                u.Birthdate = DateOnly.FromDateTime(dateTimePicker1.Value);
                if(userId == 0)
                {
                    db.Users.Add(u);
                    db.SaveChanges();
                    MessageBox.Show("User Created Successfully !");

                }
                else
                {
                    db.SaveChanges();
                    MessageBox.Show("User Updated Successfully !");
                }

                this.Hide();
                Dashboard form = new Dashboard(GlobalUtil.email);
                form.Show();
            }

        }

        
    }
}
