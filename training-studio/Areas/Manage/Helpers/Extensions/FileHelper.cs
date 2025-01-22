namespace training_studio.Areas.Manage.Helpers.Extensions;

public static class FileHelper
{
    public static async Task<string> SaveFileAsync(string directory, IFormFile file)
    {
        if (Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        string filePath = Path.Combine(directory, fileName);

        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fs);
        }
        return fileName;
    }

    public static bool Delete(string webroot, string folderName, string fileName)
    {
        string path = Path.Combine(webroot, folderName, fileName);
        if (!File.Exists(path))
        {
            return false;
        }
        File.Delete(path);
        return true;
    }
}
