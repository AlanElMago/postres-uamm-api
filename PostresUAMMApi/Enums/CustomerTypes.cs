using Google.Cloud.Firestore;

namespace PostresUAMMApi.Enums;

[FirestoreData(ConverterType = typeof(FirestoreEnumNameConverter<CustomerTypes>))]
public enum CustomerTypes
{
    None = 0,
    Student = 1,
    Teacher = 2
}
