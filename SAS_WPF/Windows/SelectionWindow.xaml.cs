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
using System.Windows.Shapes;

namespace SAS_WPF.Windows
{
    public partial class SelectionWindow : Window
    {
        public SelectionWindow(string uid)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            using (var ctx = new SAS())
            {
                User selectedUser = null;
                var selectedUsers = ctx.Users.Where(x => x.UID == uid).Select(x => x);

                foreach (var user in selectedUsers)
                {
                    selectedUser = new User
                    {
                        ID = user.ID,
                        UID = user.UID,
                        Admin = user.Admin,
                        Username = user.Username
                    };

                    tbWelcome.Text = $"Welkom {selectedUser.Username}";
                }

                if (selectedUser.Admin)
                {
                    btnAdmin.Visibility = Visibility.Visible;
                }
            }
        }

        public SelectionWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            var w = new Windows.EditItems();
            w.ShowDialog();
        }
    }
}
