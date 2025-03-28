using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using autobazar.Models;
using autobazar.Utilities;
using Microsoft.Win32;

namespace autobazar.ViewModels
{
    internal class VozyViewModel : BaseViewModel
    {
        private ObservableCollection<Vuz> _vozy;
        public ObservableCollection<Tuple<string, double, double>> SeskupeneVozy { get; set; } = new();

        public RelayCommand VyplnTabulkyCommand => new RelayCommand(_execute => VyplnTabulky(), _canExecute => { return true; });



        public ObservableCollection<Vuz> Vozy
        {
            get { return _vozy; }
            set
            {
                _vozy = value;
                OnPropertyChanged(nameof(Vozy));
            }
        }

        private void VyplnTabulky()
        {
            OpenFileDialog otevriSoubor = new OpenFileDialog();
            otevriSoubor.Filter = "XML Files (*.xml)|*.xml";
            bool? vysledek = otevriSoubor.ShowDialog();
            if (vysledek == true)
            {
                string soubor = otevriSoubor.FileName;
                NactiData(soubor);
                SpocitejSeskupeneCeny();
            }
            else
            {
                MessageBox.Show("XML soubor nevybran");
            }
        }
        public void NactiData(string cesta)
        {
            XmlDocument dokument = new XmlDocument();
            dokument.Load(cesta);
            XmlNodeList nodes = dokument.SelectNodes("Vozy/Vuz");
            Vozy = new ObservableCollection<Vuz>();
            foreach (XmlNode node in nodes)
            {
                Vuz vuz = new Vuz
                {
                    Model = node.SelectSingleNode("Model")?.InnerText ?? "Neznámý model",
                    Datum = DateTime.TryParse(node.SelectSingleNode("DatumProdeje")?.InnerText, out DateTime datum) ? datum : DateTime.Now,
                    BezDPH = double.TryParse(node.SelectSingleNode("Cena")?.InnerText, out double cena) ? cena : 0.0,
                    DPH = double.TryParse(node.SelectSingleNode("DPH")?.InnerText, out double dph) ? dph : 0.0
                };

                Vozy.Add(vuz);
            }
        }

        public void SpocitejSeskupeneCeny()
        {
            SeskupeneVozy.Clear();


            var soucetZmen = Vozy
                .Where(v => v.Datum.DayOfWeek == DayOfWeek.Saturday || v.Datum.DayOfWeek == DayOfWeek.Sunday)
                .GroupBy(v => v.Model)
                .Select(g => Tuple.Create(
                    g.Key ?? "Neznámý model",
                    g.Sum(v => v.BezDPH),
                    g.Sum(v => v.Cena)
                ))
                .ToList();

            foreach (var vuz in soucetZmen)
            {
                SeskupeneVozy.Add(vuz);
            }

        }
    }
}
