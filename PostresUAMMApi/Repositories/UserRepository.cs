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

        return users;
    }

    public async Task<User> GetUserAsync(string id)
    {
        DocumentSnapshot userDocSnapshot = await _firestoreDb.Collection("users").Document(id).GetSnapshotAsync();
        User user = userDocSnapshot.ConvertTo<User>();

        return user;
    }

    public async Task<User> GetUserByFirebaseUidAsync(string firebaseAuthUid)
    {
        Query userQuery = _firestoreDb.Collection("users").WhereEqualTo("firebaseAuthUid", firebaseAuthUid);
        QuerySnapshot userQuerySnapshot = await userQuery.GetSnapshotAsync();

        if (userQuerySnapshot.Count <= 0)
        {
            throw new InvalidOperationException($"User with firebaseAuthUid {firebaseAuthUid} does not exist in the database");
        }

        User user = userQuerySnapshot.Documents[0].ConvertTo<User>();

        return user;
    }

    public async Task<User> AddUserAsync(User user)
    {
        CollectionReference usersCollectionReference = _firestoreDb.Collection("users");
        DocumentReference documentReference = await usersCollectionReference.AddAsync(user);
        DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();
        User newUser = documentSnapshot.ConvertTo<User>();

        return newUser;
    }

    public async Task<User> UpdateUserAsync(string id, User user)
    {
        DocumentReference documentReference = _firestoreDb.Collection("users").Document(id);

        await documentReference.SetAsync(user, SetOptions.Overwrite);

        DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();
        User updatedUser = documentSnapshot.ConvertTo<User>();

        return updatedUser;
    }
}
