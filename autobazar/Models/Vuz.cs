using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace autobazar.Models
{
    public class Vuz
    {
        public string Model { get; set; }
        public DateTime Datum { get; set; }
        public double BezDPH { get; set; }
        public double DPH { get; set; }
        public double Cena => BezDPH * (100 + DPH) / 100;
    }
}
