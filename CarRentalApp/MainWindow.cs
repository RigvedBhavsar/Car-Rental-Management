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
    public partial class MainWindow : Form
    {
        private Login _login;
        public String _roleName;
        public User _user;
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(Login login ,User user)
        {
            InitializeComponent();
            _login = login;
            _user = user;
            _roleName = user.UserRoles.FirstOrDefault().Role.shortName;
        }

        private void addRentalRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Using ShowDialog() and mdiparent propert in this "Sequence" we can active only one window at a time
            //Creating Object of Form
            ManageRentalRecords rec = new ManageRentalRecords();
            var addRentalRecord = new AddEditRentalRecord(rec);
            //Displaying the Form
            addRentalRecord.ShowDialog();
            //Assignt Property so that form stays inser main Window(Parent)
            addRentalRecord.MdiParent = this;
        }

        private void manageVehicleListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Another way of activating only one window at a time
            //Creating the Object of Form Class
            var openForms = Application.OpenForms.Cast<Form>();
            //Checking id any form is Open
            var isOpen = openForms.Any(q => q.Name == "ManageVehicleListing");

            if (!isOpen)
            {
                //Creating Object of the Form
                //Here We Need to Add Populate Method ERROR
                var vehicleListing = new ManageVehicleListing();
                //Assignt Property so that form stays inser main Window(Parent)
                vehicleListing.MdiParent = this;
                //Displaying the Form
                vehicleListing.Show();
            }
        }

        private void viewArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //We have Declare one method in Utils File that helps us to activate only one window at a time
            if (!Utils.FormIsOpen("ManageRentalRecords"))
            {
                //Creating Object of the Form
                var manageRentalRecords = new ManageRentalRecords();
                //Assignt Property so that form stays inser main Window(Parent)
                manageRentalRecords.MdiParent = this;
                //Displaying the Form
                manageRentalRecords.Show();
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //We Are Calling Close Method of Login Form WhenEver MinWindow is Closing
            _login.Close();
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            var manageUsers = new ManageUsers();
            manageUsers.ShowDialog();
            manageUsers.MdiParent = this;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (_user.password == Utils.DefaultHashedPassword())
            {
                var resetPassword = new ResetPassword(_user);
                resetPassword.ShowDialog();
            }

            var userName = _user.username;
            toolStatusStrip.Text = $"Logged In As: {userName}";
            if (_roleName != "admin")
            {
                manageUsersToolStripMenuItem.Enabled = false;
            }
        }
    }
}

/*
-------------------------------------------------------------------------------------------------------------------
                                    HOW TO ADD DATABSE TO OUR APPLICATION USING ADO.Net
-------------------------------------------------------------------------------------------------------------------
    1.Go To the Solution Explorer and Right click on Your Project and select Option Add New Item and slect 
        Choose ADO.net Entity Data Model Give the Name

    2. Selct EF Designer Model (default Selected) click Next

    3. chose  Add Connction Select Data Source as Microsoft SQL Server(Sql CLient)
        Add Server Name (Pc-Name usually) 
        Select DataBase Name
        Test Connection 
        Click OK
        Click Next and then Chose Entity Framework

    4. It Will ask For Which object you Want to import from databse to our project 
       Selct Option Table and Click Finish.

    5. An Entity Diagram will Open in .edmx file

-------------------------------------------------------------------------------------------------------------------
                                    HOW TO UPDATE DATABSE .edmx FILE
-------------------------------------------------------------------------------------------------------------------

    WhenEver You Create Any Changes in Database like (add Fields to Table , Edit Column Name) our .edmx file
    wont get Reflected the changes.
    
    For Relecting that Changes into Our .edmx file follow -
        1.Right Click on Empty Space And click Update Model from Databse
        2.Will Get Similar Menu Go to Refrsh Tab and Select Tables menu and click Finish.

        SomeTimes it Wont Work Expectedly in that case we have to Select all models from our .edmx file and Delete it.
        Now Follow Similar Process as Above and Instead of Refresh select Add Tab now.

-------------------------------------------------------------------------------------------------------------------
                                    HOW TO ADD PASSWORD ENCRYPTIN TO THE APPLICATION
-------------------------------------------------------------------------------------------------------------------
            We have Add The 3rd Party laibtraris for it .   
    
       1. Right Click on Project -> Manage Nuget Packages ->Browse ->(You will explore packages List)
       2. Search for Security -> select "System.Security.Cryptography.Algorithms".
       3. Install It.
       4. Verify are references are updated ? 
 */