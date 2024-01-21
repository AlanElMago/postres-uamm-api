using Google.Cloud.Firestore;

namespace PostresUAMMApi.Enums;

[FirestoreData(ConverterType = typeof(FirestoreEnumNameConverter<CustomerTypesEnum>))]
public enum CustomerTypesEnum
{
    None = 0,
    Student = 1,
    Teacher = 2
}
