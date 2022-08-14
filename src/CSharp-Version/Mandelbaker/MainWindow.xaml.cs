using Mandelbaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Newtonsoft.Json;
using System.IO;
using Mandelbaker.ViewModels;
using System.Drawing;
using System.Windows.Forms;

namespace Mandelbaker
{
    public partial class MainWindow : Window
    {
        MandelbrotGeneratorViewModel _viewModel;
        public MainWindow()
        {
            _viewModel = new();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void RenderMandelbrot(object sender, RoutedEventArgs e)
        {
            MandelbrotCalculationInformation mci;

            if (_viewModel.Method == "CPU")
            {
                mci = Mandelbrot.SaveCPUMandelbrot(_viewModel.Resolution, _viewModel.Iterations, _viewModel.XLeft, _viewModel.YTop, _viewModel.Zoom, _viewModel.Directory, _viewModel.Filename);
            }
            else
            {
                mci = Mandelbrot.SaveGPUMandelbrot(_viewModel.Resolution, _viewModel.Iterations, _viewModel.XLeft, _viewModel.YTop, _viewModel.Zoom, _viewModel.Directory, _viewModel.Filename);
            }
            _viewModel.SetOutput("Render complete: " + mci.ToString());
        }
        private void RenderMatrix(object sender, RoutedEventArgs e)
        {
            MandelbrotCalculationInformation mci;
            List<MandelbrotCalculationInformation> mcis;

            (mci, mcis) = Mandelbrot.RenderMatrix(_viewModel.Resolution, _viewModel.Iterations, _viewModel.DimensionSize, _viewModel.XLeft, _viewModel.YTop, _viewModel.Zoom, _viewModel.Directory, _viewModel.Method == "GPU" ? true : false);
            _viewModel.SetOutput("Render complete: " + mci.ToString());

            string jsonString = JsonConvert.SerializeObject((mci, mcis), Formatting.Indented);

            Directory.CreateDirectory(@"C:\Mandelbaker\CalculationInformation\");
            DateTime now = DateTime.Now;
            string date = now.ToString("dd-MM-yyyy_HH-mm-ss");
            string jsonFilename = @$"C:\Mandelbaker\CalculationInformation\Matrix_{_viewModel.Resolution}px_{date}.json";
            File.WriteAllText(jsonFilename, jsonString);
        }
        private void OpenFolder(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    _viewModel.Directory = dialog.SelectedPath;
                }
            }
        }
    }
}
