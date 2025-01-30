using CoolShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoolShop.Services
{
    public class ClothService
    {
        const string Root = Constants.ClothesApiRoot;
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
        public async Task<IEnumerable<Cloth>> Get()
        {
            try
            {
                HttpClient client = GetClient();
                string result = await client.GetStringAsync(Root);
                return JsonSerializer.Deserialize<IEnumerable<Cloth>>(result, options);
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
        
        public async Task<IEnumerable<Cloth>> Find(string search)
        {
            try
            {
                HttpClient client = GetClient();

                IEnumerable<Cloth> allClothes = await Get();
                IEnumerable<Cloth> foundClothes = allClothes.Where
                    (c =>
                    c.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.Description.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0
                    );

                return foundClothes;
            }
            catch (Exception ex)
            {
                // Обработка исключений
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                throw;
            }
        }
    }
    public class MaterialService
    {
        const string Root = Constants.MaterialsApiRoot;
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
        public async Task<IEnumerable<Material>> Get()
        {
            try
            {
                HttpClient client = GetClient();
                string result = await client.GetStringAsync(Root);
                return JsonSerializer.Deserialize<IEnumerable<Material>>(result, options);
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
    }
    public class MaterialInClothService
    {
        const string Root = Constants.MaterialsInClothesApiRoot;
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
        public async Task<IEnumerable<MaterialInCloth>> Get()
        {
            try
            {
                HttpClient client = GetClient();
                string result = await client.GetStringAsync(Root);
                return JsonSerializer.Deserialize<IEnumerable<MaterialInCloth>>(result, options);
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
    }
}
