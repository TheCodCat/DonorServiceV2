using DonorApplication.ViewModel;

namespace DonorApplication;

public partial class AuthorizationPage : ContentPage
{
	public AuthorizationPage(AuthorizationViewModel authorizationViewModel)
	{
		InitializeComponent();
		authorizationViewModel.page = this;
		BindingContext = authorizationViewModel;
	}

	public void ChangeText(Color color)
	{
		CallbackText.TextColor = color;
	}
}