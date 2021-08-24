using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ResetPassword : Form
    {
        private readonly CarRentalEntities1 _db;
        private User _user;
        public ResetPassword(User user)
        {
            InitializeComponent();
            _db = new CarRentalEntities1();
            _user = user;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                var password = tbpassword.Text;
                var confirm_password = tbConfirmPassword.Text;
                var user = _db.Users.FirstOrDefault(q => q.id == _user.id);
                if (password != confirm_password)
                {
                    MessageBox.Show("Password Do Not Match. Please Try Again");
                }
                user.password = Utils.HashPassword(password);
                _db.SaveChanges();
                MessageBox.Show("Password Changed Successfully");
                Close();
            }
            catch (Exception)
            {
                    MessageBox.Show("Error : please Try Again ");
            }
        }
    }
}
