using System;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;

namespace ThatBlokeCalledJay.BlobLite
{
    /// <summary>
    /// BlobLiteClient interface to expose common lightweight blob functionality.
    /// </summary>
    public interface IBlobLiteClient
    {
        /// <summary>
        /// Try to load plain text from the specified blob. If the container or blob doesn't exist, null will be returned.
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<BlobOperationResult<string>> TryLoadPlainText(string containerName, string blobName);

        /// <summary>
        /// Try to save <paramref name="plainText"/> to the specified blob.
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <param name="plainText">The text to save.</param>
        /// <param name="createContainerIfNoExists">If a container with the specified <paramref name="containerName"/> doesn't exist, should it be created?</param>
        /// <param name="eTag">If provided offers optimistic concurrency.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="StorageException"></exception>
        Task<BlobOperationResult> TrySavePlainText(string containerName, string blobName, string plainText, bool createContainerIfNoExists = false, string eTag = null);

        /// <summary>
        /// Try to load the specified object from json stored in the specified blob. If the container or blob doesn't exist, null will be returned.
        /// </summary>
        /// <typeparam name="T">The object to parse the blob json.</typeparam>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<BlobOperationResult<T>> TryLoadFromJson<T>(string containerName, string blobName);

        /// <summary>
        /// Try to save <paramref name="data"/> as json to the specified blob.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <param name="data">The object to save.</param>
        /// <param name="createContainerIfNoExists">If a container with the specified <paramref name="containerName"/> doesn't exist, should it be created?</param>
        /// <param name="eTag">If provided offers optimistic concurrency.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="StorageException"></exception>
        Task<BlobOperationResult> TrySaveAsJson<T>(string containerName, string blobName, T data, bool createContainerIfNoExists = false, string eTag = null);

        /// <summary>
        /// Try to delete the specified blob.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task TryDeleteBlob(string containerName, string blobName);
    }
}