using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automobili
{
    public partial class Glavna : Form
    {
        string korisnikIme;
        string pravaA;
        public Glavna(string korisnik, string prava)
        {
            InitializeComponent();
            korisnikIme = korisnik;
            toolStripStatusLabel1.Text = korisnikIme;
            pravaA = prava;
        }
        automobili auto;
        Ugovori ugovor;
        listViewPoslovni poslovni;
        narudjbenice narudbenice;
        Narudj_pregled narPr;

        private void automobiliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void ugovoriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ugovor == null)
            {
                ugovor = new Ugovori(korisnikIme, pravaA);
                ugovor.MdiParent = this;
                ugovor.FormClosed += ugovor_FormClosed;
                ugovor.Show();
                ugovor.WindowState = FormWindowState.Maximized;
            }
            else
            {
                ugovor.Close();
                ugovor = new Ugovori(korisnikIme, pravaA);
                ugovor.MdiParent = this;
                ugovor.FormClosed += ugovor_FormClosed;
                ugovor.Show();
                ugovor.WindowState = FormWindowState.Maximized;
            }
        }
        void ugovor_FormClosed(object sender, FormClosedEventArgs e)
        {
            ugovor = null;
            //throw new NotImplementedException();
        }

        private void Glavna_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void poslovniPartneriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (poslovni == null)
            {
                poslovni = new listViewPoslovni();
                poslovni.MdiParent = this;
                poslovni.FormClosed += poslovni_FormClosed;
                poslovni.Show();
                poslovni.WindowState = FormWindowState.Maximized;
            }
            else
            {
                poslovni.Close();
                poslovni = new listViewPoslovni();
                poslovni.MdiParent = this;
                poslovni.FormClosed += poslovni_FormClosed;
                poslovni.Show();
                poslovni.WindowState = FormWindowState.Maximized;
            }
        }
        void poslovni_FormClosed(object sender, FormClosedEventArgs e)
        {
            poslovni = null;
            //throw new NotImplementedException();
        }

        private void ugovoriToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Izv_ugovori izvUgovori = new Izv_ugovori();
            izvUgovori.ShowDialog();
        }

        private void kategorijeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Kategorije kat = new Kategorije();
            kat.ShowDialog();
        }

        private void vozilaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (auto == null)
            {
                auto = new automobili(pravaA, korisnikIme);
                auto.MdiParent = this;
                auto.FormClosed +=auto_FormClosed;
                auto.Show();
                auto.WindowState = FormWindowState.Maximized;
            }
            else
            {
                auto.Close();
                auto = new automobili(pravaA, korisnikIme);
                auto.MdiParent = this;
                auto.FormClosed += auto_FormClosed;
                auto.Show();
                auto.WindowState = FormWindowState.Maximized;
            }
        }
        void auto_FormClosed(object sender, FormClosedEventArgs e)
        {
            auto = null;
            //throw new NotImplementedException();
        }

        private void pRLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime dtOd = DateTime.Now;
            DateTime dtDo = DateTime.Now;

            PRL prlForma = new PRL("", "", "", "", "", dtOd.ToString(), dtDo.ToString(), pravaA, korisnikIme);
            prlForma.ShowDialog();
        }

        private void izradaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (narudbenice == null)
            {
                narudbenice = new narudjbenice(korisnikIme, pravaA);
                narudbenice.MdiParent = this;
                narudbenice.FormClosed += narudbenice_FormClosed;
                narudbenice.Show();
                narudbenice.WindowState = FormWindowState.Maximized;
            }
            else
            {
                narudbenice.Close();
                narudbenice = new narudjbenice(korisnikIme, pravaA);
                narudbenice.MdiParent = this;
                narudbenice.FormClosed += narudbenice_FormClosed;
                narudbenice.Show();
                narudbenice.WindowState = FormWindowState.Maximized;
            }
        }
        void narudbenice_FormClosed(object sender, FormClosedEventArgs e)
        {
            narudbenice = null;
            //throw new NotImplementedException();
        }

        private void pregledIZaključivanjeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (narPr == null)
            {
                narPr = new Narudj_pregled(korisnikIme);
                narPr.MdiParent = this;
                narPr.FormClosed += narPr_FormClosed;
                narPr.Show();
                narPr.WindowState = FormWindowState.Maximized;
            }
            else
            {
                narPr.Close(); 
                narPr = new Narudj_pregled(korisnikIme);
                narPr.MdiParent = this;
                narPr.FormClosed += narPr_FormClosed;
                narPr.Show();
                narPr.WindowState = FormWindowState.Maximized;
            }
        }
        void narPr_FormClosed(object sender, FormClosedEventArgs e)
        {
            narPr = null;
            //throw new NotImplementedException();
        }

        private void finUgovoriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ugovori_financijsko ugFinanc = new Ugovori_financijsko();
            ugFinanc.ShowDialog();
        }

        private void pRLToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PRL_ispis prlIspis = new PRL_ispis();
            prlIspis.ShowDialog();
        }

        private void kopirajToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackupBaze backupBaze;
            backupBaze = new BackupBaze();
            backupBaze.Backup();
        }

        private void vratiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackupBaze backupBaze;
            backupBaze = new BackupBaze();
            backupBaze.Restore();
        }
    }
}
