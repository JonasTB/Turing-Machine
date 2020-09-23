using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Automatos_Data.Controllers
{
    public class TuringMachine
    {
        public enum Estado { Rodando, Pronto, Finalizado }
        public enum ResultadoFinal { Valido, Invalido }
        
        public TuringMachineData Dado { get; }
        public string EstadoAtual { get; private set; }
        public string[] EstadoFim { get; }
        public TuringMachineInstrucoes[] Instrucoes { get; }
        public Estado MachineEstado { get; private set; }
        public ResultadoFinal? Resultado { get; private set; }

        private TuringMachine(TuringMachineData dado, string estadoInicial, string[] estadoFim, TuringMachineInstrucoes[]
            instrucoes)
        {
            Dado = dado;
            EstadoAtual = estadoInicial;
            EstadoFim = estadoFim;
            Instrucoes = instrucoes;
            MachineEstado = Estado.Pronto;
            Resultado = null;
        }
        
        public TuringMachineInstrucoes getInstrucao() => Instrucoes.FirstOrDefault(i => i.IsValid(this));

        public static TuringMachine FromFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("File Not Found", path);

            var lines = File.ReadAllLines(path);
            return FromText(lines);
        } 
        public static TuringMachine FromText(string[] linhas)
        {
            TuringMachineData dado = null;
            string estadoInicial = null;
            string[] estadoFim = new string[0];
            List<TuringMachineInstrucoes> instrucoes = new List<TuringMachineInstrucoes>();

            foreach (var linha in linhas)
            {
                var linhaLimpa = linha.Trim();
                
                if(linhaLimpa.StartsWith("@")) continue;
                if (linhaLimpa.StartsWith("fita "))
                {
                    dado = new TuringMachineData(linhaLimpa.Substring(5));//Ignore 'fita '
                } else if (linhaLimpa.StartsWith("init "))
                {
                    estadoInicial = linhaLimpa.Substring(5);
                } else if (linhaLimpa.StartsWith("accept "))
                {
                    estadoFim = linhaLimpa.Substring(7).Trim().Split(",");
                }
                else if (!string.IsNullOrWhiteSpace(linhaLimpa))
                {
                        var instruction = TuringMachineInstrucoes.FromString(linhaLimpa);
                        instrucoes.Add(instruction);
                }
            }
            
            return new TuringMachine(dado, estadoInicial, estadoFim, instrucoes.ToArray());
        }

        public void Run()
        {
            if (MachineEstado == Estado.Pronto)
                MachineEstado = Estado.Rodando;
            
            if (MachineEstado != Estado.Rodando)
                return;

            var instruction = Instrucoes.FirstOrDefault(i => i.IsValid(this));
            if (instruction == null)
            {
                Stop();
                return;
            }

            EstadoAtual = instruction.SairEstadoInstrucoes;
            Dado.Escreva(instruction.ResultadoInstrucoes);

            switch (instruction.MovimentoInstrucao)
            {
                case TuringMachineInstrucoes.Movement.Left:
                    Dado.MoverAnterior();
                    break;
                case TuringMachineInstrucoes.Movement.Right:
                    Dado.MoverProximo();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(instruction.MovimentoInstrucao));
            }
        }

        private void Stop()
        {
            MachineEstado = Estado.Finalizado;
            Resultado = EstadoFim.Contains(EstadoAtual) ? ResultadoFinal.Valido : ResultadoFinal.Invalido;
        }
    }
}
