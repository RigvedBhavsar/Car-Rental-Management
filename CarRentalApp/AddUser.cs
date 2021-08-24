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
    public partial class AddUser : Form
    {
        private readonly CarRentalEntities1 _db;
        private ManageUsers _manageUsers;
        public AddUser(ManageUsers manageusers)
        {
            InitializeComponent();
            _db = new CarRentalEntities1();
            _manageUsers = manageusers;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            //Fetching Entire List of Roles from databses
            var roles = _db.Roles.ToList();
            cbRoles.DataSource = roles;
            cbRoles.ValueMember = "id";
            cbRoles.DisplayMember = "name";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                var userName = tbUserName.Text;
                var roleId = (int)cbRoles.SelectedValue;
                var password = Utils.DefaultHashedPassword();
                var user = new User
                {
                    username = userName,
                    password = password,
                    isActive = true
                };
                //Adding Data to User Tables
                _db.Users.Add(user);
                _db.SaveChanges();

                var userid = user.id;

                var userRole = new UserRole
                {
                    roleId = roleId,
                    userId = userid,
                };
                //Adding Data into UserRoles
                _db.UserRoles.Add(userRole);
                _db.SaveChanges();

                MessageBox.Show("New User Added Sucessfully");
                _manageUsers.PopulateGrid();
                Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error Has Occured");
            }
        }

    }
}
