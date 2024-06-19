using Microsoft.KernelMemory;
using System.Diagnostics;

namespace RetailApp.ApiService.Services;

public class ImportService
{
    private readonly IKernelMemory kernelMemory;
    private readonly ILogger<ImportService> logger;

    public ImportService(IKernelMemory kernelMemory, ILogger<ImportService> logger)
    {
        this.kernelMemory = kernelMemory;
        this.logger = logger;
    }
    public async Task Import()
    {
        int page = 1;
        var categories = new[] { "/men/new-arrivals/view-all", "/ladies/new-arrivals/view-all" };
        var uri = "https://api.hm.com/search-services/v1/it_IT/listing/resultpage?page={0}&sort=RELEVANCE&pageId={1}&page-size=72&categoryId=men_newarrivals_all&filters=sale:false||oldSale:false||isNew:true&touchPoint=DESKTOP&skipStockCheck=false";

        using var client = new HttpClient();
        NewCollection currentCollection;
        var products = new List<string>();
        var producstUri = new List<Uri>();
        foreach (var c in categories)
        {
            do
            {
                currentCollection = await client.GetFromJsonAsync<NewCollection>(string.Format(uri, page, c));
                foreach (var m in currentCollection.PlpList.ProductList)
                {
                    Debug.WriteLine($"{m.ProductName} - {m.Id}");
                    Debug.WriteLine($"{m.Url}");
                    logger.LogInformation($"{m.ProductName} - {m.Id}");
                    var pageUri = string.Format("https://www2.hm.com{0}", m.Url);
                    producstUri.Add(new Uri(pageUri));
                }
                if (currentCollection!.Pagination.NextPageNum.HasValue)
                    page = currentCollection!.Pagination.NextPageNum.Value;
            }
            while (currentCollection!.Pagination.NextPageNum != null);
            page = 1;
        }

        foreach (var c in producstUri)
        {
            try
            {
                var documentId = await kernelMemory.ImportWebPageAsync(c.ToString());
                Debug.WriteLine($"Imported {c.ToString()} for {documentId}");
            }
            catch (Exception ex)
            {
                await Task.Delay(5000);
            }
        }
    }
}
