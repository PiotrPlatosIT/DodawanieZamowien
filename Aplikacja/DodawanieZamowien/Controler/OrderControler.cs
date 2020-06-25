using DodawanieZamowien.Models;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DodawanieZamowien.Controler
{
    class OrderControler
    {
        public static void AddItemToOrder(OrderModel order, OrderItemModel orderItem)
        {
            if (order.Items.Exists(o => o.Name == orderItem.Name))
            {
                order.Items.First(o => o.Name == orderItem.Name).Quantity += orderItem.Quantity;
            }
            else
            {
                order.Items.Add(orderItem);
            }
        }

        public static void CreateXML(OrderModel order)
        {
            string pathXML = GetPathToSaveFile();
            if (!string.IsNullOrWhiteSpace(pathXML))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(OrderModel));
                using (Stream o = File.Create(pathXML))
                {
                    serializer.Serialize(o, order);
                }
            }
        }

        public static void SaveOrderToDataBase(OrderModel orderModel)
        {
            using (var db = new BazaEntities())
            {
                var order = new Orders
                {
                    Ord_PurchaserName = orderModel.Name,
                    Ord_PurchaserLastname = orderModel.LastName,
                    Ord_PurchaserDateOfBirth = orderModel.DateOfBirdth
                };

                db.Orders.Add(order);

                foreach (OrderItemModel item in orderModel.Items)
                {
                    var orderItem = new OrderItems
                    {
                        Orders = order,
                        Products=db.Products.FirstOrDefault(o=>o.Prd_Name==item.Name),
                        Ori_Quantity=item.Quantity
                    };
                    db.OrderItems.Add(orderItem);
                }
                db.SaveChanges();
            }
        }

        static private string GetPathToSaveFile()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                saveFileDialog.DefaultExt = "xml";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return saveFileDialog.FileName;
                }
                else
                    return String.Empty;
            }
        }
    }
}
