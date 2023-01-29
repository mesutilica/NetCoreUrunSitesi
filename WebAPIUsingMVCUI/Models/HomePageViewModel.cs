using Entities;

namespace NetCoreUrunSitesi.Models
{
    public class HomePageViewModel
    {
        public List<Slider> Sliders { get; set; }
        public List<Product> Products { get; set; }
        public List<News> News { get; set; }
    }
}
