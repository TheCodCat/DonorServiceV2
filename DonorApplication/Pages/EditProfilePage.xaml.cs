using DonorApplication.ViewModel;

namespace DonorApplication;

public partial class EditProfilePage : ContentPage
{
	public EditProfilePage(EditProfileViewModel editProfileViewModel)
	{
		InitializeComponent();
		BindingContext = editProfileViewModel;
	}
}