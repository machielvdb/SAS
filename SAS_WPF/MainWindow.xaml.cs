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

namespace SAS_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CardIo CardIo { get; set; }
        public bool connActive = false;
        private readonly object lock1 = new object();

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CardIo = new CardIo();
            this.DataContext = CardIo;
            BindingOperations.EnableCollectionSynchronization(CardIo.Devices, lock1);
            CardIo.ReaderStateChanged += CardIo_ReaderStateChanged;
            textBlockReaderState.Text = "Scan je kaart.";
        }

        private void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            var w = new Windows.NewUser();
            w.ShowDialog();
        }

        private void CardIo_ReaderStateChanged(CardIo.ReaderState readerState)
        {
            // Triggers when a card is detected/removed and updates the textblock text.
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => CardIo_ReaderStateChanged(readerState));
                return;
            }

            // When a card is detected, fetch the UID from the card and pass it along to the next window.
            if (readerState == CardIo.ReaderState.CardReady)
            {
                string uid = CardIo.GetCardUID();
                if (this.IsActive)
                {
                    var User = CheckCardForUniqueUser(uid);
                    if (User)
                    {
                        var w = new Windows.SelectionWindow(uid);
                        w.ShowDialog();
                    }
                }
            }
        }

        private bool CheckCardForUniqueUser(string uid)
        {
            // Fetch the users matching this UID, throw an error if there is none or multiple
            using (var ctx = new SAS())
            {   
                User selectedUser = null;
                var cardUsers = ctx.Users.Where(x => x.UID == uid).Select(x => x).ToList();

                if (cardUsers.Count > 1)
                {
                    MessageBox.Show("Meerdere gebruikers koppelt aan kaart!");
                    return false;
                }

                else if (cardUsers.Count == 0)
                {
                    MessageBox.Show("Kaart niet gekend.");
                    return false;
                }

                else
                {
                    foreach (var user in cardUsers)
                    {
                        selectedUser = new User
                        {
                            ID = user.ID,
                            UID = user.UID,
                            Admin = user.Admin,
                            Username = user.Username,
                            Pincode = user.Pincode,
                            Orders = user.Orders
                        };
                    }
                    return true;
                }
            }
        }
    }
}