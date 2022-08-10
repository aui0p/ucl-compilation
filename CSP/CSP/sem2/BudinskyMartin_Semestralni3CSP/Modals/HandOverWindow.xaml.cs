using EntityGarbageWPF.Entity;
using EntityGarbageWPF.Util;
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

namespace EntityGarbageWPF.Modals
{
    /// <summary>
    /// Interaction logic for HandOverWindow.xaml
    /// </summary>
    public partial class HandOverWindow : Window
    {
        public HandOverWindow(MainWindow w)
        {
            InitializeComponent();
            LoadCategories();
            comboBox1.IsEnabled = false;
            m = w;
        }
        private MainWindow m;
        private Accessory operatedAccessory;
        private Category operatedCategory;

        /// <summary>
        /// Komplexni metoda po stisku <see cref="accept"/>
        /// Provede vsechny validace vstupu (jestli jsou validni cisla a string vstupy)
        /// Pripadne vse ulozi do databaze a do <see cref="MainWindow.Accessories"/> a na instanci <see cref="w"/> zavola UI metody z MainWindow
        /// Zaloguje udaje o vydavanem zbozi do tabulky <see cref="Logging"/> v DB
        /// </summary>
        private void accept_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox1.SelectedItem != null && comboBox.SelectedItem != null)
            {
                operatedAccessory = (Accessory)comboBox1.SelectedItem;
                operatedCategory = (Category)comboBox.SelectedItem;

                using (var db = new AccessoriesContext())
                {
                    var original = db.Accessories.Single(c => c.AccessoryId == operatedAccessory.AccessoryId);
                    //Kontrola konzistence vstupu
                    if (Validator.isNumber(textBox.Text) && ContentChecker.stringIsNotEmpty(customerTextBox.Text) && Validator.Validate(customerTextBox.Text))
                    {
                        int previousAmount = original.Amount;
                        //Kontrola nezapornosti
                        if (Validator.isBiggerThanZero(previousAmount - Convert.ToInt32(textBox.Text)))
                        {
                            //Expedice zbozi a uprava v db + v listu v MainWindow
                            operatedAccessory.Amount = previousAmount - Convert.ToInt32(textBox.Text);
                            if (original != null)
                            {
                                Accessory acc = m.Accessories.Single(c => c.AccessoryId == original.AccessoryId);
                                if (acc != null) { acc.Amount = operatedAccessory.Amount; }
                                
                                db.Entry(original).CurrentValues.SetValues(operatedAccessory);

                                //LOgovani zbozi
                                db.Logs.Add(new Logging { Date = DateTime.Now, Customer = customerTextBox.Text, AccessoryId = operatedAccessory.AccessoryId, Amount = Convert.ToInt32(textBox.Text) });

                                db.SaveChanges();

                                m.revoke();
                                this.Close();
                            }
                        }
                    }

                }
                
            }
            else MessageBox.Show("Category & acessory must be selected!");
        }

        /// <summary>
        /// Event zmeny vyberu <see cref="comboBox"/>, zapne <see cref="comboBox1"/> a nacte do nej <see cref="List{Accessory}"/> dle vybrane <see cref="Category"/>
        /// </summary>
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox1.IsEnabled = true;
            loadAccessorties((Category)comboBox.SelectedItem);

        }

        /// <summary>
        ///  Nacte do <see cref="comboBox1"/> <see cref="List{Accessory}"/> dle vybrane <see cref="Category"/>
        /// </summary>
        private void loadAccessorties(Category category)
        {
            using (var db = new AccessoriesContext())
            {
                //var query = db.Accessories.Where(c => c.CategoryId == category.CategoryId).Where(x => x.ToBeShown == true).ToList();
                var query = db.Categories.Attach(category).Accessories.Where(x => x.ToBeShown == true).ToList();
                comboBox1.ItemsSource = query;
            }
        }

        /// <summary>
        /// Nacte vsechny kategorie z DB
        /// </summary>
        private void LoadCategories()
        {
            using (var db = new AccessoriesContext())
            {
                comboBox.ItemsSource = db.Categories.ToList();
            }
        }



        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// Event stisknuti storno tlacitka, zavre okno
        /// </summary>
        private void stornoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
