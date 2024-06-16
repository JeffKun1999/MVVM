namespace NotesJR.Views;

public partial class AllNotesPageJR : ContentPage
{
    public AllNotesPageJR()
    {
        InitializeComponent();

    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        notesCollection.SelectedItem = null;

    }
}