using FloralFusion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloralFusion.Application.Models.ModelsForOrders
{
    public class WishlistItemModel
    {
        public long FlowerId { get; set; }

        public int Quantity { get; set; }

        public long WishlistId { get; set; }
    }
}
