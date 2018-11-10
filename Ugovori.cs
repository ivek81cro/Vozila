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
using System.Text.RegularExpressions;
using System.Globalization;

namespace Automobili
{
    public partial class Ugovori : Form
    {
        string pravaA;
        string korisnikIme;
        public Ugovori(string korisnik, string prava)
        {
            InitializeComponent();
            popuniCombobox();
            pravaA = prava;
            korisnikIme = korisnik;
            dateOd.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            dateOd.Format = DateTimePickerFormat.Custom;
            dateDo.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            dateDo.Format = DateTimePickerFormat.Custom;
            dateVozRodjen.CustomFormat = "dd.MM.yyyy";
            dateVozRodjen.Format = DateTimePickerFormat.Custom;
            loadDataGrid();
            Autocompletekorisnik();
            Autocompletevozac();
            int brojRedova = dataGridView2.RowCount;
            label30.Text = brojRedova.ToString();
            pravaA = prava;
            if (prava != "0")
            {
                btnBrisanje.Visible = false;
            }
            txtUprava.Text = "-";
            txtSektor.Text = "-";
        }
        DataTable dbdataset;
        void loadDataGrid()
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            MySqlConnection connection = new MySqlConnection(constring);
            MySqlCommand cmdDataBase = new MySqlCommand("select brugovora as 'Br.ugovora', regoznaka as 'Reg.oznaka'" +
                ", model as 'Model', kategorija as'Kategorija', od as 'Od datuma', do as 'Do datuma', dana as 'Ukupno dana'," +
                "korisnik as 'Korisnik', vozac as 'Vozac', pocetnokm as 'Pocetna km', otvorio as 'Ugovor Otvorio'," + 
                "zavrsnokm as 'Zavrsna km', zakljucio as 'Ugovor zakljucio'" + 
                ",statusug as 'Status', napomena as 'Napomena' from madjo.ugovori;", connection);

            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = cmdDataBase;
                dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bsource = new BindingSource();

