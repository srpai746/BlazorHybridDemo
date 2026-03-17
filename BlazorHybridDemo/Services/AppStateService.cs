namespace BlazorHybridDemo.Services;

public class AppStateService
{
    private string _userName = "";
    private int _counter = 0;
    private readonly List<string> _items = new();

    public string UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            LastUpdated = DateTime.Now;
            NotifyStateChanged();
        }
    }

    public int Counter
    {
        get => _counter;
        set
        {
            _counter = value;
            LastUpdated = DateTime.Now;
            NotifyStateChanged();
        }
    }

    public List<string> Items => _items;

    public DateTime LastUpdated { get; private set; } = DateTime.Now;

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    public void AddItem(string item)
    {
        _items.Add(item);
        LastUpdated = DateTime.Now;
        NotifyStateChanged();
    }

    public void RemoveItem(string item)
    {
        _items.Remove(item);
        LastUpdated = DateTime.Now;
        NotifyStateChanged();
    }

    public void ClearItems()
    {
        _items.Clear();
        LastUpdated = DateTime.Now;
        NotifyStateChanged();
    }
}