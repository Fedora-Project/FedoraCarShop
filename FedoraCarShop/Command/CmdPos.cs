using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using FedoraCarShop.Json;
using UnityEngine;

namespace FedoraCarShop.Command
{
    internal class CmdPos : IScript
    {
        public CmdPos()
        {
            CommandHandler.RegisterCommand("CSpos", new Action<ShPlayer>(SetPos));
            CommandHandler.RegisterCommand("bl", new Action<ShPlayer>(addtoblacklist));
        }

        private void SetPos(ShPlayer p)
        {
            ConfigInfo data = Config.LoadConfig();
            data.PosX = p.GetPosition.x;
            data.PosY = p.GetPosition.y;
            data.PosZ = p.GetPosition.z;
            data.RotX = p.GetRotation.x;
            data.RotY = p.GetRotation.y;
            data.RotZ = p.GetRotation.z;
            data.RotW = p.GetRotation.w;
            Config.SaveConfig(data);
            p.svPlayer.SendGameMessage($"Pos Set !");
        }
        
        private void addtoblacklist(ShPlayer p)
        {
            if (p.IsMount<ShTransport>(out var m))
            {
                ConfigInfo data = Config.LoadConfig();
                if (!data.BlackListCars.Contains(p.curMount.name))
                {
                    data.BlackListCars.Add(p.curMount.name);
                    Config.SaveConfig(data);
                    Event.CarBuy.transports.Remove(Event.CarBuy.transports.Find(x => x.name == p.curMount.name));
                    p.svPlayer.SendGameMessage(Config.LoadConfig().bl);
                }
                else
                {
                    data.BlackListCars.Remove(p.curMount.name);
                    Event.CarBuy.transports.Add((ShTransport)p.curMount);
                    Config.SaveConfig(data);
                    p.svPlayer.SendGameMessage(Config.LoadConfig().bls);
                }
            }
        }
    }
}
