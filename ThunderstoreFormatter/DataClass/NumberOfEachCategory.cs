namespace ThunderstoreFormatter.DataClass;

public class NumberOfEachCategory
{
    private String categoryName;
    private int number;

    public string CategoryName
    {
        get => categoryName;
        set => categoryName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Number
    {
        get => number;
        set => number = value;
    }
}