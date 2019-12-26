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

            // ALTERES OS LIMITES AQUI
            Limites = new Dictionary<MyDefinitionId, MyFixedPoint> {
                {CriarID("SteelPlate"), 500},
                {CriarID("ConstructionComponent"), 200},
                {CriarID("PowerCell"), 50},
                {CriarID("ComputerComponent"), 200},
                {CriarID("LargeTube"), 200},
                {CriarID("MotorComponent"), 200},
                {CriarID("Display"), 100},
                {CriarID("MetalGrid"), 200},
                {CriarID("InteriorPlate"), 300},
                {CriarID("SmallTube"), 300},
                {CriarID("RadioCommunicationComponent"), 100},
                {CriarID("BulletproofGlass"), 200},
                {CriarID("GirderComponent"), 200},
                {CriarID("ExplosivesComponent"), 0},
                {CriarID("DetectorComponent"), 200},
                {CriarID("MedicalComponent"), 30},
                {CriarID("GravityGeneratorComponent"), 0},
                {CriarID("Superconductor"), 0},
                {CriarID("ThrustComponent"), 0},
                {CriarID("ReactorComponent"), 0},
                {CriarID("SolarCell"), 100},
            };

            // ALRETE OS LIMITES ACIMA
            Cache = new Dictionary<MyItemType, MyDefinitionId> {
                {CriarIDX("SteelPlate"), CriarID("SteelPlate")},
                {CriarIDX("Construction"), CriarID("ConstructionComponent")},
                {CriarIDX("PowerCell"), CriarID("PowerCell")},
                {CriarIDX("Computer"), CriarID("ComputerComponent")},
                {CriarIDX("LargeTube"), CriarID("LargeTube")},
                {CriarIDX("Motor"), CriarID("MotorComponent")},
                {CriarIDX("Display"), CriarID("Display")},
                {CriarIDX("MetalGrid"), CriarID("MetalGrid")},
                {CriarIDX("InteriorPlate"), CriarID("InteriorPlate")},
                {CriarIDX("SmallTube"), CriarID("SmallTube")},
                {CriarIDX("RadioCommunication"), CriarID("RadioCommunicationComponent")},
                {CriarIDX("BulletproofGlass"), CriarID("BulletproofGlass")},
                {CriarIDX("Girder"), CriarID("GirderComponent")},
                {CriarIDX("Explosives"), CriarID("ExplosivesComponent")},
                {CriarIDX("Detector"), CriarID("DetectorComponent")},
                {CriarIDX("Medical"), CriarID("MedicalComponent")},
                {CriarIDX("GravityGenerator"),CriarID("GravityGeneratorComponent")},
                {CriarIDX("Superconductor"), CriarID("Superconductor")},
                {CriarIDX("Thrust"), CriarID("ThrustComponent")},
                {CriarIDX("Reactor"), CriarID("ReactorComponent")},
                {CriarIDX("SolarCell"), CriarID("SolarCell")},
            };

            Runtime.UpdateFrequency |= UpdateFrequency.Update10;
        }

        Dictionary<MyDefinitionId, MyFixedPoint> Limites;
        Dictionary<MyItemType, MyDefinitionId> Cache;
 
        public static MyDefinitionId CriarID(string s)
        {
            return MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/" + s);
        }
        public static MyDefinitionId CriarIDX(string s)
        {
            return MyDefinitionId.Parse("MyObjectBuilder_Component/" + s);
        }

        public void Save(){}

        public void Main(string argument, UpdateType updateSource)
        {
            // Copia do Limites para alterar depois
            Dictionary<MyDefinitionId, MyFixedPoint> Restante = Limites.ToDictionary(entry => entry.Key, entry => (MyFixedPoint)entry.Value);

            Restante = VerificarFilaMontadora(Restante, "Montadoras");
            Restante = VerificarEstoque(Restante, GridTerminalSystem.GetBlockWithName("Correios") as IMyTerminalBlock);
            AdicionarNaFila(Restante, GridTerminalSystem.GetBlockWithName("Montadora Principal") as IMyAssembler);
        }

        public Dictionary<MyDefinitionId, MyFixedPoint> VerificarFilaMontadora(Dictionary<MyDefinitionId, MyFixedPoint> Restante, string Grupo)
        {
            List<IMyTerminalBlock> Montadoras = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlockGroupWithName(Grupo).GetBlocks(Montadoras);

            for (int i = 0; i < Montadoras.Count; i++)
            {
                IMyAssembler Montadora = Montadoras[i] as IMyAssembler;

                List<MyProductionItem> Fila = new List<MyProductionItem>();
                Montadora.GetQueue(Fila);

                for (int q = 0; q < Fila.Count; q++)
                {
                    if (Restante.ContainsKey(Fila[q].BlueprintId)) {
                        Restante[Fila[q].BlueprintId] -= Fila[q].Amount;
                    }
                }

                Restante = VerificarEstoque(Restante, Montadora);
            }

            return Restante;
        }

        public Dictionary<MyDefinitionId, MyFixedPoint> VerificarEstoque(Dictionary<MyDefinitionId, MyFixedPoint> Restante, IMyTerminalBlock Bau)
        {
            List<MyInventoryItem> Items = new List<MyInventoryItem>();
            Bau.GetInventory().GetItems(Items);

           
            for (int i = 0; i < Items.Count; i++)
            {
                if (Cache.ContainsKey(Items[i].Type)) {
                    Restante[Cache[Items[i].Type]] -= Items[i].Amount;
                }
            }

            return Restante;
        }

        public void AdicionarNaFila(Dictionary<MyDefinitionId, MyFixedPoint> Restante, IMyAssembler Montadora)
        {
            foreach (KeyValuePair<MyDefinitionId, MyFixedPoint> Item in Restante)
            {
                if (Item.Value > 0) {
                    Montadora.AddQueueItem(Item.Key, Item.Value);
                }
            }
        }
    }
}
