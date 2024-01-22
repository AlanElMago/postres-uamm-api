using Google.Cloud.Firestore;

namespace PostresUAMMApi.Enums;

[FirestoreData(ConverterType = typeof(FirestoreEnumNameConverter<UserRoles>))]
public enum UserRoles
{
    None,
    Admin,
    Baker,
    Customer,
    Seller
}
