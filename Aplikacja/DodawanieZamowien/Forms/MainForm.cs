using DodawanieZamowien.Controler;
using DodawanieZamowien.Forms;
using DodawanieZamowien.Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace DodawanieZamowien
{
    public partial class MainForm : MetroFramework.Forms.MetroForm
    {
        OrderModel order = new OrderModel();
        public MainForm()
        {
            InitializeComponent();
            DisplayOrderItems();
        }

        private void DisplayOrderItems()
        {
            try
            {
                gridProducts.DataSource = order.Items.ToList();
                gridProducts.Columns[0].HeaderText = "Nazwa produktu";
                gridProducts.Columns[1].HeaderText = "Cena";
                gridProducts.Columns[2].HeaderText = "Il sztuk";
            }
            catch (Exception e)
            {
                MetroFramework.MetroMessageBox.Show(this, "Błąd wyświetlania listy pozycji zamówienia!" + Environment.NewLine + e.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                var addProductForm = new AddProductForm();
                if (addProductForm.ShowDialog() == DialogResult.OK)
                {
                    OrderControler.AddItemToOrder(order, addProductForm.SelectedOrderItem);
                    DisplayOrderItems();
                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, "Błąd dodawania produktu!" + Environment.NewLine + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridProducts.CurrentRow != null)
                {
                    OrderItemModel itemToDelete = order.Items.First(o => o.Name == gridProducts.CurrentRow.Cells[0].Value.ToString());
                    order.Items.Remove(itemToDelete);
                    DisplayOrderItems();
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Nie zaznaczono żadnej pozycji!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, "Błąd usuwania produktu!" + Environment.NewLine + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridProducts.CurrentRow != null)
                {
                    string productName = gridProducts.CurrentRow.Cells[0].Value.ToString();
                    int quantity = int.Parse(gridProducts.CurrentRow.Cells[2].Value.ToString());

                    var addProductForm = new AddProductForm(productName,quantity);
                    if (addProductForm.ShowDialog() == DialogResult.OK)
                    {
                        OrderItemModel itemToDelete = order.Items.First(o => o.Name == gridProducts.CurrentRow.Cells[0].Value.ToString());
                        order.Items.Remove(itemToDelete);
                        OrderControler.AddItemToOrder(order, addProductForm.SelectedOrderItem);
                        DisplayOrderItems();
                    }
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Nie zaznaczono żadnej pozycji!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, "Błąd edytowania produktu!" + Environment.NewLine + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreateXML_Click(object sender, EventArgs e)
        {
            try
            {
                if (LoadData())
                {
                    OrderControler.CreateXML(order);
                    MetroFramework.MetroMessageBox.Show(this, "Pomyślnie wygenerowano plik XML.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, "Błąd tworzenia pliku xml!" + Environment.NewLine + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool LoadData()
        {
            order.Name = txtName.Text;
            order.LastName = txtLastname.Text;
            order.DateOfBirdth = dtDateOfBirdth.Value;

            if(string.IsNullOrWhiteSpace(order.Name))
            {
                MetroFramework.MetroMessageBox.Show(this, "Wypełnij pole z imieniem!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if(string.IsNullOrWhiteSpace(order.LastName))
            {
                MetroFramework.MetroMessageBox.Show(this, "Wypełnij pole z nazwiskiem!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (order.Items.Count==0)
            {
                MetroFramework.MetroMessageBox.Show(this, "Dodaj pozycje do zamówienia!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnSaveOrderToDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                if (LoadData())
                {
                    OrderControler.SaveOrderToDataBase(order);
                    MetroFramework.MetroMessageBox.Show(this, "Pomyślnie zapisano zamówienie do bazy.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, "Błąd zapisywania zamówienia do bazy!" + Environment.NewLine + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
