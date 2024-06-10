using ThunderstoreFormatter.SQLite.Model;

namespace ThunderstoreFormatter.DataClass;

public class Category
{
    public string CategoryName
    {
        get => _categoryName;
        set => _categoryName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<Mod> Mods
    {
        get => _mods;
        set
        {
            _mods = value ?? throw new ArgumentNullException(nameof(value));
            _numberInCategory = _mods.Count;
        }
    }
    public Category(string categoryName)
    {
        _categoryName = categoryName;
    }

    private string _categoryName;
    private int _numberInCategory;
    private List<Mod> _mods = new List<Mod>();
    
}