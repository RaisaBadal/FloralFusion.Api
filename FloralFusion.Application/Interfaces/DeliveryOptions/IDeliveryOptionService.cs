using FloralFusion.Application.Models;
using FloralFusion.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloralFusion.Application.Interfaces.DeliveryOptions
{
    public interface IDeliveryOptionService:ICrud<DeliveryOptionModel,long>
    {
    }
}
