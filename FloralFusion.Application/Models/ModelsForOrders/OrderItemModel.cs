using FloralFusion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloralFusion.Application.Models.ModelsForOrders
{
    public class OrderItemModel
    {
        public long FlowerId { get; set; }

        public long OrderId { get; set; }
    }
}
