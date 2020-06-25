using DodawanieZamowien.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DodawanieZamowien.Forms
{
    public partial class AddProductForm : MetroFramework.Forms.MetroForm
    {
        public OrderItemModel SelectedOrderItem { get; private set; } = new OrderItemModel();
        BazaEntities db = new BazaEntities();

        public AddProductForm(string productName="", int quantity=1)
        {
            InitializeComponent();
            txtProcutName.Text = productName;
            numQuntity.Value = quantity;
            DisplayProducts();
        }

        private void DisplayProducts()
        {
            try
            {
                gridProducts.DataSource = db.
                Products.
                Select(p => new { Produkt = p.Prd_Name, Cena = p.Prd_ListPrice }).
                Where(p => p.Produkt.Contains(txtProcutName.Text)).
                ToList();

                gridProducts.Columns[0].HeaderText = "Nazwa produktu";
            }
            catch (Exception e)
            {
                MetroFramework.MetroMessageBox.Show(this, "Błąd wczytywania produktów!" + Environment.NewLine + e.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtProcutName_TextChanged(object sender, EventArgs e)
        {
            DisplayProducts();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (gridProducts.CurrentRow != null)
            {
                SelectedOrderItem = new OrderItemModel
                {
                    Name = gridProducts.CurrentRow.Cells[0].Value.ToString(),
                    Quantity = (int)numQuntity.Value,
                    ListPrice = (decimal)(gridProducts.CurrentRow.Cells[1].Value),
                };
                DialogResult = DialogResult.OK;
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Nie zaznaczono żadnej pozycji!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gridProducts_DoubleClick(object sender, EventArgs e)
        {
            btnAddProduct_Click(sender, e);
        }
    }
}
