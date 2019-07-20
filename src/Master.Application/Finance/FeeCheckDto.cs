using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Finance
{
    [AutoMap(typeof(FeeCheck))]
    public class FeeCheckDto
    {
        public int Id { get; set; }
        public string CheckNumber { get; set; }
        public decimal CheckFee { get; set; }
        public DateTime CheckDate { get; set; }
        public int CheckDaySpan { get; set; }
        public string CheckCompany { get; set; }
        public string CheckBank { get; set; }
    }
}
