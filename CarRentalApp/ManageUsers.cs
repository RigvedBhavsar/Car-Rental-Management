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
    public partial class ManageUsers : Form
    {
        private readonly CarRentalEntities1 _db;
        public ManageUsers()
        {
            InitializeComponent();
            _db = new CarRentalEntities1();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            //Only One Instace will Appear at time
            if (!Utils.FormIsOpen("AddUser"))
            {
                var addUser = new AddUser(this);
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }
        }

        private void btnResetPass_Click(object sender, EventArgs e)
        {
            try
            {
                //Getting Id of Selected Rows
                var id = (int)dgvUserList.SelectedRows[0].Cells["id"].Value;

                //Getting Details of the Selected car
                var user = _db.Users.FirstOrDefault(q => q.id == id);

                var hashed_password = Utils.DefaultHashedPassword();
                //Assignnign new Hashed Password
                user.password = hashed_password;
                //Saving Password
                _db.SaveChanges();

                MessageBox.Show($"{user.username}'s Password has Been Reset!!");
                PopulateGrid();
            }
            catch (Exception Ex)
            {
                MessageBox.Show($"ERROR:{Ex.Message}");
            }
        }

        private void btnDeactivateUser_Click(object sender, EventArgs e)
        {
            try
            {
                //Getting Id of Selected Rows
                var id = (int)dgvUserList.SelectedRows[0].Cells["id"].Value;

                //Getting Details of the Selected car
                var user = _db.Users.FirstOrDefault(q => q.id == id);
                
                //Deactivating OR Activating User Status
                user.isActive = user.isActive == true ? false : true;
              
                //Saving Password
                _db.SaveChanges();

                MessageBox.Show($"{user.username}'s Activity Status Changed!!");
                PopulateGrid();
            }
            catch (Exception Ex)
            {
                MessageBox.Show($"ERROR:{Ex.Message}");
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            var user = _db.Users.Select(q => new
            {
                q.id,
                q.username,
                q.UserRoles.FirstOrDefault().Role.name,
                q.isActive
            }).ToList();

            dgvUserList.DataSource = user;
            dgvUserList.Columns["username"].HeaderText = "UserName";
            dgvUserList.Columns["name"].HeaderText = "Role Name";
            dgvUserList.Columns["isActive"].HeaderText = "Activity";

            dgvUserList.Columns["id"].Visible = false;
        }
    }
}
