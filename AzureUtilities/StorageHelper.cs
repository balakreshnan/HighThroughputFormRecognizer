﻿using Azure.Storage.Blobs;
using Azure.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Azure.Core;
using System.Security.Principal;
using Azure.Security.KeyVault.Secrets;
namespace AzureUtilities
{
    public class StorageHelper
    {
        private readonly ILogger<StorageHelper> logger;
        public StorageHelper(ILogger<StorageHelper> logger)
        {
            this.logger = logger;

        }

        public BlobServiceClient CreateStorageClient(string storageAccountName)
        {
            var serviceClient = new BlobServiceClient(new Uri($"https://{storageAccountName}.blob.core.windows.net"), AadHelper.TokenCredential);
            return serviceClient;
        }

        public BlobContainerClient CreateBlobContainerClient(string containerName, BlobServiceClient serviceClient)
        {
            var container = serviceClient.GetBlobContainerClient(containerName);
            return container;
        }
        public BlobContainerClient CreateBlobContainerClient(string containerName, string storageAccountName)
        {
            var serviceClient = new BlobServiceClient(new Uri($"https://{storageAccountName}.blob.core.windows.net"), AadHelper.TokenCredential);
            return CreateBlobContainerClient(containerName, serviceClient);
        }

        public async Task<string> GetStorageConnectionString(string keyVaultName, string storageAcctName)
        {
            CancellationTokenSource src = new CancellationTokenSource();

            SecretClient secretClient = new SecretClient(new Uri($"https://{keyVaultName}.vault.azure.net"), AadHelper.TokenCredential);
            var secret = await secretClient.GetSecretAsync("STORAGE-KEY");
            var key = secret.Value.Value;
            var connectionStr = $"DefaultEndpointsProtocol=https;AccountName={storageAcctName};AccountKey={key};EndpointSuffix=core.windows.net";
            return connectionStr;


        }


    }
}
