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
    public partial class AddEditVehicle : Form
    {
        private bool isEditMode;
        private ManageVehicleListing _manageVehicleListing;
        //Creating the Reference of the Database 
        private readonly CarRentalEntities1 _db;
        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Add Vehicle";
            this.Text = "Add New Vehicle";
            isEditMode = false;
            _manageVehicleListing = manageVehicleListing;
            _db = new CarRentalEntities1();
        }

        //Parametrize Contructor
        public AddEditVehicle(TypesOfCar carToEdit , ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Vehicle";
            this.Text = "Update Vehicle";
            _manageVehicleListing = manageVehicleListing;

            if (carToEdit ==null)
            {
                MessageBox.Show("Please Ensure That you have Selected Valid Record");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities1();
                //Passing Object to The Car
                PopulateFields(carToEdit);
            }
            
        }
       
        private void PopulateFields(TypesOfCar car)
        {
            lblId.Text = car.VehicleId.ToString();
            tbBrand.Text = car.VehicleBrand;
            tbModel.Text = car.VehicleModel;
            tbNumber.Text = car.VehicleNumber;
            tbVin.Text = car.VehicleVIN;
            tbYear.Text = car.VehicleYear.ToString();
        }
        
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (isEditMode)
                {
                    try
                    {
                        var id = int.Parse(lblId.Text);
                        //Getting Car Details of the selected id 
                        //WheenEver we want a single value from the table use FirstorDefault()
                        var car = _db.TypesOfCars.FirstOrDefault(q => q.VehicleId == id);
                        //Updating the Changes
                        car.VehicleBrand = tbBrand.Text;
                        car.VehicleModel = tbModel.Text;
                        car.VehicleVIN = tbVin.Text;
                        car.VehicleYear = int.Parse(tbYear.Text);
                        car.VehicleNumber = tbNumber.Text;
                        //Saving Changes
                        _db.SaveChanges();
                        _manageVehicleListing.populateGrid();
                        MessageBox.Show("Record Updated");
                        this.Close();
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show($"ERROR:{Ex.Message}");
                    }
                }
                else
                {
                    try
                    {
                        //Creating New Model of TypesOfCar Table With Direct Initilizing the values
                        var newCar = new TypesOfCar
                        {
                            VehicleBrand = tbBrand.Text,
                            VehicleModel = tbModel.Text,
                            VehicleVIN = tbVin.Text,
                            VehicleYear = int.Parse(tbYear.Text),
                            VehicleNumber = tbNumber.Text
                        };

                        //Adding Data into Table
                        _db.TypesOfCars.Add(newCar);
                        //Saving Changes
                        _db.SaveChanges();
                        _manageVehicleListing.populateGrid();
                        MessageBox.Show("Record Inserted");
                        this.Close();
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show($"ERROR:{Ex.Message}");
                    }
                    
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show($"ERROR:{Ex.Message}");
            }
        }

        private void btnCancle_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
