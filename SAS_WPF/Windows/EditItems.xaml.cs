using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

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
            RefreshList();
        }

        private void btnAddDrink_Click(object sender, RoutedEventArgs e)
        {
            // Opens the AddItem window with a true parameter to specify it's a drink.
            var w = new AddItem();
            w.Show();
            RefreshList();
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
                ctx.SaveChanges();
                RefreshList();
            }
        }

        private void RefreshList()
        {
            lbDrinks.Items.Clear();
            lbDrinks.DataContext = null;

            using (var ctx = new SAS())
            {
                var drinks = ctx.Drinks.Select(x => x).Distinct();

                foreach (var x in drinks)
                {
                    lbDrinks.Items.Add(x);
                }
            }

            lbDrinks.DisplayMemberPath = "Name";
            lbDrinks.SelectedValuePath = "ID";

            lbDrinks.DataContext = new List<Drink>();
        }

        private void Grid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RefreshList();
        }
    }
}
