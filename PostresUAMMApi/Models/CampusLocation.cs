using Google.Cloud.Firestore;

namespace PostresUAMMApi.Models;

[FirestoreData]
public class CampusLocation
{
    [FirestoreDocumentId]
    public string? Id { get; set; }

    [FirestoreProperty(Name = "name")]
    public string? Name { get; set; }

    [FirestoreProperty(Name = "isIndoors")]
    public bool IsIndoors { get; set; }
}
