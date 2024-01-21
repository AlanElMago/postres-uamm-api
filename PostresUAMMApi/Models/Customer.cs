using Google.Cloud.Firestore;
using PostresUAMMApi.Enums;

namespace PostresUAMMApi.Models;

[FirestoreData]
public class Customer
{
    [FirestoreDocumentId]
    public string? Id { get; set; }

    [FirestoreProperty(Name = "userId")]
    public string? UserId { get; set; }

    [FirestoreProperty(Name = "customerType")]
    public CustomerTypesEnum CustomerType { get; set; } = CustomerTypesEnum.None;

    [FirestoreProperty(Name = "isVerifiedByAdmin")]
    public bool IsVerifiedByAdmin { get; set; } = false;

    // composite property
    public User? User { get; set; }
}
