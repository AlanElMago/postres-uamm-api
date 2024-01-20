using Google.Cloud.Firestore;

namespace PostresUAMMApi.Models;

[FirestoreData]
public class Role()
{
    [FirestoreDocumentId]
    public string? Id { get; set; }

    [FirestoreProperty]
    public string? Name { get; set; }
}
