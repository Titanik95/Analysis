using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analysis.Others
{
    class Common
    {
        public static string GetConnectionString()
        { 
            return ConfigurationManager.ConnectionStrings[1].ToString();
        }
    }
}
