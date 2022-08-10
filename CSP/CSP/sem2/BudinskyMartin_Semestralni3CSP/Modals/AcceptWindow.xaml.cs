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
using EntityGarbageWPF.Entity;
using EntityGarbageWPF.Util;

namespace EntityGarbageWPF
{
    /// <summary>
    /// Interaction logic for AcceptWindow.xaml
    /// </summary>
    public partial class AcceptWindow : Window
    {
        public AcceptWindow(MainWindow w)
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
                    if (Validator.isNumber(textBox.Text))
                    {
                        int previousAmount = original.Amount;
                        //Kontrola mista na sklade
                        if (Validator.isNotOverloaded(Convert.ToInt32(textBox.Text) + previousAmount))
                        {
                            //Prijeti zbozi a ulozeni do db + do listu v MainWindow
                            operatedAccessory.Amount = Convert.ToInt32(textBox.Text) + previousAmount;
                            if (original != null)
                            {
                                Accessory acc = m.Accessories.Single(c => c.AccessoryId == original.AccessoryId);
                                if (acc != null) { acc.Amount = operatedAccessory.Amount; }


                                db.Entry(original).CurrentValues.SetValues(operatedAccessory);
                                db.SaveChanges();

                                m.revoke();
                                this.Close();
                            }
                        }
                    }

                }
            }
            else MessageBox.Show("Category & acessorty must be selected!");
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
