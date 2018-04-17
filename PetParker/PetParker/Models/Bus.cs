using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetParker.Models
{
    internal class Bus : Vehicle
    {

        public Bus(string registerNumber)
        {
            Type = VehicleType.Bus;
            CostPerHour = 6;
            RegisterNumber = registerNumber;
            CostPerDay = 45;
            CostPerDayPlus = 40;
        }

        
    }
}
