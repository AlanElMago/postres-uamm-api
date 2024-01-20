using Google.Cloud.Firestore;
using PostresUAMMApi.Models;

namespace PostresUAMMApi.Repositories;

public class UserRepository(FirestoreDb firestoreDb, RoleRepository roleRepository)
{
    private readonly FirestoreDb _firestoreDb = firestoreDb;
    private readonly RoleRepository _roleRepository = roleRepository;

    public async Task<List<User>> GetUsersAsync()
    {
        Query allUsersQuery = _firestoreDb.Collection("users");
        QuerySnapshot allUsersQuerySnapshot = await allUsersQuery.GetSnapshotAsync();

        List<User> users = allUsersQuerySnapshot.Select(userDocSnapshot => userDocSnapshot.ConvertTo<User>()).ToList();
        List<Role> roles = await _roleRepository.GetRolesAsync();

        foreach (User user in users)
        {
            foreach (DocumentReference userRoleId in user.RoleIds)
            {
                foreach (Role role in roles)
                {
                    if (userRoleId.Id == role.Id)
                    {
                        user.Roles.Add(role);
                    }
                }
            }
        }

        return users;
    }
}
