using ThunderstoreFormatter.SQLite.Model;

namespace ThunderstoreFormatter.DataClass;

public class Category
{
    public string CategoryName
    {
        get => _categoryName;
        set => _categoryName = value ?? throw new ArgumentNullException(nameof(value));
    }
    public int NumberInCategory => _numberInCategory;

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
    public Category(string categoryName, Mod firstCategoryMod)
    {
        _categoryName = categoryName;
        AddMod(firstCategoryMod);
    }

    public void AddMod(Mod modToAdd)
    {
        _mods.Add(modToAdd);
        _numberInCategory = _mods.Count;
    }

    private string _categoryName;
    private int _numberInCategory;

    

    private List<Mod> _mods = new List<Mod>();
    
}