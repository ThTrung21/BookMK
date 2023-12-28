using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.ViewModels.InsertFormViewModels
{
    public class InsertBookViewModel:ViewModelBase
    {
        private Int64 _id;
        public Int64 ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }
        private string _releaseyear;
        public string ReleaseYear
        {
            get { return _releaseyear; }
            set { _releaseyear = value; OnPropertyChanged(nameof(ReleaseYear)); }
        }
        private string _sellprice;
        public string SellPrice
        {
            get { return _sellprice; }
            set { _sellprice = value; OnPropertyChanged(nameof(SellPrice)); }
        }


        private string[] _genre;
        public string[] Genre
        {
            get => _genre;
            set
            {
                if (value.Length > 3)
                {
                    // Take appropriate action, e.g., trim the array or throw an exception
                    throw new ArgumentException("Genre array cannot have more than 3 elements.");
                    
                }

                _genre = value;
            }
        }

        public InsertBookViewModel() { }

    }
}
