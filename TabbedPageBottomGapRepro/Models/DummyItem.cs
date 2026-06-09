namespace TabbedPageBottomGapRepro.Models;

public class DummyItem
{
    public string Title { get; set; }
    public string Description { get; set; }

    public DummyItem(string title, string description)
    {
        Title = title;
        Description = description;
    }
}
