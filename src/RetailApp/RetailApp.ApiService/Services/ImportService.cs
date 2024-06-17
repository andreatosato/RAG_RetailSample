﻿using Microsoft.KernelMemory;

namespace RetailApp.ApiService.Services;

public class ImportService
{
    private readonly IKernelMemory kernelMemory;

    public ImportService(IKernelMemory kernelMemory)
    {
        this.kernelMemory = kernelMemory;
    }
    public async Task Import()
    {
        int page = 1;
        var categories = new[] { "/men/new-arrivals/view-all", "/ladies/new-arrivals/view-all" };
        var uri = "https://api.hm.com/search-services/v1/it_IT/listing/resultpage?page={0}&sort=RELEVANCE&pageId={1}&page-size=72&categoryId=men_newarrivals_all&filters=sale:false||oldSale:false||isNew:true&touchPoint=DESKTOP&skipStockCheck=false";

        using var client = new HttpClient();
        NewCollection currentCollection;
        var products = new List<string>();
        foreach (var c in categories)
        {
            do
            {
                currentCollection = await client.GetFromJsonAsync<NewCollection>(string.Format(uri, page, c));
                Parallel.ForEach(currentCollection.PlpList.ProductList, async (m) =>
                {
                    await kernelMemory.ImportWebPageAsync(string.Format("https://www2.hm.com{0}", m.Url));
                });
                //foreach (var m in currentCollection.PlpList.ProductList)
                //{
                //    await kernelMemory.ImportWebPageAsync(string.Format("https://www2.hm.com{0}", m.Url));
                //}
                if (currentCollection!.Pagination.NextPageNum.HasValue)
                    page = currentCollection!.Pagination.NextPageNum.Value;
            }
            while (currentCollection!.Pagination.NextPageNum != null);
            page = 1;
        }
    }
}