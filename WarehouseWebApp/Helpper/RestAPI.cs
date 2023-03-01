using WarehouseWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Crmf;

namespace EcommerceWebsite.Helpper
{
    public static class RestAPI
    {
        public static HttpClient client = new HttpClient();

        public static async Task<string> GetJSON(string path)
        {
            string result = "";
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }
    }
}
