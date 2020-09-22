using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Automatos_Data.Controllers
{
    public class TuringMachineData : IEnumerable<TuringMachineDataEntry>
    {
        public TuringMachineDataEntry Center { get; private set; }

        public char DadoAtual => Center.Dado;
        public void MoverProximo() => Center = Center.Proximo;
        public void MoverAnterior() => Center = Center.Anterior;

        public void Escreva(char dado)
        {
            if (dado != '_') Center.Dado = dado;
        }

        public string LerTodos() => new string(this.Select(e => e.Dado).Where(c => c != '_').ToArray());

        public TuringMachineData()
        {
            Center = new TuringMachineDataEntry();
        }

        public TuringMachineData(string dadoInicial) : this()
        {
            var entrada = Center;
            for (int i = 0; i < dadoInicial.Length; i++)
            {
                entrada.Dado = dadoInicial[i];

                if (i != dadoInicial.Length - 1)
                    entrada = entrada.Proximo;
            }
        }

        public TuringMachineDataEntry this[int index]
        {
            get
            {
                var entrada = Center;
                while (index > 0)
                {
                    index -= 1;
                    entrada = entrada.Proximo;
                }

                while (index < 0)
                {
                    index += 1;
                    entrada = entrada.Anterior;
                }

                return entrada;
            }
        }

        public IEnumerator<TuringMachineDataEntry> GetEnumerator()
        {
            var entrada = Center;

            while (entrada._anterior != null)
                entrada = entrada._anterior;

            while (entrada != null)
            {
                yield return entrada;
                entrada = entrada._proximo;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class TuringMachineDataEntry
    {
        internal TuringMachineDataEntry _proximo;
        internal TuringMachineDataEntry _anterior;

        public TuringMachineDataEntry Anterior => _anterior ??= new TuringMachineDataEntry{_proximo = this};
        public TuringMachineDataEntry Proximo => _proximo ??= new TuringMachineDataEntry {_anterior = this};
        
        public char Dado { get; set; }

        public TuringMachineDataEntry() : this('_')
        {
        }

        public TuringMachineDataEntry(char dado) => Dado = dado;
    }
}