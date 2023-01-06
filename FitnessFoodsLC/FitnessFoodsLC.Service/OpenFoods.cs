using cloudscribe.HtmlAgilityPack;

using FitnessFoodsLC.Domain.Products;
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

        /// <summary>
        /// Get a list of foods and add to database
        /// </summary>
        /// <returns></returns>
        public async Task GetFoods()
        {
            int page = 1;
            int maxItens = 100;

            string initUrl = "/product";
            string endUrl = "\" title=\"";
            List<string> productsUrlToAdd = await GetProductUrl(page, initUrl, endUrl, maxItens);

            while (productsUrlToAdd.Count < maxItens)
            {
                page++;
                productsUrlToAdd.AddRange(await GetProductUrl(page, initUrl, endUrl, maxItens - productsUrlToAdd.Count));
            }

            foreach (var productUrl in productsUrlToAdd)
            {
                await GetFoodInfo(_openFoodUrl + productUrl);
            }

        }

        /// <summary>
        /// Get the Url of each product in a page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="initUrl"></param>
        /// <param name="endUrl"></param>
        /// <param name="maxItens"></param>
        /// <returns></returns>
        private async Task<List<string>> GetProductUrl(int page, string initUrl, string endUrl, int maxItens)
        {
            List<HtmlNode> productLinks = await GetProductsLink(page);

            List<string> productsUrlToAdd = new();
            foreach (var productUrl in productLinks)
            {
                if (productsUrlToAdd.Count >= maxItens)
                    break;

                var html = productUrl.InnerHtml;
                var fistIndex = html.IndexOf(initUrl);
                var subHtml = html[fistIndex..];
                var lastIndex = subHtml.LastIndexOf(endUrl);
                var url = subHtml[0..lastIndex];
                var exist = _productRepository.Exist(url).Result;
                if (!exist)
                {
                    productsUrlToAdd.Add(url);
                }
            }

            return productsUrlToAdd;
        }

        /// <summary>
        /// Get the HTML that contains a link of the product
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private async Task<List<HtmlNode>> GetProductsLink(int page)
        {
            HttpClient client = new();
            var response = await client.GetStringAsync(_openFoodUrl + '/' + page);

            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(response);

            var productLinks = htmlDoc.DocumentNode.Descendants("li")
                .Where(nodes => nodes.InnerHtml.Contains("product/"))
                .ToList();
            return productLinks;
        }

        /// <summary>
        /// Get the info of a product 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
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

            var codeString = barCode?.Split(' ')[0];
            long? code = null;

            if (codeString != null && codeString.Length > 0)
                code = long.Parse(codeString);

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

            var imageUrl = htmlDoc.DocumentNode.Descendants("img")
                .FirstOrDefault(i => i.Id == "og_image")?.Attributes
                .FirstOrDefault(a => a.Value.Contains("images.openfoodfacts.org"))?.Value;

            var product = new Product
            {
                Barcode = barCode,
                Code = code,
                ProductName = name,
                Quantity = quantity,
                Categories = categories,
                Packaging = packaging,
                Brands = brands,
                ImageUrl = imageUrl,
                Url = url,
                ImportedT = DateTime.Now.ToUniversalTime(),
                Status = CrossCutting.Enums.Status.imported
            };

            await _productRepository.Add(product);

            await Task.CompletedTask;
        }

        /// <summary>
        /// Get the value of propertie of a product
        /// </summary>
        /// <param name="propertie"></param>
        /// <param name="propertieName"></param>
        /// <returns></returns>
        private static Task<string?> GetPropertieValue(HtmlNode? propertie, string propertieName)
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

