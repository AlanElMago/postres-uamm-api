using Google.Cloud.Firestore;

namespace PostresUAMMApi.Enums;

[FirestoreData(ConverterType = typeof(FirestoreEnumNameConverter<PastryRequestStates>))]
public enum PastryRequestStates
{
    None = 0,
    Pending = 1,
    Accepted = 2,
    Rejected = 3,
    Cancelled = 4,
    Completed = 5
}