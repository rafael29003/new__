using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;


namespace IOiMO
{
    public class Predmet
    {
        public string Name { get; set; } // Название предмета
        public int Weight { get; set; } // Вес предмета
        public int Cost { get; set; } // Стоимость предмета

        // Конструктор для инициализации предмета
        public Predmet(string name, int weight, int cost)
        {
            Name = name;
            Weight = weight;
            Cost = cost;

        }
    }
}


