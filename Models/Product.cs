using Google.Cloud.Firestore;

namespace Ass1_BE.Models
{
    [FirestoreData]
    public class Product
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty("name")]
        public string Name { get; set; }

        [FirestoreProperty("description")]
        public string Description { get; set; }

        [FirestoreProperty("price")]
        public decimal Price { get; set; }

        [FirestoreProperty("imageUrl")]
        public string? ImageUrl { get; set; }
    }
}
