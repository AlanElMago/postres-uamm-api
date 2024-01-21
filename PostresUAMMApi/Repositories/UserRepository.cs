using Google.Cloud.Firestore;
using PostresUAMMApi.Models;

namespace PostresUAMMApi.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetUsersAsync();

    Task<User> GetUserAsync(string id);

    Task<User> GetUserByFirebaseUidAsync(string firebaseAuthUid);

    Task<User> AddUserAsync(User user);

    Task<User> UpdateUserAsync(string id, User user);
}

public class UserRepository(FirestoreDb firestoreDb) : IUserRepository
{
    private readonly FirestoreDb _firestoreDb = firestoreDb;

    public async Task<List<User>> GetUsersAsync()
    {
        Query usersCollQuery = _firestoreDb.Collection("users");
        QuerySnapshot allUsersQuerySnapshot = await usersCollQuery.GetSnapshotAsync();
        List<User> users = allUsersQuerySnapshot
            .Select(userDocSnapshot => userDocSnapshot.ConvertTo<User>())
            .ToList();

        return users;
    }

    public async Task<User> GetUserAsync(string id)
    {
        DocumentSnapshot userDocSnapshot = await _firestoreDb.Collection("users").Document(id).GetSnapshotAsync();

        if (!userDocSnapshot.Exists)
        {
            throw new InvalidOperationException($"User with id {id} does not exist in the database");
        }

        User user = userDocSnapshot.ConvertTo<User>();

        return user;
    }

    public async Task<User> GetUserByFirebaseUidAsync(string firebaseAuthUid)
    {
        Query userQuery = _firestoreDb.Collection("users").WhereEqualTo("firebaseAuthUid", firebaseAuthUid);
        QuerySnapshot userQuerySnapshot = await userQuery.GetSnapshotAsync();

        if (userQuerySnapshot.Count <= 0)
        {
            throw new InvalidOperationException(
                $"User with firebaseAuthUid {firebaseAuthUid} does not exist in the database");
        }

        User user = userQuerySnapshot.Documents[0].ConvertTo<User>();

        return user;
    }

    public async Task<User> AddUserAsync(User user)
    {
        CollectionReference usersCollRef = _firestoreDb.Collection("users");
        DocumentReference userDocRef = await usersCollRef.AddAsync(user);
        DocumentSnapshot userDocSnapshot = await userDocRef.GetSnapshotAsync();
        User newUser = userDocSnapshot.ConvertTo<User>();

        return newUser;
    }

    public async Task<User> UpdateUserAsync(string id, User user)
    {
        DocumentReference userDocRef = _firestoreDb.Collection("users").Document(id);

        await userDocRef.SetAsync(user, SetOptions.Overwrite);

        DocumentSnapshot userDocSnapshot = await userDocRef.GetSnapshotAsync();
        User updatedUser = userDocSnapshot.ConvertTo<User>();

        return updatedUser;
    }
}
