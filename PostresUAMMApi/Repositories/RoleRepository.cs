using Google.Cloud.Firestore;
using PostresUAMMApi.Models;

namespace PostresUAMMApi.Repositories;

public class RoleRepository(FirestoreDb firestoreDb)
{
    private FirestoreDb _firestoreDb = firestoreDb;

    public async Task<List<Role>> GetRolesAsync()
    {
        Query allRolesQuery = _firestoreDb.Collection("roles");
        QuerySnapshot allRolesQuerySnapshot = await allRolesQuery.GetSnapshotAsync();
        
        return allRolesQuerySnapshot.Select(roleDocSnapshot => roleDocSnapshot.ConvertTo<Role>()).ToList();;
    }

    public async Task<Role> GetRoleAsync(string roleId)
    {
        DocumentReference roleDocRef = _firestoreDb.Collection("roles").Document(roleId);
        DocumentSnapshot roleDocSnapshot = await roleDocRef.GetSnapshotAsync();

        if (!roleDocSnapshot.Exists)
        {
            throw new Exception($"Role with id {roleId} does not exist in the database");
        }

        return roleDocSnapshot.ConvertTo<Role>();
    }
}
