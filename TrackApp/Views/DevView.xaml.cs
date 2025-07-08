using TrackApp.ViewModels;

namespace TrackApp.Views;

public partial class DevView : ContentPage
{
    public DevView()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (MyMap != null)
        {
            if (MyMap.MapElements.Count == 0)
                MyMap.MapElements.Add(((DevViewModel)BindingContext).Track);
        }
    }
}