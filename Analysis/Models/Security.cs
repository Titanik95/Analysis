using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analysis.Models
{
    [Serializable]
    class Security: INotifyPropertyChanged
    {
        string security;
        DateTime dateFrom;
        DateTime timeFrom;
        DateTime dateTo;
        DateTime timeTo;
        bool autoUpdate;
        [NonSerialized]
        double volumeBuy;
        [NonSerialized]
        double volumeSell;
        [NonSerialized]
        double volumeBalance;

        public string SecurityName
        {
            get { return security; }
            set
            {
                if (security == value) return;
                security = value;
                OnPropertyChanged("SecurityName");
            }
        }

        public DateTime DateFrom
        {
            get { return dateFrom; }
            set
            {
                if (dateFrom == value) return;
                dateFrom = value;
                OnPropertyChanged("DateFrom");
            }
        }

        public DateTime TimeFrom
        {
            get { return timeFrom; }
            set
            {
                if (timeFrom == value) return;
                timeFrom = value;
                OnPropertyChanged("TimeFrom");
            }
        }

        public DateTime DateTo
        {
            get { return dateTo; }
            set
            {
                if (dateTo == value) return;
                dateTo = value;
                OnPropertyChanged("DateTo");
            }
        }

        public DateTime TimeTo
        {
            get { return timeTo; }
            set
            {
                if (timeTo == value) return;
                timeTo = value;
                OnPropertyChanged("TimeTo");
            }
        }

        public bool AutoUpdate
        {
            get { return autoUpdate; }
            set
            {
                autoUpdate = value;
                if (autoUpdate)
                {
                    timeTo = DateTime.Now;
                    OnPropertyChanged("TimeTo");
                }
                OnPropertyChanged("AutoUpdate");
            }
        }

        public double VolumeBuy
        {
            get { return volumeBuy; }
            set
            {
                if (volumeBuy == value) return;
                volumeBuy = value;
                volumeBalance = volumeBuy - volumeSell;
                OnPropertyChanged("VolumeBuy");
                OnPropertyChanged("VolumeBalance");
            }
        }

        public double VolumeSell
        {
            get { return volumeSell; }
            set
            {
                if (volumeSell == value) return;
                volumeSell = value;
                volumeBalance = volumeBuy - volumeSell;
                OnPropertyChanged("VolumeSell");
                OnPropertyChanged("VolumeBalance");
            }
        }

        public double VolumeBalance
        {
            get { return volumeBalance; }
            set
            {
                if (volumeBalance == value) return;
                volumeBalance = value;
                OnPropertyChanged("VolumeBalance");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
