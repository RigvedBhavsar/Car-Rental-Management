using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    static class Utils
    {

        public static bool FormIsOpen(string name)
        {
            //Check is Window already open
            {
                var OpenForms = Application.OpenForms.Cast<Form>();
                var isOpen = OpenForms.Any(q => q.Name == name);
                return isOpen;
            }
        }

        public static string HashPassword(String password)
        {
            //Reference to Encrypting Algorithm
            SHA256 sha = SHA256.Create();

            //Conver the input string into a byte array and compute the hash
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            //Crrating new StringBuilder to collect the Bytes
            StringBuilder sBuilder = new StringBuilder();

            //Loop Through each byte of Hashed Data and convert it into Heaxadecimal Data
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            var hashed_password = sBuilder.ToString();

            return hashed_password;
        }

        public static string DefaultHashedPassword()
        {
            //Reference to Encrypting Algorithm
            SHA256 sha = SHA256.Create();

            //Conver the input string into a byte array and compute the hash
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes("password@123"));

            //Crrating new StringBuilder to collect the Bytes
            StringBuilder sBuilder = new StringBuilder();

            //Loop Through each byte of Hashed Data and convert it into Heaxadecimal Data
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            var hashed_password = sBuilder.ToString();

            return hashed_password;
        }
    }
}
