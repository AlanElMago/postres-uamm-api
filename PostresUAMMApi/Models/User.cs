using Google.Cloud.Firestore;

namespace PostresUAMMApi.Models;

[FirestoreData]
public class User
{
    [FirestoreDocumentId]
    public string? Id { get; set; }

    [FirestoreProperty(Name = "roleIds")]
    public List<DocumentReference> RoleIds { get; set; } = [];

    public List<Role> Roles { get; } = [];
}
