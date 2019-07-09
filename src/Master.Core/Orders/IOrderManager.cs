using Master.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Orders
{
    public interface IOrderManager: IDataModule<Order, int>
    {
    }
}
