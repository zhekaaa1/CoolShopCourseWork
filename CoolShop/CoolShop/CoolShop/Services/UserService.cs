using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CoolShop.Models;


namespace CoolShop.Services
{
    public class UserService
    {
        const string Root = Constants.UsersApiRoot; // обращайте внимание на конечный слеш
        // настройки для десериализации для нечувствительности к регистру символов
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем всех друзей
        public async Task<IEnumerable<User>> Get()
        {
            try
            {
                HttpClient client = GetClient();
                string result = await client.GetStringAsync(Root);
                return JsonSerializer.Deserialize<IEnumerable<User>>(result, options);
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


        // добавляем одного друга
        public async Task<User> Add(User user)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(Root,
                new StringContent(
                    JsonSerializer.Serialize(user),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            else
            {
                Console.WriteLine("!!!!!!!!!!!!!!!!!! HttpStatusCode.OK");
            }
            return JsonSerializer.Deserialize<User>(
                await response.Content.ReadAsStringAsync(), options);
        }
        // обновляем друга
        public async Task<User> Update(User user)
        {
            HttpClient client = GetClient();
            var response = await client.PutAsync(Root,
                new StringContent(
                    JsonSerializer.Serialize(user),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonSerializer.Deserialize<User>(
                await response.Content.ReadAsStringAsync(), options);
        }
        // удаляем друга
        public async Task<User> Delete(int id)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(Root + id);
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonSerializer.Deserialize<User>(
               await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<User> Find(string login, string password)
        {
            try
            {
                HttpClient client = GetClient();

                // Получаем всех друзей с сервера
                IEnumerable<User> allUsers = await Get();

                // Выполняем фильтрацию по логину и паролю
                User foundFriend = allUsers.FirstOrDefault(f => f.Mail == login && f.Password == password);

                return foundFriend;
            }
            catch (Exception ex)
            {
                // Обработка исключений
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                throw;
            }
        }
        
    }
}
