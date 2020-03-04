using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Finance
{
    [AutoMap(typeof(Voucher))]
    public class VoucherDto
    {
        public int Id { get; set; }
        public decimal Fee { get; set; }
        public int UnitId { get; set; }
        public string Remarks { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}
