using HKQTravellingAuthenication.Data;
using HKQTravellingAuthenication.Models.Tour;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using System.Net;

namespace HKQTravellingAuthenication.Areas.Tour.Extension
{
    public static class ImageHandler
    {
        public static async Task DownloadAndSaveImage(string htmlString, string destinationFolder, long tourId, ApplicationDbContext data)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlString);

                var imgNodes = doc.DocumentNode.SelectNodes("(//p[last()]/img)[last()]");

                if (imgNodes != null && imgNodes.Any())
                {
                    foreach (var imgNode in imgNodes)
                    {
                        string imageUrl = imgNode.GetAttributeValue("src", "");
                        string fileExtension = Path.GetExtension(imageUrl);

                        if (!string.IsNullOrEmpty(imageUrl) && !string.IsNullOrEmpty(fileExtension))
                        {
                            using (WebClient webClient = new WebClient())
                            {
                                string fileName = $"image_{DateTime.Now:yyyyMMddHHmmssfff}{fileExtension}";
                                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destinationFolder, fileName);

                                int count = 1;
                                while (File.Exists(fullPath))
                                {
                                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                                    fileName = $"{fileNameWithoutExtension}_{count}{fileExtension}";
                                    fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destinationFolder, fileName);
                                    count++;
                                }

                                await webClient.DownloadFileTaskAsync(imageUrl, fullPath);

                                var tourImage = new TourImages
                                {
                                    DayNumber = 1,
                                    //ImageUrl = Path.Combine(destinationFolder, fileName),
                                    ImageUrl = fileName,
                                    TourId = tourId
                                };
                                data.tourImages.Add(tourImage);
                            }
                        }
                    }
                    await data.SaveChangesAsync();
                    Console.WriteLine("Tất cả hình ảnh đã được tải và lưu thành công!");
                }
                else
                {
                    Console.WriteLine("Không tìm thấy đường dẫn hình ảnh trong chuỗi HTML.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
            }
        }
    }
}
