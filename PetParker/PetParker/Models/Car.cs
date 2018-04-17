using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetParker.Models
{
    internal class Car : Vehicle
    {
        public Car(string registerNumber)
        {
            Type = VehicleType.Car;;
            CostPerHour = 4;
            RegisterNumber = registerNumber;
            CostPerDay = 35;
            CostPerDayPlus = 30;
        }

    }
}