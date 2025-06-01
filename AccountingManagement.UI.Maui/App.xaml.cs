namespace AccountingManagement.UI.Maui;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Option 1: Create the Window first, then assign the Page.
        // This is the most common and straightforward way.
        var window = new Window(new MainPage()); 

        // If you need to access the 'activationState' or do more complex
        // window initialization that relies on the base method, you might do this:
        // var window = base.CreateWindow(activationState); // This call might expect MainPage to be set if not handled correctly
        // window.Page = new MainPage();

        // For simplicity and directness, new Window(new MainPage()) is often preferred
        // if you don't have specific needs for the base.CreateWindow() return value first.

        return window; 
    }
}