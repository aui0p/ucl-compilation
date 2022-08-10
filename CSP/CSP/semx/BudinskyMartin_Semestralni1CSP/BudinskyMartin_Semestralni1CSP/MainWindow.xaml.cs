using BudinskyMartin_Semestralni1CSP.Controllers;
using BudinskyMartin_Semestralni1CSP.Util;
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
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;

namespace BudinskyMartin_Semestralni1CSP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool contentHasChanged = false;
        private MeetingController meetingCentresController = new MeetingController();

        private MeetingCentre selectedCentre;
        private MeetingRoom selectedRoom;
        public MainWindow()
        {
            InitializeComponent();
            Title = "EBC Meeting Rooms Manager";

        }

        #region Menu buttons
        /// <summary>
        /// Otevre <see cref="OpenFileDialog"/> a nacte zavola metodu pro import dat z <see cref="MeetingController"/>.
        /// Priradi ItemsSource pro <see cref="meetingCentresListBox"/> ziskanim Listu <see cref="MeetingCentre"/> z <see cref="MeetingController"/>.
        /// Vyresetuje UI prvky od moznych zadanych dat.
        /// </summary>
        private void importData(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                meetingCentresController.loadMeetingCentresFromFile(dialog.FileName);
                meetingCentresListBox.ItemsSource = meetingCentresController.getMeetingCentres();
                meetingRoomsListBox.ItemsSource = null; //reset meeting roomu aby po reimportu zustaly prazdne, v pripade ze uzivatel nacvakal rucne data
                clearMeetingRoomDetails();
                clearMeetingCentreDetails();
            }
            
        }

        /// <summary>
        /// Pomoci <see cref="FileParser"/> ulozi data do souboru, ze ktereho uzivatel importoval.
        /// Prepne <see cref="contentHasChanged"/> na stav nezmenenych dat. 
        /// </summary>
        /// <remarks>Prislo mi to jako lepsi reseni, nez ukladat jeste mimo importovany soubor. Samozrejme existuje reseni pro ulozeni bez puvodniho importu.</remarks>
        private void saveData(object sender, RoutedEventArgs e)
        {
            FileParser.exportDataToImportedFile(meetingCentresController.getMeetingCentres(), meetingCentresController.getPathToImportedFile());
            contentHasChanged = false;
        }

        /// <summary>
        ///  Dotaze se uzivatele na ulozeni byla-li zmenena dat pomoci <see cref="contentHasChanged"/>.
        /// Pomoci <see cref="FileParser"/> ulozi data do souboru, ze ktereho uzivatel importoval a ukonci aplikaci, pripadne ji ukonci bez ulozeni.
        /// </summary>
        private void exitApp(object sender, RoutedEventArgs e)
        {
            
            if (contentHasChanged)
            {
                string msg = "Data has changed. Do you want to save it before closing?";
                MessageBoxResult result =
                  MessageBox.Show(
                    msg,
                    "EBC Meeting Rooms Manager",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    contentHasChanged = false;
                    FileParser.exportDataToImportedFile(meetingCentresController.getMeetingCentres(), meetingCentresController.getPathToImportedFile());
                    Application.Current.Shutdown();

                }
                else
                {
                    contentHasChanged = false; //primarne kvuli predejiti znovuzavolani dialogu v eventu Closing, ktery se shutdownem zavola
                    Application.Current.Shutdown();
                }
            }
            else
                Application.Current.Shutdown();
            
        }
        #endregion



        #region ListBoxes change events

        /// <summary>
        /// Event zmeny vyberu prvku <see cref="meetingCentresListBox"/>.
        /// Vezme aktualne vybrany prvek z <see cref="meetingCentresListBox"/> a ulozi ho do <see cref="selectedCentre"/> pro dalsi operace.
        /// Zaroven priradi <see cref="meetingRoomsListBox"/> svuj list <see cref="MeetingRoom"/> jako ItemsSource a zobrazi detaily <see cref="MeetingCentre"/> do UI prvku.
        /// </summary>
        private void meetingCentresListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MeetingCentre c = (MeetingCentre)meetingCentresListBox.SelectedItem;

            if (meetingCentresListBox.SelectedItem != null) //osetreni proti padu pri reimportu dat s vybranym Itemem
            {
                clearMeetingRoomDetails();
                meetingRoomsListBox.ItemsSource = c.meetingRooms;
                selectedCentre = c;
                showMeetingCentreDetails(c);
            }
        }

        /// <summary>
        /// Event zmeny vyberu prvku <see cref="meetingRoomsListBox"/>.
        /// Vezme aktualne vybrany prvek z <see cref="meetingRoomsListBox"/> a ulozi ho do <see cref="selectedRoom"/> pro dalsi operace.
        /// Zaroven zobrazi detaily <see cref="MeetingRoom"/> do UI prvku.
        /// </summary>
        private void meetingRoomsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(meetingRoomsListBox.SelectedItem != null)
            {
                MeetingRoom r = (MeetingRoom)meetingRoomsListBox.SelectedItem;
                selectedRoom = r;
                showMeetingRoomDetails(r);
            }
            
        }
        #endregion



        /// <summary>
        /// Metody <see cref="meetingCentresListBox"/>.
        /// V kazde z nich je nejprve zavolana kontrola vybrani navazujiciho obsahu, ci potvrzeni akce.
        /// Kazda z nich pote zavola a zobrazi modalni okno s pozadovanym konstruktorem a pripadne vyresetuje UI prvky.
        /// </summary>
        #region MeetingCentres ListBox action buttons
        private void buttonMCNew_Click(object sender, RoutedEventArgs e)
        {
            ModalWindow w = new ModalWindow(ModalActionController.ActionType.AddNewMeetingCentre, this, ModalActionController.ContentType.MeetingCentre);
            w.ShowDialog();
        }

        private void buttonMCEdit_Click(object sender, RoutedEventArgs e)
        {
            if(ContentChecker.isSelectedMeetingCentre((MeetingCentre) meetingCentresListBox.SelectedItem))
            {
                ModalWindow w = new ModalWindow(selectedCentre, ModalActionController.ActionType.UpdateMeetingCentre, this, ModalActionController.ContentType.MeetingCentre);
                w.ShowDialog();
            }
        }
        private void buttonMCDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((ContentChecker.isSelectedMeetingCentre((MeetingCentre)meetingCentresListBox.SelectedItem)) && ContentChecker.deleteMeetingCentreConfirm())
            {
                performAction(ModalActionController.ActionType.RemoveMeetingCentre, (MeetingCentre)selectedCentre);
                selectedRoom = null;
                selectedCentre = null;
                clearMeetingCentreDetails();
                clearMeetingRoomDetails();
                revokeDataSources();
            }
        }
        #endregion



        /// <summary>
        /// Metody <see cref="meetingRoomsListBox"/>.
        /// V kazde z nich je nejprve zavolana kontrola vybrani navazujiciho obsahu, ci potvrzeni akce.
        /// Kazda z nich pote zavola a zobrazi modalni okno s pozadovanym konstruktorem a pripadne vyresetuje UI prvky.
        /// </summary>
        #region MeetingRooms ListBox action buttons
        private void buttonMRNew_Click(object sender, RoutedEventArgs e)
        {
            if (ContentChecker.isSelectedMeetingCentre((MeetingCentre)meetingCentresListBox.SelectedItem))
            {
                ModalWindow w = new ModalWindow(null, selectedCentre, ModalActionController.ActionType.AddNewMeetingRoom, this, ModalActionController.ContentType.MeetingRoom);
                w.ShowDialog();
            }
        }

        private void buttonMREdit_Click(object sender, RoutedEventArgs e)
        {
            if (ContentChecker.isMeetingRoomSelected((MeetingRoom)meetingRoomsListBox.SelectedItem))
            {
                ModalWindow w = new ModalWindow(selectedRoom, selectedCentre, ModalActionController.ActionType.UpdateMeetingRoom, this, ModalActionController.ContentType.MeetingRoom);
                w.ShowDialog();
            }
        }
        private void buttonMRDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((ContentChecker.isMeetingRoomSelected((MeetingRoom)meetingRoomsListBox.SelectedItem)) && ContentChecker.deleteMeetingRoomConfirm())
            {
                performAction(ModalActionController.ActionType.RemoveMeetingRoom, (MeetingRoom)selectedRoom);

                selectedRoom = null;
                clearMeetingRoomDetails();
                revokeDataSources();
            }
        }
        #endregion


        /// <summary>
        /// Hlavni metoda delegujici <see cref="MeetingController"/> k vykonani specificke akce, je volana z modalniho okna. 
        /// Metoda zaroven dle potreby vyuziva metod k resetu UI prvku a indikuje zmenu dat pomoci <see cref="contentHasChanged"/>."
        /// </summary>
        /// <param name="type">Reprezentuje akci, ktera ma byt vykonana.</param>
        /// <param name="toChange">Reprezentuje objekt predany z modalniho okna, nasledne je pretypovan na konkretni typ dle potreby a predan <see cref="MeetingController"/>.</param>
        ///<remarks>Indikace zmeny dat je velmi trivialni - aplikace bude hlasit zmenu dat ikdyz uzivatel jen klikne na Edit button a nasledne potvrdi OK data beze zmeny...</remarks>
        public void performAction(ModalActionController.ActionType type, object toChange)
        {
            contentHasChanged = true;
            switch (type)
            {
                case ModalActionController.ActionType.AddNewMeetingCentre:
                    meetingCentresController.addMeetingCentre((MeetingCentre)toChange);
                    revokeDataSources();
                    break;

                case ModalActionController.ActionType.UpdateMeetingCentre:
                    meetingCentresController.updateMeetingCentre((MeetingCentre)toChange);
                    revokeDataSources();
                    showMeetingCentreDetails((MeetingCentre)toChange);
                    break;

                case ModalActionController.ActionType.RemoveMeetingCentre:
                    meetingCentresController.removeMeetingCentre((MeetingCentre)toChange);
                    revokeDataSources();
                    clearMeetingCentreDetails();
                    break;

                case ModalActionController.ActionType.AddNewMeetingRoom:
                    meetingCentresController.addMeetingRoom((MeetingRoom)toChange, selectedCentre);
                    revokeDataSources();
                    break;

                case ModalActionController.ActionType.UpdateMeetingRoom:
                    meetingCentresController.updateMeetingRoom((MeetingRoom)toChange, selectedCentre);
                    revokeDataSources();
                    showMeetingRoomDetails((MeetingRoom)toChange);
                    break;

                case ModalActionController.ActionType.RemoveMeetingRoom:
                    meetingCentresController.removeMeetingRoom((MeetingRoom)toChange, selectedCentre);
                    revokeDataSources();
                    clearMeetingRoomDetails();
                    break;

                
                default: break;
            }
        }

        /// <summary>
        /// Dle aktualniho stavu zresetuje zobrazeni dat v obou ListBoxech <see cref="meetingCentresListBox"/> <see cref="meetingRoomsListBox"/>.
        /// </summary>
        ///<remarks>asi by bylo lepe vyreseno uzitim databindingu...</remarks>
        private void revokeDataSources()
        {
            meetingRoomsListBox.ItemsSource = null;
            meetingCentresListBox.ItemsSource = null;
            meetingCentresListBox.ItemsSource = meetingCentresController.getMeetingCentres();
            if (selectedCentre != null)
            {
                meetingCentresListBox.SelectedItem = selectedCentre;
                meetingRoomsListBox.ItemsSource = selectedCentre.meetingRooms;
            }
            if (meetingCentresListBox.SelectedItem == null) meetingRoomsListBox.ItemsSource = null;

        }

        /// <summary>
        /// Vraci instanci <see cref="MeetingController"/>.
        /// </summary>
        public MeetingController getMeetingController()
        {
            return meetingCentresController;
        }

       
        
        #region MeetingCentre and MeetingRoom details render and reset actions
        /// <summary>
        /// Nastavuje hodnoty detailu Meeting Centre do UI prvku.
        /// </summary>
        /// <param name="meetingCentre">Reprezentuje aktualne vybrane Meetin Centre z ListBoxu</param>
        private void showMeetingCentreDetails(MeetingCentre meetingCentre)
        {
            meetingCentreDetailNameTextBox.Text = meetingCentre.name;
            meetingCentreDetailCodeTextBox.Text = meetingCentre.code;
            meetingCentreDetailDescriptionTextBox.Text = meetingCentre.description;
        }

        /// <summary>
        /// Nastavuje hodnoty detailu Meeting Room do UI prvku.
        /// </summary>
        /// <param name="meetingRoom">Reprezentuje aktualne vybrany Meetin Room z ListBoxu</param>
        private void showMeetingRoomDetails(MeetingRoom meetingRoom)
        {
            meetingRoomDetailNameTextBox.Text = meetingRoom.name;
            meetingRoomDetailCodeTextBox.Text = meetingRoom.code;
            meetingRoomDetailDescriptionTextBox.Text = meetingRoom.description;
            meetingRoomDetailCapacityTextBox.Text = meetingRoom.capacity.ToString();
            meetingRoomDetailHasVideoConference.IsChecked = meetingRoom.videoConference;
        }

        /// <summary>
        /// Resetuje hodnoty detailu Meeting Centre v UI prvcich.
        /// </summary>
        private void clearMeetingCentreDetails()
        {
            meetingCentreDetailNameTextBox.Text = "";
            meetingCentreDetailCodeTextBox.Text = "";
            meetingCentreDetailDescriptionTextBox.Text = "";
        }

        /// <summary>
        /// Resetuje hodnoty detailu Meeting Room v UI prvcich.
        /// </summary>
        private void clearMeetingRoomDetails()
        {
            meetingRoomDetailNameTextBox.Text = "";
            meetingRoomDetailCodeTextBox.Text = "";
            meetingRoomDetailDescriptionTextBox.Text = "";
            meetingRoomDetailCapacityTextBox.Text = "";
            meetingRoomDetailHasVideoConference.IsChecked = false;
        }
        #endregion


        /// <summary>
        /// Upravuje Closing event pro dotazani se uzivatele na ulozeni byla-li zmenena dat pomoci <see cref="contentHasChanged"/>.
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (contentHasChanged)
            {
                string msg = "Data has changed. Do you want to save it before closing?";
                MessageBoxResult result =
                  MessageBox.Show(
                    msg,
                    "EBC Meeting Rooms Manager",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    FileParser.exportDataToImportedFile(meetingCentresController.getMeetingCentres(), meetingCentresController.getPathToImportedFile());
                    Application.Current.Shutdown(); //trochu sado-maso reseni, e.Close = true mi nefungovalo.

                }
                else
                    Application.Current.Shutdown();
            }
        }
        
    }
}
