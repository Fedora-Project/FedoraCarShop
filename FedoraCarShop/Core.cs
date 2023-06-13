using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrokeProtocol.API;
using UnityEngine;

namespace FedoraCarShop
{
    public class Core : Plugin
    {
        public static string Pub = "&4Buy The Plugin On FedoraTeam Discord : https://discord.gg/5cyzda8zy9";
        public Core()
        {
            Info = new PluginInfo("FedoraCarShop", "fcs", "Fedora Car shop download on the discord" , "https://discord.gg/5cyzda8zy9");
            if (!Directory.Exists("./FedoraCore/Carshop")) { Directory.CreateDirectory("./FedoraCore/Carshop"); Debug.Log("[Fedora] " + "Creating Folder waiting pls..."); }
        }
    }
}
