using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TBStyler.WPF.Win32;
using System.Windows.Threading;
using System.Drawing;
using System.Windows.Automation.Peers;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Media;
using TBStyler.Settings;

namespace TBStyler.WPF.ViewModels;

internal class ColorPickerViewModel : ObservableObject
{
    private string hexValue;
    public string HexValue
    {
        get => hexValue;
        set
        {
            if (hexValue == value) return;
            SetProperty(ref hexValue, value);
            SetFromHexCode();
            SetPreviewColor();
        }
    }

    private int redValue;
    public int RedValue
    {
        get => redValue;
        set
        {
            if (redValue == value) return;
            SetProperty(ref redValue, value);
            settings.TaskbarStyleRed = value;
            SetHexCode();
            SetPreviewColor();
        }
    }

    private int greenValue;
    public int GreenValue
    {
        get => greenValue;
        set
        {
            if (greenValue == value) return;
            SetProperty(ref greenValue, value);
            settings.TaskbarStyleGreen = value;
            SetHexCode();
            SetPreviewColor();
        }
    }

    private int blueValue;
    public int BlueValue
    {
        get => blueValue;
        set 
        {
            if (blueValue == value) return;
            SetProperty(ref blueValue, value);
            settings.TaskbarStyleBlue = value;
            SetHexCode();
            SetPreviewColor();
        }
    }

    private int alphaValue;
    public int AlphaValue
    {
        get => alphaValue;
        set
        {   
            if (alphaValue == value) return;
            SetProperty(ref alphaValue, value);
            settings.TaskbarStyleAlpha = value;
            SetPreviewColor();
        }
    }

    private SolidColorBrush previewColor;
    public SolidColorBrush PreviewColor
    {
        get => previewColor;
        set
        {
            if (previewColor == value) return;
            SetProperty(ref previewColor, value);
        }
    }

    public ICommand GetColorFromPixelCommand;
    private SettingsDTO settings { get; set; }
    public ColorPickerViewModel(SettingsDTO settings)
    {
        this.settings = settings;

        RedValue = settings.TaskbarStyleRed;
        GreenValue = settings.TaskbarStyleGreen;
        BlueValue = settings.TaskbarStyleBlue;
        AlphaValue = settings.TaskbarStyleAlpha;

        GetColorFromPixelCommand = new RelayCommand(() =>
        {
            new Thread(() =>
            {
                ColorThread();
            }).Start();
        });
    }

    private void SetHexCode()
    {
        string hexR = RedValue.ToString("X2");
        string hexG = GreenValue.ToString("X2");
        string hexB = BlueValue.ToString("X2");

        HexValue = $"#{hexR}{hexG}{hexB}";
    }

    private void SetFromHexCode()
    {
        if (HexValue.StartsWith("#"))
        {
            HexValue = HexValue.Substring(1); // Remove the '#' character if present
        }

        if (HexValue.Length != 6)
        {
            throw new ArgumentException("Hexadecimal color code must be 6 characters long (excluding '#').");
        }

        RedValue = Convert.ToByte(HexValue.Substring(0, 2), 16); // Red component
        GreenValue = Convert.ToByte(HexValue.Substring(2, 2), 16); // Green component
        BlueValue = Convert.ToByte(HexValue.Substring(4, 2), 16); // Blue component
    }

    private void SetPreviewColor()
    {
        PreviewColor = new SolidColorBrush(
        System.Windows.Media.Color.FromArgb(
                (byte)AlphaValue,
                (byte)RedValue,
                (byte)GreenValue,
                (byte)BlueValue)
        );
    }

    private void ColorThread()
    {
        PointAPI lpPoint = new PointAPI();
        bool x = (GetAsyncKeyState(1) == 0);

        do
        {
            GetCursorPos(ref lpPoint);

            System.Drawing.Color colorp = GetColorAt(lpPoint.x, lpPoint.y);

            Application.Current.MainWindow.Dispatcher.Invoke(() =>
            {
                // sAlpha.Value = colorp.A;
                RedValue = colorp.R;
                GreenValue = colorp.G;
                BlueValue = colorp.B;
            });

        } while (!(GetAsyncKeyState(1) == 0));
    }

    private readonly Bitmap bmp = new Bitmap(1, 1);

    private System.Drawing.Color GetColorAt(int x, int y)
    {
        Rectangle bounds = new Rectangle(x, y, 1, 1);

        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.CopyFromScreen(bounds.Location, System.Drawing.Point.Empty, bounds.Size);
        }

        return bmp.GetPixel(0, 0);
    }

}
