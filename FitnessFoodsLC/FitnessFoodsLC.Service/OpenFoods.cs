using cloudscribe.HtmlAgilityPack;

using FitnessFoodsLC.Interface;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

namespace FitnessFoodsLC.Service
{
    public class OpenFoods : IOpenFoods
    {
        private readonly string _openFoodUrl;
        private readonly IProductRepository _productRepository;

        public OpenFoods(IProductRepository productRepository)
        {
            _openFoodUrl = "https://world.openfoodfacts.org";
            _productRepository = productRepository;
        }


        public async Task GetFoods()
        {
            HttpClient client = new();
            var response = await client.GetStringAsync(_openFoodUrl);

            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(response);

            var productLinks = htmlDoc.DocumentNode.Descendants("li")
                .Where(nodes => nodes.InnerHtml.Contains("product/"))
                .ToList();

            string initUrl = "/product";
            string endUrl = "\" title=\"";
            List<string> productsUrlToAdd = new();
            productsUrlToAdd.AddRange(from product in productLinks
                                   let html = product.InnerHtml
                                   let fistIndex = html.IndexOf(initUrl)
                                   let subHtml = html[fistIndex..]
                                   let lastIndex = subHtml.LastIndexOf(endUrl)
                                   let url = subHtml[0..lastIndex]
                                   let exist = _productRepository.Exist(url).Result
                                   where !exist
                                   select url);

            //Parallel.ForEach(productsUrlToAdd, async productUrl => await GetFoodInfo(_openFoodUrl + productUrl));

            foreach(var productUrl in productsUrlToAdd)
            {
                await GetFoodInfo(_openFoodUrl + productUrl);
            }

        }

        public async Task GetFoodInfo(string url)
        {
            HttpClient client = new();
            var response = await client.GetStringAsync(url);

            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(response);

            var info = htmlDoc.DocumentNode.Descendants("p").ToList();

            var barCode = await GetPropertieValue(
                info.FirstOrDefault(i => i.Id == "barcode_paragraph"), 
                "Barcode:");
            var code = barCode.Split(' ')[0];
            var name = await GetPropertieValue(
                info.FirstOrDefault(i => i.Id == "field_generic_name"),
                "Common name:");
            var quantity = await GetPropertieValue(
                info.FirstOrDefault(i => i.Id == "field_quantity"),
                "Quantity:");
            var categories = await GetPropertieValue(
                info.FirstOrDefault(i => i.Id == "field_categories"),
                "Categories:");
            var packaging = await GetPropertieValue(
                info.FirstOrDefault(i => i.Id == "field_packaging"),
                "Packaging:");
            var brands = await GetPropertieValue(
                info.FirstOrDefault(i => i.Id == "field_brands"),
                "Brands:");

            var image = htmlDoc.DocumentNode.Descendants("img").First(i => i.Id == "og_image");

            await Task.CompletedTask;
        }

        private Task<string?> GetPropertieValue(HtmlNode? propertie, string propertieName)
        {
            if (propertie != null)
            {
                string text = propertie.InnerText;
                var propertieIndex = text.IndexOf(propertieName) + propertieName.Length;
                var newText = text[propertieIndex..].Trim();

                return Task.FromResult<string?>(newText);
            }

            return Task.FromResult<string?>(null);
        }
    }
}

