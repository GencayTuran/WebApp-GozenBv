using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Models
{
    public class StockLogItemDamaged
    {
        //TODO: Status counts for all the amount in this model, do this per one item.
        public int Id { get; set; }
        public string LogCode { get; set; }
        public int StockId { get; set; }
        public int StockAmount { get; set; }
        public string ProductNameBrand { get; set; }
        public int Status { get; set; }
    }
}
