using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mandelbaker.ViewModels
{
    public class MandelbrotGeneratorViewModel : INotifyPropertyChanged
    {
        // Generic Properties
        public int Resolution { get; set; } = 1000;
        public int Iterations { get; set; } = 255;
        public double XLeft { get; set; } = -2;
        public double YTop { get; set; } = 1.5;
        public double Zoom { get; set; } = 1;
        private string _directory = string.Empty;
        public string Directory
        {
            get => _directory;
            set
            {
                _directory = value;
                if (value.Last() != '/' && value.Last() != '\\')
                    _directory += @"\";
                NotifyPropertyChanged();
            }
        }
        public string Method { get; set; }
        public List<string> Methods { get; set; }
        private string _output = string.Empty;
        public string Output
        {
            get => _output;
            set
            {
                _output = value;
                NotifyPropertyChanged();
            }
        }

        public async void SetOutput(string output)
        {
            Output = output;
            await Task.Delay(5000);
            Output = string.Empty;
        }

        // Single Image Properties
        private string? _filename;
        public string? Filename
        {
            get => _filename;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _filename = null;
                    return;
                }

                if (value.EndsWith(".png"))
                {
                    if (value.Length > 4)
                    {
                        _filename = value;
                    }
                }
                else
                {
                    _filename = value + ".png";
                }
            }
        }

        // Matrix Properties
        private int _dimensionSize = 5;
        public int DimensionSize
        {
            get => _dimensionSize;
            set
            {
                if (value >= 1)
                {
                    _dimensionSize = value;
                }
            }
        }


        public MandelbrotGeneratorViewModel()
        {
            Methods = new()
            {
                "CPU",
                "GPU"
            };
            Method = "CPU";
            Directory = @"C:\Mandelbaker\Output\";
        }


        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
