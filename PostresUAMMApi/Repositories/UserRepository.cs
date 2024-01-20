using Google.Cloud.Firestore;
using PostresUAMMApi.Models;

namespace PostresUAMMApi.Repositories;

public class UserRepository(FirestoreDb firestoreDb)
{
    private readonly FirestoreDb _firestoreDb = firestoreDb;

    public async Task<List<User>> GetUsersAsync()
    {
        Query allUsersQuery = _firestoreDb.Collection("users");
        QuerySnapshot allUsersQuerySnapshot = await allUsersQuery.GetSnapshotAsync();
        List<User> users = allUsersQuerySnapshot.Select(userDocSnapshot => userDocSnapshot.ConvertTo<User>()).ToList();

        Query allRolesQuery = _firestoreDb.Collection("roles");
        QuerySnapshot allRolesQuerySnapshot = await allRolesQuery.GetSnapshotAsync();        
        List<Role> roles = allRolesQuerySnapshot.Select(roleDocSnapshot => roleDocSnapshot.ConvertTo<Role>()).ToList();

        foreach (User user in users)
        {
            user.Roles.AddRange(roles.Where(role => user.RoleIds.Any(userRoleId => userRoleId.Id == role.Id)));
        }

        return users;
    }
}
