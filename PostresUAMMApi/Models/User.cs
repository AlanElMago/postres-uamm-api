using Google.Cloud.Firestore;
using PostresUAMMApi.Enums;

namespace PostresUAMMApi.Models;

[FirestoreData]
public class User
{
    [FirestoreDocumentId]
    public string? Id { get; set; }

    [FirestoreProperty(Name = "firebaseAuthUid")]
    public string? FirebaseAuthUid { get; set; }

    [FirestoreProperty(Name = "roles")]
    public List<RolesEnum> Roles { get; set; } = [RolesEnum.None];

    [FirestoreProperty(Name = "isEnabled")]
    public bool IsEnabled { get; set; } = true;
}
