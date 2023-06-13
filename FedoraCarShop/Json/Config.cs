using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FedoraCarShop.Json
{
    public class ConfigInfo
    {
        public string CarPanel = "véhicule Menu";
        public string Error1 = "&4vous avez déjà un véhicule de sortie !";
        public string Great1 = "&2vous venez de sortir votre véhicule";
        public string Error2 = "&6[Fedora] &7tu as déjà ce &cvéhicule";
        public string Great2 = "&2vous venez d'acheté ";
        public string Great22 = " &fpour ";
        public string NoMoneyEnought = "&4MON REUF T'es trop pauvre";
        public string GarageOption = "&6[GARAGE] &fsortir un véhicule";
        public string CarOption = "&7pour";
        public string bl = "you just blacklist that car";
        public string bls = "you just Unblacklist that car";
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float RotX { get; set; }
        public float RotY { get; set; }
        public float RotZ { get; set; }
        public float RotW { get; set; }
        public List<string> BlackListCars = new List<string>();
        public Dictionary<string,int> CarsPrice = new Dictionary<string,int>();
    }

    public static class Config
    {
        public static string PathConfig = "./FedoraCore/Carshop/Config.json";

        public static ConfigInfo LoadConfig()
        {
            if (!File.Exists(PathConfig))
            {
                SetupConfig();
            }
            else
            {
                var json = File.ReadAllText(PathConfig);
                return JsonConvert.DeserializeObject<ConfigInfo>(json);
            }
            ConfigInfo data = new ConfigInfo();
            Debug.Log("default config \n\n choosed !");
            return data;
        }

        public static void SaveConfig(ConfigInfo data)
        {
            foreach (var Car in Event.CarBuy.transports)
            {
                if (data.CarsPrice.ContainsKey(Car.name))
                {
                    data.CarsPrice.TryGetValue(Car.name, out int price);
                    data.CarsPrice.Remove(Car.name);
                    data.CarsPrice.Add(Car.name, price);
                }
                else
                {
                    data.CarsPrice.Add(Car.name, Car.value);
                }
            }
            using (StreamWriter Sw = File.CreateText(PathConfig))
            {
                Sw.Write(JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            Debug.Log("Save Fedora Config ...");

        }

        public static void SetupConfig()
        {
            if (!File.Exists(PathConfig))
            {
                ConfigInfo data = new ConfigInfo();
                using (StreamWriter Sw = File.CreateText(PathConfig))
                {
                    Sw.Write(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
                Debug.Log("creating Fedora Config ...");
            }
        }
    }
}
