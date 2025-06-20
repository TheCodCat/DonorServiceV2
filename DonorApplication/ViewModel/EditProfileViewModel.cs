﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DonorApplication.Singlton;
using Force.DeepCloner;
using Models.Enums;
using Models.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;

namespace DonorApplication.ViewModel
{
    public partial class EditProfileViewModel(UserData userData) : ObservableObject
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
				BloodTypeEnum.TreePlus, BloodTypeEnum.TreeMinus,
				BloodTypeEnum.FourPlus, BloodTypeEnum.FourMinus
            };

		[ObservableProperty]
		private List<Record> historyPoints = new List<Record>();

        [ObservableProperty]
        private Donor? donor;

        partial void OnDonorChanged(Donor value)
        {
            ChangeViewData(value);
            //GetHistory(value);
		}

        [ObservableProperty]
        private bool isNotAuth;

        [ObservableProperty]
        private bool isAuthAcount;

        [ObservableProperty]
        private bool isEditProfile;

		public void Init()
        {
            Donor = userData.Donor ?? null;

            IsNotAuth = Donor == null;
            IsAuthAcount = !isNotAuth;
            IsEditProfile = Donor == null ? false : Donor.IsEdit;
		}

		public async void GetHistoryOpen() => GetHistory(Donor);

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
            if (IsEditProfile)
            {
                SelectTypeBlood = Donor.BloodTypeEnum;
				ChangeViewData(Donor);
				return;
            }

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
				Donor = JsonConvert.DeserializeObject<Donor>(body);
			}
		}

        private async void GetHistory(Donor donor)
        {
			if(donor == null) return;

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri("http://localhost:5292/GetDonorRecords"),
				Headers =
				{
					{ "DonorId", donor.Id.ToString() },
				},
			};

			using (var response = await _httpClient.SendAsync(request))
			{
				response.EnsureSuccessStatusCode();
				var body = await response.Content.ReadAsStringAsync();
				HistoryPoints = JsonConvert.DeserializeObject<List<Record>>(body) ?? new List<Record>();
			}
		}

        [RelayCommand]
        private async void ChangeIconProfile()
        {
            var result = await FilePicker.PickAsync(PickOptions.Images);
            if (result == null) return;

            var stream = await result.OpenReadAsync();
            MemoryStream memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);

            string base64 = Convert.ToBase64String(memoryStream.ToArray());

            if (Donor is not null)
            {
                var clone = userData.Donor.DeepClone();

                clone.Base64Image = base64;

                userData.Donor = clone;

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("http://localhost:5292/EditingProfile/changeProfileIcon"),
                    Headers =
                    {
                        { "DonorId", $"{userData.Donor.Id}" },
                    },
                    Content = new StringContent($"\"{userData.Donor.Base64Image}\"")
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                    }
                };
                using (var response = await _httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Donor = JsonConvert.DeserializeObject<Donor>(body);
                }
            }

        }
    }
}
