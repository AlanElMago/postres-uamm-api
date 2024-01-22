using Google.Cloud.Firestore;

namespace PostresUAMMApi.Enums;

[FirestoreData(ConverterType = typeof(FirestoreEnumNameConverter<UserRoles>))]
public enum UserRoles
{
    None = 0,
    Admin = 1,
    Baker = 2,
    Customer = 3,
    Seller = 4
}
