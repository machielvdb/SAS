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
    public partial class AddItem : Window
    {
        bool IsDrink = false;
        public AddItem(bool isDrink)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            IsDrink = isDrink;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string itemName = tbName.Text;
            using (var ctx = new SAS())
            {
                var drinkQuery = ctx.Drinks.Where(x => x.Name == itemName).Select(x => x).ToList();

                if (drinkQuery.Count >= 1)
                {
                    MessageBox.Show("Item bestaat reeds.");
                    return;
                }

                else
                {
                    var newDrink = new Drink
                    {
                        ID = Guid.NewGuid(),
                        Name = tbName.Text
                    };

                    ctx.Drinks.Add(newDrink);
                    ctx.SaveChanges();
                    MessageBox.Show("Item opgeslagen.");
                    this.Close();
                }
            }
        }
    }
}