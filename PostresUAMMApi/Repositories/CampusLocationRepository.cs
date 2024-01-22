using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using PostresUAMMApi.Models;

namespace PostresUAMMApi.Repositories;

public interface ICampusLocationRepository
{
    Task<List<CampusLocation>> GetCampusLocationsAsync();

    Task<CampusLocation?> GetCampusLocationAsync(string id);
}

public class CampusLocationRepository(FirestoreDb firestoreDb) : ICampusLocationRepository
{
    private readonly FirestoreDb _firestoreDb = firestoreDb;

    public async Task<List<CampusLocation>> GetCampusLocationsAsync()
    {
        Query campusLocationsCollQuery = _firestoreDb.Collection("campusLocations");
        QuerySnapshot allCampusLocationsQuerySnapshot = await campusLocationsCollQuery.GetSnapshotAsync();
        List<CampusLocation> campusLocations = allCampusLocationsQuerySnapshot
            .Select(campusLocationDocSnapshot => campusLocationDocSnapshot.ConvertTo<CampusLocation>())
            .ToList();

        return campusLocations;
    }

    public async Task<CampusLocation?> GetCampusLocationAsync(string id)
    {
        DocumentSnapshot campusLocationDocSnapshot = await _firestoreDb
            .Collection("campusLocations")
            .Document(id)
            .GetSnapshotAsync();

        if (!campusLocationDocSnapshot.Exists)
        {
            throw new InvalidOperationException($"CampusLocation with id {id} does not exist in the database");
        }

        CampusLocation campusLocation = campusLocationDocSnapshot.ConvertTo<CampusLocation>();

        return campusLocation;
    }
}
