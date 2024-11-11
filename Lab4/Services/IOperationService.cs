using Lab4.ViewModels;

namespace Lab4.Services
{
    public interface IOperationService
    {
        HomeViewModel GetHomeViewModel(int numberRows = 10);
    }
}
