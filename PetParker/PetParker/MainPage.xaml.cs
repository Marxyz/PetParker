using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PetParker.Models;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PetParker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public TicketRepository TicketRepository { get; }
        private Ticket _clickedTicket;

        public MainPage()
        {
            
            
            TicketRepository = new TicketRepository();
            
            InitializeComponent();
            AddingButton.Click += AddingButtonOnClick;

            var refreshTimer = new DispatcherTimer();
            refreshTimer.Tick += RefreshTimerOnTick;
            refreshTimer.Interval = new TimeSpan(0,0,0,1);
            refreshTimer.Start();
        }

      


        private void RefreshTimerOnTick(object sender, object o)
        {
            var now = DateTime.Now;
            //All data page
            ActualTimeTextBlock.Text = string.Format("{0:dd/MM/yyyy HH:mm:ss}", now);
            SumOfEnteredVehicle.Text = $"Ilość stojących pojazdów: {TicketRepository.WorkingTickets.Count }.";
            SumOfExitedVehicle.Text =  $"Ilość pojazdów które wyjechały: {TicketRepository.FinishedTickets.Count }.";
            MoneyUntilNowAcquiredTextBlock.Text = $"Pieniądze z zakończonych biletów: {TicketRepository.FinishedTicketSum} złotych.";
            WorkingTicketMoneyTextBlock.Text = $"Pieniądze w pracujących biletach: {TicketRepository.WorkingTicketSum} złotych.";
            WorkingTicketRealMoneyTextBlock.Text = $"Szacowane pieniądze wg. czasu, w pracujących biletach: {TicketRepository.WorkingTicketRealSum} złotych.";
            SumOfMoney.Text = $"Suma zarobionych i pracujących pieniędzy: {TicketRepository.Sum}";
            LongerThan10HoursTextBlock.Text = $@"Ilość aut stojących powyżej 10 godzin: {
                TicketRepository.WorkingTickets.
                    Where(t => now - t.EnterDateTime > new TimeSpan(0, 10, 0, 0)).ToList().Count +
                TicketRepository.FinishedTickets.
                    Where(t => now - t.EnterDateTime > new TimeSpan(0, 10, 0, 0)).ToList().Count}.";

            LongerThan24HoursTextBlock.Text = $@"Ilość aut stojących powyżej 24 godzin: {
                TicketRepository.WorkingTickets.
                    Where(t => now - t.EnterDateTime > new TimeSpan(1,0,0,0)).ToList().Count +
                TicketRepository.FinishedTickets.
                    Where(t => now - t.EnterDateTime > new TimeSpan(0, 24, 0, 0)).ToList().Count}.";


            CarCountTextBlock.Text =$"Łączna suma obsłużonych pojazdów: {TicketRepository.WorkingTickets.Count + TicketRepository.FinishedTickets.Count}.";
        }


        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            var buttonName = ((Button)sender).Name;
            AddingViewBox.Visibility = Visibility.Visible;
            if (buttonName == "AutoButton")
            {
                BusButton.IsEnabled = false;
            }
            else
            {
                AutoButton.IsEnabled = false;
            }
            RegisterTextBox.Focus(FocusState.Keyboard);
            
        }

        private void AddingButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!BusButton.IsEnabled)
            {
                TicketRepository.AddTicket(new Ticket(new Car(RegisterTextBox.Text)));
                BusButton.IsEnabled = true;
            }
            else
            {
                TicketRepository.AddTicket(new Ticket(new Bus(RegisterTextBox.Text)));
                AutoButton.IsEnabled = true;
            }
            AddingViewBox.Visibility = Visibility.Collapsed;
            RegisterTextBox.Text = String.Empty;
        }



        private void WorkingTicketGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            _clickedTicket = (Ticket)e.ClickedItem;
            WorkingTicketOptionsGrid.Visibility = Visibility.Visible;
        }

        private void EditButtonFinish_OnClick(object sender, RoutedEventArgs e)
        {
            TicketRepository.FinishTicket(ref _clickedTicket);
            _clickedTicket = null;
            WorkingTicketOptionsGrid.Visibility = Visibility.Collapsed;
            WorkingTicketGridView.SelectedItem = null;
        }

        private void EditButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            TicketRepository.DeleteTicketChangeId(ref _clickedTicket);
            _clickedTicket = null;
        }


        private void RegisterTextBox_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {

                if (!BusButton.IsEnabled)
                {
                    TicketRepository.AddTicket(new Ticket(new Car(RegisterTextBox.Text)));
                    BusButton.IsEnabled = true;
                }
                else
                {
                    TicketRepository.AddTicket(new Ticket(new Bus(RegisterTextBox.Text)));
                    AutoButton.IsEnabled = true;
                }
                AddingViewBox.Visibility = Visibility.Collapsed;
                RegisterTextBox.Text = String.Empty;
            }
        }

        private void RegisterTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var selectionStart = ((TextBox)sender).SelectionStart;
            ((TextBox)sender).Text = ((TextBox)sender).Text.ToUpper();
            ((TextBox)sender).SelectionStart = selectionStart;

        }

        private void DiscardAddingButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!BusButton.IsEnabled)
            {
                BusButton.IsEnabled = true;
            }
            else
            {
                AutoButton.IsEnabled = true;
            }
            AddingViewBox.Visibility = Visibility.Collapsed;
            RegisterTextBox.Text = String.Empty;
        }
        
    }
}
