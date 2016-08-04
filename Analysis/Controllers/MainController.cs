using Analysis.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Analysis.Controllers
{
    class MainController
    {
        DataManager dataManager;
        Dictionary<string, Security> trackingSecurities;

        public MainController()
        {
            dataManager = new DataManager();
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

        public void UpdateSecurities()
        {
            dataManager.UpdateSecurities(trackingSecurities);
        }

        public bool ContainsSecurity(string secName)
        {
            return trackingSecurities.ContainsKey(secName);
        }

        public void SaveSecurities()
        {
            using (FileStream fs = new FileStream(Properties.Resources.SecuritiesFileName, FileMode.OpenOrCreate))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    bf.Serialize(fs, trackingSecurities);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка сериализации отслеживаемых инструментов");
                }

            }
        }

        void InitTrackingSecurities()
        {
            trackingSecurities = new Dictionary<string, Security>();

            using (FileStream fs = new FileStream(Properties.Resources.SecuritiesFileName, FileMode.OpenOrCreate))
            {
                BinaryFormatter bf = new BinaryFormatter();

                try
                {
                    trackingSecurities = ((Dictionary<string, Security>)bf.Deserialize(fs));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка десериализации инструментов");
                }

            }
        }
    }
}
