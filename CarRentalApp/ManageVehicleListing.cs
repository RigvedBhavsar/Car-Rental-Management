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
    public partial class ManageVehicleListing : Form
    {
        //Creating the Reference of the Database 
        private readonly CarRentalEntities1 _db;
        public ManageVehicleListing()
        {
            InitializeComponent();
            //Creating the Object of the Database
            _db = new CarRentalEntities1();
        }
        private void ManageVehicleListing_Load(object sender, EventArgs e)
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
            //Getting Vehicle List
            //Select (VehicleId as CarID , VehicleBrand as CarBrand) from TypesOfCars
            var cars = _db.TypesOfCars.Select
                    (q => new
                    {
                        CarBrand = q.VehicleBrand,
                        CarModel = q.VehicleModel,
                        CarVin = q.VehicleVIN,
                        CarYear = q.VehicleYear,
                        CarNumber = q.VehicleNumber,
                        Id = q.VehicleId
                    }).ToList();

            //Assignning Vehicles to the Data Grid View
            dgvVehicleList.DataSource = cars;

            //Seeting Column Name
            dgvVehicleList.Columns[0].HeaderText = "BRAND";
            dgvVehicleList.Columns[1].HeaderText = "MODEL";
            dgvVehicleList.Columns[2].HeaderText = "VIN";
            dgvVehicleList.Columns[3].HeaderText = "YEAR";
            dgvVehicleList.Columns[4].HeaderText = "NUMBER";
            //Hiding Visibility
            dgvVehicleList.Columns[5].Visible = false;
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            //Creating Default Object of AddEditVehicle
            var addEditVehicle = new AddEditVehicle(this);
            //Assigning parent of this form to child form
            addEditVehicle.MdiParent = this.MdiParent;
            //Displaying Form
            addEditVehicle.Show();
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                //Getting Id of Selected Rows
                var id = (int)dgvVehicleList.SelectedRows[0].Cells["Id"].Value;

                //Getting Details of the Selected car
                var car = _db.TypesOfCars.FirstOrDefault(q => q.VehicleId == id);

                //Creating Parametrized Object of AddEditVehicle
                var addEditVehicle = new AddEditVehicle(car , this);
                //Assigning parent of this form to child form
                addEditVehicle.MdiParent = this.MdiParent;
                //Displaying Form
                addEditVehicle.Show();
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
                var id = (int)dgvVehicleList.SelectedRows[0].Cells["Id"].Value;

                //Getting Details of the Selected car
                var car = _db.TypesOfCars.FirstOrDefault(q => q.VehicleId == id);

                //Displaying Cutom MessageBox
                DialogResult dr = MessageBox.Show("Are you Sure Want to Delete Record ? ",
                    "Delete", MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    //Deleting Selected Vehicle
                    _db.TypesOfCars.Remove(car);
                    _db.SaveChanges();
                }
                populateGrid();
            }
            catch (Exception Ex)
            { 
                MessageBox.Show($"ERROR:{Ex.Message}");
            }
           
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            populateGrid();
        }
    }
}


/*
 * While Deleting the Record From Databse if you find some error about deleting 
 * goto the table Design -> select primary key column -> relationships -> Insert AND Update Specification ->
 *  Change in Updte Rule and Delete Rule (its like On Delete Cascade On Update cascade)
 */