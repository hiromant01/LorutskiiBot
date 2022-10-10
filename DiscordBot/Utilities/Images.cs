using Discord.WebSocket;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DiscordBot.Utilities
{
    public class Images
    {
        public async Task<string> CreateImageAsync(SocketUser user, string url = "https://media.discordapp.net/attachments/923671441193443348/936047610035339274/950x450.jpg")
        {
            var avatar = await FetchImageAsync(user.GetAvatarUrl(size: 2048, format: Discord.ImageFormat.Png) ?? user.GetDefaultAvatarUrl());
            var background = await FetchImageAsync(url);
            var template = await FetchImageAsync("https://media.discordapp.net/attachments/923671441193443348/936664506136944740/bd73ce8895d4979f.png");

            template = CropToBanner(template, new Size(950, 450));
            background = CropToBanner(background, new Size(950, 450));
            avatar = ClipImageToCircle(avatar);

            using Graphics g = Graphics.FromImage(background);
            g.DrawImage(template, 0, 0);


            var bitmap = avatar as Bitmap;
            bitmap?.MakeTransparent();

            var banner = CopyRegionIntoImage(bitmap, background);
            banner = DrawTextToImage(banner, 
                $"{user.Username}", 
                Infrastructure.Logic.GetRank(user.Id.ToString()), 
                Infrastructure.Logic.GetCoin(user.Id.ToString()),
                Infrastructure.Logic.GetRollCount(user.Id.ToString())
                );

            var path = $"{Guid.NewGuid()}.png";
            banner.Save(path);
            return await Task.FromResult(path);
        }
        public async Task<string> test(SocketUser user, List<(string url, int star)> drop, string url = "https://media.discordapp.net/attachments/923671441193443348/936047610035339274/950x450.jpg")
        {
            var background = await FetchImageAsync(url);
            var width = 1000;
            var height = drop.Count % 5 == 0 ? 200 * drop.Count / 5 : 200 * ((drop.Count / 5) + 1);
            var banner = CropToBanner(background, new Size(width, height));

            var background6StarDrop = await FetchImageAsync("https://cdn.discordapp.com/attachments/923671441193443348/947427034513174548/6_.png");
            var background5StarDrop = await FetchImageAsync("https://cdn.discordapp.com/attachments/923671441193443348/947227677700550736/5_.png");
            var background4StarDrop = await FetchImageAsync("https://cdn.discordapp.com/attachments/923671441193443348/947227677490810880/4_.png");
            var background3StarDrop = await FetchImageAsync("https://media.discordapp.net/attachments/923671441193443348/947227677289500702/3_.png");
            background6StarDrop = CropToBanner(background6StarDrop, new Size(200, 200));
            background5StarDrop = CropToBanner(background5StarDrop, new Size(200, 200));
            background4StarDrop = CropToBanner(background4StarDrop, new Size(200, 200));
            background3StarDrop = CropToBanner(background3StarDrop, new Size(200, 200));


            var counter = 0;
            using Graphics g = Graphics.FromImage(banner);
            for (var i = 0; i < height; i += 200)
                for (var j = 0; j < width; j += 200)
                {
                    if (counter == drop.Count) break;
                    var dropItem = ClipImageToCircle(CropToBanner(await FetchImageAsync(drop[counter].url), new Size(200, 200)));
                    switch (drop[counter].star)
                    {
                        case 6:
                            g.DrawImage(background6StarDrop, j, i);
                            break;
                        case 5:
                            g.DrawImage(background5StarDrop, j, i);
                            break;
                        case 4:
                            g.DrawImage(background4StarDrop, j, i);
                            break;
                        case 3:
                            g.DrawImage(background3StarDrop, j, i);
                            break;
                    }
                    g.DrawImage(dropItem, j, i);
                    counter++;
                }
            
            //using Graphics g = Graphics.FromImage(banner);
            //g.DrawImage(test, 0, 0);
            //g.DrawImage(ClipImageToCircle(test), 200, 0);

            var path = $"{Guid.NewGuid()}.png";
            banner.Save(path);
            return await Task.FromResult(path);
        }

        private static Bitmap CropToBanner(Image image, Size destinationSize)
        {
            var originalWidth = image.Width;
            var originalHeight = image.Height;
            //var destinationSize = new Size(950, 450);

            var heightRatio = (float)originalHeight / destinationSize.Height;
            var widthRatio = (float)originalWidth / destinationSize.Width;

            var ratio = Math.Min(heightRatio, widthRatio);

            var heightScale = Convert.ToInt32(destinationSize.Height * ratio);
            var widthScale = Convert.ToInt32(destinationSize.Width * ratio);

            var startX = (originalWidth - widthScale) / 2;
            var startY = (originalHeight - heightScale) / 2;

            var sourceRectangle = new Rectangle(startX, startY, widthScale, heightScale);
            var bitmap = new Bitmap(destinationSize.Width, destinationSize.Height);
            var destinationRectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            using var g = Graphics.FromImage(bitmap);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; // тест на улучшение графики
            g.DrawImage(image, destinationRectangle, sourceRectangle, GraphicsUnit.Pixel);

            return bitmap;
        }

        private Image ClipImageToCircle(Image image)
        {
            Image destination = new Bitmap(image.Width, image.Height, image.PixelFormat);
            var radius = image.Width / 2; //тут обрезается картинка
            var x = image.Width / 2;
            var y = image.Height / 2;

            using Graphics g = Graphics.FromImage(destination);
            var r = new Rectangle(x - radius, y - radius, radius * 2, radius * 2);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (Brush brush = new SolidBrush(Color.Transparent))
            {
                g.FillRectangle(brush, 0, 0, destination.Width, destination.Height);
            }

            var path = new GraphicsPath();
            path.AddEllipse(r);
            g.SetClip(path);
            g.DrawImage(image, 0, 0);
            return destination;
        }

        private Image CopyRegionIntoImage(Image source, Image destination)
        {
            using var grD = Graphics.FromImage(destination);
            var x = 32;
            var y = 115;

            grD.DrawImage(source, x, y, 220, 220);
            return destination;
        }

        private Image DrawTextToImage(Image image, string nickname, (int Rank, int UserExp, int ExpPerRank) expUser, int coin, int rollCount)
        {
            var brushGrey = new SolidBrush(ColorTranslator.FromHtml("#555555"));
            Graphics g = Graphics.FromImage(image);
            var test = (int)((double)expUser.UserExp / ((double)expUser.ExpPerRank / 100.0) / 100.0 * 634.0);
            g.FillRectangle(brushGrey, 262, 147, test, 42);
            image = new Bitmap(image);


            using var GrD = Graphics.FromImage(image);
            GrD.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            var drawFormat = new StringFormat
            {
                LineAlignment = StringAlignment.Near,
                Alignment = StringAlignment.Near
            };
            var font = new Font("Tahoma", 30, FontStyle.Bold);
            var fontLevel = new Font("Tahoma", 50, FontStyle.Bold);
            var fontCoinRoll = new Font("Tahoma", 38, FontStyle.Bold);
            var brushWhite = new SolidBrush(Color.White);
            //var brushGrey = new SolidBrush(ColorTranslator.FromHtml("#B3B3B3"));
            GrD.DrawString(nickname, font, brushWhite, 255, 90, drawFormat);

            GrD.DrawString("Level", fontLevel, brushWhite, 255, 199, drawFormat);
            GrD.DrawString(expUser.Rank.ToString(), fontLevel, brushWhite, 324, 271, drawFormat);

            var expString = $"exp {expUser.UserExp}/{expUser.ExpPerRank}";
            GrD.DrawString(expString, font, brushWhite, 435, 140, drawFormat);

            var coinString = $"server coin {coin}";
            GrD.DrawString(coinString, fontCoinRoll, brushWhite, 470, 211, drawFormat);
            var rollCountString = $"server roll {rollCount}";
            GrD.DrawString(rollCountString, fontCoinRoll, brushWhite, 470, 281, drawFormat);

            var img = new Bitmap(image);
            return img;
        }

        private async Task<Image> FetchImageAsync(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var backupResponse = await client.GetAsync("https://media.discordapp.net/attachments/923671441193443348/936047610035339274/950x450.jpg");
                var backupStream = await backupResponse.Content.ReadAsStreamAsync();
                return Image.FromStream(backupStream);
            }

            var stream = await response.Content.ReadAsStreamAsync();
            return Image.FromStream(stream);
        }

    }
}