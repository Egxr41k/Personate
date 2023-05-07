using Personate.MVVM.ViewModel.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Personate.MVVM.ViewModel;
public class WallsMenuViewModel : Base.ViewModel
{
    static HttpClient httpClient = new();

    readonly string[] PathToWallpapers = Directory.GetFiles(
        MainViewModel.RESOURCEPATH + "\\PersonateLib\\Wallpapers");
    public static Base.Command? UploadImageCommand { get; set; }
    public Base.Command ShowMoreCommand { get; set; }

    private int index = 0; // counter


    private ObservableCollection<WallCardViewModel> _wallCardViewModels;
    public IEnumerable<WallCardViewModel> WallCardViewModels => _wallCardViewModels;


    private void WallGridInit()
    {
        for (int column = 0; column < 23; column++)
        {
            if (index < PathToWallpapers.Length)
            {

                var request = httpClient.GetAsync("https://api.waifu.im/search/?included_tags=hentai").Result;
                var responce = request.Content.ReadAsStringAsync().Result;

                JSON? restoredPerson = JsonSerializer.Deserialize<JSON>(responce);
                //string? url = restoredPerson?.images[0].url;


                try
                {
                    _wallCardViewModels.Add(
                        new WallCardViewModel(
                            new Wallpaper(
                                restoredPerson?.images[0].url
                            )
                        )
                    );
                } catch( Exception ex ) { break; }
                index++;
            }
        }
    }
    public WallsMenuViewModel()
    {

        var request = httpClient.GetAsync("https://api.waifu.im/search/?included_tags=hentai").Result;
        var responce = request.Content.ReadAsStringAsync().Result;

        JSON? restoredPerson = JsonSerializer.Deserialize<JSON>(responce);
        string? url = restoredPerson?.images[0].url;


        _wallCardViewModels =
                new ObservableCollection<WallCardViewModel>();

        WallGridInit();

        ShowMoreCommand = new( o =>
        {
            WallGridInit();
        });
    }
}
record JSON(Hentai[] images);
record Hentai(string url);
