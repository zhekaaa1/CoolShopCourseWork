using CoolShop.Models;
using CoolShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoolShop.Services
{
    public class OrderService
    {
        const string Root = Constants.OrdersApiRoot;
        const string CityRoot = Constants.CitiesApiRoot;
        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }
        public async Task<IEnumerable<Order>> Get()
        {
            try
            {
                HttpClient client = GetClient();
                string result = await client.GetStringAsync(Root);
                return JsonSerializer.Deserialize<IEnumerable<Order>>(result, options);
            }
            catch (HttpRequestException ex)
            {
                // Обработка ошибки HTTP-запроса
                Console.WriteLine($"Ошибка HTTP-запроса: {ex.Message}");
                Console.WriteLine($"URL запроса: {Root}");
                throw; // Пробросим исключение дальше, чтобы обработать его на уровне вызова, если это необходимо
            }
            catch (Exception ex)
            {
                // Обработка других исключений
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                throw; // Пробросим исключение дальше, чтобы обработать его на уровне вызова, если это необходимо
            }
        }
        public async Task<Order> Add(Order order)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(Root,
                new StringContent(
                    JsonSerializer.Serialize(order),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            else
            {
                Console.WriteLine("!!!!!!!!!!!!!!!!!! HttpStatusCode.OK");
            }
            return JsonSerializer.Deserialize<Order>(
                await response.Content.ReadAsStringAsync(), options);
        }
        public async Task<IEnumerable<City>> GetCities()
        {
            HttpClient client = GetClient();
            await Console.Out.WriteLineAsync("!!!!!!!!!!!!! клиент получен");
            string result = await client.GetStringAsync(CityRoot);
            await Console.Out.WriteLineAsync($"!!!!!!!!!!!!! получены города: {result}");
            return JsonSerializer.Deserialize<IEnumerable<City>>(result, options);
        }
    }
}
