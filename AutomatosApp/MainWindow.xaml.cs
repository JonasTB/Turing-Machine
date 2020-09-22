using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Automatos_Data.Controllers;
using Microsoft.Win32;
using proj_automatos.Controls;

namespace proj_automatos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] input { get; set; }
        private TuringMachine TuringMachine { get; set; }
        
        private BackgroundWorker AutoPlay { get; } = new BackgroundWorker();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateData()
        {
            DataStackPanel.Children.Clear();
            var entries = TuringMachine.Dado.ToArray();
            foreach (var entry in entries)
            {
                DataStackPanel.Children.Add(new TuringMachineEntry(entry, entry == TuringMachine.Dado.Center 
                    && TuringMachine.MachineEstado != TuringMachine.Estado.Finalizado));
            }

            if (TuringMachine.MachineEstado == TuringMachine.Estado.Finalizado)
            {
                ResultadoLabel.Content = $"Resultado: ";
                ResultadoLabel.Content += TuringMachine.Resultado == TuringMachine.ResultadoFinal.Valido ? "Válido" : "Inválido";
            }
            else ResultadoLabel.Content = "";
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (TuringMachine.MachineEstado == TuringMachine.Estado.Finalizado)
                return;
            
            TuringMachine.Run();
            UpdateData();   
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            TuringMachine = TuringMachine.FromText(input);
            UpdateData();
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                input = File.ReadAllLines(ofd.FileName);
                TuringMachine = TuringMachine.FromText(input);
                UpdateData();
            }
        }
    }
}
