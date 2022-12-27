using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace CaroGame
{
    /// <summary>
    /// Interaction logic for Manager.xaml
    /// </summary>
    public partial class Manager : Window
    {
        public Manager()
        {
            InitializeComponent();
            LoadUserData();
        }



        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-MB08BDR;Initial Catalog=CaroGame;Integrated Security=True");
        private void LoadUserData()
        {


            SqlCommand cmd = new SqlCommand("Select trim(FirstName), trim(LastName), trim(Email), trim(Password), trim(Address)  From UserName;", conn);
            DataTable dt = new DataTable();
            conn.Open();
            SqlDataAdapter sdr = new SqlDataAdapter(cmd);
            sdr.Fill(dt);
            dt.Columns[0].ColumnName = "Firs tName";
            dt.Columns[1].ColumnName = "Last Name";
            dt.Columns[2].ColumnName = "Email";
            dt.Columns[3].ColumnName = "Password";
            dt.Columns[4].ColumnName = "Address";
            UserDataGrid.ItemsSource = dt.DefaultView;
            conn.Close();

        }

        private void UserDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            {
                DataGrid grid = sender as DataGrid;

                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataRowView rowView = grid.SelectedItem as DataRowView;
                    if (rowView != null)
                    {
                        DataRow row = rowView.Row;
                        textBoxFirstName.Text = string.Format(row.ItemArray[0].ToString());
                        textBoxLastName.Text = string.Format(row.ItemArray[1].ToString());
                        textBoxEmail.Text = string.Format(row.ItemArray[2].ToString());
                        passwordBox1.Text = string.Format(row.ItemArray[3].ToString());
                        textBoxAddress.Text = string.Format(row.ItemArray[4].ToString());
                    }
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            errormessage.Text = "";
            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("Delete from UserName where Email = '" + textBoxEmail.Text + "';", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                errormessage.Text = "Delete is succesfully";
                errormessage.Foreground = Brushes.Green;
                LoadUserData();
            }
            catch
            {
                errormessage.Text = "You have Deleted is Unsuccesfully";
                errormessage.Foreground = Brushes.OrangeRed;
                conn.Close();
            }



        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxEmail.Text.Length == 0)
            {
                errormessage.Text = "Enter an email.";
                errormessage.Foreground = Brushes.OrangeRed;
                textBoxEmail.Focus();
            }
            else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errormessage.Text = "Enter a valid email.";
                errormessage.Foreground = Brushes.OrangeRed;
                textBoxEmail.Select(0, textBoxEmail.Text.Length);
                textBoxEmail.Focus();
            }
            else
            {
                string firstname = textBoxFirstName.Text;
                string lastname = textBoxLastName.Text;
                string email = textBoxEmail.Text;
                string password = passwordBox1.Text;
                string address = textBoxAddress.Text;

                if (passwordBox1.Text.Length == 0)
                {
                    errormessage.Text = "Enter password.";
                    errormessage.Foreground = Brushes.OrangeRed;
                    passwordBox1.Focus();
                }
                else
                {
                    errormessage.Text = "";
                    
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Insert into UserName (FirstName,LastName,Email,Password,Address) values('" + firstname + "','" + lastname + "','" + email + "','" + password + "','" + address + "')", conn);
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        errormessage.Text = "You have Registered successfully.";
                        errormessage.Foreground = Brushes.Green;
                        LoadUserData();
                    }
                    catch
                    {
                        errormessage.Text = "You have Registered Unsuccessfully.";
                        errormessage.Foreground = Brushes.OrangeRed;
                        conn.Close();

                    }

                }
            }
        }


        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string firstname = textBoxFirstName.Text;
            string lastname = textBoxLastName.Text;
            string email = textBoxEmail.Text;
            string password = passwordBox1.Text;
            string address = textBoxAddress.Text;

            try
            {

                SqlCommand cmd = new SqlCommand("Update UserName set FirstName='" + firstname + "', LastName='" + lastname + "', Password='" + password + "',Address='" + address + "' where Email = '" + email + "';", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                errormessage.Text = "You have Updated successfully";
                errormessage.Foreground = Brushes.Green;

                LoadUserData();
            }
            catch
            {
                errormessage.Text = "You have Updated Unsuccessfully";
                errormessage.Foreground = Brushes.OrangeRed;

                conn.Close();
            }
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            Close();
            login.Show();
        }
    }
}
