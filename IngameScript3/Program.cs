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

        MyCommandLine Argumentos = new MyCommandLine();

        public void Main(string argument, UpdateType updateSource)
        {
            if (!Argumentos.TryParse(argument))
            {
                Echo("Você deve passar os argumentos");
                return;
            }

            string Grupos = Argumentos.Argument(0);
            float Velocidade = 0;

            if (!float.TryParse(Argumentos.Argument(1), out Velocidade)) {
                Echo("Fomato inválido");
                return; 
            }

            IMyBlockGroup GrupoPistoes = GridTerminalSystem.GetBlockGroupWithName(Grupos) as IMyBlockGroup;
            if (GrupoPistoes == null)
            {
                Echo("Grupo não existe");
                return;
            }

            List<IMyTerminalBlock> Pistoes = new List<IMyTerminalBlock>();
            GrupoPistoes.GetBlocks(Pistoes);

            for (int i = 0; i < Pistoes.Count; i++)
            {
                IMyPistonBase Pistao = Pistoes[i] as IMyPistonBase;
                Pistao.Velocity = Velocidade;
            }
        }
    }
}
