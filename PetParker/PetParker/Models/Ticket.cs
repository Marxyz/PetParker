using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PetParker.Models
{
    public class Ticket : BaseViewModel
    {

        private readonly DispatcherTimer _timer;
        private double _realCost;
        private int _cost;
        private string _costText;
        private string _realCostText;
        private TimeSpan _layoverTimeSpan;
        private string _layoverTimeSpanText;
        private string _exitDateTimeText;
        private string _enterDateTimeText;
        private DateTime _exitDateTime;


        public string CostText
        {
            get { return _costText; }
            set
            {
                _costText = value;
                OnPropertyChanged();
            }
        }

        public string RealCostText
        {
            get { return _realCostText; }
            set
            {
                _realCostText = value;
                OnPropertyChanged();
            }
        }

        public string LayoverTimeSpanText
        {
            get { return _layoverTimeSpanText; }
            set
            {
                _layoverTimeSpanText = value;
                OnPropertyChanged();
            }
        }


        public string EnterDateTimeText
        {
            get { return _enterDateTimeText; }
            set
            {
                _enterDateTimeText = value;
                OnPropertyChanged();
            }
        }

        public string ExitDateTimeText
        {
            get { return _exitDateTimeText; }
            set
            {
                _exitDateTimeText = value;
                OnPropertyChanged();
            }
        }

        public string TicketVehicleBrandText { get; set; }
        

        public DateTime EnterDateTime{ get; set; }

        public DateTime ExitDateTime
        {
            get { return _exitDateTime; }
            set
            {
                _exitDateTime = value;
                _exitDateTimeText = $"Data wyjazdu\n{ExitDateTime}";
                OnPropertyChanged();
            }
        }

        public Vehicle TicketVehicle { get; set; }
        public bool IsCompleted { get; set; }
        public int ID { get; set; }


        public TimeSpan LayoverTimeSpan
        {
            get { return _layoverTimeSpan; }
            set
            {
                _layoverTimeSpan = value;
                OnPropertyChanged();
            }
        }

        public double RealCost
        {
            get { return _realCost; }
            set
            {
                _realCost = value;
                OnPropertyChanged();
            }
        }

        public int Cost
        {
            get { return _cost; }
            set
            {
                _cost = value;
                OnPropertyChanged();
            }
        }

        public Ticket(Vehicle vehicle)
        {
            this.IsCompleted = false;
            this.TicketVehicle = vehicle;
            this.EnterDateTime = DateTime.Now;

            EnterDateTimeText = $"Data przyjazdu\n{EnterDateTime}";
            TicketVehicleBrandText = this.TicketVehicle.Type == VehicleType.Car ? "Samochód" : "Bus";

            _timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 1)};
            _timer.Tick += TimerOnTick;
            _timer.Start();
        }

        private double CalculateRealCost(TimeSpan timeDelta,int precision)
        {
            var cost = timeDelta.TotalSeconds * (TicketVehicle.CostPerHour * 1.0 / 3600);
            cost  = Math.Round(Math.Pow(10,precision) * cost) / Math.Pow(10,precision);
            return cost;
        }

        private void TimerOnTick(object sender, object o)
        {
            var timeDelta = DateTime.Now - EnterDateTime;
            LayoverTimeSpan = timeDelta;
            Cost = CalculateCost(timeDelta);
            RealCost = CalculateRealCost(timeDelta,4);
            LayoverTimeSpanText =
                $"Czas postoju: \n{LayoverTimeSpan.Days} dni, {LayoverTimeSpan.Hours} godzin, " +
                $"{LayoverTimeSpan.Minutes} minut, i {LayoverTimeSpan.Seconds} sekund.";
            RealCostText = $"Koszt realny: {RealCost}";
            CostText = $"Do zapłaty: {Cost}";

        }

        

        private int CalculateCost(TimeSpan timeDelta)
        {
            var cost = 0;
            var hours = timeDelta.Hours;
            var minutes = timeDelta.Minutes;
            var days = timeDelta.Days;
            cost += hours * TicketVehicle.CostPerHour;
            if (minutes >= 0 && minutes <= 30 )
            {
                cost += TicketVehicle.CostPerHour/2 ;
            }
            else
            {
                cost += TicketVehicle.CostPerHour;
            }
            if (hours >= 9 && hours < 24)
            {
                cost = TicketVehicle.CostPerDay;
            }
            if (days > 1 && days < 3)
            {
                cost += TicketVehicle.CostPerDay * days;
            }
            if (days> 3)
            {
                cost += TicketVehicle.CostPerDayPlus * days;
            }

            return cost;
        }

        public void StopCostTimer()
        {
            _timer.Stop();
        }
    }
}
