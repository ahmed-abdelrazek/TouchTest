using Caliburn.Micro;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using TouchTest.WPF.Helpers;
using TouchTest.WPF.Models;

namespace TouchTest.WPF.ViewModels
{
    public class MainViewModel : Screen
    {
        string _name;
        string _address;
        string _phone;
        Client _selectedClient;
        IAPIHelper _apiHelper;

        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                NotifyOfPropertyChange(() => Address);
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                NotifyOfPropertyChange(() => Phone);
            }
        }

        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                NotifyOfPropertyChange(() => SelectedClient);
                NotifyOfPropertyChange(() => CanEdit);
            }
        }

        BindableCollection<Client> _clients { get; set; }

        public BindableCollection<Client> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                NotifyOfPropertyChange(() => Clients);
            }
        }

        public MainViewModel()
        {
            Clients = new BindableCollection<Client>();
        }

        public MainViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task GetAllClients()
        {
            using (HttpResponseMessage response = await _apiHelper.APIClient.GetAsync("api/clients"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var Items = new List<Client>();
                    var content = await response.Content.ReadAsStringAsync();
                    Clients = JsonConvert.DeserializeObject<BindableCollection<Client>>(content);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        public bool CanAdd
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Name);
            }
        }

        public async Task Add()
        {
            Client json = new Client
            {
                Name = Name,
                Address = Address,
                Phone = Phone
            };

            using (HttpResponseMessage response = await _apiHelper.APIClient.PostAsJsonAsync("api/Clients", json))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    json = JsonConvert.DeserializeObject<Client>(content);
                    Clients.Add(json);
                    MessageBox.Show("تم الاضافة بنجاح", Name);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public bool CanEdit
        {
            get
            {
                return SelectedClient != null;
            }
        }

        public void Edit()
        {
            Id = SelectedClient.Id;
            Name = SelectedClient.Name;
            Address = SelectedClient.Address;
            Phone = SelectedClient.Phone;
        }

        public async Task Save()
        {
            Client json = new Client
            {
                Id = Id,
                Name = Name,
                Address = Address,
                Phone = Phone
            };

            using (HttpResponseMessage response = await _apiHelper.APIClient.PutAsJsonAsync($"api/Clients/{SelectedClient.Id}", json))
            {
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(SelectedClient.Name + " تم التعديل بنجاح من ", Name);
                    Clients[Clients.IndexOf(SelectedClient)] = json;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Delete()
        {
            using (HttpResponseMessage response = await _apiHelper.APIClient.DeleteAsync($"api/Clients/{SelectedClient.Id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("تم الحذف بنجاح", SelectedClient.Name);
                    Clients.Remove(SelectedClient);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task RefreshDataGrid()
        {
            await GetAllClients();
        }
    }
}