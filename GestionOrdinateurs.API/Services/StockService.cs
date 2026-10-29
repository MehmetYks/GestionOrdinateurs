namespace GestionOrdinateurs.API.Services;

public class StockService
{
    public int StockSSD {get; set;} = 10;
    private readonly object _lock = new object();
    public async void ConsommerSSD()
    {
        Task.Delay(10).Wait();

        lock (_lock)
        {
            StockSSD--;
        }
    }
}
