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
    public partial class AddEditRentalRecord : Form
    {
        private bool isEditMode;
        private ManageRentalRecords _manageRentalRecords;
        //Reference to the Entire Databse Model we have inserted through ADO.Net
        private readonly CarRentalEntities1 _db;
        public AddEditRentalRecord(ManageRentalRecords manageRentalRecords = null)
        {
            InitializeComponent();
            lblTitle.Text = "Add Rental Record";
            this.Text = "Add Rental Record";
            _manageRentalRecords = manageRentalRecords;
            isEditMode = false;
            //Initializing the Object
            _db = new CarRentalEntities1();
        }
        public AddEditRentalRecord(CarRentalRecord recordToEdit , ManageRentalRecords manageRentalRecords = null)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Rental Record";
            this.Text = "Edit Rental Record";
            _manageRentalRecords = manageRentalRecords;
            if (recordToEdit == null)
            {
                MessageBox.Show("Please Ensure That you have Selected Valid Record");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities1();
                //Passing Object to The Car
                PopulateFields(recordToEdit);
            }
        }

        private void PopulateFields(CarRentalRecord recordToEdit)
        {
            tbCustName.Text = recordToEdit.CustomerName;
            dtRentedfrom.Value = (DateTime)recordToEdit.DateRented;
            dtRentedto.Value = (DateTime)recordToEdit.DateReturned;
            tbCost.Text = recordToEdit.Cost.ToString();
            lblRecordId.Text = recordToEdit.CustomerId.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Local Variables
                string customerName = tbCustName.Text;
                var dateFrom = dtRentedfrom.Value;
                var dateTo = dtRentedto.Value;
                var carType = cbtype.Text;
                double cost = Convert.ToDouble(tbCost.Text);
                var isValid = true;
                var errorMessage = "";

                //Check if Name is Empty or Not
                if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(carType))
                {
                    isValid = false;
                    errorMessage += "Error : Please Fill All Fields \n\r";
                }

                //Date Filter
                if (dateFrom > dateTo)
                {
                    isValid = false;
                    errorMessage += "Error : Please Select Right Date";
                }

                if (isValid)
                {
                    var rentalRecord = new CarRentalRecord();
                    if (isEditMode)
                    {
                        //If in Edit Mode Get the id , Retrive data from databse and place it in record object
                        //Updating Record
                        var id = int.Parse(lblRecordId.Text);
                        rentalRecord = _db.CarRentalRecords.FirstOrDefault(q => q.CustomerId == id);
                    }
                    //Populate Record Object with Values from the form
                    rentalRecord.CustomerName = customerName;
                    rentalRecord.DateRented = dateFrom;
                    rentalRecord.DateReturned = dateTo;
                    rentalRecord.Cost = (decimal)cost;
                    rentalRecord.TypeofCarId = (int)cbtype.SelectedValue;

                    //If Not in Edit Mode, Then Add Record Object in Databse
                    //Inserting Record
                    if (!isEditMode)
                    {
                        _db.CarRentalRecords.Add(rentalRecord);
                    }
                    //Commiting Changes
                    _db.SaveChanges();
                    _manageRentalRecords.populateGrid();

                    MessageBox.Show($"Customer Name :{customerName}\n\r" +
                        $"Rented From :{dateFrom}\n\r" +
                        $"Rented To :{dateTo}\n\r" +
                        $"Car :{carType}\n\r" +
                        $"Cost :{cost}\n\r");

                    Close();
                } 
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Getting the tyoes of Cars from the Databse
            //Selct * from TypesOfCars
            //var cars = carRentalEntities.TypesOfCars.ToList();

            //Select ()
            var cars = _db.TypesOfCars.Select(
                q => new
                {
                    Id = q.VehicleId,
                    Name = q.VehicleBrand
                    +" " + q.VehicleModel
                }).ToList();
            
            //Initializing members of Combo box 
            cbtype.DisplayMember = "Name";       
            cbtype.ValueMember = "Id";
            //Where VehicleId & VehicleName are the Column Name of the Table
            
            //Initializing combobox 
            cbtype.DataSource = cars ;

        }
    }
}
