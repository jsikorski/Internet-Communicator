using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Common.Contacts;

namespace Client.Services
{
    public class NumbersToNamesConverter : INumbersToNamesConverter
    {
        private readonly IContactsStorageController _contactsStorageController;

        public NumbersToNamesConverter(
            IContactsStorageController contactsStorageController)
        {
            _contactsStorageController = contactsStorageController;
        }

        public string Convert(int number)
        {
            IEnumerable<ContactStoredData> contactsDatas = _contactsStorageController.Load();
            return ConvertNumber(number, contactsDatas);
        }

        public string ConvertGroup(IEnumerable<int> numbers)
        {
            IEnumerable<ContactStoredData> contactsDatas = _contactsStorageController.Load();

            string aggregatedName = string.Empty;
            foreach (var number in numbers)
            {
                aggregatedName += ConvertNumber(number, contactsDatas);
                if (number != numbers.Last())
                {
                    aggregatedName += ", ";
                }
            }

            return aggregatedName;
        }

        private string ConvertNumber(int number, IEnumerable<ContactStoredData> contactsDatas)
        {
            if (contactsDatas.Count(contact => contact.Number == number) == 1)
            {
                return contactsDatas.Single(contact => contact.Number == number).Name;
            }
            else
            {
                return number.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
