using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetParker.Models
{
    public class Vehicle : BaseViewModel
    {
        public VehicleType Type; 
        public int CostPerHour { get; set; }
        public string RegisterNumber { get; protected set; }
        public int CostPerDay { get; set; }
        public int CostPerDayPlus { get; set; }
    }

    public enum VehicleType
    {
        Car,
        Bus
    }
}















































