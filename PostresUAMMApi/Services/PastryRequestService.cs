using PostresUAMMApi.Enums;
using PostresUAMMApi.Models;
using PostresUAMMApi.Models.Forms;
using PostresUAMMApi.Repositories;

namespace PostresUAMMApi.Services;

public interface IPastryRequestService
{
    public Task<PastryRequest> MakePastryRequest(PastryRequestForm form);
}

public class PastryRequestService(IPastryRequestRepository pastryRequestRepository) : IPastryRequestService
{
    private readonly IPastryRequestRepository _pastryRequestRepository = pastryRequestRepository;

    public Task<PastryRequest> MakePastryRequest(PastryRequestForm form)
    {
        ArgumentNullException.ThrowIfNull(form);
        ArgumentException.ThrowIfNullOrWhiteSpace(form.CustomerId);
        ArgumentException.ThrowIfNullOrWhiteSpace(form.CampusLocationId);

        PastryRequest pastryRequest = new()
        {
            CustomerId = form.CustomerId,
            CampusLocationId = form.CampusLocationId,
            State = PastryRequestStates.Pending
        };

        return _pastryRequestRepository.AddPastryRequestAsync(pastryRequest);
    }
}
