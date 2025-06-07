using DonorApplication.ViewModel;

namespace DonorApplication
{
	public partial class App : Application
	{
		AuthorizationViewModel viewModel;
		public App(AuthorizationViewModel authorizationViewModel)
		{
			InitializeComponent();
			viewModel = authorizationViewModel;
		}

		protected override Window CreateWindow(IActivationState? activationState)
		{
			return new Window(new AppShell());
		}
	}
}