using DonorApplication.Singlton;
using DonorApplication.ViewModel;

namespace DonorApplication;

public partial class RecordPage : ContentPage
{
	public RecordPage(UserData userData)
	{
		InitializeComponent();
		BindingContext = new RecordsViewModel(this, userData);
	}
}