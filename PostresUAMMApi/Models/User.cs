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

    [FirestoreProperty(Name = "fullName")]
    public string? FullName { get; set; }

    [FirestoreProperty(Name = "roles")]
    public List<UserRoles> Roles { get; set; } = [UserRoles.None];

    [FirestoreProperty(Name = "isEnabled")]
    public bool IsEnabled { get; set; } = true;

    [FirestoreDocumentCreateTimestamp]
    public DateTime CreateTime { get; set;}

    [FirestoreDocumentUpdateTimestamp]
    public DateTime UpdateTime { get; set;}
}
