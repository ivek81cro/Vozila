using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Automobili
{
    public partial class Ulaz : Form
    {
        public Ulaz()
        {
            InitializeComponent();
            txtKorisnik.Text = "Tomislav";
            txtLozinka.Text = "admin";
        }

        private void btnUlaz_Click(object sender, EventArgs e)
        {
            try
            {
                string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
                string upit = "select * from madjo.korisnik where BINARY ime='" + txtKorisnik.Text + "' and BINARY lozinka='" + 
                               txtLozinka.Text + "';";


                MySqlConnection veza = new MySqlConnection(constring);
                MySqlCommand naredba = new MySqlCommand(upit, veza);
                MySqlDataReader citac;
                veza.Open();
                citac = naredba.ExecuteReader();
                int brojac = 0;

                while (citac.Read())
                {
                    brojac = brojac + 1;
                }
                if (brojac == 1)
                {
                    string prava = citac.GetString("prava");
                    string korisnik = citac.GetString("ime");
                    this.Hide();
                    Glavna glavna = new Glavna(korisnik, prava);
                    glavna.Show();
                }
                else
                {
                    MessageBox.Show("Korisničko ime i/ili lozinka netočna");
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtLozinka_Enter(object sender, EventArgs e)
        {
            AcceptButton = btnUlaz;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.elabdesign.net/Program/Database_madjo.sql");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://downloads.businessobjects.com/akdlm/crnetruntime/clickonce/CRRuntime_32bit_13_0_23.msi");
        }
    }
}
