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
using BudinskyMartin_Semestralni1CSP.Controllers;
using BudinskyMartin_Semestralni1CSP.Util;

namespace BudinskyMartin_Semestralni1CSP
{
    /// <summary>
    /// Interaction logic for ModalWindow.xaml
    /// </summary>
    public partial class ModalWindow : Window
    {
        private InputValidator inputValidator = new InputValidator();
        private MeetingCentre operatedMeetingCentre;
        private MeetingRoom operatedMeetingRoom;
        private ModalActionController.ActionType currentActionType;

        private MainWindow mainWindow;
        private string previousCode;
        
        public ModalWindow()
        {
            InitializeComponent();
        }

        #region Constructor overloads
        /// <summary>
        /// Pretizeni konstruktoru <see cref="ModalWindow"/> pro vytvoreni noveho <see cref="MeetingCentre"/>.
        /// Vola <see cref="contentProvider(ModalActionController.ContentType)"/> pro zobrazeni/skryti spravnych UI prvku pomoci <param name="contentType"></param>.
        /// Prirazuje instanci <see cref="MainWindow"/> z <param name="mainWindow"></param>.
        /// Prirazuje aktualni <see cref="ModalActionController.ActionType"/> z <param name="actionType"></param>.
        /// </summary>
        public ModalWindow(ModalActionController.ActionType actionType, Window mainWindow, ModalActionController.ContentType contentType)
        {
            InitializeComponent();
            contentProvider(contentType);
            this.mainWindow = (MainWindow)mainWindow;
            currentActionType = actionType;
        }

        /// <summary>
        /// Pretizeni konstruktoru <see cref="ModalWindow"/> pro upravu <see cref="MeetingCentre"/>.
        /// Vola <see cref="contentProvider(ModalActionController.ContentType)"/> pro zobrazeni/skryti spravnych UI prvku pomoci <param name="contentType"></param>.
        /// Prirazuje instanci <see cref="MainWindow"/> z <param name="mainWindow"></param>.
        /// Prirazuje aktualni <see cref="ModalActionController.ActionType"/> z <param name="actionType"></param>.
        /// Prirazuje aktualni <see cref="MeetingCentre"/> do <see cref="operatedMeetingCentre"/> z <param name="meetingCentre"></param>.
        /// </summary>
        public ModalWindow(MeetingCentre meetingCentre, ModalActionController.ActionType actionType, Window mainWindow, ModalActionController.ContentType contentType)
        {
            InitializeComponent();
            operatedMeetingCentre = meetingCentre;
            contentProvider(contentType);
            
            this.mainWindow = (MainWindow)mainWindow;
            currentActionType = actionType;
        }


        /// <summary>
        /// Pretizeni konstruktoru <see cref="ModalWindow"/> pro upravu nebo vytvoreni noveho <see cref="MeetingRoom"/>.
        /// Vola <see cref="contentProvider(ModalActionController.ContentType)"/> pro zobrazeni/skryti spravnych UI prvku pomoci <param name="contentType"></param>.
        /// Prirazuje instanci <see cref="MainWindow"/> z <param name="mainWindow"></param>.
        /// Prirazuje aktualni <see cref="ModalActionController.ActionType"/> z <param name="actionType"></param>.
        /// Prirazuje aktualni <see cref="MeetingCentre"/> do <see cref="operatedMeetingCentre"/> z <param name="parentMeetingCentre"></param>.
        /// Prirazuje aktualni <see cref="MeetingRoom"/> do <see cref="operatedMeetingRoom"/> z <param name="meetingRoom"></param>.
        /// </summary>
        public ModalWindow(MeetingRoom meetingRoom, MeetingCentre parentMeetingCentre, ModalActionController.ActionType actionType, Window mainWindow, ModalActionController.ContentType contentType)
        {

            InitializeComponent();
            operatedMeetingRoom = meetingRoom;
            operatedMeetingCentre = parentMeetingCentre;
            contentProvider(contentType);
            this.mainWindow = (MainWindow)mainWindow;
            currentActionType = actionType;

        }
        #endregion



        #region Buttons click events
        /// <summary>
        ///Event kliknuti na <see cref="okButton"/>. Zavola <see cref="proceedAction"/> s aktualnim <see cref="ModalActionController.ActionType"/>.
        /// </summary>
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            proceedAction(currentActionType);
        }

        /// <summary>
        /// Event kliknuti na <see cref="stornoButton"/>. Zavre modalni okno a zahodi vsechny mozne zmeny dat v nem provedene.
        /// </summary>
        private void stornoButton_Click(object sender, RoutedEventArgs e)
        { 
            this.Close();
        }
        #endregion


