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

namespace CarRentalApp
{
    public partial class Login : Form
    {
        //Database Instance
        private readonly CarRentalEntities1 _db;
        public Login()
        {
            InitializeComponent();
            //Creating Object of the Database
            _db = new CarRentalEntities1();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Checking the input value with the Database Value
            try
            {
                //Reference to Encrypting Algorithm
                SHA256 sha = SHA256.Create(); 

                var userName = tbUserName.Text.Trim();
                var passWord = tbPassword.Text;


                var hashed_password = Utils.HashPassword(passWord);

                //Check in Database formatching Username , Password , and isActive 
                var user = _db.Users.FirstOrDefault(q => q.username == userName && q.password == hashed_password
                                && q.isActive == true);
                //If User Not Found
                if (user == null)
                {
                    MessageBox.Show("ERROR : Plase Provide Valid Credentials");
                }
                else
                {
                    //var role = user.UserRoles.FirstOrDefault();
                    //var roleName = role.Role.shortName;
                    //we Pass this to the Mainwindow and calling close method of this form through the MainWindow
                    //Whenever its closing
                    var mainWindow = new MainWindow(this , user);
                    mainWindow.Show();
                    Hide();
                }
            }
            catch (Exception Ex)
            { 
                MessageBox.Show("ERROR : Something Went Wrong");
            }
        }
    }
}
