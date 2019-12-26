using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public Program()
        {

        }

        public void Save()
        {

        }

        public void Main(string argument, UpdateType updateSource)
        {
     
            // Entidades
            IMyGasTank Tank = GridTerminalSystem.GetBlockWithName("Hydrogen Tank") as IMyGasTank;
            IMyEntity Bau = GridTerminalSystem.GetBlockWithName("Hydrogen Refill Box") as IMyEntity;

            // Inventario do Tank
            IMyInventory InventarioTank = Tank.GetInventory(0);
            IMyInventory InventarioBau = Bau.GetInventory(0);


            TransferirTudo(InventarioBau, InventarioTank);

            Tank.RefillBottles();

            TransferirTudo(InventarioTank, InventarioBau);

        }

        public void TransferirTudo(IMyInventory InventarioEnviador, IMyInventory InventarioReceptor)
        {
            // Itens do Inventario do Tank
            List<MyInventoryItem> Items = new List<MyInventoryItem>();
            InventarioEnviador.GetItems(Items);

            for (int i = 0; i < Items.Count; i++)
            {
           
                InventarioEnviador.TransferItemTo(InventarioReceptor, Items[i]);
            }
        }
    }
}
