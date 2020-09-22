using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
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

namespace proj_automatos.Controls
{
    /// <summary>
    /// Interaction logic for TuringMachineEntry.xaml
    /// </summary>
    public partial class TuringMachineEntry : UserControl
    {
        public TuringMachineEntry()
        {
            InitializeComponent();
        }

        public TuringMachineEntry(TuringMachineDataEntry turingMachineDataEntry, bool isSource)
        {
            InitializeComponent();

            if (isSource)
            {
                EntryContent.Foreground = Brushes.Black;
                Border.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#d2dae2");
                Arrow.Visibility = Visibility.Visible;
            } else
            {
                EntryContent.Foreground = Brushes.White;
                Border.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#303952");
                Arrow.Visibility = Visibility.Hidden;
            }

            EntryContent.Content = turingMachineDataEntry.Dado;
        }
    }
}
