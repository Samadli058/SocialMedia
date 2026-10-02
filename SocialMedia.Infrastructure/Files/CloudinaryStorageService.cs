using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SocialMedia.Application.Files;
using System.Text.RegularExpressions;

namespace SocialMedia.Infrastructure.Files;

public class CloudinaryStorageService : IFileStorageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryStorageService(IOptions<CloudinarySettings> config)
    {
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<string> SaveImageAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
        {
            throw new InvalidOperationException("Fayl boş ola bilməz.");
        }

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "social_media/images",
            Transformation = new Transformation()
                .Quality("auto")
                .FetchFormat("auto")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

        if (uploadResult.Error != null)
        {
            throw new InvalidOperationException(uploadResult.Error.Message);
        }

        return uploadResult.SecureUrl.ToString();
    }

    public async Task<string> SaveVideoAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
        {
            throw new InvalidOperationException("Fayl boş ola bilməz.");
        }

        await using var stream = file.OpenReadStream();

        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "social_media/videos",
            Transformation = new Transformation()
                .Quality("auto")
                .FetchFormat("auto")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

        if (uploadResult.Error != null)
        {
            throw new InvalidOperationException(uploadResult.Error.Message);
        }

        return uploadResult.SecureUrl.ToString();
    }

    public async Task DeleteAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return;
        }

        var publicId = GetPublicIdFromUrl(fileUrl);

        if (string.IsNullOrEmpty(publicId))
        {
            return;
        }

        var isVideo = fileUrl.Contains("/video/upload/");

        var deletionParams = new DeletionParams(publicId)
        {
            ResourceType = isVideo ? ResourceType.Video : ResourceType.Image
        };

        await _cloudinary.DestroyAsync(deletionParams);
    }

    private static string? GetPublicIdFromUrl(string url)
    {
        var match = Regex.Match(url, @"/upload/(?:v\d+/)?(social_media/.+)\.[a-zA-Z0-9]+$");
        return match.Success ? match.Groups[1].Value : null;
    }
}