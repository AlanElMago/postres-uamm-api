using Google.Cloud.Firestore;
using PostresUAMMApi.Models;

namespace PostresUAMMApi.Repositories;

public interface IPastryRequestRepository
{
    public Task<List<PastryRequest>> GetPastryRequestsAsync(string id);

    public Task<PastryRequest> GetPastryRequestAsync(string id);

    public Task<PastryRequest> AddPastryRequestAsync(PastryRequest pastryRequest);

    public Task<PastryRequest> UpdatePastryRequestAsync(PastryRequest pastryRequest);
}

public class PastryRequestRepository(FirestoreDb firestoreDb) : IPastryRequestRepository
{
    private readonly FirestoreDb _firestoreDb = firestoreDb;

    public async Task<List<PastryRequest>> GetPastryRequestsAsync(string id)
    {
        Query pastryRequestsCollQuery = _firestoreDb.Collection("pastryRequests").WhereEqualTo("customerId", id);
        QuerySnapshot allPastryRequestsQuerySnapshot = await pastryRequestsCollQuery.GetSnapshotAsync();
        List<PastryRequest> pastryRequests = allPastryRequestsQuerySnapshot
            .Select(pastryRequestDocSnapshot => pastryRequestDocSnapshot.ConvertTo<PastryRequest>())
            .ToList();

        return pastryRequests;
    }

    public async Task<PastryRequest> GetPastryRequestAsync(string id)
    {
        DocumentSnapshot pastryRequestDocSnapshot = await _firestoreDb
            .Collection("pastryRequests")
            .Document(id)
            .GetSnapshotAsync();

        if (!pastryRequestDocSnapshot.Exists)
        {
            throw new InvalidOperationException($"PastryRequest with id {id} does not exist in the database");
        }

        PastryRequest? pastryRequest = pastryRequestDocSnapshot.ConvertTo<PastryRequest>();

        return pastryRequest;
    }

    public async Task<PastryRequest> AddPastryRequestAsync(PastryRequest pastryRequest)
    {
        DocumentReference pastryRequestDocRef = await _firestoreDb.Collection("pastryRequests").AddAsync(pastryRequest);
        DocumentSnapshot pastryRequestDocSnapshot = await pastryRequestDocRef.GetSnapshotAsync();
        PastryRequest? addedPastryRequest = pastryRequestDocSnapshot.ConvertTo<PastryRequest>();

        return addedPastryRequest;
    }

    public async Task<PastryRequest> UpdatePastryRequestAsync(PastryRequest pastryRequest)
    {
        DocumentReference pastryRequestDocRef = _firestoreDb.Collection("pastryRequests").Document(pastryRequest.Id);

        await pastryRequestDocRef.SetAsync(pastryRequest);

        DocumentSnapshot pastryRequestDocSnapshot = await pastryRequestDocRef.GetSnapshotAsync();
        PastryRequest? updatedPastryRequest = pastryRequestDocSnapshot.ConvertTo<PastryRequest>();

        return updatedPastryRequest;
    }
}
