namespace WMS.Models.Common;

public class BaseFilter
{
    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string OrderBy { get; set; }

    public SortingOrder? SortingOrder { get; set; }
}

public enum SortingOrder
{
    ASC = 1,
    DESC = 2,
}