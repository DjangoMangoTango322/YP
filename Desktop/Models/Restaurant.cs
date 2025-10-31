using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Models
{
    public class Restaurant : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _address;
        private int _capacity;
        private TimeSpan _openTime;
        private TimeSpan _closeTime;
        private string _tematic;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(nameof(Address)); }
        }

        public int Capacity
        {
            get => _capacity;
            set { _capacity = value; OnPropertyChanged(nameof(Capacity)); }
        }

        public TimeSpan OpenTime
        {
            get => _openTime;
            set { _openTime = value; OnPropertyChanged(nameof(OpenTime)); }
        }

        public TimeSpan CloseTime
        {
            get => _closeTime;
            set { _closeTime = value; OnPropertyChanged(nameof(CloseTime)); }
        }

        public string Tematic
        {
            get => _tematic;
            set { _tematic = value; OnPropertyChanged(nameof(Tematic)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}