        /// <summary>
        /// Hlavni metoda modalniho okna ktera je volana pri kliknuti na <see cref="okButton"/>, deleguje vykonani akce.
        /// Metoda vola <see cref="performValidationAndAction"/> s patricnym pretizenim a preda ji data z UI prvku.
        /// <param name="type">Type akce k vykonani predavan konstruktorem z <see cref="MainWindow"/>. Je predan <see cref="performValidationAndAction"/></param>.
        /// </summary>
        private void proceedAction(ModalActionController.ActionType type)
        {
            
            switch (type)
            {
                case ModalActionController.ActionType.AddNewMeetingCentre:
                    performValidationAndAction(nameTextBox.Text, codeTextBox.Text, descriptionTextBox.Text, type);
                    break;
                case ModalActionController.ActionType.UpdateMeetingCentre:
                    performValidationAndAction(nameTextBox.Text, codeTextBox.Text, previousCode, descriptionTextBox.Text, type);
                    break;
                case ModalActionController.ActionType.AddNewMeetingRoom:
                    performValidationAndAction(nameTextBox.Text, codeTextBox.Text, descriptionTextBox.Text, capacityTextBox.Text, hasVideoConference.IsChecked.Value, type);
                    break;
                case ModalActionController.ActionType.UpdateMeetingRoom:
                    performValidationAndAction(nameTextBox.Text, codeTextBox.Text, previousCode, descriptionTextBox.Text, capacityTextBox.Text, hasVideoConference.IsChecked.Value, type);
                    break;

                default: break;
            }
        }


