using Google.Cloud.Firestore;

namespace PostresUAMMApi.Enums;

[FirestoreData(ConverterType = typeof(FirestoreEnumNameConverter<RolesEnum>))]
public enum RolesEnum
{
    None,
    Admin,
    Baker,
    Customer,
    Seller
}
