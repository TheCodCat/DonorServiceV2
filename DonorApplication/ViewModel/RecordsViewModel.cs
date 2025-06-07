using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DonorApplication.Singlton;
using Models.Models;
using Newtonsoft.Json;

namespace DonorApplication.ViewModel
{
	public partial class RecordsViewModel : ObservableObject
	{
		private HttpClient _httpClient;
		private UserData _userData;
		private Page page;

		[ObservableProperty]
		private bool isNotAuth;

		[ObservableProperty]
		private bool isAuthAcount;

		[ObservableProperty]
		private List<DiliveryPoint> diliveryPoints;

		[ObservableProperty]
		private bool isRequest;

		public RecordsViewModel(Page page, UserData userData)
		{
			this.page = page;
			_httpClient = new HttpClient();
			_userData = userData;

			IsNotAuth = userData.Donor == null;
			IsAuthAcount = userData.Donor != null;

			LoadDiliveryPoints(DiliveryPoints);
		}

		[RelayCommand]
		private async void MoveToRecomendation()
		{
			await page.Navigation.PushAsync(new RecomendationPage());
		}

		[RelayCommand]
		private async void LoadDiliveryPoints(List<DiliveryPoint> diliveryPoints)
		{
			try
			{
				IsRequest = true;

				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Get,
					RequestUri = new Uri("http://localhost:5292/GetAllDilivery"),
				};
				using (var response = await _httpClient.SendAsync(request))
				{
					response.EnsureSuccessStatusCode();
					var body = await response.Content.ReadAsStringAsync();
					DiliveryPoints = JsonConvert.DeserializeObject<List<DiliveryPoint>>(body) ?? null;
				}

			}
			catch (HttpRequestException ex)
			{

			}
			finally
			{
				IsRequest = false;
			}

		}

		[RelayCommand]
		private async void SignUpForChange(int indexDilivery)
		{
			try
			{
				IsRequest = true;

				await Task.Delay(1000);

				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri("http://localhost:5292/Addrecods"),
				};

				request.Headers.Add("donorid", _userData.Donor.Id.ToString());
				request.Headers.Add("diliveryPointId", indexDilivery.ToString());

				using (var response = await _httpClient.SendAsync(request))
				{
					response.EnsureSuccessStatusCode();
					var body = await response.Content.ReadAsStringAsync();
				}
			}
			catch (HttpRequestException ex)
			{

			}
			finally
			{
				IsRequest = false;
			}
		}
	}
}