                bsource.DataSource = dbdataset;
                dataGridView2.DataSource = bsource;
                sda.Update(dbdataset);

                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void popuniCombobox()
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.automobili where status='Slobodno';";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;
            comboBox1.Items.Clear();
            comboBox1.Text = "";

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string registracije = citac.GetString("regoznaka");
                    comboBox1.Items.Add(registracije);
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void popuniComboboxSD()
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.uprava_sd where firma='" + txtKorisnik.Text + "';";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;
            txtUprava.Items.Clear();
            txtUprava.Text = "";
            txtUprava.Items.Add('-');

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string uprava = citac.GetString("firma_uprava");
                    txtUprava.Items.Add(uprava);
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void popuniComboboxSektor()
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.uprava_sd where firma='" + txtKorisnik.Text + "' and firma_uprava='" + txtUprava.Text + "';";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;
            txtSektor.Items.Clear();
            txtSektor.Text = "";
            txtSektor.Items.Add('-');

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string sektor = citac.GetString("sektor");
                    txtSektor.Items.Add(sektor);
                }
                veza.Close();
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.automobili where regoznaka='" + comboBox1.Text + "';";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    txtReg.Text = citac.GetString("regoznaka");
                    txtModel.Text = citac.GetString("model");
                    txtPocStanjeKm.Text = citac.GetString("stanjekm");
                    txtKateg.Text = citac.GetString("kategorija");
                    txtStatGoriva.Text = citac.GetString("stanjegoriva");

                }
                veza.Close();
                txtKorisnik.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBrojDana_Click(object sender, EventArgs e)
        {
            string datumOd = dateOd.Text;
            string datumDo = dateDo.Text;
            DateTime mydatetime1 = new DateTime();
            DateTime mydatetime2 = new DateTime();
            mydatetime1 = DateTime.Parse(datumOd);
            mydatetime2 = DateTime.Parse(datumDo);
            TimeSpan ukDana = mydatetime2.Date - mydatetime1.Date;
            int ukDana1 = Convert.ToInt32(ukDana.Days);
            ukDana1++;
            txtUkDana.Text = ukDana1.ToString();

        }
        //Gumb za unos ugovora u bazu podataka
        private void btnUnesi_Click(object sender, EventArgs e)
        {
            if (txtReg.Text != "" && txtBrUg.Text != "" && txtKorisnik.Text != "" && txtVozIme.Text != "")
            {
                int ukDana = Convert.ToInt32(txtUkDana.Text);
                if (txtUkDana.Text != "" && ukDana > 0)
                {
                    var brugovora = Regex.Match(txtBrUg.Text, @"\d+").Value;
                    string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
                    string upit = "insert into madjo.ugovori (regoznaka, model, kategorija, brugovora, od, do, dana, korisnik, vozac, statusug," +
                                  " otvorio, zakljucio, napomena, vozrodjen, vozmjesto, vozbroi, vozbrvozacka, vozadresa, vozbrputovnice," +
                                  " pocetnokm, zavrsnokm)" +
                                  "values ('" + txtReg.Text + "','" + txtModel.Text + "','" + txtKateg.Text + "','" + brugovora + "','" + dateOd.Text +
                                  "','" + dateDo.Text + "','" + txtUkDana.Text + "','" + txtKorisnik.Text + "','" + txtVozIme.Text +
                                  "','Otvoren','" + korisnikIme + "','" + " " + "','" + txtNapomena.Text + "','" + dateVozRodjen.Text +
                                  "','" + txtVozMjRodj.Text + "','" + txtVozBrOI.Text + "','" + txtVozBrVozacke.Text + "','" + txtVozAdresa.Text +
                                  "','" + txtBrPutovnice.Text + "','" + txtPocStanjeKm.Text + "','" + txtZavStKm.Text + "');" +
                                  "update madjo.automobili set status='U najmu', korisnik='" + txtKorisnik.Text + "', uprava_sd_pf='" +txtUprava.Text + 
                                  "', sektor='" + txtSektor.Text + "',stanjekm='" + txtPocStanjeKm.Text + "' where regoznaka='" + comboBox1.Text + "';";
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
                        MessageBox.Show("Ugovor unesen.");
                        loadDataGrid();
                        ClearAllText(this);
                        this.Refresh();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + Environment.NewLine + "Broj ugovora vec postoji.");
                    }
                    popuniCombobox();
                }
                else
                {
                    MessageBox.Show("Provjerite dali ste unjeli tocno datume i da je ukupan broj dana veci od 0.");
                }
            }
            else
            {
                MessageBox.Show("Odaberite vozilo i provjerite dali ste unjeli ostale podatke.");
            }
        }

        private void btnZakljuci_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text != "Zakljucen")
            {
                if (txtZavStKm.Text != "" && txtUkPrijedjeno.Text != "" && txtPoDanu.Text != "" && txtStatGoriva.Text != "")
                {
                    int pocKm = Convert.ToInt32(txtPocStanjeKm.Text);
                    int zavrKm = Convert.ToInt32(txtZavStKm.Text);
                    if (pocKm < zavrKm)
                    {
                        string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
                        string upit = "update madjo.ugovori set statusug='Zakljucen', zakljucio='" + korisnikIme + "', zavrsnokm='" + txtZavStKm.Text +
                                      "',napomena='" + txtNapomena.Text + "',ukcijena='" + txtZaPl.Text + "',rtvpodanu='" + rtvPoDan.ToString("N") + 
                                      "', rtv='" +txtRTV.Text + "', cijenasartv='" + txtUkCjNajma.Text + "',pdv='" + txtPDV.Text + 
                                      "',do='" + dateDo.Text + "', dana='" + txtUkDana.Text + "' where brugovora='" + txtBrUg.Text + "' and zakljucio=' ';" +
                                      "update madjo.automobili set status='Slobodno', stanjekm='" + txtZavStKm.Text + "', stanjegoriva='" +
                                      txtStatGoriva.Text + "', korisnik='-', uprava_sd_pf='-', sektor='-' where regoznaka='" + txtReg.Text + "';";
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
                            MessageBox.Show("Ugovor zakljucen.");
                            loadDataGrid();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        popuniCombobox();
                        ClearAllText(this);
                        this.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Pocetna kilometraza je veca od zavrsne.");
                    }
                }
                else
                {
                    MessageBox.Show("Odaberite ugovor, unesite stanje goriva,\nupisite zavrsnu kilometrazu i kliknite na 'Izracunaj km'.");
                }
            }
            else
            {
                MessageBox.Show("Ugovor je vec zakljucen.");
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnPRL_Click(object sender, EventArgs e)
        {
            PRL prlForma = new PRL(txtBrUg.Text, txtReg.Text, txtModel.Text, txtKorisnik.Text, txtVozIme.Text, dateOd.Text, dateDo.Text, pravaA, korisnikIme );
            prlForma.ShowDialog();
        }

        void Autocompletekorisnik()
        {
            txtKorisnik.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtKorisnik.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection kor = new AutoCompleteStringCollection();

            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.korisnici;";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string nazivK = citac.GetString("naziv");
                    kor.Add(nazivK);
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtKorisnik.AutoCompleteCustomSource = kor;
        }
        void Autocompletevozac()
        {
            txtVozIme.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtVozIme.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection kor = new AutoCompleteStringCollection();

            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.vozac where nazivfirme='" + txtKorisnik.Text + "';";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string nazivV = citac.GetString("ime_prezime");
                    kor.Add(nazivV);
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtVozIme.AutoCompleteCustomSource = kor;
        }
        double CijenaPoDan;
        double rtvPoDan;
        double pdv;
        private void btnRacunajKm_Click(object sender, EventArgs e)
        {
            if (txtPocStanjeKm.Text != "")
            {
                int pocetnakm = Convert.ToInt32(txtPocStanjeKm.Text);
                int zavrsnostkm = Convert.ToInt32(txtZavStKm.Text);
                int prijedenoKm = zavrsnostkm - pocetnakm;
                txtUkPrijedjeno.Text = prijedenoKm.ToString();

                string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
                string upit = "select * from madjo.kategorije where kategorija='" + txtKateg.Text + "';";
                MySqlConnection veza = new MySqlConnection(constring);
                MySqlCommand naredba = new MySqlCommand(upit, veza);
                MySqlDataReader citac;

                try
                {

                    veza.Open();
                    citac = naredba.ExecuteReader();
                    while (citac.Read())
                    {
                        CijenaPoDan = citac.GetDouble("cijena");

                    }
                    veza.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                txtPoDanu.Text = CijenaPoDan.ToString("N");

                double ukDana = Double.Parse(txtUkDana.Text);
                double ukCijena = CijenaPoDan * ukDana;


                double zaPlatit = Math.Round(ukCijena, 2);
                txtZaPl.Text = zaPlatit.ToString("N");

                int trenutniMj = DateTime.Today.Month;
                int trenutnaGod = DateTime.Today.Year;
                int danaUmj = DateTime.DaysInMonth(trenutnaGod, trenutniMj);
                double rtvPoDan1 = 80.00 / danaUmj;
                rtvPoDan = Math.Round(rtvPoDan1, 2);
                double rtv = rtvPoDan * ukDana;
                double rtvRound = Math.Round(rtv, 2);
                if (Math.Abs(80 - 1) < Math.Abs(rtvRound) && Math.Abs(rtvRound) < Math.Abs(81 - 1))
                {
                    rtvRound = 80.00;
                }
                else
                {

                }

                double izrPdv = (ukCijena + rtvRound) * 0.25;
                pdv = Math.Round(izrPdv, 2);
                txtPDV.Text = pdv.ToString("N");

                txtRTV.Text = rtvRound.ToString("N");

                double ukCijNaj1 = ukCijena + pdv + rtvRound;
                double ukCijNaj = Math.Round(ukCijNaj1, 2);
                txtUkCjNajma.Text = ukCijNaj.ToString("N");
            }
            else
            {
                MessageBox.Show("Provjerite dali ste unjeli pocetno stanje kilometara");
            }

            

        }

        private void txtKorisnik_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                String selItem = this.txtKorisnik.Text;
                Autocompletevozac();
                popuniComboboxSD();
                popuniComboboxSektor();
            }
        }

        private void txtVozIme_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                String selItem = this.txtVozIme.Text;

                string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
                string upit = "select * from madjo.vozac where ime_prezime='" + selItem + "';";
                MySqlConnection veza = new MySqlConnection(constring);
                MySqlCommand naredba = new MySqlCommand(upit, veza);
                MySqlDataReader citac;

                try
                {
                    veza.Open();
                    citac = naredba.ExecuteReader();
                    while (citac.Read())
                    {
                        txtBrPutovnice.Text = citac.GetString("brojputovnice");
                        txtVozBrOI.Text = citac.GetString("brojosobne");
                        txtVozMjRodj.Text = citac.GetString("mjestorodjenja");
                        txtVozBrVozacke.Text = citac.GetString("brojvozacke");
                        txtVozAdresa.Text = citac.GetString("adresa");
                        string datumR = citac.GetString("datrodenja");
                        DateTime dtOd = DateTime.Parse(datumR);
                        dateVozRodjen.Value = dtOd;
                    }
                    veza.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void txtFKorisnik_TextChanged(object sender, EventArgs e)
        {
            DataView dw = new DataView(dbdataset);
            dw.RowFilter = string.Format("Korisnik LIKE '%{0}%'", txtFKorisnik.Text);
            dataGridView2.DataSource = dw;
            int brojRedova = dataGridView2.RowCount;
            label30.Text = brojRedova.ToString();
        }

        private void txtFStatus_TextChanged(object sender, EventArgs e)
        {
            DataView dw = new DataView(dbdataset);
            dw.RowFilter = string.Format("Status LIKE '%{0}%'", txtFStatus.Text);
            dataGridView2.DataSource = dw;
            int brojRedova = dataGridView2.RowCount;
            label30.Text = brojRedova.ToString();
        }

        private void txtFReg_TextChanged(object sender, EventArgs e)
        {
            DataView dw = new DataView(dbdataset);
            dw.RowFilter = string.Format("Reg.oznaka LIKE '%{0}%'", txtFReg.Text);
            dataGridView2.DataSource = dw;
            int brojRedova = dataGridView2.RowCount;
            label30.Text = brojRedova.ToString();
        }

        private void txtFBrUg_TextChanged(object sender, EventArgs e)
        {
            DataView dw = new DataView(dbdataset);
            dw.RowFilter = string.Format("Br.ugovora LIKE '%{0}%'", txtFBrUg.Text);
            dataGridView2.DataSource = dw;
            int brojRedova = dataGridView2.RowCount;
            label30.Text = brojRedova.ToString();
        }
        //gumb za brisanje ugovora
        private void btnBrisanje_Click(object sender, EventArgs e)
        {
            if (txtBrUg.Text != "")
            {
                string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
                string upit = "delete from madjo.ugovori where brugovora='" + txtBrUg.Text + "';";
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
                    MessageBox.Show("Ugovor obrisan.");
                    loadDataGrid();
                    txtBrUg.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + Environment.NewLine + "Odaberite ugovor.");
                }
                popuniCombobox();
            }
            else
            {
                MessageBox.Show("Odaberite ugovor.");
            }
        }
        //ucitavanje odabranih podataka iz datagridview-a
        private void dataGridView2_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            comboBox1.Text = "";
            if (e.RowIndex >= 0)
            {
                DataGridViewRow red = this.dataGridView2.Rows[e.RowIndex];

                txtReg.Text = red.Cells["Reg.oznaka"].Value.ToString();
                txtBrUg.Text = red.Cells["Br.ugovora"].Value.ToString();
                txtKorisnik.Text = red.Cells["Korisnik"].Value.ToString();
                txtVozIme.Text = red.Cells["Vozac"].Value.ToString();
                txtUkDana.Text = red.Cells["Ukupno dana"].Value.ToString();
                txtModel.Text = red.Cells["Model"].Value.ToString();
                txtNapomena.Text = red.Cells["Napomena"].Value.ToString();
                txtPocStanjeKm.Text = red.Cells["Pocetna km"].Value.ToString();
                txtKateg.Text = red.Cells["Kategorija"].Value.ToString();
                string datumOd = red.Cells["Od datuma"].Value.ToString();
                DateTime dtOd = DateTime.Parse(datumOd);
                dateOd.Value = dtOd;
                string datumDo = red.Cells["Do datuma"].Value.ToString();
                DateTime dtDo = DateTime.Parse(datumDo);
                dateDo.Value = dtDo;
                lblStatus.Text = red.Cells["Status"].Value.ToString();
                if (lblStatus.Text == "Zakljucen")
                {
                    if (lblStatus.ForeColor != System.Drawing.Color.Green)
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    //lblStatus.Text = red.Cells["Status"].Value.ToString();
                }
                if (lblStatus.Text == "Otvoren")
                {
                    if (lblStatus.ForeColor != System.Drawing.Color.Red)
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                    //lblStatus.Text = red.Cells["Status"].Value.ToString();
                }

            }
        }
        //kontrola za unos zavrsne kilometraze, samo brojevi, backspace i del key
        private void txtZavStKm_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtPocStanjeKm_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtUprava_TextChanged(object sender, EventArgs e)
        {
            popuniComboboxSektor();
        }

        private void Ugovori_Activated(object sender, EventArgs e)
        {
            
        }
    }
}
