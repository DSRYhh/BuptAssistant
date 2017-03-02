using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcardData
{
    public class TransactionRecord : IEnumerable<string>
    {
        public string OperatingTime { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string Balance { get; set; }
        public string Operator { get; set; }
        public string Station { get; set; }
        public string Terminal { get; set; }

        public override string ToString()
        {
            return $"{OperatingTime}   \t{Amount}\t{Station}\t                 余额：{Balance}";
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
                        OperatingTime = entry;
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
                new List<string>() { OperatingTime, Description, Amount, Balance, Operator, Station, Terminal }
                    .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
