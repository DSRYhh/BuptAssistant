using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EcardData
{
    public class TransactionRecord : IEnumerable<string>
    {
        public string OperatingTimeString { get; set; }

        public DateTime OperatingDate
        {
            get
            {
                string pattern = @"([0-9]+?)\/([0-9]+?)\/([0-9]+?) ([0-9]+?):([0-9]+?):([0-9]+)";
                Regex regex = new Regex(pattern);
                var match = regex.Match(OperatingTimeString);
                var group = match.Groups;


                try
                {
                    int year = int.Parse(group[1].Value);
                    int month = int.Parse(group[2].Value);
                    int date = int.Parse(group[3].Value);
                    int hour = int.Parse(group[4].Value);
                    int minute = int.Parse(group[5].Value);
                    int second = int.Parse(group[6].Value);

                    return new DateTime(year, month, date, hour, minute, second);
                }
                catch (Exception)
                {
                    return DateTime.MinValue;
                }
            }
        }

        public string Description { get; set; }
        public string Amount { get; set; }
        public string Balance { get; set; }
        public string Operator { get; set; }
        public string Station { get; set; }
        public string Terminal { get; set; }

        public override string ToString()
        {
            string time = OperatingDate.ToString(@"MM\/dd HH:mm");
            return $"{time}\t {Amount}\t {Station}";
        }
        public TransactionRecord(ICollection<string> list)
        {
            if (list.Count != 7) throw new ArgumentException();
            int i = 0;
            foreach (var entry in list)
            {
                switch (i)
                {
                    case 0:
                        OperatingTimeString = entry;
                        break;
                    case 1:
                        this.Description = entry;
                        break;
                    case 2:
                        this.Amount = entry;
                        break;
                    case 3:
                        this.Balance = entry;
                        break;
                    case 4:
                        this.Operator = entry;
                        break;
                    case 5:
                        this.Station = entry;
                        break;
                    case 6:
                        this.Terminal = entry;
                        break;
                }
                i++;
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return
                new List<string>() { OperatingTimeString, Description, Amount, Balance, Operator, Station, Terminal }
                    .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
