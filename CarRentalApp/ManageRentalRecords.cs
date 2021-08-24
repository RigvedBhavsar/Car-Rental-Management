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
    public partial class ManageRentalRecords : Form
    {
        //Creating the Reference of the Database 
        private readonly CarRentalEntities1 _db;
        public ManageRentalRecords()
        {
            InitializeComponent();
            _db = new CarRentalEntities1();
        }
        private void ManageRentalRecords_Load(object sender, EventArgs e)
        {
            try
            {
                populateGrid();
            }
            catch (Exception Ex)
            {
                MessageBox.Show($"Error : {Ex.Message}");
            }
        }

        public void populateGrid()
        {
            var records = _db.CarRentalRecords.Select(q => new
            {
                Customer = q.CustomerName,
                DateOut = q.DateRented,
                DateIn = q.DateReturned,
                Id = q.CustomerId,
                q.Cost ,      //If our name and coloum name are same we can condense it
                Car = q.TypesOfCar.VehicleBrand + " " + q.TypesOfCar.VehicleModel   // Behave Like Innner Join
            }).ToList() ;

            dgvRecordList.DataSource = records;

            //Seeting Column Name
            dgvRecordList.Columns["Customer"].HeaderText = "NAME";
            dgvRecordList.Columns["DateOut"].HeaderText = "RENTED";
            dgvRecordList.Columns["DateIn"].HeaderText = "RETURNED";
            dgvRecordList.Columns["Cost"].HeaderText = "COST";
            dgvRecordList.Columns["Car"].HeaderText = "CAR";

            //Hiding Visibility
            dgvRecordList.Columns["Id"].Visible = false;

        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            populateGrid();
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            //Creating Default Object of AddEditVehicle
            var addRentalRecord = new AddEditRentalRecord(this);
            //Assigning parent of this form to child form
            addRentalRecord.MdiParent = this.MdiParent;
            //Displaying Form
            addRentalRecord.Show();
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                //Getting Id of Selected Rows
                var id = (int)dgvRecordList.SelectedRows[0].Cells["Id"].Value;

                //Getting Details of the Selected car
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.CustomerId == id);

                //Creating Parametrized Object of AddEditVehicle
                var addEditRentalRecord = new AddEditRentalRecord(record , this);
                //Assigning parent of this form to child form
                addEditRentalRecord.MdiParent = this.MdiParent;
                //Displaying Form
                addEditRentalRecord .Show();
            }
            catch (Exception Ex)
            {
                MessageBox.Show($"ERROR:{Ex.Message}");
            }
        }

        private void btnDelCar_Click(object sender, EventArgs e)
        {
            try
            {
                //Getting Id of Selected Rows
                var id = (int)dgvRecordList.SelectedRows[0].Cells["Id"].Value;

                //Getting Details of the Selected car
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.CustomerId == id);

                //Displaying Cutom MessageBox
                DialogResult dr = MessageBox.Show("Are you Sure Want to Delete Record ? ",
                    "Delete", MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    //Deleting Selected Vehicle
                    _db.CarRentalRecords.Remove(record);
                    _db.SaveChanges();
                }
                populateGrid();
            }
            catch (Exception Ex)
            {
                MessageBox.Show($"ERROR:{Ex.Message}");
            }
        }

        
    }
}
