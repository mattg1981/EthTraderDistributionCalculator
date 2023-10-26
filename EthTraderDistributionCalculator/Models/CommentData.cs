using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthTraderDistributionCalculator.Models
{
    internal class CommentData : KarmaData
    {
        public string Submission { get; set; }

        [Ignore]
        public bool IsFromDaily { get; set; }
    }
}
