using Master.Domain;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Orders
{
    public class OrderManager : ModuleServiceBase<Order, int>, IOrderManager
    {
    }
}
