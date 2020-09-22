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
            PlayButton.IsEnabled = false;
            ForwardButton.IsEnabled = false;
            StopButton.IsEnabled = false;
            ResetButton.IsEnabled = false;
            AutoPlay.DoWork += AutoPlayOnDoWork;
            AutoPlay.ProgressChanged += AutoPlayOnProgressChanged;
            AutoPlay.WorkerReportsProgress = true;
            AutoPlay.WorkerSupportsCancellation = true;
            AutoPlay.RunWorkerCompleted += AutoPlayOnRunWorkerCompleted;
        }

        private void UpdateData()
        {
            DataStackPanel.Children.Clear();
            var entries = TuringMachine.Dado.ToArray();
            foreach (var entry in entries)
            {
                DataStackPanel.Children.Add(new TuringMachineEntry(entry, entry == TuringMachine.Dado.Center));
            }

            var instruction = TuringMachine.getInstrucao();
            CurrentStateLabel.Content = TuringMachine.EstadoAtual;
            CurrentDataLabel.Content = TuringMachine.Dado.DadoAtual;
            if (instruction != null)
            {
                NextStateLabel.Content = instruction.SairEstadoInstrucoes;
                InputDataLabel.Content = instruction.ResultadoInstrucoes;
                MovementLabel.Content =
                    instruction.MovimentoInstrucao == TuringMachineInstrucoes.Movement.Left ? "<" : ">";
            }
            else
            {
                NextStateLabel.Content = "";
                InputDataLabel.Content = "";
                MovementLabel.Content = "";
            }
            
            MachineStateLabel.Content =
                TuringMachine.MachineEstado == TuringMachine.Estado.Finalizado
                    ? TuringMachine.Resultado.ToString()
                    : TuringMachine.MachineEstado.ToString();

            if (TuringMachine.MachineEstado == TuringMachine.Estado.Finalizado)
            {
                ResultBorder.Visibility = Visibility.Visible;
                ResultBorder.Background =
                    TuringMachine.Resultado == TuringMachine.ResultadoFinal.Valido
                        ? Brushes.SpringGreen
                        : Brushes.Crimson;
            } else ResultBorder.Visibility = Visibility.Hidden;
        }

        private void AutoPlayOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (TuringMachine.MachineEstado != TuringMachine.Estado.Finalizado)
            {
                PlayButton.IsEnabled = true;
                ForwardButton.IsEnabled = true;
            }
            ResetButton.IsEnabled = true;
        }

        private void AutoPlayOnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UpdateData();
        }

        private void AutoPlayOnDoWork(object sender, DoWorkEventArgs e)
        {
            while (!AutoPlay.CancellationPending && TuringMachine.MachineEstado != TuringMachine.Estado.Finalizado)
            {
                TuringMachine.Run();
                AutoPlay.ReportProgress(0);
                Thread.Sleep(350);
            }
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        
        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (TuringMachine.MachineEstado == TuringMachine.Estado.Finalizado)
                return;
            
            TuringMachine.Run();
            UpdateData();

            if (TuringMachine.MachineEstado == TuringMachine.Estado.Finalizado)
            {
                PlayButton.IsEnabled = false;
                ForwardButton.IsEnabled = false;
            }
                
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            AutoPlay.RunWorkerAsync();
            ForwardButton.IsEnabled = false;
            PlayButton.IsEnabled = false;
            ResetButton.IsEnabled = false;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            AutoPlay.CancelAsync();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            TuringMachine = TuringMachine.FromText(input);
            UpdateData();

            PlayButton.IsEnabled = true;
            ForwardButton.IsEnabled = true;
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                input = File.ReadAllLines(ofd.FileName);
                TuringMachine = TuringMachine.FromText(input);
                UpdateData();
                
                PlayButton.IsEnabled = true;
                ForwardButton.IsEnabled = true;
                StopButton.IsEnabled = true;
                ResetButton.IsEnabled = true;
            }
        }
    }
}
