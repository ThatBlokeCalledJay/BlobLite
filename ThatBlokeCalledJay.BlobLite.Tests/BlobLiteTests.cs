using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;

namespace ThatBlokeCalledJay.BlobLite.Tests
{
    [TestClass]
    public class BlobLiteTests
    {
        private const string UnitTestContainer = "unit-tests";

        private IBlobLiteClient CreateClient()
        {
            var cfg = CommonTestHelpers.LoadConfiguration();

            var connectionString = cfg["storageConnectionString"];

            var client = new BlobLiteClient(connectionString);

            return client;
        }

        [TestMethod]
        public void Test_InitClient_ExpectsConnectionString()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var client = new BlobLiteClient("");
            });
        }

        [TestMethod]
        public async Task Test_PlainText_LoadSave()
        {
            var client = CreateClient();

            var testText = Guid.NewGuid().ToString();

            var blobName = $"bloblite-test-{Guid.NewGuid().ToString()}.txt";

            await client.TrySavePlainText(UnitTestContainer, blobName, testText, true);

            var retrievedText = await client.TryLoadPlainText(UnitTestContainer, blobName);

            Assert.AreEqual(testText, retrievedText.Result);

            await client.TryDeleteBlob(UnitTestContainer, blobName);
        }

        [TestMethod]
        public async Task Test_PlainText_SaveETagFailure()
        {
            var client = CreateClient();

            var testText = Guid.NewGuid().ToString();

            var blobName = $"bloblite-test-{Guid.NewGuid().ToString()}.txt";

            var result1 = await client.TrySavePlainText(UnitTestContainer, blobName, testText, true);
            var result2 = await client.TrySavePlainText(UnitTestContainer, blobName, testText, true);

            Assert.AreNotEqual(result1.ETag, result2.ETag);

            await Assert.ThrowsExceptionAsync<StorageException>(async () =>
            {
                await client.TrySavePlainText(UnitTestContainer, blobName, testText, true, result1.ETag);
            });

            var result3 = await client.TrySavePlainText(UnitTestContainer, blobName, testText, true, result2.ETag);

            Assert.AreNotEqual(result2.ETag, result3.ETag);

            await client.TryDeleteBlob(UnitTestContainer, blobName);
        }

        [TestMethod]
        public async Task Test_JsonObject_LoadSave()
        {
            var client = CreateClient();

            var testObject = BlobLiteTestClass.ProtoType();

            var blobName = $"bloblite-test-{Guid.NewGuid().ToString()}.json";

            await client.TrySaveAsJson(UnitTestContainer, blobName, testObject, true);

            var retrievedObject = await client.TryLoadFromJson<BlobLiteTestClass>(UnitTestContainer, blobName);

            Assert.AreEqual(testObject.IntProperty, retrievedObject.Result.IntProperty);
            Assert.AreEqual(testObject.BoolProperty, retrievedObject.Result.BoolProperty);
            Assert.AreEqual(testObject.StringProperty, retrievedObject.Result.StringProperty);

            await client.TryDeleteBlob(UnitTestContainer, blobName);
        }

        [TestMethod]
        public async Task Test_JsonObject_SaveETagFailure()
        {
            var client = CreateClient();

            var testObject = BlobLiteTestClass.ProtoType();

            var blobName = $"bloblite-test-{Guid.NewGuid().ToString()}.json";

            var result1 = await client.TrySaveAsJson(UnitTestContainer, blobName, testObject, true);
            var result2 = await client.TrySaveAsJson(UnitTestContainer, blobName, testObject, true);

            Assert.AreNotEqual(result1.ETag, result2.ETag);

            await Assert.ThrowsExceptionAsync<StorageException>(async () =>
            {
                await client.TrySaveAsJson(UnitTestContainer, blobName, testObject, true, result1.ETag);
            });

            var result3 = await client.TrySaveAsJson(UnitTestContainer, blobName, testObject, true, result2.ETag);

            Assert.AreNotEqual(result2.ETag, result3.ETag);

            await client.TryDeleteBlob(UnitTestContainer, blobName);
        }
    }

    public class BlobLiteTestClass
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
        public bool BoolProperty { get; set; }

        public static BlobLiteTestClass ProtoType()
        {
            var rnd = new Random(DateTime.Now.Millisecond);

            return new BlobLiteTestClass
            {
                BoolProperty = true,
                StringProperty = Guid.NewGuid().ToString(),
                IntProperty = rnd.Next(int.MinValue, int.MaxValue)
            };
        }
    }
}