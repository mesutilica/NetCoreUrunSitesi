namespace WebAPIUsing.Utils
{
    public class FileHelper
    {
        public static async Task<string> FileLoaderAsync(IFormFile formFile, string filePath = "")
        {
            var fileName = "";

            if (formFile != null && formFile.Length > 0)
            {
                fileName = formFile.FileName;
                string directory = Directory.GetCurrentDirectory() + "/wwwroot/Img/" + filePath + fileName;
                using var stream = new FileStream(directory, FileMode.Create);
                await formFile.CopyToAsync(stream);
            }

            return fileName;
        }
        public static bool FileRemover(string fileName, string filePath = "/wwwroot/Img/")
        {
            string directory = Directory.GetCurrentDirectory() + filePath + fileName;
            if (File.Exists(directory)) // File.Exists metodu kendisine parametrede verilen dosyanın var olup olmadığını kontrol eder ve buna göre geriye dosya varsa true, yoksa false döndürür.
            {
                File.Delete(directory); // verilen dizindeki dosyayı sunucudan sil.
                return true; // silme başarılıysa geriye true dön
            }
            return false; // Buraya düştüyse silme başarısızdır geriye false dön ki metodu kullanacağımız yerde işlemin başarısız olduğunu bilelim.
        }
    }
}
