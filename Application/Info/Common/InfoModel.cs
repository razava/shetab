using Application.Common.Interfaces.Info;

namespace Application.Info.Common;

public class InfoModel
{
    public List<InfoSingleton>? Singletons { get; set; } = null;
    public List<InfoChart>? Charts { get; set; } = null;
    public List<LocationInfo>? Locations { get; set; } = null;
    public InfoModel Add(InfoSingleton singleton)
    {
        if(Singletons is null)
            Singletons = new List<InfoSingleton>();

        Singletons.Add(singleton);
        return this;
    }
    public InfoModel Add(InfoChart chart)
    {
        if(Charts is null)
            Charts = new List<InfoChart>();

        Charts.Add(chart);
        return this;
    }
    public InfoModel Add(LocationInfo location)
    {
        if (Locations is null)
            Locations = new List<LocationInfo>();

        Locations.Add(location);
        return this;
    }
    public InfoModel Add(InfoModel infoDto)
    {
        if(Singletons is null)
            Singletons = new List<InfoSingleton>();
        if(Charts is null)
            Charts = new List<InfoChart>();
        if (Locations is null)
            Locations = new List<LocationInfo>();

        if (infoDto.Singletons is not null)
            Singletons.AddRange(infoDto.Singletons);

        if(infoDto.Charts is not null)
            Charts.AddRange(infoDto.Charts);

        if(infoDto.Locations is not null)
            Locations.AddRange(infoDto.Locations);
        return this;
    }
}

public record InfoSingleton(string Value, string Title, string? Icon);

public record InfoChart(string ChartTitle, string ChartIcon, bool IsStacked, bool IsHorizontal)
{
    public List<InfoSerie> Series { get; set; } = new List<InfoSerie>();
    public InfoChart Add(InfoSerie serie)
    {
        Series.Add(serie);
        return this;
    }

    public InfoChart Sort(int index)
    {
        if (Series.Count == 0)
            return this;
        if(!Series.SelectMany(s => s.Values).Any())
            return this;

        var keys = Series[index].Values.Select(v => Double.Parse(v.Value)).ToList();
        for (var i  = 0; i < Series.Count; i++)
        {
            Series[i].Values = sortLike(Series[i].Values, keys);
        }

        return this;
    }

    private List<T> sortLike<T, T2>(List<T> sourceList, List<T2> sortKeys)
    {
        var t = sourceList.ToArray();
        var k = sortKeys.ToArray();

        Array.Sort(k, t);
        return t.Reverse().ToList();
    }
    public record LocationInfo(Guid ReportId, double Latitude, double Longitude);
    /*
    public InfoChart Sort(int serieIndex = 0)
    {
        if (Series.Count < serieIndex)
            return this;

        var keys = Series[0].Values.Select(p => double.Parse(p.Value)).ToArray();
        for (int i = 0; i < Series.Count; i++)
        {
            var tmpKeys = (double[])keys.Clone();
            var tmp = Series[i].Values.ToArray();
            Array.Sort(tmpKeys, tmp);
            Series[i].Values = new List<DataItem>(tmp);
            Series[i].Values.Reverse();
        }
        return this;
    }
    */
}

public record InfoSerie(string Title, string Icon)
{
    public List<DataItem> Values { get; set; } = new List<DataItem>();
    public InfoSerie Add(DataItem item)
    {
        Values.Add(item);
        return this;
    }

    public InfoSerie Add(List<DataItem> items)
    {
        Values.AddRange(items);
        return this;
    }

    public InfoSerie Add(string Title, string Value, string DisplayValue, string? Parameters = null)
    {
        Values.Add(new DataItem(Title, Value, DisplayValue, Parameters));
        return this;
    }
}

public record DataItem(string Title, string Value, string DisplayValue, string? Parameters = null);

public record LocationItem(Guid ReportId, double Latitude, double Longitude);
public record LocationInfo(List<LocationItem> Locations);