using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ThatBlokeCalledJay.BlobLite
{
    /// <summary>
    /// A simple lightweight blob client.
    /// </summary>
    public class BlobLiteClient : IBlobLiteClient
    {
        private const string ContentTypeAppJson = "application/json";
        private const string ContentTypePlainText = "text/plain";

        private readonly CloudBlobClient _blobClient;

        /// <summary>
        /// Instantiate a new instance of BlobClientLite.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BlobLiteClient(string connectionString) : this(new BlobLiteConfiguration(connectionString))
        { }

        /// <summary>
        /// Instantiate a new instance of BlobClientLite.
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BlobLiteClient(IBlobLiteConfiguration configuration)
        {
            if(configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            ValidateParameter(configuration.ConnectionString, "BlobLiteClient ConnectionString");

            var storageAccount = CloudStorageAccount.Parse(configuration.ConnectionString);

            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        private static void ValidateParameter(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(paramName, $"{paramName} cannot be null or empty.");
        }

        /// <inheritdoc />
        public async Task<BlobOperationResult<string>> TryLoadPlainText(string containerName, string blobName)
        {
            ValidateParameter(containerName, nameof(containerName));
            ValidateParameter(blobName, nameof(blobName));

            var container = _blobClient.GetContainerReference(containerName);

            if (!await container.ExistsAsync())
                return null;

            var blob = container.GetBlockBlobReference(blobName);

            if (!await blob.ExistsAsync())
                return null;

            var plainText = await blob.DownloadTextAsync();

            return new BlobOperationResult<string>(plainText, blob.Properties.ETag);
        }

        /// <inheritdoc />
        public async Task<BlobOperationResult> TrySavePlainText(string containerName, string blobName, string plainText, bool createContainerIfNoExists = false, string eTag = null)
        {
            ValidateParameter(containerName, nameof(containerName));
            ValidateParameter(blobName, nameof(blobName));

            var container = _blobClient.GetContainerReference(containerName);

            if (!await container.ExistsAsync() && createContainerIfNoExists)
                await container.CreateAsync();

            var blob = container.GetBlockBlobReference(blobName);

            blob.Properties.ContentType = ContentTypePlainText;

            AccessCondition accessCondition = null;

            if (!string.IsNullOrWhiteSpace(eTag))
                accessCondition = AccessCondition.GenerateIfMatchCondition(eTag);

            await blob.UploadTextAsync(plainText, null, accessCondition, null, null);

            return new BlobOperationResult(blob.Properties.ETag);
        }

        /// <inheritdoc />
        public async Task<BlobOperationResult<T>> TryLoadFromJson<T>(string containerName, string blobName)
        {
            ValidateParameter(containerName, nameof(containerName));
            ValidateParameter(blobName, nameof(blobName));

            var container = _blobClient.GetContainerReference(containerName);

            if (!await container.ExistsAsync())
                return null;

            var blob = container.GetBlockBlobReference(blobName);

            if (!await blob.ExistsAsync())
                return null;

            var json = await blob.DownloadTextAsync();

            var resultObject = JsonConvert.DeserializeObject<T>(json);

            return new BlobOperationResult<T>(resultObject, blob.Properties.ETag);
        }

        /// <inheritdoc />
        public async Task<BlobOperationResult> TrySaveAsJson<T>(string containerName, string blobName, T data, bool createContainerIfNoExists = false, string eTag = null)
        {
            ValidateParameter(containerName, nameof(containerName));
            ValidateParameter(blobName, nameof(blobName));

            var container = _blobClient.GetContainerReference(containerName);

            if (!await container.ExistsAsync() && createContainerIfNoExists)
                await container.CreateAsync();

            var blob = container.GetBlockBlobReference(blobName);

            blob.Properties.ContentType = ContentTypeAppJson;

            var json = JsonConvert.SerializeObject(data);

            AccessCondition accessCondition = null;

            if (!string.IsNullOrWhiteSpace(eTag))
                accessCondition = AccessCondition.GenerateIfMatchCondition(eTag);

            await blob.UploadTextAsync(json, null, accessCondition, null, null);

            return new BlobOperationResult(blob.Properties.ETag);
        }

        /// <inheritdoc />
        public async Task TryDeleteBlob(string containerName, string blobName)
        {
            ValidateParameter(containerName, nameof(containerName));
            ValidateParameter(blobName, nameof(blobName));

            var container = _blobClient.GetContainerReference(containerName);

            if (!await container.ExistsAsync())
                return;

            var blob = container.GetBlockBlobReference(blobName);

            await blob.DeleteIfExistsAsync();
        }
    }
}