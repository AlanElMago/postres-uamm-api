using Google.Cloud.Firestore;
using PostresUAMMApi.Models;

namespace PostresUAMMApi.Repositories;

public interface ISellerRepository
{
    Task<List<Seller>> GetSellersAsync();

    Task<Seller> GetSellerAsync(string id);

    Task<Seller> GetSellerByUserIdAsync(string userId);

    Task<Seller> AddSellerAsync(Seller seller);

    Task<Seller> UpdateSellerAsync(string id, Seller seller);
}

public class SellerRepository(FirestoreDb firestoreDb) : ISellerRepository
{
    private readonly FirestoreDb _firestoreDb = firestoreDb;

    public async Task<List<Seller>> GetSellersAsync()
    {
        Query sellersCollQuery = _firestoreDb.Collection("sellers");
        QuerySnapshot allSellersQuerySnapshot = await sellersCollQuery.GetSnapshotAsync();
        List<Seller> sellers = allSellersQuerySnapshot
            .Select(sellerDocSnapshot => sellerDocSnapshot.ConvertTo<Seller>())
            .ToList();

        return sellers;
    }

    public async Task<Seller> GetSellerAsync(string id)
    {
        DocumentSnapshot sellerDocSnapshot = await _firestoreDb.Collection("sellers").Document(id).GetSnapshotAsync();

        if (!sellerDocSnapshot.Exists)
        {
            throw new InvalidOperationException($"Seller with id {id} does not exist in the database");
        }

        Seller seller = sellerDocSnapshot.ConvertTo<Seller>();

        return seller;
    }

    public async Task<Seller> GetSellerByUserIdAsync(string userId)
    {
        Query sellerQuery = _firestoreDb.Collection("sellers").WhereEqualTo("userId", userId);
        QuerySnapshot sellerQuerySnapshot = await sellerQuery.GetSnapshotAsync();

        if (sellerQuerySnapshot.Count <= 0)
        {
            throw new InvalidOperationException($"Seller with userId {userId} does not exist in the database");
        }

        Seller seller = sellerQuerySnapshot.Documents[0].ConvertTo<Seller>();

        return seller;
    }

    public async Task<Seller> AddSellerAsync(Seller seller)
    {
        CollectionReference sellersCollRef = _firestoreDb.Collection("sellers");
        DocumentReference sellerDocRef = await sellersCollRef.AddAsync(seller);
        DocumentSnapshot sellerDocSnapshot = await sellerDocRef.GetSnapshotAsync();
        Seller newSeller = sellerDocSnapshot.ConvertTo<Seller>();

        return newSeller;
    }

    public async Task<Seller> UpdateSellerAsync(string id, Seller seller)
    {
        DocumentReference sellerDocRef = _firestoreDb.Collection("sellers").Document(id);

        await sellerDocRef.SetAsync(seller, SetOptions.Overwrite);

        DocumentSnapshot sellerDocSnapshot = await sellerDocRef.GetSnapshotAsync();
        Seller updatedSeller = sellerDocSnapshot.ConvertTo<Seller>();

        return updatedSeller;
    }
}
