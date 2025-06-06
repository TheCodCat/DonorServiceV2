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

		[ObservableProperty]
		private string login;

		[ObservableProperty]
		private string password;

		[ObservableProperty]
		private bool isRequest;

		[RelayCommand]
		public async void Authorization()
		{
			try
			{
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
					userData.Donor = JsonConvert.DeserializeObject<Donor>(body) ?? new Donor();
				}
			}
			catch (HttpRequestException)
			{

			}
			finally
			{
				IsRequest = false;
			}
		}
	}
}
