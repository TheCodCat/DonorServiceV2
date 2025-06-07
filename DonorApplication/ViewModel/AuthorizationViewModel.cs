using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DonorApplication.Singlton;
using Models.DTO;
using Models.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DonorApplication.ViewModel
{
	public partial class AuthorizationViewModel(UserData userData) : ObservableObject
	{
		HttpClient _httpClient = new HttpClient();
		public AuthorizationPage page { get; set; }
		[ObservableProperty]
		private string login;

		[ObservableProperty]
		private string password;

		[ObservableProperty]
		private bool isRequest;

		[ObservableProperty]
		private string callbackText = string.Empty;


		[RelayCommand]
		public async void Authorization()
		{
			try
			{
				CallbackText = string.Empty;

				AuthDTO authDTO = new AuthDTO(Login, Password);
				string json = JsonConvert.SerializeObject(authDTO, Formatting.Indented);

				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri("http://localhost:5292/authorization"),
					Content = new StringContent(json)
					{
						Headers =
						{
							ContentType = new MediaTypeHeaderValue("application/json")
						}
					}
				};
				IsRequest = true;

				await Task.Delay(1000);

				using (var response = await _httpClient.SendAsync(request))
				{
					response.EnsureSuccessStatusCode();

					var body = await response.Content.ReadAsStringAsync();
					userData.Donor = JsonConvert.DeserializeObject<Donor>(body) ?? null;

					if (userData.Donor != null)
						await page.Navigation.PushAsync(new EditProfilePage(new EditProfileViewModel(userData)));
				}
			}
			catch (HttpRequestException)
			{
				CallbackText = "Неправильный логин или пароль";
				page.ChangeText(Color.FromRgb(255, 0, 0));
			}
			finally
			{
				IsRequest = false;
			}
		}

		[RelayCommand]
		public async void Registration()
		{
			try
			{
				CallbackText = string.Empty;
				AuthDTO authDTO = new AuthDTO(Login, Password);
				string json = JsonConvert.SerializeObject(authDTO, Formatting.Indented);
				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri("http://localhost:5292/registration"),
					Content = new StringContent(json)
					{
						Headers =
						{
							ContentType = new MediaTypeHeaderValue("application/json")
						}
					}
				};

				IsRequest = true;
				
				await Task.Delay(1000);

				using (var response = await _httpClient.SendAsync(request))
				{
					response.EnsureSuccessStatusCode();
					var body = await response.Content.ReadAsStringAsync();
					userData.Donor = JsonConvert.DeserializeObject<Donor>(body) ?? null;

					if (userData.Donor != null)
					{
						CallbackText = "Аккаунт успешно зарегистрирован";
						page.ChangeText(Color.FromRgb(0,255,0));
					}

				}
			}
			catch (HttpRequestException)
			{
				CallbackText = "Логин уже занят, повторите попытку";
				page.ChangeText(Color.FromRgb(255, 0, 0));
			}
			finally
			{
				IsRequest = false;
			}
		}
	}
}
