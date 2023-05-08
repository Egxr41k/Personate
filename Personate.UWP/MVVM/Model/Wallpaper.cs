using System;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Microsoft.Win32;
using System.Linq;
using System.Threading.Tasks;

namespace Personate.UWP.MVVM.Model
{
    public class Wallpaper
    {
        public BitmapImage image;
        public string path;
        public string resolution;
        public string name;

        public Wallpaper(string path)
        {
            this.path = path;
            image = new BitmapImage(new Uri(path));
            name = path.Split('\\').Last();
            resolution = $"{image.PixelWidth}" + "x" + $"{image.PixelHeight}";
        }
        public static async Task<string> OpenImage()
        {
            //OpenFileDialog ofd = new()
            //{
            //    Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*",
            //    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            //};
            //if (ofd.ShowDialog() == true)
            //{
            //    path = ofd.FileName;
            //    result = true;
            //}


            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");


            StorageFile file = await picker.PickSingleFileAsync();
            return file.Name;
        }

        public void SaveImage()
        {
            //SaveFileDialog sfd = new()
            //{
            //    Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*",
            //    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            //};
            //if (sfd.ShowDialog() == true)
            //    System.Drawing.Image.FromFile(path).Save(
            //        sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        //public void SetImageToBg()
        //{
        //    if (!Win32.SystemParametersInfo(0x14, 0, path, 0x01 | 0x02))
        //        Console.WriteLine("Error");
        //}

    }
}