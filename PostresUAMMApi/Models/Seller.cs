using Google.Cloud.Firestore;

namespace PostresUAMMApi.Models;

[FirestoreData]
public class Seller
{
    [FirestoreDocumentId]
    public string? Id { get; set; }

    [FirestoreProperty(Name = "userId")]
    public string? UserId { get; set; }

    [FirestoreProperty(Name = "isAvailable")]
    public bool IsAvailable { get; set; }

    [FirestoreProperty(Name = "statusMessage")]
    public string? StatusMessage { get; set; }

    [FirestoreDocumentCreateTimestamp]
    public DateTime CreateTime { get; set; }

    [FirestoreDocumentUpdateTimestamp]
    public DateTime UpdateTime { get; set; }

    // composite property
    public User? User { get; set; }
}