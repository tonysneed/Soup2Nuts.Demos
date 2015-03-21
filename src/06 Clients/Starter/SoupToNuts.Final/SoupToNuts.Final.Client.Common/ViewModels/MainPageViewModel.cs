using SimpleMvvmToolkit;

namespace SoupToNuts.Final.Client.Common.ViewModels
{
    public class MainPageViewModel : ViewModelBase<MainPageViewModel>
    {
        private string _bannerText = "Northwind Customer Orders";
        public string BannerText
        {
            get
            {
                return _bannerText;
            }
            set
            {
                _bannerText = value;
                NotifyPropertyChanged(m => m.BannerText);
            }
        }
    }
}
