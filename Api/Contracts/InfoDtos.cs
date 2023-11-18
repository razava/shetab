namespace Api.Contracts;

public class InfoDto
{
    public List<InfoSingleton> Singletons { get; set; } = new List<InfoSingleton>();
    public List<InfoChart> LineCharts { get; set; } = new List<InfoChart>();

    public void Add(InfoDto infoDto)
    {
        Singletons.AddRange(infoDto.Singletons);
        LineCharts.AddRange(infoDto.LineCharts);
    }
}

public class InfoSingleton
{
    public string Value { get; set; }
    public string Title { get; set; }
    public string Icon { get; set; }
}

public class InfoChart
{
    public List<InfoSerie> Series { get; set; } = new List<InfoSerie>();
    public string ChartTitle { get; set; }
    public string ChartIcon { get; set; }
    public bool IsStacked { get; set; }
    public bool IsHorizontal { get; set; }

    public void Sort(int serieIndex = 0)
    {
        if (Series.Count < serieIndex)
            return;

        var keys = Series[0].Values.Select(p => double.Parse(p.Value)).ToArray();
        for (int i = 0; i < Series.Count; i++)
        {
            var tmpKeys = (double[])keys.Clone();
            var tmp = Series[i].Values.ToArray();
            Array.Sort(tmpKeys, tmp);
            Series[i].Values = new List<DataItem>(tmp);
            Series[i].Values.Reverse();
        }
    }
}

public class InfoSerie
{
    public List<DataItem> Values { get; set; } = new List<DataItem>();
    public string Title { get; set; }
    public string Icon { get; set; }
}

public class DataItem
{
    public DataItem(string title, string value, string displayValue, object parameters = null)
    {
        Title = title;
        Value = value;
        DisplayValue = displayValue;
        Parameters = parameters;
    }
    public string Title { get; set; }
    public string Value { get; set; }
    public string DisplayValue { get; set; }
    public object Parameters { get; set; }
}





public record ChartDto(
    int Id,
    int Order,
    int Code,
    string Title);



//public class TempDataItem
//{
//    public TempDataItem(DateTime title, string value, string displayValue, object parameters = null)
//    {
//        Title = title;
//        Value = value;
//        DisplayValue = displayValue;
//        Parameters = parameters;
//    }
//    public DateTime Title { get; set; }
//    public string Value { get; set; }
//    public string DisplayValue { get; set; }
//    public object Parameters { get; set; }
//}


