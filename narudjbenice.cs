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
    public partial class narudjbenice : Form
    {
        string korisnik;
        string prava;
        public narudjbenice(string korisnikIme, string pravaA)
        {
            InitializeComponent();
            popuniComboboxAuto();
            popuniComboboxPartner();
            korisnik = korisnikIme;
            prava = pravaA;
        }
        void popuniComboboxAuto()
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.automobili";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;
            combVozilo.Items.Clear();
            combVozilo.Text = "";

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string registracije = citac.GetString("regoznaka");
                    combVozilo.Items.Add(registracije);
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void popuniComboboxPartner()
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.korisnici";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;
            comboPartner.Items.Clear();
            comboPartner.Text = "";

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string registracije = citac.GetString("naziv");
                    comboPartner.Items.Add(registracije);
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void combVozilo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.automobili where regoznaka='" + combVozilo.Text + "';";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    lblReg.Text = citac.GetString("regoznaka");
                    lblModel.Text = citac.GetString("model");
                    lblStKm.Text = citac.GetString("stanjekm");
                    lblStGor.Text = citac.GetString("stanjegoriva");

                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboPartner_SelectedIndexChanged(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.korisnici where naziv='" + comboPartner.Text + "';";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    lblNazPart.Text = citac.GetString("naziv");
                    lblAdrPart.Text = "Adresa: " + citac.GetString("adresa");
                    lblOibPar.Text = "OIB: " + citac.GetString("oib");

                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUnos_Click(object sender, EventArgs e)
        {
            int brnar = 0;
            dateNar.Format = DateTimePickerFormat.Custom;
            dateNar.CustomFormat = "dd-MM-yyyy";            
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit1 = "SELECT brnarudb FROM madjo.narudbenice;";            
                          
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit1, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    brnar = citac.GetInt32("brnarudb");
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            string upit = "insert into madjo.narudbenice (brnarudb, registracija, model, stanjekm, stanjegoriva, napomena, partner, adresa, oib," +
                          "datum, dokumenti, oprema, status, izradio)" +
                          "values('" + (brnar+1) + "','" + lblReg.Text + "','" + lblModel.Text + "','" + lblStKm.Text + "','" + lblStGor.Text +
                          "','" + txtNapomena.Text + "','" + lblNazPart.Text + "','" + lblAdrPart.Text + "','" + lblOibPar.Text +
                          "','" + dateNar.Text + "','" + txtOprema.Text + "','" + txtDokumenti.Text + "','Otvoreno', '" + korisnik + "');";
            //"update madjo.automobili set status='Servis' where regoznaka='" + lblReg.Text + "';"
            veza = new MySqlConnection(constring);
            naredba = new MySqlCommand(upit, veza);

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    
                }
                veza.Close();
                MessageBox.Show("Unos završen.");
                NarIspis narPrint = new NarIspis();
                narPrint.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
