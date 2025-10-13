using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;
using System.Threading.Tasks;

namespace PizzaPlace.Services;

public class StockService(IStockRepository stockRepository) : IStockService
{
    public Task<bool> HasInsufficientStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        throw new NotImplementedException("Sufficient stock must be checked.");
    }

    public async Task<ComparableList<StockDto>> GetStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        var stockTypes = Enum.GetValues(typeof(StockType)).Cast<StockType>();
        var stockList = new ComparableList<StockDto>();

        foreach (var type in stockTypes)
        {
            var stock = await stockRepository.GetStock(type);
            if (stock != null)
            {
                stockList.Add(stock);
            }
        }

        return stockList;
    }
}
