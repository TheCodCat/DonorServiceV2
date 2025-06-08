using DonorApplication.ViewModel;

namespace DonorApplication;

public partial class EditProfilePage : ContentPage
{
	private EditProfileViewModel editProfileViewModel;
	public EditProfilePage(EditProfileViewModel editProfileViewModel)
	{
		InitializeComponent();
		this.editProfileViewModel = editProfileViewModel;
		BindingContext = this.editProfileViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		editProfileViewModel.Init();
    }
}