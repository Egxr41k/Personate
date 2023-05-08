using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace Personate.UWP.MVVM.ViewModel
{
    public class CursorsMenuViewModel : ObservableObject
    {
        readonly string[] PathToCursors = Directory.GetDirectories(
            MainViewModel.RESOURCEPATH + "\\PersonateLib\\Cursors");
        public static ICommand UploadCursorCommand { get; set; }

        //private void CursorGridInit()
        //{
        //    CursorCardViewModel[] cursors = new CursorCardViewModel[36];
        //    ContentControl[] controls = new ContentControl[36];
        //    for (int row = 0; row < 4; row++)
        //    {
        //        for (int column = 0; column < 4; column++)
        //        {
        //            foreach (string CursorName in PathToCursors)
        //            {
        //                CursorModel.CursorTitle = CursorName.Split('\\').Last();

        //                string[] CursorsColor = Directory.GetDirectories(CursorName);

        //                foreach (string CursorColor in CursorsColor)
        //                {

        //                    CursorModel.CursorColor = CursorColor.Split('\\').Last();

        //                    string[] files = Directory.GetFiles(CursorColor);
        //                    CursorModel.CursorCount = files.Length;
        //                    foreach (string file in files)
        //                    {

        //                        string filename = file.Split('\\').Last();
        //                        if (filename == "pointer.cur")
        //                        {
        //                            CursorModel.CursorImg = file;
        //                        }
        //                    }

        //                    cursors[index] = new();
        //                    controls[index] = new()
        //                    {
        //                        Content = cursors[index]
        //                    };
        //                    SetToGrid(controls[index], row, column);
        //                    //index++;
        //                }
        //            }
        //        }
        //    }
        //}

        public CursorsMenuViewModel()
        {
            //ColumnsInit(4);
            //RowsInit(2, 195);
            //CursorGridInit();
            //ShowMoreCommand = new Base.Command(o =>
            //{
            //    //helper.RowsInit(2, 195);
            //});
            //GridControl = MainGrid;
        }
    }
}