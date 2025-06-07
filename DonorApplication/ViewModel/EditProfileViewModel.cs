using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DonorApplication.Singlton;
using Models.Enums;
using Models.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;

namespace DonorApplication.ViewModel
{
    public partial class EditProfileViewModel : ObservableObject
    {
        private HttpClient _httpClient = new HttpClient();

        [ObservableProperty]
        private string fullName;

		[ObservableProperty]
		private string fullNameIcon;

        [ObservableProperty]
        private BloodTypeEnum selectTypeBlood;

        [ObservableProperty]
        private ObservableCollection<BloodTypeEnum> bioTypes =
            new ObservableCollection<BloodTypeEnum>() 
            { 
                BloodTypeEnum.OnePlus, BloodTypeEnum.OneMinus,
                BloodTypeEnum.TwoPlus, BloodTypeEnum.TwoMinus,
            };

        [ObservableProperty]
        private Donor donor;

        partial void OnDonorChanged(Donor value)
        {
            ChangeViewData(value);
        }

        [ObservableProperty]
        private bool isNotAuth;

        [ObservableProperty]
        private bool isAuthAcount;

		public EditProfileViewModel(UserData userData)
        {

            Donor = userData.Donor;

            IsNotAuth = Donor == null;
            IsAuthAcount = !isNotAuth;
            //ChangeViewData();
		}

        private void ChangeViewData(Donor donor)
        {
            if (donor == null) return;

			var items = donor?.FullName.Split(' ');
			StringBuilder fullname = new StringBuilder();

			foreach (var item in items)
			{
				fullname.Append(item.Substring(0, 1));
			}
			FullNameIcon = fullname.ToString();

            FullName = Donor.FullName;

            SelectTypeBlood = Donor.BloodTypeEnum;
		}

        [RelayCommand]
        private async void EditProfile()
        {
			Tuple<string, BloodTypeEnum> tuple = new Tuple<string, BloodTypeEnum>(FullName, SelectTypeBlood);
            string json = JsonConvert.SerializeObject(tuple);

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("http://localhost:5292/editProfile"),
				Content = new StringContent(json)
				{
					Headers =
		            {
			            ContentType = new MediaTypeHeaderValue("application/json")
		            }
				}
			};
            request.Headers.Add("donorId", Donor.Id.ToString());
			using (var response = await _httpClient.SendAsync(request))
			{
				response.EnsureSuccessStatusCode();
				var body = await response.Content.ReadAsStringAsync();
				Console.WriteLine(body);
			}
		}
    }
}
