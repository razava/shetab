using Domain.Models.Relational.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Infrastructure.Storage;

public class StorageService : IStorageService
{
    private readonly string _destinationPath;
    private readonly List<Size> _imageQualities;
    private readonly List<string> _allowedExtensions;
    private readonly long _maxAllowdFileSize;
    private readonly List<string> _videoExtensions;
    private readonly List<string> _docExtensions;
    private readonly List<string> _voiceExtensions;
    private readonly List<string> _imageExtensions;


    public StorageService(IOptions<StorageOptions> oooo,
        IWebHostEnvironment webHostEnvironment)
    {
        _destinationPath = webHostEnvironment.WebRootPath;
        var so = oooo.Value;
        _imageQualities = so.ImageQualities;

        _imageExtensions = so.ImageExtensions.Split(',').ToList();
        _imageExtensions = _imageExtensions.Select(x => x.Trim().ToLower()).ToList();

        _videoExtensions = so.VideoExtensions.Split(',').ToList();
        _videoExtensions = _videoExtensions.Select(x => x.Trim().ToLower()).ToList();

        _docExtensions = so.DocExtensions.Split(',').ToList();
        _docExtensions = _docExtensions.Select(x => x.Trim().ToLower()).ToList();

        _voiceExtensions = so.VoiceExtensions.Split(',').ToList();
        _voiceExtensions = _voiceExtensions.Select(x => x.Trim().ToLower()).ToList();

        _allowedExtensions = _imageExtensions.Union(_videoExtensions).Union(_voiceExtensions).Union(_docExtensions).ToList();

        _maxAllowdFileSize = so.MaxFileSize;
    }

    public async Task<ICollection<Media>> WriteFileAsync(ICollection<IFormFile> files, AttachmentType attachmentType)
    {
        var result = new List<Media>();
        foreach (var file in files)
        {
            var r = await WriteFileAsync(file, attachmentType);
            if (r != null)
            {
                result.Add(r);
            }
        }

        return result;
    }

    public async Task<Media?> WriteFileAsync(IFormFile file, AttachmentType attachmentType)
    {
        //if (!isAcceptedExtension(file.FileName))
        //    return null;
        if (file == null)
            return null;

        if (file.Length > _maxAllowdFileSize)
        {
            return null;
        }

        if (!isAcceptedExtension(file.FileName))
        {
            return null;
        }

        string fileName;
        string path;
        string sub;
        switch (attachmentType)
        {
            case AttachmentType.Avatar:
                sub = "Avatar";
                break;
            case AttachmentType.Report:
                sub = "Report";
                break;
            case AttachmentType.Poll:
                sub = "Poll";
                break;
            default:
                sub = "Misc";
                break;
        }
        string relativePath;
        Media result;
        try
        {
            var extension = file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            var mediaType = GetMediaType(extension);

            fileName = DateTime.Now.Ticks.ToString(); //Create a new Name for the file due to security reasons.
            relativePath = Path.Combine("Attachments", sub);
            var pathBuilt = Path.Combine(_destinationPath, relativePath);

            if (!Directory.Exists(pathBuilt))
            {
                Directory.CreateDirectory(pathBuilt);
            }

            path = Path.Combine(pathBuilt,
               fileName);

            if (mediaType != MediaType.Image)
            {
                path = path + "." + extension;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    result = new Media()
                    {
                        AlternateText = "",
                        MediaType = mediaType,
                        Title = "",
                        Url = Path.Combine(relativePath, fileName + "." + extension)
                    };
                }
            }
            else
            {
                Image image = Image.Load(file.OpenReadStream());
                result = await writeImage(image, _destinationPath, relativePath, fileName, _imageQualities);
            }

        }
        catch(Exception e)
        {
            throw e.InnerException!;
        }

