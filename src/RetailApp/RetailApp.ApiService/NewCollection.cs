using System.Text.Json.Serialization;

namespace RetailApp.ApiService;


public class NewCollection
{
    [JsonPropertyName("requestDateTime")]
    public DateTime? RequestDateTime { get; set; }

    [JsonPropertyName("responseSource")]
    public string ResponseSource { get; set; }

    [JsonPropertyName("pagination")]
    public Pagination Pagination { get; set; }

    [JsonPropertyName("plpList")]
    public PlpList PlpList { get; set; }
}

public class Availability
{
    [JsonPropertyName("stockState")]
    public string StockState { get; set; }

    [JsonPropertyName("comingSoon")]
    public bool? ComingSoon { get; set; }
}

public class Facet
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("selectedCount")]
    public int? SelectedCount { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("min")]
    public double? Min { get; set; }

    [JsonPropertyName("max")]
    public double? Max { get; set; }

    [JsonPropertyName("filterValues")]
    public List<FilterValue> FilterValues { get; set; }

    [JsonPropertyName("groups")]
    public List<Group> Groups { get; set; }
}

public class FacetValue
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("count")]
    public int? Count { get; set; }

    [JsonPropertyName("selected")]
    public bool? Selected { get; set; }
}

public class FilterValue
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("count")]
    public int? Count { get; set; }

    [JsonPropertyName("selected")]
    public bool? Selected { get; set; }
}

public class Group
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("facetValues")]
    public List<FacetValue> FacetValues { get; set; }
}

public class Image
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public class Option
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("label")]
    public string Label { get; set; }
}

public class Pagination
{
    [JsonPropertyName("currentPage")]
    public int? CurrentPage { get; set; }

    [JsonPropertyName("nextPageNum")]
    public int? NextPageNum { get; set; }

    [JsonPropertyName("totalPages")]
    public int? TotalPages { get; set; }
}

public class PlpList
{
    [JsonPropertyName("productList")]
    public List<ProductList> ProductList { get; set; }

    [JsonPropertyName("sortOptions")]
    public SortOptions SortOptions { get; set; }

    [JsonPropertyName("numberOfHits")]
    public int? NumberOfHits { get; set; }

    [JsonPropertyName("sliceRanges")]
    public List<SliceRange> SliceRanges { get; set; }

    [JsonPropertyName("facets")]
    public List<Facet> Facets { get; set; }
}

public class Price
{
    [JsonPropertyName("priceType")]
    public string PriceType { get; set; }

    [JsonPropertyName("price")]
    public double? PriceValue { get; set; }

    [JsonPropertyName("minPrice")]
    public double? MinPrice { get; set; }

    [JsonPropertyName("maxPrice")]
    public double? MaxPrice { get; set; }

    [JsonPropertyName("formattedPrice")]
    public string FormattedPrice { get; set; }
}

public class ProductList
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("trackingId")]
    public string TrackingId { get; set; }

    [JsonPropertyName("productName")]
    public string ProductName { get; set; }

    [JsonPropertyName("external")]
    public bool? External { get; set; }

    [JsonPropertyName("brandName")]
    public string BrandName { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("showPriceMarker")]
    public bool? ShowPriceMarker { get; set; }

    [JsonPropertyName("prices")]
    public List<Price> Prices { get; set; }

    [JsonPropertyName("availability")]
    public Availability Availability { get; set; }

    [JsonPropertyName("swatches")]
    public List<Swatch> Swatches { get; set; }

    [JsonPropertyName("productMarkers")]
    public List<ProductMarker> ProductMarkers { get; set; }

    [JsonPropertyName("images")]
    public List<Image> Images { get; set; }

    [JsonPropertyName("hasVideo")]
    public bool? HasVideo { get; set; }

    [JsonPropertyName("colorName")]
    public string ColorName { get; set; }

    [JsonPropertyName("isPreShopping")]
    public bool? IsPreShopping { get; set; }

    [JsonPropertyName("isOnline")]
    public bool? IsOnline { get; set; }

    [JsonPropertyName("modelImage")]
    public string ModelImage { get; set; }

    [JsonPropertyName("colors")]
    public string Colors { get; set; }

    [JsonPropertyName("colourShades")]
    public string ColourShades { get; set; }

    [JsonPropertyName("productImage")]
    public string ProductImage { get; set; }

    [JsonPropertyName("newArrival")]
    public bool? NewArrival { get; set; }

    [JsonPropertyName("isLiquidPixelUrl")]
    public bool? IsLiquidPixelUrl { get; set; }

    [JsonPropertyName("colorWithNames")]
    public string ColorWithNames { get; set; }

    [JsonPropertyName("mainCatCode")]
    public string MainCatCode { get; set; }

    [JsonPropertyName("sellingAttribute")]
    public string SellingAttribute { get; set; }

    [JsonPropertyName("qualityMarker")]
    public string QualityMarker { get; set; }
}

public class ProductMarker
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}


public class SliceRange
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("from")]
    public int? From { get; set; }

    [JsonPropertyName("to")]
    public int? To { get; set; }
}

public class SortOptions
{
    [JsonPropertyName("selected")]
    public string Selected { get; set; }

    [JsonPropertyName("options")]
    public List<Option> Options { get; set; }
}

public class Swatch
{
    [JsonPropertyName("articleId")]
    public string ArticleId { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("colorName")]
    public string ColorName { get; set; }

    [JsonPropertyName("colorCode")]
    public string ColorCode { get; set; }

    [JsonPropertyName("trackingId")]
    public string TrackingId { get; set; }

    [JsonPropertyName("productImage")]
    public string ProductImage { get; set; }
}


