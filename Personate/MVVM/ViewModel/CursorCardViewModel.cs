using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;


namespace Personate.MVVM.ViewModel;
internal class CursorCardViewModel : Base.ViewModel
{
    public static Base.Command? CursorViewCommand { get; set; }

    private BitmapImage cursorImg;
    public BitmapImage CursorImg
    {
        get => cursorImg;
        set => Set(ref cursorImg, value);
    }

    private string cursorTitle;
    public string CursorTitle
    {
        get => cursorTitle;
        set => Set(ref cursorTitle, value);
    }

    private string cursorCount;
    public string CursorCount
    {
        get => cursorCount;
        set => Set(ref cursorCount, value);
    }

    protected virtual void Init()
    {
        CursorImg = new BitmapImage(new Uri(CursorModel.CursorImg));
        CursorTitle = CursorModel.CursorTitle + " " + CursorModel.CursorColor;
        CursorCount = CursorModel.CursorCount.ToString();
    }

    public CursorCardViewModel()
    {
        Init();
    }
}