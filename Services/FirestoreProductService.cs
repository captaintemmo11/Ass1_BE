using Google.Cloud.Firestore;
using Ass1_BE.Models;

namespace Ass1_BE.Services
{
    public class FirestoreProductService
    {
        private readonly FirestoreDb _db;
        private const string COLLECTION = "products";

        public FirestoreProductService(FirestoreDb db)
        {
            _db = db;
        }

        // 🟢 Lấy tất cả sản phẩm
        public async Task<List<Product>> GetAllAsync()
        {
            var snapshot = await _db.Collection(COLLECTION).GetSnapshotAsync();
            return snapshot.Documents
                .Select(d =>
                {
                    var prod = d.ConvertTo<Product>();
                    prod.Id = d.Id;
                    return prod;
                })
                .ToList();
        }

        // 🟢 Lấy sản phẩm theo ID
        public async Task<Product?> GetByIdAsync(string id)
        {
            var doc = await _db.Collection(COLLECTION).Document(id).GetSnapshotAsync();
            if (!doc.Exists) return null;

            var prod = doc.ConvertTo<Product>();
            prod.Id = doc.Id;
            return prod;
        }

        // 🟢 Tạo sản phẩm mới
        public async Task<Product> CreateAsync(Product p)
        {
            var docRef = await _db.Collection(COLLECTION).AddAsync(p);
            p.Id = docRef.Id;
            return p;
        }

        // 🟢 Cập nhật sản phẩm
        public async Task<bool> UpdateAsync(string id, Product p)
        {
            var docRef = _db.Collection(COLLECTION).Document(id);
            var doc = await docRef.GetSnapshotAsync();

            if (!doc.Exists) return false;

            await docRef.SetAsync(p, SetOptions.Overwrite);
            return true;
        }

        // 🟢 Xóa sản phẩm
        public async Task<bool> DeleteAsync(string id)
        {
            var docRef = _db.Collection(COLLECTION).Document(id);
            var doc = await docRef.GetSnapshotAsync();

            if (!doc.Exists) return false;

            await docRef.DeleteAsync();
            return true;
        }
    }
}
