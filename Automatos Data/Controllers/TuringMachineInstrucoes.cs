using System;
using System.Linq;

namespace Automatos_Data.Controllers
{
    public class TuringMachineInstrucoes
    {
        public enum Movement { Left, Right }
        
        public string EstadoInstrucoes { get; }
        public char DadosInstrucoes { get; }
        public string SairEstadoInstrucoes { get; }
        public char ResultadoInstrucoes { get; }
        public Movement MovimentoInstrucao { get; }

        public bool IsValid(TuringMachine machine)
        {
            return machine.EstadoAtual == EstadoInstrucoes &&
                   machine.Dado.DadoAtual == DadosInstrucoes;
        }
        
        private TuringMachineInstrucoes(string estadoInstrucoes, 
            char dadosInstrucoes, 
            string sairEstadoInstrucoes, 
            char resultadoInstrucoes, 
            Movement movimentoInstrucao)
        {
            EstadoInstrucoes = estadoInstrucoes;
            DadosInstrucoes = dadosInstrucoes;
            SairEstadoInstrucoes = sairEstadoInstrucoes;
            ResultadoInstrucoes = resultadoInstrucoes;
            MovimentoInstrucao = movimentoInstrucao;
        }

        public static TuringMachineInstrucoes FromString(string source)
        {
            //Clear input
            var input = source
                .Replace(" ", string.Empty);

            var inputs = input.Split(',');
            
            //Validation
            if (inputs.Length != 5)
                throw new ArgumentException($"Invalid input string: {source}", nameof(source));
            
            if (string.IsNullOrEmpty(inputs[0]))
                throw new ArgumentException($"Instruction state cannot be null: {source}", nameof(source));

            if (inputs[1].Length != 1)
                throw new ArgumentException($"Instruction Data must be char: {source}", nameof(source));
            
            if (string.IsNullOrEmpty(inputs[2]))
                throw new ArgumentException($"Instruction Exit state cannot be null: {source}", nameof(source));
            
            if (inputs[3].Length != 1)
                throw new ArgumentException($"Instruction Output must be char: {source}", nameof(source));
            
            var instructionStage = inputs[0];
            var instructionData = inputs[1].First();
            var instructionExitState = inputs[2];
            var instructionOutput = inputs[3].First();
            var instructionMovement = inputs[4] == "<" ? Movement.Left :
                inputs[4] == ">" ? Movement.Right : throw new ArgumentException($"Movement Direction must be '>' or '<': {source}", nameof(source));
            
            return new TuringMachineInstrucoes(
                instructionStage, 
                instructionData, 
                instructionExitState, 
                instructionOutput, 
                instructionMovement);
        }
    }
}