using EntityGarbageWPF.Entity;
using EntityGarbageWPF.Modals;
using EntityGarbageWPF.Util;
using System;
using System.Collections.Generic;
using System.Data;
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


namespace EntityGarbageWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadAccessories();
            
        }
        public List<Accessory> Accessories = new List<Accessory>();


        //testovaci metodka pro zjednoduseni pridavani kategorii do DB po ruznych dropech a upravach...
        //private void InsertCategories()
        //{
        //    using (var db = new AccessoriesContext())
        //    {
        //        Category c = new Category { Name = "Voda" };
        //        Category c2 = new Category { Name = "Psací potřeby" };
        //        db.Categories.Add(c);
        //        db.Categories.Add(c2);
        //        db.SaveChanges();
        //    }
        //}

        /// <summary>
        /// Nacte <see cref="List{Accessory}"/> (nacte to zbozi, ktere ma byt precteno <see cref="Accessory.ToBeShown"/> a seradi jej) z DB a nastavi ho jako <see cref="dataGrid.ItemsSource"/> 
        /// </summary>
        private void LoadAccessories()
        {
            using (var db = new AccessoriesContext())
            {
                var cat = db.Accessories.Include("Category").ToList();
                //Trochu polopaticke trideni a filtrovani
                Accessories = cat;
                Accessories = Accessories.Where(x => x.ToBeShown == true).ToList();
                Accessories = Accessories.OrderBy(a => a.Category.Name).ThenBy(a => a.Name).ToList();
               
                dataGrid.ItemsSource = Accessories;
                
            }
        }

        /// <summary>
        /// Otevre <see cref="Modal"/> pro vytvoreni noveho zbozi
        /// </summary>
        private void newAccessoryButton_Click(object sender, RoutedEventArgs e)
        {
            Modal m = new Modal(this, false);
            m.Show();
        }

        

        /// <summary>
        /// Proiteruje <see cref="Accessories"/>, upozorni uzivatele na dochazejici zasoby daneho zbozi a oznaci jeho radek cervene v <see cref="dataGrid"/>
        /// </summary>
        private void CheckAvailability()
        {
            foreach (var item in Accessories)
            {
                if (item.Amount < item.MinAmount && item.ToBeShown == true)
                {
                    MessageBox.Show("Accessory " + item.Name + " is running out of supplies!");
                    DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(item);
                    row.Background = Brushes.Red;

                }
            }
        }

        /// <summary>
        /// Zkontroluje vybrani <see cref="Accessory"/> a v pripade uspechu otevre modalni okno pro update
        /// </summary>
        private void updateAccessoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentChecker.isSelectedAccessory((Accessory)dataGrid.SelectedItem))
            {
                var c = (Accessory)dataGrid.SelectedItem;

                Modal m = new Modal(c, this, true);
                m.Show();
            }
        }

        /// <summary>
        /// Zkontroluje potvrzeni smazani <see cref="Accessory"/>, pokud OK, tak smaze (i z DB), zaroven zaktualizuje UI
        /// </summary>
        private void deleteAccessoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentChecker.isSelectedAccessory((Accessory)dataGrid.SelectedItem) && ContentChecker.deleteAccessoryConfirm())
            {
                Accessory temp = (Accessory)dataGrid.SelectedItem;

                using (var db = new AccessoriesContext())
                {
                    var original = db.Accessories.Single(c => c.AccessoryId == temp.AccessoryId);

                    if (original != null)
                    {
                        temp.ToBeShown = false;
                        Accessories.Remove(temp);

                        db.Entry(original).CurrentValues.SetValues(temp);
                        db.SaveChanges();
                    }

                }
                revoke();
            }
        }

        /// <summary>
        /// Metoda pro update UI (refreshuje <see cref="dataGrid.ItemsSource"/> a znovu kontroluje dostupnost zasob na sklade
        /// </summary>
        public void revoke()
        {
            Accessories = Accessories.OrderBy(a => a.Category.Name).ThenBy(a => a.Name).ToList();
            dataGrid.Items.Refresh();
            dataGrid.ItemsSource = Accessories;
            CheckAvailability();
        }

        /// <summary>
        /// Otevre <see cref="AcceptWindow"/> pro prijeti zbozi
        /// </summary>
        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            AcceptWindow w = new AcceptWindow(this);
            w.Show();
        }

        /// <summary>
        /// Otevre <see cref="HandOverWindow"/> pro expedici zbozi
        /// </summary>
        private void handoverButton_Click(object sender, RoutedEventArgs e)
        {
            HandOverWindow w = new HandOverWindow(this);
            w.Show();
        }

        /// <summary>
        /// Kontroluje dostupnost zbozi na skladu po nacteni <see cref="dataGrid"/>
        /// </summary>
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            CheckAvailability();
        }
    }
}
