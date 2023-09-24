using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeehiveManagementSystem
{
    static class HoneyVault
    {
        private static string statusReport = "";
        public static string StatusReport
        {
            get
            {
                statusReport = $"Vault report: \n{honey: 0.00} units of honey \n{nectar: 0.00} units of nectar \n";

                if (honey < LOW_LEVEL_WARNING)
                {
                    statusReport += "LOW HONEY - ADD A HONEY MANUFACTURER \n";
                }

                if (nectar < LOW_LEVEL_WARNING)
                {
                    statusReport += "LOW NECTAR - ADD A NECTAR COLLECTOR \n";
                }

                return statusReport;
            }
        }

        private static float honey = 25f;
        private static float nectar = 100f;

        private const float NECTAR_CONVERSION_RATIO = 0.19f;
        private const float LOW_LEVEL_WARNING = 10f;

        public static void CollectNectar(float amount)
        {
            if (amount > 0f)
            {
                nectar += amount;
            }
        }

        public static void ConvertNectarToHoney(float amount)
        {
            if (amount > nectar)
            {
                amount = nectar;
            }
            
            nectar -= amount;
            honey += (amount * NECTAR_CONVERSION_RATIO);
        }

        public static bool ConsumeHoney(float amount)
        {
            if (amount <= honey)
            {
                honey -= amount;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
