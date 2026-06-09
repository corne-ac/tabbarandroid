using TabbedPageBottomGapRepro.Models;

namespace TabbedPageBottomGapRepro.Pages;

public partial class Tab3Page : ContentPage
{
    public Tab3Page()
    {
        InitializeComponent();
        BindingContext = this;
        Items = GenerateDummyData();
        collectionView.ItemsSource = Items;
    }

    public List<DummyItem> Items { get; set; } = new();

    private List<DummyItem> GenerateDummyData()
    {
        return new List<DummyItem>
        {
            new("Item 1", "Description for first item"),
            new("Item 2", "Description for second item"),
            new("Item 3", "Description for third item"),
            new("Item 4", "Description for fourth item"),
            new("Item 5", "Description for fifth item"),
            new("Item 6", "Description for sixth item"),
            new("Item 7", "Description for seventh item"),
            new("Item 8", "Description for eighth item"),
        };
    }

    private void OnRefreshClicked(object? sender, EventArgs e)
    {
        Items = GenerateDummyData();
        collectionView.ItemsSource = Items;
    }
}
