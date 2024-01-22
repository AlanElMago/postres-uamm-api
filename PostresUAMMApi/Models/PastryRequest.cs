using Google.Cloud.Firestore;
using PostresUAMMApi.Enums;

namespace PostresUAMMApi.Models;

[FirestoreData]
public class PastryRequest
{
    [FirestoreDocumentId]
    public string? Id { get; set; }

    [FirestoreProperty(Name = "customerId")]
    public string? CustomerId { get; set; }

    [FirestoreProperty(Name = "campusLocationId")]
    public string? CampusLocationId { get; set; }

    [FirestoreProperty(Name = "state")]
    public PastryRequestStates? State { get; set; }

    [FirestoreProperty(Name = "statusMessage")]
    public string? StatusMessage { get; set; }

    [FirestoreDocumentCreateTimestamp]
    public DateTime CreateTime { get; set; }

    [FirestoreDocumentUpdateTimestamp]
    public DateTime UpdateTime { get; set; }

    // composite properties
    public Customer? Customer { get; set; }

    public CampusLocation? CampusLocation { get; set; }
}
