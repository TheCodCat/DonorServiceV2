using DonorApplication.ViewModel;

namespace DonorApplication;

public partial class AuthorizationPage : ContentPage
{
	public AuthorizationPage(AuthorizationViewModel authorizationViewModel)
	{
		InitializeComponent();
		BindingContext = authorizationViewModel;
	}
}