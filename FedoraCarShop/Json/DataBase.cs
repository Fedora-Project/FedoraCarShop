using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using BrokeProtocol.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FedoraCarShop.Json
{
    public class CarShop
    {
        public List<string> Cars { get; set; } = new List<string>();
    }

    public class DataBase
    {
        public DataBase() 
        {
            if (!Directory.Exists(PathConfig)) { Directory.CreateDirectory(PathConfig); Debug.Log("[Fedora] " + "Creating Folder waiting pls..."); }
        }

        public string PathConfig = "./FedoraCore/Carshop/Database";
        
        public string GetPlayerDataPath(string name)
        {
            return Path.Combine(PathConfig, name) + ".json";
        }

        public CarShop LoadData(ShPlayer player)
        {   
            if (!File.Exists(GetPlayerDataPath(player.username)))
            {
                CarShop data = new CarShop();
                using (StreamWriter Sw = File.CreateText(GetPlayerDataPath(player.username)))
                {
                    Sw.Write(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
                Debug.Log("[Fedora] Creating carshop database ...");
                return data;
            }
            else
            {
                using (StreamReader Sr = new StreamReader(GetPlayerDataPath(player.username)))
                {
                    return JsonConvert.DeserializeObject<CarShop>(Sr.ReadToEnd());
                }
            }
        }

        public void SaveData(CarShop data, string playername)
        {
            using (StreamWriter Sw = new StreamWriter(GetPlayerDataPath(playername)))
            {
                Sw.Write(JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            Debug.Log("[Fedora] Updating carshop database ...");
        }
    }
}