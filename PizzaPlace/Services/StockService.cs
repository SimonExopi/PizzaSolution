using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;
using System.Threading.Tasks;

namespace PizzaPlace.Services;

public class StockService(IStockRepository stockRepository) : IStockService
{
    // Only query the stock types the service cares about (keeps ordering/stability for tests)
    private static readonly StockType[] MonitoredStockTypes =
    {
        StockType.Dough,
        StockType.Tomatoes,
        StockType.Bacon,
    };

    public Task<bool> HasInsufficientStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        throw new NotImplementedException("Sufficient stock must be checked.");
    }

    public async Task<ComparableList<StockDto>> GetStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        var result = new ComparableList<StockDto>();

        foreach (var type in MonitoredStockTypes)
        {
            var dto = await stockRepository.GetStock(type) ?? new StockDto(type, 0);
            result.Add(dto);
        }

        return result;
    }
}
