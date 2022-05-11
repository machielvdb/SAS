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
        private DateTime ScanTime;
        public SelectionWindow(string uid, DateTime scanTime)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            savedUID = uid;
            ScanTime = scanTime;

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
                    if (!drink.IsBlocked)
                    {
                        cbDrink1.Items.Add(drink);
                        cbDrink2.Items.Add(drink);
                    }
                }

                cbDrink1.DataContext = null;
                cbDrink1.DisplayMemberPath = "Name";
                cbDrink1.SelectedValuePath = "ID";
                cbDrink1.DataContext = new List<Drink>();

                cbDrink2.DataContext = null;
                cbDrink2.DisplayMemberPath = "Name";
                cbDrink2.SelectedValuePath = "ID";
                cbDrink2.DataContext = new List<Drink>();

                if (selectedUser.Admin)
                {
                    btnAdmin.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            var w = new Windows.EditItems();
            w.ShowDialog();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbDrink1.SelectedItem is null || cbDrink2.SelectedItem is null)
            {
                MessageBox.Show("Gelieve 2 drankjes te selecteren.");
            }

            else
            {
                using (var ctx = new SAS())
                {
                    var order = new Order();
                    var drink1 = new DrinkOrder();
                    var drink2 = new DrinkOrder();

                    var selectedDrink1 = cbDrink1.SelectedItem as Drink;
                    var selectedDrink2 = cbDrink2.SelectedItem as Drink;

                    drink1.DrinkID = selectedDrink1.ID;
                    drink2.DrinkID = selectedDrink2.ID;

                    order.ID = Guid.NewGuid();
                    drink1.ID = Guid.NewGuid();
                    drink2.ID = Guid.NewGuid();

                    drink1.OrderID = order.ID;
                    drink2.OrderID = order.ID;

                    order.WarmMeal = (bool)checkWarm.IsChecked;
                    order.FullDay = (bool)checkDay.IsChecked;
                    order.Time = ScanTime;

                    var users = ctx.Users.Where(x => x.UID == savedUID).Select(x => x);

                    foreach (var user in users)
                    {
                        order.UserID = user.ID;
                    }

                    ctx.Orders.Add(order);
                    ctx.DrinkOrders.Add(drink1);
                    ctx.DrinkOrders.Add(drink2);
                    ctx.SaveChanges();
                    MessageBox.Show("Bestelling doorgegeven.");
                    Close();
                }
            }
        }
    }
}
