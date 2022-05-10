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
    /// <summary>
    /// Interaction logic for EditItems.xaml
    /// </summary>
    public partial class EditItems : Window
    {
        public EditItems()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Fetches all unique food and drink items in the database and displays them in their respective listboxes.
            using (var ctx = new SAS())
            {
                var drinks = ctx.Drinks.Select(Name => Name).Distinct();

                foreach (var x in drinks)
                {
                    lbDrinks.Items.Add(x.Name.ToString());
                }      
            }
        }

        private void btnAddDrink_Click(object sender, RoutedEventArgs e)
        {
            // Opens the AddItem window with a true parameter to specify it's a drink.
            var w = new AddItem(true);
            w.Show();
        }

        private void btnDeleteSelection_Click(object sender, RoutedEventArgs e)
        {
            var selectedDrink = lbDrinks.SelectedItem as Drink;

            using (var ctx = new SAS())
            {
                foreach (var drink in ctx.Drinks)
                {
                    if (drink.ID == selectedDrink.ID)
                    {
                        ctx.Drinks.Remove(drink);
                    }
                }
            }
        }
    }
}
