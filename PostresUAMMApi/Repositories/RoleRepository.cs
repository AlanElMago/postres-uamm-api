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
        List<Role> roles = new(4);

        foreach (DocumentSnapshot roleDocSnapshot in allRolesQuerySnapshot)
        {
            string roleId = roleDocSnapshot.Id;
            string roleName = roleDocSnapshot.GetValue<string>("name");

            roles.Add(new Role(roleId, roleName));
        }

        return roles;
    }

    public async Task<Role> GetRoleAsync(string roleId)
    {
        DocumentReference roleDocRef = _firestoreDb.Collection("roles").Document(roleId);
        DocumentSnapshot roleDocSnapshot = await roleDocRef.GetSnapshotAsync();

        if (!roleDocSnapshot.Exists)
        {
            throw new Exception($"Role with id {roleId} does not exist in the database");
        }

        string roleName = roleDocSnapshot.GetValue<string>("name");

        return new Role(roleId, roleName);
    }
}
