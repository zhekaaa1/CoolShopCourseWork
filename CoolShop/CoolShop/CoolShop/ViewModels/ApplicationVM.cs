using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using CoolShop.Services;
using CoolShop.Models;
using CoolShop.Views;
using Xamarin.Forms.Internals;


namespace CoolShop.ViewModels
{
    public class ApplicationVM
    {
        bool initialized = false;   // была ли начальная инициализация
        User selectedUser;  // выбранный друг
        Cloth selectedCloth;
        private bool isBusy;    // идет ли загрузка с сервера
        public bool LoggedIn = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<User> Users { get; set; }
        UserService userService = new UserService();
        MaterialService materialService = new MaterialService();
        MaterialInClothService materialInClothService = new MaterialInClothService();


        public ObservableCollection<Cloth> Clothes { get; set; }
        ClothService clothService = new ClothService();

        public ICommand SearchCommand { get; set; }
        //пользователи
        public ICommand LoginCommand { get; set; }
        public ICommand ToLoginCommand { get; set; }
        public ICommand RegistrateCommand { get; set; }
        public ICommand UpdateUserCommand { get; set; }

        //одежда
        public ICommand GetClothesCommand { get; set; }
        

        public INavigation Navigation { get; set; }
        public ApplicationVM()
        {
            Users = new ObservableCollection<User>();
            Clothes = new ObservableCollection<Cloth>();
            SearchCommand = new Command(Search);
            LoginCommand = new Command(Login);
            ToLoginCommand = new Command(ToLogin);
            RegistrateCommand = new Command(Registrate);
            UpdateUserCommand = new Command(UpdateUser);
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
                OnPropertyChanged("IsLoaded");
            }
        }
        public bool IsLoaded
        {
            get { return !isBusy; }
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        //НАЗАД
        private void Back()
        {
            Navigation.PopAsync();
        }

        public Cloth SelectedCloth
        {
            get { return selectedCloth; }
            set
            {
                if (selectedCloth != value)
                {
                    Cloth tempCloth = new Cloth()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        ImgPath = value.ImgPath,
                        Description = value.Description
                    };
                    selectedCloth = null;
                    OnPropertyChanged("SelectedCloth");
                    Navigation.PushAsync(new ClothDetailPage(tempCloth, this));
                }
            }
        }







        //ПОИСКОВАЯ СТРОКА
        public async void Search(object request)
        {
            string stringrequest = request as string;
            if (stringrequest != null)
            {
                //код поиска...
            }
            else
            {
                //вернуть искходное состояние...
            }
        }




