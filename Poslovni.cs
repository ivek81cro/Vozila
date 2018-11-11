using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Automobili
{
    public partial class listViewPoslovni : Form
    {
        public listViewPoslovni()
        {
            InitializeComponent();
            LoadDatagrid();
        }
        DataTable dbdataset;
        void LoadDatagrid()
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            MySqlConnection connection = new MySqlConnection(constring);
            MySqlCommand cmdDataBase = new MySqlCommand("select naziv as 'Naziv'" +
                ", adresa as 'Adresa', oib as 'OIB', grad as 'Grad', pobroj as 'Post. broj' from madjo.korisnici;", connection);

            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = cmdDataBase;
                dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bsource = new BindingSource();

                bsource.DataSource = dbdataset;
                dataGridView1.DataSource = bsource;
                sda.Update(dbdataset);

                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void ClearAllText(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Clear();
                else
                    ClearAllText(c);
            }
        }

        private void Poslovni_Load(object sender, EventArgs e)
        {

        }

        private void btnUnos_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "insert into madjo.korisnici (naziv, adresa, oib, grad, pobroj)" +
                          "values ('" + txtNaziv.Text + "','" + txtAdresa.Text + "','" + txtOIB.Text + "','" +
                          txtGrad.Text + "','" + txtPobroj.Text + "');";

            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {

                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            LoadDatagrid();
            ClearAllText(this);
            txtOIB.Text = "";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtNaziv.Text != "")
            {
                string nazivFirme = txtNaziv.Text;                
                Vozaci vozaci = new Vozaci(nazivFirme);
                vozaci.ShowDialog();
            }
            else
            {
                MessageBox.Show("Molim odaberite firmu.");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow red = this.dataGridView1.Rows[e.RowIndex];

                txtNaziv.Text = red.Cells["Naziv"].Value.ToString();
                txtAdresa.Text = red.Cells["Adresa"].Value.ToString();
                txtOIB.Text = red.Cells["OIB"].Value.ToString();
                txtGrad.Text = red.Cells["Grad"].Value.ToString();
                txtPobroj.Text = red.Cells["Post. broj"].Value.ToString();    

            }
            AutocompleteGrupaText();
            AutocompleteGrupa();
            AutocompleteSektor();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Vozaci_preglaed vozPregled = new Vozaci_preglaed();
            vozPregled.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "delete from madjo.korisnici where oib='" + txtOIB.Text + "';" +
                          "delete from madjo.uprava_sd where firma='" + txtNaziv.Text + "';";

            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {

                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            LoadDatagrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "insert into madjo.uprava_sd (firma, firma_uprava, sektor)" +
                          "values ('" + txtNaziv.Text + "','" + txtGrupa.Text + "','" + txtSektor.Text + "');";

            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {

                }
                veza.Close();
                MessageBox.Show("Dodano na popis.");
                ClearAllText(this);
                txtOIB.Text = "";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "";
            if (txtNaziv.Text != "" && txtGrupa.Text != "" && txtSektor.Text != "")
            {
                upit = "delete from madjo.uprava_sd where firma='" + txtNaziv.Text + "' and sektor='" + txtSektor.Text +
                              "' and firma_uprava='" + txtGrupa.Text + "';";
            } 
            else if (txtNaziv.Text != "" && txtGrupa.Text != "")
            {
                upit = "delete from madjo.uprava_sd where firma='" + txtNaziv.Text +
                              "' and firma_uprava='" + txtGrupa.Text + "';";
            }
            else
            {
                MessageBox.Show("Vaš zahtjev ne ispunjava zadane parametre.");
            }

            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {

                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            LoadDatagrid();
        }
        void AutocompleteGrupaText()
        {
            txtGrupa.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtGrupa.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection upr = new AutoCompleteStringCollection();

            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.uprava_sd where firma='" + txtNaziv.Text + "';";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string nazivV = citac.GetString("firma_uprava");
                    upr.Add(nazivV);
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtGrupa.AutoCompleteCustomSource = upr;
        }
        void AutocompleteGrupa()
        {
            

            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.uprava_sd where firma='" + txtNaziv.Text + "' ORDER BY firma_uprava;";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            listViewPartneri.Items.Clear();

            listViewPartneri.View = View.Details;
            if (listViewPartneri.Visible == false)
            {
                listViewPartneri.Columns.Add("Grupa");
                listViewPartneri.Columns.Add("Sektor");
            }
            listViewPartneri.Visible = true;


            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    var item = new ListViewItem();
                    if (citac.GetString("firma_uprava") != "")
                    {
                        item.Text = citac["firma_uprava"].ToString();
                        item.SubItems.Add(citac["sektor"].ToString());
                        listViewPartneri.Items.Add(item);
                    }
                }
                veza.Close();
                listViewPartneri.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //txtGrupa.AutoCompleteCustomSource = upr;
        }
        void AutocompleteSektor()
        {
            txtSektor.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtSektor.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection sek = new AutoCompleteStringCollection();

            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.uprava_sd where firma='" + txtNaziv.Text + "';";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string nazivV = citac.GetString("sektor");
                    sek.Add(nazivV);
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtSektor.AutoCompleteCustomSource = sek;
        }
        private void txtGrupa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                String selItem = this.txtGrupa.Text;
            }
        }

        
    }
}
