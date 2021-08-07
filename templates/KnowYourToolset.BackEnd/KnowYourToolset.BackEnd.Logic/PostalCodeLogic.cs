using System;
using System.Collections.Generic;
using Serilog;

namespace KnowYourToolset.BackEnd.Logic
{
    public class PostalCodeLogic : IPostalCodeLogic
    {
        private readonly List<string> _cities = new() { "New York", "Chicago", "Minneapolis", "Seattle", "Huntington Beach", "Dallas" };

        public string GetCityForPostalCode(string postalCode)
        {

            Log.Information("Got into the postal code logic.");
            if (string.Equals(postalCode, "11111", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("Simulated low level error that should not be visible.");
            }

            if (string.Equals(postalCode, "22222", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ApplicationException($"Bad input value of {postalCode}");
            }

            var rnd = new Random();
            return _cities[rnd.Next(_cities.Count)];
        }
    }
}