        //ПОЛЬЗОВАТЕЛИ
        public void ToLogin()
        {
            Navigation.PushAsync(new AuthPage());
        }
        public async void Login(object userObject)
        {
            User user = userObject as User;
            if (user != null)
            {
                try
                {
                    IsBusy = true;
                    User foundUser = await userService.Find(user.Mail, user.Password);

                    if (foundUser != null)
                    {
                        LoggedUser.User = foundUser;
                        LoggedIn = true;
                        await Navigation.PushAsync(new ProfilePage(foundUser, this));
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Application.Current.MainPage.DisplayAlert("Ошибка", "Пользователь не найден - возвращено NULL в Find", "OK");
                        });
                    }
                }
                catch (Exception ex)
                {
                    // Обработка исключений
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                    // Можно добавить логику для вывода сообщения об ошибке пользователю
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "userObject=NULL", "OK");
            }
        }

        private async void Registrate(object userObject)
        {

            User user = userObject as User;
            if (user != null)
            {
                User addedUser = await userService.Add(user);
                Console.WriteLine(addedUser.Id);
                LoggedUser.User = addedUser;

                await Navigation.PushAsync(new ProfilePage(LoggedUser.User, this));
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "addedUser=NULL", "OK");
                });
            }
        }
        private async void UpdateUser(object userObject)
        {
            User user = userObject as User;
            if (user != null)
            {
                User updatedUser = await userService.Update(user);
                Console.WriteLine(updatedUser.Id + "\n" + updatedUser.IName);
                LoggedUser.User = updatedUser;

                await Navigation.PushAsync(new ProfilePage(LoggedUser.User, this));
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "ДТП", "OK");
                });
            }
        }


        //ОДЕЖДА
        public async Task GetClothes()
        {
            if (initialized == true) return;
            IsBusy = true;
            IEnumerable<Cloth> clothes = await clothService.Get();

            // очищаем список
            //Friends.Clear();
            while (Clothes.Any())
                Clothes.RemoveAt(Clothes.Count - 1);

            // добавляем загруженные данные
            foreach (Cloth f in clothes)
                Clothes.Add(f);
            IsBusy = false;
            initialized = true;
        }
        //public async Task<ObservableCollection<Material>> GetMaterials (Cloth cloth)
        //{
        //    IEnumerable<Material> allmaterials = await materialService.Get();
        //    IEnumerable<MaterialInCloth> materials = await materialinclothService.Get();
        //    ObservableCollection<Material> foundMaterials = new ObservableCollection<Material>();
        //    foreach (MaterialInCloth material in materials)
        //    {
        //        MaterialInCloth material1 = materials.FirstOrDefault(m => m.Cloth.Id == cloth.Id);
        //        if (material1 == null)
        //        {
        //            foreach (Material material2 in allmaterials)
        //            {
        //                Material material3 = allmaterials.FirstOrDefault(m => m.Id == material1.Material.Id);
        //                foundMaterials.Add(material3);
        //            }
        //        }
        //    }
        //    return foundMaterials;
        //}
        public async Task<IEnumerable<Material>> GetMaterialsByClothId(int clothId)
        {
            await Console.Out.WriteLineAsync($"!!!!!!!!!!!!!!!!!ID ОДЕЖДЫ: {clothId}");
            // Получить все записи MaterialInCloth для определенной одежды
            IEnumerable<MaterialInCloth> materialsInCloth = await materialInClothService.Get();
            foreach (MaterialInCloth material1 in materialsInCloth)
            {
                await Console.Out.WriteLineAsync($"!!!!!!!!!!!!!!!!!ИМЯ: {material1.MaterialId}");
            }

            // Фильтровать записи MaterialInCloth по clothId
            IEnumerable<MaterialInCloth> materialsForClothInMaterialInCloth = materialsInCloth.Where(m => m.ClothId == clothId);
            foreach (MaterialInCloth material1 in materialsForClothInMaterialInCloth)
            {
                await Console.Out.WriteLineAsync($"!!!!!!!!!!!!!!!!!ИМЯ: {material1.MaterialId}");
            }

            // Получить идентификаторы материалов для данной одежды
            IEnumerable<int> materialIds = materialsForClothInMaterialInCloth.Select(m => m.MaterialId);
            foreach (int id in materialIds)
            {
                await Console.Out.WriteLineAsync($"!!!!!!!!!!!!!!!!!ИМЯ: {id}");
            }

            // Получить материалы по полученным идентификаторам
            IEnumerable<Material> allMaterials = await materialService.Get();
            foreach (Material material in allMaterials)
            {
                await Console.Out.WriteLineAsync($"!!!!!!!!!!!!!!!!!ВСЕ ИМЕНА ИМЯ: {material.Name}");
            }

            // Фильтровать материалы по найденным идентификаторам
            IEnumerable<Material> materialsForCloth = allMaterials.Where(m => materialIds.Contains(m.Id));
            await Console.Out.WriteLineAsync($"!!!!!!!!!!!!!!!!!НАЙДЕНО МАТЕРИАЛОВ:");
            foreach (var material in materialsForCloth)
            {
                await Console.Out.WriteLineAsync($"!!!!!!!!!!!!!!!!!ИМЯ: {material.Name}");
            }    

            return materialsForCloth;
        }

    }




    public class ViewModelLocator
    {
        private static ApplicationVM applicationVM;

        public static ApplicationVM ApplicationVM
        {
            get
            {
                if (applicationVM == null)
                {
                    applicationVM = new ApplicationVM();
                    // инициализируем Navigation
                    applicationVM.Navigation = Application.Current.MainPage.Navigation;
                }
                return applicationVM;
            }
        }
    }
}
