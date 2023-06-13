using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using BrokeProtocol.Required;
using FedoraCarShop.Json;
using BrokeProtocol.Utility;
using UnityEngine;
using BrokeProtocol.Utility.Jobs;

namespace FedoraCarShop.Event
{
    internal class CarBuy : PlayerEvents
    {
        public static List<ShTransport> transports { get; set; } = new List<ShTransport>();
        Dictionary<ShPlayer, ShTransport> Cars = new Dictionary<ShPlayer, ShTransport>();
        DataBase Db = new DataBase();

        [CustomTarget]
        public void FedoraCarShop(ShEntity target, ShPlayer player)
        {
            List<LabelID> options = new List<LabelID>();
            options.Add(new LabelID(Config.LoadConfig().GarageOption, "garage"));
            foreach (var transport in transports)
            {
                Config.LoadConfig().CarsPrice.TryGetValue(transport.name, out int price);
                options.Add(new LabelID($"&1[{transport.name}] " + Config.LoadConfig().CarOption + $" &2{price}$", transport.name));
            }
            player.svPlayer.SendOptionMenu(Config.LoadConfig().CarPanel, player.ID, "fedcarpanel", options.ToArray(), new LabelID[] { new LabelID("Buy", "Buy"), new LabelID("Pub", "pub") });
        }

        [CustomTarget]
        public void FedoraGarage(ShEntity trigger, ShPhysical physical)
        {
            if (physical is ShPlayer player)
            {
                if (player.IsMount<ShTransport>(out _))
                {
                    if (Cars.ContainsKey(player)) Cars.Remove(player);
                    player.curMount.Destroy();
                }
                List<LabelID> options = new List<LabelID>();
                foreach (var transport in Db.LoadData(player).Cars)
                {
                    if (!Config.LoadConfig().BlackListCars.Contains(transport))
                    {
                        options.Add(new LabelID($"&1[{transport}]", transport));
                    }
                }
                player.svPlayer.SendOptionMenu(Config.LoadConfig().CarPanel, player.ID, "garage2", options.ToArray(), new LabelID[] { new LabelID("Sortir", "Sortir") });
            }
        }

        [Execution(ExecutionMode.Additive)]
        public override bool OptionAction(ShPlayer player, int targetID, string id, string optionID, string actionID)
        {
            if (id == "fedcarpanel")
            {
                if (optionID == "garage")
                {
                    List<LabelID> options = new List<LabelID>();
                    foreach (var transport in Db.LoadData(player).Cars)
                    {
                        if (!Config.LoadConfig().BlackListCars.Contains(transport))
                        {
                            options.Add(new LabelID($"&1[{transport}]", transport));
                        }
                    }
                    player.svPlayer.SendOptionMenu(Config.LoadConfig().CarPanel, player.ID, "garage2", options.ToArray(), new LabelID[] { new LabelID("Sortir", "Sortir") });
                }
                else
                {
                    if (actionID == "Buy")
                    {
                        ShTransport option = transports.Find(x => x.name == optionID);
                        Config.LoadConfig().CarsPrice.TryGetValue(option.name, out int price);
                        if (!(player.MyMoneyCount < price))
                        {
                            var data = Db.LoadData(player);
                            if (!data.Cars.Contains(optionID))
                            {
                                Debug.Log("log: " + data);
                                data.Cars.Add(optionID);
                                Db.SaveData(data, player.username);
                                player.TransferMoney(DeltaInv.RemoveFromMe, price);
                                player.svPlayer.SendGameMessage(Config.LoadConfig().Great2 + option.name + Config.LoadConfig().Great22 + price + "$");
                            }
                            else
                            {
                                player.svPlayer.SendGameMessage(Config.LoadConfig().Error2);
                            }
                        }
                        else
                        {
                            player.svPlayer.SendGameMessage(Config.LoadConfig().NoMoneyEnought);
                        }
                    }
                    else if (actionID == "pub")
                    {
                        player.svPlayer.SendGameMessage(Core.Pub);
                    }
                }
            }
            else if (id == "garage2")
            {
                    spawncar(optionID, player);
            }
            return true;
        }

        private Vector3 getpos()
        {
            return new Vector3(Config.LoadConfig().PosX, Config.LoadConfig().PosY, Config.LoadConfig().PosZ);
        }

        private Quaternion getrot()
        {
            return new Quaternion(Config.LoadConfig().RotX, Config.LoadConfig().RotY, Config.LoadConfig().RotZ, Config.LoadConfig().RotW);
        }

        public void spawncar(string carname, ShPlayer player)
        {
            var carprefab = transports.Find(x => x.name == carname);
            if (Cars.ContainsKey(player))
            {
                if (Cars.TryGetValue(player, out ShTransport thecar))
                {
                    thecar.Destroy();
                }
                Cars.Remove(player);
            }
            var pos = player.GetPosition;
            pos.y += 3f;
            player.svPlayer.SendGameMessage(Config.LoadConfig().Great1);
            var car = player.svPlayer.svManager.AddNewEntity(carprefab, player.GetPlace, getpos(), getrot(), false, player, false);
            car.svTransport.SvSetTransportOwner(player);
            player.svPlayer.SvMount(car, 0);
            Cars.Add(player, car);
        }
    }
}
