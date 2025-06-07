using DonorApplication.ViewModel;

namespace DonorApplication;

public partial class RecomendationPage : ContentPage
{
	public RecomendationPage()
	{
		InitializeComponent();
		BindingContext = new RecomendationViewModel();
	}
}