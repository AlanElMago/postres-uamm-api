using PostresUAMMApi.Enums;

namespace PostresUAMMApi.Models.Forms;

public class PastryRequestForm
{
    public string? CustomerId { get; set; }

    public string? CampusLocationId { get; set; }

    public PastryRequestStates State { get; set; } = Enums.PastryRequestStates.Pending;

    public string StatusMessage { get; set; } = "";
}
