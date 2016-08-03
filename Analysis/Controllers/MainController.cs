using Analysis.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analysis.Controllers
{
    class MainController
    {
        Dictionary<string, Security> trackingSecurities;

        public MainController()
        {
            InitTrackingSecurities();
        }

        public ObservableCollection<Security> GetTrackingSecurities()
        {
            ObservableCollection<Security> res = new ObservableCollection<Security>();
            foreach (var sec in trackingSecurities.Values)
                res.Add(sec);

            return res;
        }

        public void AddTrackingSecurity(Security sec)
        {
            trackingSecurities.Add(sec.SecurityName, sec);
        }

        public void RemoveTrackingSecurity(string secName)
        {
            trackingSecurities.Remove(secName);
        }

        void InitTrackingSecurities()
        {
            trackingSecurities = new Dictionary<string, Security>();
            
        }
    }
}
