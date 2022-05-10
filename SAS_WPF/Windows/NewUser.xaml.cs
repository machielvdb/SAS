using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SnappyWinscard;
using Microsoft.SqlServer;

namespace SAS_WPF.Windows
{
    /// <summary>
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        public CardIo CardIo { get; set; }
        public string CardUID { get; set; }
        public NewUser()
        {
            InitializeComponent();
            CardIo = new CardIo();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!CardIo.ConnectCard())
            {
                MessageBox.Show("Geen kaart gevonden");
            }

            else
            {
                using (var ctx = new SAS())
                {
                    // Gets a list of all users on the database and checks if the UID already exists.
                    bool uidExists = false;
                    CardUID = CardIo.GetCardUID();
                    var userlist = ctx.Users.ToList();

                    foreach (var User in userlist)
                    {
                        if (CardUID == User.UID)
                        {
                            uidExists = true;
                        }
                    }

                    if (uidExists)
                    {
                        MessageBox.Show("Deze kaart is reeds gekoppeld.");
                        CardUID = string.Empty;
                    }

                    else
                    {
                        bool admin = cbAdmin.IsChecked.Value;

                        if (admin)
                        {
                            User newUser = new User();
                            var userGuid = Guid.NewGuid();
                            newUser.ID = userGuid;
                            newUser.UID = CardUID;
                            newUser.Username = tbUsername.Text;
                            newUser.Admin = admin;
                            newUser.Pincode = Int16.Parse(tbPincode.Text);
                            newUser.IsBlocked = false;

                            ctx.Users.Add(newUser);
                            ctx.SaveChanges();
                            MessageBox.Show($"Kaart gekoppeld aan {newUser.Username}");
                        }

                        else
                        {
                            User newUser = new User();
                            newUser.ID = Guid.NewGuid();
                            newUser.UID = CardUID;
                            newUser.Username = tbUsername.Text;
                            newUser.Admin = admin;
                            newUser.IsBlocked = false;

                            ctx.Users.Add(newUser);
                            ctx.SaveChanges();
                            MessageBox.Show($"Kaart gekoppeld aan {newUser.Username}");
                        }
                    }
                }
            }
        }

        private void btnAdminUnchecked(object sender, RoutedEventArgs e)
        {
            tbPincode.IsEnabled = false;
            tbPincode.Text = "Admin only";
        }

        private void btnAdminChecked(object sender, RoutedEventArgs e)
        {
            tbPincode.IsEnabled = true;
            tbPincode.Text = "Pincode";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
