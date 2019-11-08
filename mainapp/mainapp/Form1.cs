using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mainapp
{
    public partial class Form1 : Form
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        List<string> stany = new List<string>();
        List<string> decyzje = new List<string>();
        List<double> prawdopodobienstwa = new List<double>();

        int[,] wyniki;
        int[,] straty;
        double p1 = 0.0;
        double p2 = 0.0;
        double p3 = 0.0;
        double p4 = 0.0;
        

        

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonDodajStan_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textboxNowyStan.Text))
            {
                string[] stanyString = textboxNowyStan.Text.Split();
                foreach (string stan in stanyString)
                {
                    stany.Add(stan);
                }

                textboxNowyStan.Text = "";
                WyswietlStany(stany);
            }
            else
            {
                MessageBox.Show("Nie można dodać pustego stanu.", "Błąd");
            }
        }

        private void WyswietlStany(List<string> Stany)
        {
            string stany = null;
            foreach (string stan in Stany)
            {
                stany += stan + " ";
            }

            labelStany.Text = "Stany: " + stany;
        }

        private void buttonUsunStan_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textboxNowyStan.Text))
            {
                string[] stanyString = textboxNowyStan.Text.Split();
                foreach (string stan in stanyString)
                {
                    if (stany.Contains(stan))
                    {
                        stany.Remove(stan);
                    }
                }

                textboxNowyStan.Text = "";
                WyswietlStany(stany);
            }
        }

        private void buttonDodajDecyzje_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textboxNowaDecyzja.Text))
            {
                string[] decyzjeString = textboxNowaDecyzja.Text.Split();
                foreach (string decyzja in decyzjeString)
                {
                    decyzje.Add(decyzja);
                }

                textboxNowaDecyzja.Text = "";
                WyswietlDecyzje(decyzje);
            }
            else
            {
                MessageBox.Show("Nie można dodać pustej decyzji", "Błąd");
            }
        }

        private void WyswietlDecyzje(List<string> Decyzje)
        {
            string decyzje = null;
            foreach (string stan in Decyzje)
            {
                decyzje += stan + " ";
            }

            labelDecyzje.Text = "Decyzje: " + decyzje;
        }

        private void buttonUsunDecyzje_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textboxNowaDecyzja.Text))
            {
                string[] decyzjeString = textboxNowaDecyzja.Text.Split();
                foreach (string decyzja in decyzjeString)
                {
                    if (decyzje.Contains(decyzja))
                    {
                        decyzje.Remove(decyzja);
                    }
                }

                textboxNowaDecyzja.Text = "";
                WyswietlDecyzje(decyzje);
            }
        }

        private void buttonObliczTabele_Click(object sender, EventArgs e)
        {
            if (SprawdzWymagane())
            {
                wyniki = new int[decyzje.Count, stany.Count];
                double koszt = 0;
                double zyskAkademicki = 0;
                double zyskWakacje = 0;
                double wynik = 0;
                dgv.Rows.Clear();

                decyzje.Sort();
                stany.Sort();

                for (int i = 0; i < decyzje.Count; i++)
                {
                    for (int j = 0; j < stany.Count; j++)
                    {
                        switch (Convert.ToInt32(decyzje[i]))
                        {
                            case 200:
                                koszt = 200 * Convert.ToDouble(textboxCenaJednaPartia.Text);
                                zyskAkademicki = 200 * Convert.ToDouble(textboxCenaWRoku.Text);
                                wynik = zyskAkademicki - koszt;
                                wyniki[i, j] = (int)wynik;
                                break;

                            case 400:
                                koszt = 400 * Convert.ToDouble(textboxCenaDwiePartie.Text);
                                if (Convert.ToInt32(stany[j]) <= 400)
                                {
                                    zyskAkademicki = Convert.ToInt32(stany[j]) * Convert.ToDouble(textboxCenaWRoku.Text);
                                    zyskWakacje = (400 - Convert.ToInt32(stany[j])) * Convert.ToDouble(textboxCenaWakacje.Text);
                                    wynik = (zyskAkademicki + zyskWakacje) - koszt;
                                }
                                else
                                {
                                    zyskAkademicki = 400 * Convert.ToDouble(textboxCenaWRoku.Text);
                                    wynik = zyskAkademicki - koszt;
                                }
                                wyniki[i, j] = (int)wynik;
                                break;

                            case 600:
                                koszt = 600 * Convert.ToDouble(textboxCenaTrzyPartie.Text);
                                if (Convert.ToInt32(stany[j]) < 600)
                                {
                                    zyskAkademicki = Convert.ToInt32(stany[j]) * Convert.ToDouble(textboxCenaWRoku.Text);
                                    zyskWakacje = (600 - Convert.ToInt32(stany[j])) * Convert.ToDouble(textboxCenaWakacje.Text);
                                    wynik = (zyskAkademicki + zyskWakacje) - koszt;
                                }
                                else
                                {
                                    zyskAkademicki = 600 * Convert.ToDouble(textboxCenaWRoku.Text);
                                    wynik = zyskAkademicki - koszt;
                                }
                                wyniki[i, j] = (int)wynik;
                                break;

                            default:
                                break;
                        }
                    }
                }

                WypelnijTabele(wyniki);
            }
        }

        private void WypelnijTabele(int[,] wyniki)
        {
            for (int i = 0; i < decyzje.Count; i++)
            {
                switch (stany.Count)
                {
                    case 2:
                        if (dgv.Rows.Count <= 1)
                            dgv.Rows.Add("", stany[0], stany[1]);
                        dgv.Rows.Add(decyzje[i], wyniki[i, 0], wyniki[i, 1]);
                        break;

                    case 3:
                        if (dgv.Rows.Count <= 1)
                            dgv.Rows.Add("", stany[0], stany[1], stany[2]);
                        dgv.Rows.Add(decyzje[i], wyniki[i, 0], wyniki[i, 1], wyniki[i, 2]);
                        break;

                    case 4:
                        if (dgv.Rows.Count <= 1)
                            dgv.Rows.Add("", stany[0], stany[1], stany[2], stany[3]);
                        dgv.Rows.Add(decyzje[i], wyniki[i, 0], wyniki[i, 1], wyniki[i, 2], wyniki[i, 3]);
                        break;

                    case 5:
                        if (dgv.Rows.Count <= 1)
                            dgv.Rows.Add("", stany[0], stany[1], stany[2], stany[3], stany[4]);
                        dgv.Rows.Add(decyzje[i], wyniki[i, 0], wyniki[i, 1], wyniki[i, 2], wyniki[i, 3], wyniki[i, 4]);
                        break;

                    case 6:
                        if (dgv.Rows.Count <= 1)
                            dgv.Rows.Add("", stany[0], stany[1], stany[2], stany[3], stany[4], stany[5]);
                        dgv.Rows.Add(decyzje[i], wyniki[i, 0], wyniki[i, 1], wyniki[i, 2], wyniki[i, 3], wyniki[i, 4], wyniki[i, 5]);
                        break;
                    case 7:

                        if (dgv.Rows.Count <= 1)
                            dgv.Rows.Add("", stany[0], stany[1], stany[2], stany[3], stany[4], stany[5], stany[6]);
                        dgv.Rows.Add(decyzje[i], wyniki[i, 0], wyniki[i, 1], wyniki[i, 2], wyniki[i, 3], wyniki[i, 4], wyniki[i, 5], wyniki[i, 6]);
                        break;

                    default:
                        break;
                }
            }

            ZnajdzDecyzjeZdominowane(wyniki);
        }

        private void ZnajdzDecyzjeZdominowane(int[,] wyniki)
        {
            List<int> indeksy = new List<int>();
            switch (stany.Count)
            {
                case 2:
                    for (int i = 0; i < decyzje.Count; i++)
                    {
                        for (int j = 0; j < decyzje.Count; j++)
                        {
                            if (i != j && wyniki[i, 0] >= wyniki[j, 0] && wyniki[i, 1] >= wyniki[j, 1])
                            {
                                indeksy.Add(j);
                            }
                        }
                    }
                    break;

                case 3:
                    for (int i = 0; i < decyzje.Count; i++)
                    {
                        for (int j = 0; j < decyzje.Count; j++)
                        {
                            if (i != j && wyniki[i, 0] >= wyniki[j, 0] && wyniki[i, 1] >= wyniki[j, 1] && wyniki[i, 2] >= wyniki[j, 2])
                            {
                                indeksy.Add(j);
                            }
                        }
                    }
                    break;

                case 4:
                    for (int i = 0; i < decyzje.Count; i++)
                    {
                        for (int j = 0; j < decyzje.Count; j++)
                        {
                            if (i != j && wyniki[i, 0] >= wyniki[j, 0] && wyniki[i, 1] >= wyniki[j, 1] && wyniki[i, 2] >= wyniki[j, 2] && wyniki[i, 3] >= wyniki[j, 3])
                            {
                                indeksy.Add(j);
                            }
                        }
                    }
                    break;

                case 5:
                    for (int i = 0; i < decyzje.Count; i++)
                    {
                        for (int j = 0; j < decyzje.Count; j++)
                        {
                            if (i != j && wyniki[i, 0] >= wyniki[j, 0] && wyniki[i, 1] >= wyniki[j, 1] && wyniki[i, 2] >= wyniki[j, 2] && wyniki[i, 3] >= wyniki[j, 3] && wyniki[i, 4] >= wyniki[j, 4])
                            {
                                indeksy.Add(j);
                            }
                        }
                    }
                    break;

                case 6:
                    for (int i = 0; i < decyzje.Count; i++)
                    {
                        for (int j = 0; j < decyzje.Count; j++)
                        {
                            if (i != j && wyniki[i, 0] >= wyniki[j, 0] && wyniki[i, 1] >= wyniki[j, 1] && wyniki[i, 2] >= wyniki[j, 2] && wyniki[i, 3] >= wyniki[j, 3] && wyniki[i, 4] >= wyniki[j, 4] && wyniki[i, 5] >= wyniki[j, 5])
                            {
                                indeksy.Add(j);
                            }
                        }
                    }
                    break;
                case 7:
                    for (int i = 0; i < decyzje.Count; i++)
                    {
                        for (int j = 0; j < decyzje.Count; j++)
                        {
                            if (i != j && wyniki[i, 0] >= wyniki[j, 0] && wyniki[i, 1] >= wyniki[j, 1] && wyniki[i, 2] >= wyniki[j, 2] && wyniki[i, 3] >= wyniki[j, 3] && wyniki[i, 4] >= wyniki[j, 4] && wyniki[i, 5] >= wyniki[j, 5] && wyniki[i, 6] >= wyniki[j, 6])
                            {
                                indeksy.Add(j);
                            }
                        }
                    }
                    break;

                default:
                    break;
            }

            if (indeksy.Count <= 0)
            {
                labelDecyzjeZdominowane.Text = "Decyzje zdominowane: brak";
            }
            else
            {
                string ind = null;
                foreach (int i in indeksy.Distinct())               {
                    ind += dgv.Rows[i + 1].Cells[0].Value.ToString() + " ";
                }

                labelDecyzjeZdominowane.Text = "Decyzje zdominowane: " + ind;
            }
        }

        private bool SprawdzWymagane()
        {

            if (!String.IsNullOrEmpty(textboxCenaJednaPartia.Text) &&
                !String.IsNullOrEmpty(textboxCenaJednaPartia.Text) && !String.IsNullOrEmpty(textboxCenaJednaPartia.Text) &&
                !String.IsNullOrEmpty(textboxCenaJednaPartia.Text) && !String.IsNullOrEmpty(textboxCenaJednaPartia.Text))
            {
                if (stany.Count > 0 && decyzje.Count > 0)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Brak stanów i decyzji.", "Błąd");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Nie wypełniono wymaganych pól.", "Błąd");
                return false;
            }
        }

        private void buttonKryteriumHurwicza_Click(object sender, EventArgs e)
        {
            List<WierszKolumna> najlepsze = new List<WierszKolumna>();
            string kryterium = "Hurwicza";

            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }
            }

            foreach (DataGridViewRow row in dgv2.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }
            }

            for (int i = 0; i < decyzje.Count; i++)
            {
                for (int j = 0; j < stany.Count; j++)
                {
                    if (j == 0)
                    {
                        WierszKolumna tmp = new WierszKolumna();
                        tmp.Wartosc = wyniki[i, j];
                        tmp.Wiersz = i;
                        tmp.Kolumna = j;

                        najlepsze.Add(tmp);
                    }
                    else
                    {
                        WierszKolumna tmp2 = new WierszKolumna();
                        tmp2.Wartosc = wyniki[i, j];
                        tmp2.Wiersz = i;
                        tmp2.Kolumna = j;

                        if (tmp2.Wartosc == najlepsze.Last().Wartosc)
                        {
                            najlepsze.Add(tmp2);
                        }
                        else if (tmp2.Wartosc > najlepsze.Last().Wartosc)
                        {
                            najlepsze.Clear();
                            najlepsze.Add(tmp2);
                        }
                    }
                }
            }

            foreach (WierszKolumna wk in najlepsze)
            {
                dgv.Rows[wk.Wiersz + 1].Cells[wk.Kolumna + 1].Style.BackColor = Color.Green;
            }

            WypiszInformacjeONajlepszejDecyzji(kryterium, najlepsze.First().Wiersz);
        }

        private void buttonKryteriumWalda_Click(object sender, EventArgs e)
        {
            List<WierszKolumna> najgorsze = new List<WierszKolumna>();
            List<WierszKolumna> najlepszeZNajgorszych = new List<WierszKolumna>();
            string kryterium = "Walda";

            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }
            }

            foreach (DataGridViewRow row in dgv2.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }
            }

            for (int i = 0; i < decyzje.Count; i++)
            {
                List<WierszKolumna> najgorszeNaWiersz = new List<WierszKolumna>();
                for (int j = 0; j < stany.Count; j++)
                {
                    if (j == 0)
                    {
                        WierszKolumna tmp = new WierszKolumna();
                        tmp.Wartosc = wyniki[i, j];
                        tmp.Wiersz = i;
                        tmp.Kolumna = j;

                        najgorszeNaWiersz.Add(tmp);
                    }
                    else
                    {
                        WierszKolumna tmp2 = new WierszKolumna();
                        tmp2.Wartosc = wyniki[i, j];
                        tmp2.Wiersz = i;
                        tmp2.Kolumna = j;

                        if (tmp2.Wartosc == najgorszeNaWiersz.Last().Wartosc)
                        {
                            najgorszeNaWiersz.Add(tmp2);
                        }
                        else if (tmp2.Wartosc < najgorszeNaWiersz.Last().Wartosc)
                        {
                            najgorszeNaWiersz.Clear();
                            najgorszeNaWiersz.Add(tmp2);
                        }
                    }
                }

                foreach (WierszKolumna wk in najgorszeNaWiersz)
                {
                    najgorsze.Add(wk);
                }

                najgorszeNaWiersz.Clear();
            }

            najgorsze.Sort(delegate (WierszKolumna c1, WierszKolumna c2) { return c1.Wartosc.CompareTo(c2.Wartosc); });

            foreach (WierszKolumna wk in najgorsze)
            {
                if (wk.Wartosc == najgorsze.Last().Wartosc)
                {
                    najlepszeZNajgorszych.Add(wk);
                }
            }

            foreach (WierszKolumna wk in najlepszeZNajgorszych)
            {
                dgv.Rows[wk.Wiersz + 1].Cells[wk.Kolumna + 1].Style.BackColor = Color.Green;
            }

            WypiszInformacjeONajlepszejDecyzji(kryterium, najlepszeZNajgorszych.First().Wiersz);
        }

        private void buttonKryteriumSavagea_Click(object sender, EventArgs e)
        {
            List<WierszKolumna> najgorsze = new List<WierszKolumna>();
            List<WierszKolumna> najlepszeZNajgorszych = new List<WierszKolumna>();
            string kryterium = "Savage'a";

            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }
            }

            foreach (DataGridViewRow row in dgv2.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }
            }

            for (int i = 0; i < decyzje.Count; i++)
            {
                List<WierszKolumna> najgorszeNaWiersz = new List<WierszKolumna>();
                for (int j = 0; j < stany.Count; j++)
                {
                    if (j == 0)
                    {
                        WierszKolumna tmp = new WierszKolumna();
                        tmp.Wartosc = straty[i, j];
                        tmp.Wiersz = i;
                        tmp.Kolumna = j;

                        najgorszeNaWiersz.Add(tmp);
                    }
                    else
                    {
                        WierszKolumna tmp2 = new WierszKolumna();
                        tmp2.Wartosc = straty[i, j];
                        tmp2.Wiersz = i;
                        tmp2.Kolumna = j;

                        if (tmp2.Wartosc == najgorszeNaWiersz.Last().Wartosc)
                        {
                            najgorszeNaWiersz.Add(tmp2);
                        }
                        else if (tmp2.Wartosc > najgorszeNaWiersz.Last().Wartosc)
                        {
                            najgorszeNaWiersz.Clear();
                            najgorszeNaWiersz.Add(tmp2);
                        }
                    }
                }

                foreach (WierszKolumna wk in najgorszeNaWiersz)
                {
                    najgorsze.Add(wk);
                }

                najgorszeNaWiersz.Clear();
            }

            najgorsze.Sort(delegate (WierszKolumna c1, WierszKolumna c2) { return c1.Wartosc.CompareTo(c2.Wartosc); });

            foreach (WierszKolumna wk in najgorsze)
            {
                if (wk.Wartosc == najgorsze.First().Wartosc)
                {
                    najlepszeZNajgorszych.Add(wk);
                }
            }

            foreach (WierszKolumna wk in najlepszeZNajgorszych)
            {
                dgv2.Rows[wk.Wiersz].Cells[wk.Kolumna].Style.BackColor = Color.Green;
            }

            foreach (WierszKolumna wk in najlepszeZNajgorszych)
            {
                dgv.Rows[wk.Wiersz + 1].Cells[wk.Kolumna + 1].Style.BackColor = Color.Green;
            }

            WypiszInformacjeONajlepszejDecyzji(kryterium, najlepszeZNajgorszych.First().Wiersz);
        }

        private void buttonKryteriumLaplacea_Click(object sender, EventArgs e)
        {
            string kryterium = "LaPlace'a";

            p1 = Convert.ToDouble(textboxP1.Text);
            p2 = Convert.ToDouble(textboxP2.Text);
            p3 = Convert.ToDouble(textboxP3.Text);
            p4 = Convert.ToDouble(textboxP4.Text);
            prawdopodobienstwa.Add(p1);
            prawdopodobienstwa.Add(p2);
            prawdopodobienstwa.Add(p3);
            prawdopodobienstwa.Add(p4);

            double sredniaWartoscDecyzji = 0.0;
            List<IndeksWartosc> srednieWartosciDecyzji = new List<IndeksWartosc>();
            double sredniaStrataDecyzji = 0.0;
            List<IndeksWartosc> srednieStratyDecyzji = new List<IndeksWartosc>();

            List<double> najlepszeWKolumnie = new List<double>();
            List<double> najlepszeDlaStanuNatury = new List<double>();

            double OWDI = 0.0;
            double OW = 0.0;
            double ODI = 0.0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }
            }

            foreach (DataGridViewRow row in dgv2.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }
            }

            for (int i = 0; i < decyzje.Count; i++)
            {
                for (int j = 0; j < stany.Count; j++)
                {
                    sredniaWartoscDecyzji += wyniki[i, j] * prawdopodobienstwa[j];
                }

                IndeksWartosc tmp = new IndeksWartosc();
                tmp.Indeks = i + 1;
                tmp.Wartosc = sredniaWartoscDecyzji;

                srednieWartosciDecyzji.Add(tmp);
                sredniaWartoscDecyzji = 0.0;
            }

            for (int i = 0; i < decyzje.Count; i++)
            {
                for (int j = 0; j < stany.Count; j++)
                {
                    sredniaStrataDecyzji += straty[i, j] * prawdopodobienstwa[j];
                }

                IndeksWartosc tmp = new IndeksWartosc();
                tmp.Indeks = i + 1;
                tmp.Wartosc = sredniaStrataDecyzji;

                srednieStratyDecyzji.Add(tmp);
                sredniaStrataDecyzji = 0.0;
            }

            textboxSredniZysk1.Text = srednieWartosciDecyzji[0].Wartosc.ToString();
            textboxSredniZysk2.Text = srednieWartosciDecyzji[1].Wartosc.ToString();
            textboxSredniZysk3.Text = srednieWartosciDecyzji[2].Wartosc.ToString();


            srednieWartosciDecyzji.Sort(delegate (IndeksWartosc c1, IndeksWartosc c2) { return c1.Wartosc.CompareTo(c2.Wartosc); });
            dgv.Rows[srednieWartosciDecyzji.Last().Indeks].Cells[0].Style.BackColor = Color.Green;

            OW = srednieWartosciDecyzji.Last().Wartosc;

            textboxSredniaStrata1.Text = srednieStratyDecyzji[0].Wartosc.ToString();
            textboxSredniaStrata2.Text = srednieStratyDecyzji[1].Wartosc.ToString();
            textboxSredniaStrata3.Text = srednieStratyDecyzji[2].Wartosc.ToString();

            srednieStratyDecyzji.Sort(delegate (IndeksWartosc c1, IndeksWartosc c2) { return c1.Wartosc.CompareTo(c2.Wartosc); });
            dgv2.Rows[srednieStratyDecyzji.First().Indeks - 1].Cells[0].Style.BackColor = Color.Green;

            textboxOW.Text = OW.ToString();

            for (int i = 0; i < stany.Count; i++)
            {
                for (int j = 0; j < decyzje.Count; j++)
                {
                    if (j == 0)
                    {
                        najlepszeWKolumnie.Add(wyniki[j, i]);
                    }
                    else
                    {
                        if (wyniki[j, i] > najlepszeWKolumnie.Last())
                        {
                            najlepszeWKolumnie.Clear();
                            najlepszeWKolumnie.Add(wyniki[j, i]);
                        }
                    }
                }
                najlepszeDlaStanuNatury.Add(najlepszeWKolumnie.Last());
                najlepszeWKolumnie.Clear();
            }

            for (int j = 0; j < stany.Count; j++)
            {
                OWDI += najlepszeDlaStanuNatury[j] * prawdopodobienstwa[j];
            }

            textboxOWDI.Text = OWDI.ToString();

            ODI = OWDI - OW;
            textboxODI.Text = ODI.ToString();

            WypiszInformacjeONajlepszejDecyzji(kryterium, srednieWartosciDecyzji.Last().Indeks - 1);

            prawdopodobienstwa.Clear();
            srednieWartosciDecyzji.Clear();
            srednieStratyDecyzji.Clear();

            OWDI = 0.0;
            ODI = 0.0;
            OW = 0.0;
        }

        private void WypiszInformacjeONajlepszejDecyzji(string kryterium, int decyzja)
        {
            String opis = "";
            opis += "W oparciu o kryterium " + kryterium + ", najlepsza decyzja to zakup " + decyzje[decyzja] + " kremów do opalania.";

            MessageBox.Show(opis, "Podjęto decyzję");
        }

        private void buttonWczytajZPliku_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Style.BackColor = Color.White;
                    }
                }

                decyzje.Clear();
                stany.Clear();
                dgv.Rows.Clear();

                StreamReader sr = new StreamReader(openFileDialog.FileName);

                List<string> S = sr.ReadLine().Split().ToList();

                if (S[0].Equals(""))
                {
                    S.RemoveAt(0);

                }

                if (S[S.Count - 1].Equals(""))
                {
                    S.RemoveAt(S.Count - 1);
                }

                foreach (string element in S)
                {
                    stany.Add(element);
                }

                int wiersze = File.ReadAllLines(openFileDialog.FileName).Length - 1;

                for (int i = 0; i < wiersze; i++)
                {
                    string wiersz = sr.ReadLine();
                    if (wiersz.Length == 0 || wiersz.Length == 1)
                    {
                    }
                    else
                    {
                        string[] wierszPodzielony = wiersz.Split();
                        decyzje.Add(wierszPodzielony[0]);
                    }
                }

                wyniki = new int[decyzje.Count, stany.Count];
                sr.Close();

                StreamReader sr2 = new StreamReader(openFileDialog.FileName);

                for (int i = 0; i < File.ReadAllLines(openFileDialog.FileName).Length; i++)
                {
                    string wiersz = sr2.ReadLine();
                    if (i == 0)
                    { }
                    else if (wiersz.Length == 0 || wiersz.Length == 1)
                    {
                    }
                    else
                    {
                        string[] wierszPodzielony = wiersz.Split();
                        for (int j = 0; j < wierszPodzielony.Length; j++)
                        {
                            if (j != 0 && !wierszPodzielony[j].Equals(""))
                            {
                                wyniki[i - 1, j - 1] = Convert.ToInt32(wierszPodzielony[j]);
                            }
                        }
                    }
                }

                sr2.Close();

                WyswietlDecyzje(decyzje);
                WyswietlStany(stany);
                WypelnijTabele(wyniki);

                ObliczTabeleStrat(wyniki);
            }
        }

        private void buttonZapiszDoPliku_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName.Contains(".txt") ? saveFileDialog.FileName : saveFileDialog.FileName + ".txt";
                StreamWriter sw = new StreamWriter(fileName);

                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < stany.Count + 1; j++)
                    {
                        sw.Write(dgv.Rows[i].Cells[j].Value.ToString() + '\t');
                        if (j == stany.Count)
                        {
                            sw.WriteLine('\n');
                        }
                    }
                }

                sw.Close();
            }
        }

        private void ObliczTabeleStrat(int[,] Wyniki)
        {
            List<int> najlepszeWKolumnie = new List<int>();
            straty = new int[decyzje.Count, stany.Count];
            int najlepszy = 0;
            int najlepszy2 = 0;

            dgv2.Rows.Clear();
            dgv2.Columns.Clear();

            for (int i = 0; i < stany.Count(); i++)
            {
                for (int j = 0; j < decyzje.Count(); j++)
                {
                    if (i == 0 && j == 0)
                    {
                        najlepszy = Wyniki[j, i];
                    }
                    else
                    {
                        najlepszy2 = Wyniki[j, i];
                        if (najlepszy2 > najlepszy)
                        {
                            najlepszy = najlepszy2;
                        }
                    }
                }
                najlepszeWKolumnie.Add(najlepszy);
            }

            for (int i = 0; i < stany.Count(); i++)
            {
                for (int j = 0; j < decyzje.Count(); j++)
                {
                    straty[j, i] = najlepszeWKolumnie[i] - Wyniki[j, i];
                }
            }

            foreach (var item in stany)
            {
                dgv2.Columns.Add("Column", "Strata");
            }

            foreach (DataGridViewColumn item in dgv2.Columns)
            {
                item.Width = 60;
            }

            for (int i = 0; i < decyzje.Count(); i++)
            {
                dgv2.Rows.Add();
                for (int j = 0; j < stany.Count(); j++)
                {
                    dgv2.Rows[i].Cells[j].Value = straty[i, j];
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void TextboxP1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Autor: Jolanta Podolszańska","Program wspomagający decyzje");
        }
    }
}
