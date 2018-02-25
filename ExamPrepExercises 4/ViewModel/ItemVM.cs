using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamPrepExercises_4.ViewModel
{
    public class ItemVM : ViewModelBase
    {
        public ItemVM(string prodId, string name, float price, string type)
        {
            ProdId = prodId;
            Name = name;
            Price = price;
            Type = type;
        }

        public string ProdId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Type { get; set; }


    }
}
