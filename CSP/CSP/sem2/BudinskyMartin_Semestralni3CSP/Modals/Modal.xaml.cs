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

namespace EntityGarbageWPF
{
    /// <summary>
    /// Interaction logic for Modal.xaml
    /// </summary>
    public partial class Modal : Window
    {
        public Modal()
        {
            InitializeComponent();
            LoadCategories();
        }

        /// <summary>
        /// Pretizeni konstruktoru pro vytvoreni noveho zbozi
        /// </summary>
        public Modal(MainWindow m, bool isEdited)
        {
            InitializeComponent();
            LoadCategories();
            w = m;
        }

        /// <summary>
        /// Pretizeni konstruktoru pro editaci zbozi
        /// </summary>
        public Modal(Accessory AccessoryIndex, MainWindow m, bool isEdited)
        {
            InitializeComponent();

            ShowAccessoryDetails(AccessoryIndex);
            operatedAccessory = AccessoryIndex;
            w = m;
            this.isEdited = isEdited;
            
        }
        private MainWindow w;
        private bool isEdited;
        private Accessory operatedAccessory;
        /// <summary>
        /// Metoda nacitajici <see cref="List{Category}"/> z DB do <see cref="categoriesComboBox"/>
        /// </summary>
        private void LoadCategories()
        {
            using (var db = new AccessoriesContext())
            {
                categoriesComboBox.ItemsSource = db.Categories.ToList();
            }
        }

        /// <summary>
        /// Komplexni metoda po stisku <see cref="okButton"/>
        /// Nejprve urci, zda-li se jedna o edit, ci vytvoreni noveho zbozi
        /// Pote provede vsechny validace vstupu (jestli jsou validni cisla a string vstupy)
        /// Pripadne vse ulozi do databaze a do <see cref="MainWindow.Accessories"/> a na instanci <see cref="w"/> zavola UI metody z MainWindow
        /// </summary>
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if ((Category)categoriesComboBox.SelectedItem != null)
            {
                var parentCategory = (Category)categoriesComboBox.SelectedItem;
                using (var db = new AccessoriesContext())
                {
                    var cat = db.Categories.Single(c => c.CategoryId == parentCategory.CategoryId);
                    if (isEdited)
                    {
                        //Konzistence vstupu
                        if (Validator.isNumber(textBox1.Text) && ContentChecker.stringIsNotEmpty(textBox.Text) && categoriesComboBox.SelectedItem != null)
                        {
                            operatedAccessory.Name = textBox.Text;
                            operatedAccessory.Category = (Category)categoriesComboBox.SelectedItem;
                            operatedAccessory.MinAmount = Convert.ToInt32(textBox1.Text);
                            var original = db.Accessories.Single(c => c.AccessoryId == operatedAccessory.AccessoryId);

                            if (original != null)
                            {
                                //Zmena accessory v listu z mainwindow
                                Accessory acc = w.Accessories.Single(c => c.AccessoryId == original.AccessoryId);
                                if (acc != null) { acc.MinAmount = operatedAccessory.MinAmount; acc.Category = operatedAccessory.Category; acc.Name = operatedAccessory.Name; }
                                //Validace accessory
                                if (Validator.Validate(acc))
                                {
                                    db.Entry(original).CurrentValues.SetValues(operatedAccessory);
                                    db.SaveChanges();

                                    w.revoke();
                                    this.Close();
                                }
                            }
                        }
                    }

                    else //Konzistence vstupu
                        if (Validator.isNumber(textBox1.Text) && ContentChecker.stringIsNotEmpty(textBox.Text) && categoriesComboBox.SelectedItem != null)
                        {
                            var acc = new Accessory { Name = textBox.Text, CategoryId = cat.CategoryId, Amount = 0, MinAmount = Convert.ToInt32(textBox1.Text), ToBeShown = true };
                        //Validace accessory
                        if (Validator.Validate(acc))
                            {
                                w.Accessories.Add(acc);

                                db.Accessories.Add(acc);

                                cat.Accessories.Add(acc);
                                db.SaveChanges();
                                w.revoke();
                                this.Close();
                            }
                        }
                }
            }
            else MessageBox.Show("Category must be selected!");
        }

        /// <summary>
        /// Metoda vypisujici parametry <param name="a"></param> do UI prvku v pripade editace <see cref="Accessory"/>
        /// </summary>
        private void ShowAccessoryDetails(Accessory a)
        {
            if (a !=null )
            {
                using (var db = new AccessoriesContext())
                 {
                    var cat = db.Categories.Single(c => c.CategoryId == a.CategoryId);
                    var catall = db.Categories.ToList();
                    categoriesComboBox.ItemsSource = catall;
                    categoriesComboBox.SelectedItem = cat;
                    textBox.Text = a.Name;
                    textBox1.Text = a.MinAmount.ToString();
                }
            }
        }

        

        private void categoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// Storno metoda, uzavre okno a zahodi zmeny
        /// </summary>
        private void stornoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
