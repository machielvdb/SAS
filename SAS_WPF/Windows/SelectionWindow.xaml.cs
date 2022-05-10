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
        private string savedUID;
        public SelectionWindow(string uid)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            savedUID = uid;

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

                foreach (var drink in ctx.Drinks)
                {
                    cbDrink1.Items.Add(drink);
                    cbDrink2.Items.Add(drink);
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new SAS())
            {
                var order = new Order();
                var drink1 = new DrinkOrder();
                var drink2 = new DrinkOrder();

                order.ID = Guid.NewGuid();
                drink1.ID = Guid.NewGuid();
                drink2.ID = Guid.NewGuid();

                drink1.Order = order;
                drink2.Order = order;

                bool warm = (bool)checkWarm.IsChecked;
                bool fullday = (bool)checkDay.IsChecked;

                order.WarmMeal = warm;
                order.FullDay = fullday;

                var user = ctx.Users.Where(x => x.UID == savedUID).Select(x => x);
                
                foreach (var x in user)
                {
                    order.UserID = x.ID;
                }

                ctx.Orders.Add(order);
                ctx.DrinkOrders.Add(drink1);
                ctx.DrinkOrders.Add(drink2);
                ctx.SaveChanges();
            }
        }
    }
}
