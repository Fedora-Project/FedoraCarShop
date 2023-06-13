using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using FedoraCarShop.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FedoraCarShop.Event
{
    internal class Manager : ManagerEvents
    {
        [Execution(ExecutionMode.Additive)]
        public override bool Start()
        {
            foreach (ShEntity e in SceneManager.Instance.entityCollection.Values)
            {
                if (e is ShTransport && !(e is ShParachute) && !(e is ShBoat) && !(e is ShAirplane) && !(e is ShAircraft))
                {
                    if (!Config.LoadConfig().BlackListCars.Contains(e.name) /*&& !(transports.Count >= 24)*/)
                    {
                        CarBuy.transports.Add((ShTransport)e);
                    }
                }
            }
            Config.SaveConfig(Config.LoadConfig());
            return true;
        }
    }
}
