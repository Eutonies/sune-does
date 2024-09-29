namespace SuneDoes.UI.Session;

public record SessionState(Action OnUpdate)
{
    private SessionSelectedPage? _selectedPage;
    public SessionSelectedPage? SelectedPage { 
        get => _selectedPage; 
        set {
            if (_selectedPage != value)
            {
                _selectedPage = value;
                CurrentSelectedPageChanged?.Invoke(this,_selectedPage);

            }
        } }


    public event EventHandler<SessionSelectedPage?> CurrentSelectedPageChanged;


}