        #region Content deciding methods
        /// <summary>
        /// Metoda volajici nizsi metody pro zobrazeni/resetovani dat v UI prvcich modalniho okna.
        ///<param name="type">Typ aktualniho obsahu k zobrazeni.</param>
        /// </summary>
        private void contentProvider(ModalActionController.ContentType type)
        {
            switch (type)
            {
                case ModalActionController.ContentType.MeetingCentre:
                    showMeetingCentreContent();
                    showMeetingCentreDetails(operatedMeetingCentre);
                    break;
                case ModalActionController.ContentType.MeetingRoom:
                    showMeetingRoomContent();
                    showMeetingRoomDetails(operatedMeetingRoom);
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// Metoda zobrazi data do UI prvku souvisejicich pouze s obsahem <see cref="MeetingCentre"/>.
        /// </summary>
        private void showMeetingCentreDetails(MeetingCentre meetingCentre)
        {
           if(meetingCentre != null)
            {
                nameTextBox.Text = meetingCentre.name;
                codeTextBox.Text = meetingCentre.code;
                descriptionTextBox.Text = meetingCentre.description;
                previousCode = meetingCentre.code;
            }
        }

        /// <summary>
        /// Metoda zobrazi data do UI prvku souvisejicich pouze s obsahem <see cref="MeetingRoom"/>.
        /// </summary>
        private void showMeetingRoomDetails(MeetingRoom meetingRoom)
        {
            if(meetingRoom != null)
            {
                
                nameTextBox.Text = meetingRoom.name;
                codeTextBox.Text = meetingRoom.code;
                descriptionTextBox.Text = meetingRoom.description;
                capacityTextBox.Text = meetingRoom.capacity.ToString();
                hasVideoConference.IsChecked = meetingRoom.videoConference;
                previousCode = meetingRoom.code;
            }
        }

        /// <summary>
        /// Metoda zobrazi UI prvky souvisejici pouze s obsahem <see cref="MeetingCentre"/>.
        /// </summary>
        private void showMeetingCentreContent()
        {
            capacityLabel.Visibility = Visibility.Hidden;
            capacityTextBox.Visibility = Visibility.Hidden;
            hasVideoConference.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Metoda zobrazi UI prvky souvisejici pouze s obsahem <see cref="MeetingRoom"/>.
        /// </summary>
        private void showMeetingRoomContent()
        {
            capacityLabel.Visibility = Visibility.Visible;
            capacityTextBox.Visibility = Visibility.Visible;
            hasVideoConference.Visibility = Visibility.Visible;
        }
        #endregion



        #region Validation and action methods
        /// <summary>
        /// Pretizeni <see cref="performValidationAndAction"/> pro Meeting Centre.
        /// Metoda vola <see cref="InputValidator.Validate(MeetingCentre)"/> a <see cref="InputValidator.isCodeUnique(MeetingCentre, List{MeetingCentre})"/> pro overeni spravnosti vstupu.
        /// Je-li validace v poradku, metoda vola <see cref="MainWindow.performAction(ModalActionController.ActionType, object)"/> s parametry <see cref="type"/> a noveho <see cref="MeetingCentre"/>.
        /// Metoda priradi nove <see cref="MeetingCentre"/> do <see cref="operatedMeetingCentre"/>.
        /// Metoda zavre modalni okno.
        /// </summary>
        /// <remarks>Dalo by se samozrejme vyresit rovnou predavanim <see cref="previousCode"/>, cimz by odpadly 2 pretizeni, ale chtel jsem to mit alespon takto prehlednejsi.</remarks>
        private void performValidationAndAction(string name, string code, string description, ModalActionController.ActionType type)
        {
            MeetingCentre m = new MeetingCentre(name, code, description);
            if (inputValidator.Validate(m) && inputValidator.isCodeUnique(m,mainWindow.getMeetingController().getMeetingCentres()))
            {
                operatedMeetingCentre = m;
                mainWindow.performAction(type, m);
                this.Close();
            }
        }

        /// <summary>
        /// Pretizeni <see cref="performValidationAndAction"/> pro Meeting Room.
        /// Metoda vola <see cref="InputValidator.Validate(MeetingRoom)"/>, <see cref="InputValidator.isCodeUnique(MeetingRoom, MeetingCentre)"/> a <see cref="InputValidator.isNumber(string)"/> pro overeni spravnosti vstupu.
        /// Je-li validace v poradku, metoda vola <see cref="MainWindow.performAction(ModalActionController.ActionType, object)"/> s parametry <see cref="type"/> a noveho <see cref="MeetingRoom"/>.
        /// Metoda priradi novy <see cref="MeetingRoom"/> do <see cref="operatedMeetingRoom"/>.
        /// Metoda priradi novemu <see cref="MeetingRoom"/> jeho <see cref="MeetingRoom.parentMeetingCentre"/> z <see cref="operatedMeetingCentre"/>.
        /// Metoda zavre modalni okno.
        /// </summary>
        private void performValidationAndAction(string name, string code, string description, string capacityString, bool videoConference, ModalActionController.ActionType type)
        {
            if (inputValidator.isNumber(capacityString))
            {
                MeetingRoom r = new MeetingRoom(name, code, description, Convert.ToInt32(capacityString), videoConference);
                r.parentMeetingCentre = operatedMeetingCentre;
                if (inputValidator.Validate(r) && inputValidator.isCodeUnique(r,operatedMeetingCentre))
                {
                    operatedMeetingRoom = r;
                    mainWindow.performAction(type, r);
                    this.Close();
                }
            }
        }
        /// <summary>
        /// Pretizeni <see cref="performValidationAndAction"/> pro Meeting Centre pro pripad, ze by uzivatel zmenil <see cref="MeetingCentre.code"/>.
        /// Metoda vola <see cref="InputValidator.Validate(MeetingCentre)"/> a <see cref="InputValidator.isCodeUnique(MeetingCentre, List{MeetingCentre})"/> pro overeni spravnosti vstupu.
        /// Je-li validace v poradku, metoda vola <see cref="MainWindow.performAction(ModalActionController.ActionType, object)"/> s parametry <see cref="type"/> a noveho <see cref="MeetingCentre"/>.
        /// Metoda priradi nove <see cref="MeetingCentre"/> do <see cref="operatedMeetingCentre"/>.
        /// Metoda zavre modalni okno.
        /// </summary>
        private void performValidationAndAction(string name, string code, string previousCode, string description, ModalActionController.ActionType type)
        {
            MeetingCentre m = new MeetingCentre(name, code, description, previousCode);
            if (inputValidator.Validate(m) && inputValidator.isCodeUnique(m, mainWindow.getMeetingController().getMeetingCentres()))
            {
                operatedMeetingCentre = m;
                mainWindow.performAction(type, m);
                this.Close();
            }
        }

        /// <summary>
        /// Pretizeni <see cref="performValidationAndAction"/> pro Meeting Room pro pripad, ze by uzivatel zmenil <see cref="MeetingRoom.code"/>.
        /// Metoda vola <see cref="InputValidator.Validate(MeetingRoom)"/>, <see cref="InputValidator.isCodeUnique(MeetingRoom, MeetingCentre)"/> a <see cref="InputValidator.isNumber(string)"/> pro overeni spravnosti vstupu.
        /// Je-li validace v poradku, metoda vola <see cref="MainWindow.performAction(ModalActionController.ActionType, object)"/> s parametry <see cref="type"/> a noveho <see cref="MeetingRoom"/>.
        /// Metoda priradi novy <see cref="MeetingRoom"/> do <see cref="operatedMeetingRoom"/>.
        /// Metoda priradi novemu <see cref="MeetingRoom"/> jeho <see cref="MeetingRoom.parentMeetingCentre"/> z <see cref="operatedMeetingCentre"/>.
        /// Metoda zavre modalni okno.
        /// </summary>
        private void performValidationAndAction(string name, string code, string previousCode, string description, string capacityString, bool videoConference, ModalActionController.ActionType type)
        {
            if (inputValidator.isNumber(capacityString))
            {
                MeetingRoom r = new MeetingRoom(name, code, description, Convert.ToInt32(capacityString), videoConference, previousCode);
                r.parentMeetingCentre = operatedMeetingCentre;
                if (inputValidator.Validate(r) && inputValidator.isCodeUnique(r, operatedMeetingCentre))
                {
                    operatedMeetingRoom = r;
                    mainWindow.performAction(type, r);
                    this.Close();
                }
            }
        }
        #endregion
    }
}