        return result;
    }

    public async Task<Media?> WriteFileAsync(MemoryStream stream2, AttachmentType attachmentType, string extension)
    {
        var mediaType = GetMediaType(extension);
        string fileName;
        string path;
        string sub;
        switch (attachmentType)
        {
            case AttachmentType.Avatar:
                sub = "Avatar";
                break;
            case AttachmentType.Report:
                sub = "Report";
                break;
            case AttachmentType.Poll:
                sub = "Poll";
                break;
            default:
                sub = "Misc";
                break;
        }
        string relativePath;
        Media result;
        try
        {
            fileName = DateTime.Now.Ticks.ToString();
            relativePath = Path.Combine("Attachments", sub);
            var pathBuilt = Path.Combine(_destinationPath, relativePath);

            if (!Directory.Exists(pathBuilt))
            {
                Directory.CreateDirectory(pathBuilt);
            }

            path = Path.Combine(pathBuilt, fileName);

            if (mediaType != MediaType.Image)
            {
                path = path + "." + extension;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await stream2.CopyToAsync(stream);
                    result = new Media()
                    {
                        AlternateText = "",
                        MediaType = mediaType,
                        Title = "",
                        Url = Path.Combine(relativePath, fileName + "." + extension)
                    };
                }
            }
            else
            {
                Image image = Image.Load(stream2.ToArray());
                result = await writeImage(image, _destinationPath, relativePath, fileName, _imageQualities);
            }

        }
        catch (Exception e)
        {
            throw e.InnerException!;
        }

        return result;
    }

    private MediaType GetMediaType(string extension)
    {
        if (_imageExtensions.Contains(extension.ToLower()))
        {
            return MediaType.Image;
        }

        if (_videoExtensions.Contains(extension.ToLower()))
        {
            return MediaType.Video;
        }

        if (_docExtensions.Contains(extension.ToLower()))
        {
            return MediaType.Doc;
        }

        if (_voiceExtensions.Contains(extension.ToLower()))
        {
            return MediaType.Voice;
        }
        return MediaType.Other;
    }

    private bool isAcceptedExtension(string fileName)
    {
        var extension = fileName.Split('.')[fileName.Split('.').Length - 1];

        return _allowedExtensions.Contains(extension.ToLower());
    }

    private async Task<Media> writeImage(Image image, string destinationPath, string relativePath, string fileName, List<Size> imageQualities)
    {
        var resolutions = new List<Tuple<string, System.Drawing.Size>>();
        for (int i = 0; i < imageQualities.Count; i++)
        {
            resolutions.Add(new Tuple<string, System.Drawing.Size>((i + 1).ToString(), new System.Drawing.Size(imageQualities[i].Width, imageQualities[i].Height)));
        }

        Media result;
        Image thumb;
        JpegEncoder jpegEncoder = new JpegEncoder();
        using MemoryStream mStream = new MemoryStream();
        image.Save(mStream, jpegEncoder);
        string fullPath = Path.Combine(destinationPath, relativePath, fileName);
        string pathBuilt = Path.Combine(destinationPath, relativePath);
        if (!Directory.Exists(pathBuilt))
        {
            Directory.CreateDirectory(pathBuilt);
        }
        await writeToDisk(mStream, fullPath + ".jpg");

        foreach (var item in resolutions)
        {
            var xRatio = (double)item.Item2.Width / image.Width;
            var yRatio = (double)item.Item2.Height / image.Height;
            var ratio = Math.Min(xRatio, yRatio);
            var width = (int)(image.Width * ratio);
            var height = (int)(image.Height * ratio);

            mStream.Seek(0, SeekOrigin.Begin);
            mStream.SetLength(0);
            thumb = image.Clone(x => x.Resize(width, height));
            thumb.Save(mStream, jpegEncoder);
            await writeToDisk(mStream, fullPath + item.Item1 + ".jpg");
        }

        string fullRelativePath = Path.Combine(relativePath, fileName);
        result = new Media()
        {
            AlternateText = "",
            MediaType = MediaType.Image,
            Title = "",
            Url = fullRelativePath + ".jpg",
            Url2 = fullRelativePath + "1" + ".jpg",
            Url3 = fullRelativePath + "2" + ".jpg",
            Url4 = fullRelativePath + "3" + ".jpg",
        };

        return result;
    }

    private async Task writeToDisk(MemoryStream mStream, string path)
    {
        using var outStream = new FileStream(path, FileMode.Create);
        mStream.Seek(0, SeekOrigin.Begin);
        await mStream.CopyToAsync(outStream);
    }
}


