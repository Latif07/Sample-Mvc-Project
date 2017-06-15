
namespace SampleWebProject.Models {

    public class PageHeaderModel
    {
        public PageHeaderModel()
        {
            Title = string.Empty;
            EnableBackToListButton = false;
        }
        public string Title { get; set; }
        public bool EnableBackToListButton { get; set; }
    }
}