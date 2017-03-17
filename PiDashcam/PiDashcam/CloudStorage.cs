using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PiDashcam
{
	public class CloudStorage
	{
		private CloudBlobClient blobClient;
		private CloudStorageAccount storageAccount;
		private CloudBlobContainer container;

		public CloudStorage(string connectionString, string containerName)
		{
			storageAccount = CloudStorageAccount.Parse(connectionString);
			blobClient = storageAccount.CreateCloudBlobClient();
			container = blobClient.GetContainerReference(containerName);
		}

		public async Task<List<string>> All()
		{
			List<string> result = new List<string>();
			var blobs = await container.ListBlobsSegmentedAsync(new BlobContinuationToken());
			foreach (var b in blobs.Results)
			{
				result.Add(b.Uri.ToString());
			}
			return result;
		}

		public async Task<BlobResultSegment> AllBlobs()
		{
			return await container.ListBlobsSegmentedAsync(new BlobContinuationToken());
		}

		public async Task Add(string text, string blobName)
		{
			CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
			await blockBlob.UploadTextAsync(text);
		}

		public async Task Add(byte[] data, string blobName)
		{
			CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
			await blockBlob.UploadFromByteArrayAsync(data, 0, data.Length);
		}

		//This will overwrite current file if existed.
		public async Task Add(Stream fileStream, string blobName)
		{
			CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
			await blockBlob.UploadFromStreamAsync(fileStream);
		}

		public async Task Delete(string blobName)
		{
			CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
			if (await blockBlob.ExistsAsync())
			{
				await blockBlob.DeleteAsync();
			}
		}

		public async Task<string> ReadText(string blobName)
		{
			CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
			if (await blockBlob.ExistsAsync())
			{
				return await blockBlob.DownloadTextAsync();
			}
			return null;
		}
		public async Task<byte[]> ReadByteArray(string blobName)
		{
			byte[] result;
			CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
			if (await blockBlob.ExistsAsync())
			{
				result = new byte[blockBlob.Properties.Length];
				await blockBlob.DownloadToByteArrayAsync(result, 0);
				return result;
			}
			return null;
		}

		public async Task<bool> Exists(string blobName)
		{
			CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
			return await blockBlob.ExistsAsync();
		}
	}
}
