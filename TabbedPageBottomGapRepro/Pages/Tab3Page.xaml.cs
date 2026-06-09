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
        var items = new List<DummyItem>();
        for (int i = 1; i <= 30; i++)
        {
            items.Add(new DummyItem($"Item {i}", $"Description for item {i}"));
        }

        return items;
    }

    private void OnRefreshClicked(object? sender, EventArgs e)
    {
        Items = GenerateDummyData();
        collectionView.ItemsSource = Items;
    }
}
