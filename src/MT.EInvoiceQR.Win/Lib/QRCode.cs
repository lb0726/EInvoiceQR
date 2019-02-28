using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoughtWorks.QRCode.Codec;

namespace MT.EInvoiceQR.Win
{
    public class QRCode
    {
        #region 生成二维码
        /// <summary>
        /// 生成二维码.
        /// </summary>
        /// <param name="data">需要添加进去的文本</param>
        /// <param name="size">二维码尺寸大小54的倍数(1为54x54.2为108x108)</param>
        /// <returns></returns>
        public static System.Drawing.Image GCode(String data, int size)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;

            qrCodeEncoder.QRCodeScale = size;//二维码的尺寸大小54的倍数(2为108x108)
            qrCodeEncoder.QRCodeVersion = 0; // 7 => 0 试试

            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            var pbImg = qrCodeEncoder.Encode(data, System.Text.Encoding.UTF8);
            var width = pbImg.Width / 10;
            var dwidth = width * 2;
            Bitmap bmp = new Bitmap(pbImg.Width + dwidth, pbImg.Height + dwidth);
            Graphics g = Graphics.FromImage(bmp);
            var c = System.Drawing.Color.White;
            g.FillRectangle(new SolidBrush(c), 0, 0, pbImg.Width + dwidth, pbImg.Height + dwidth);
            g.DrawImage(pbImg, width, width);
            g.Dispose();
            return bmp;
        }
        #endregion

        #region 背景图
        /// <summary>
        /// 调用此函数后使此两种图片合并，类似相册，有个
        /// 背景图，中间贴自己的目标图片
        /// </summary>
        /// <param name="sourceImg">粘贴的源图片</param>
        /// <param name="destImg">粘贴的目标图片</param>
        /// <param name="width">背景图宽度</param>
        /// <param name="height">背景图高度</param>
        public static System.Drawing.Image CombinImage(System.Drawing.Image imgBack, string destImg, int width, int height)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(destImg);        //照片图片  
            if (img.Height != height || img.Width != width)
            {
                img = KiResizeImage(img, width, height, 0);
            }
            Graphics g = Graphics.FromImage(imgBack);

            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);//g.DrawImage(imgBack, 0, 0, 相框宽, 相框高); 

            //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框
            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);

            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
            GC.Collect();
            return imgBack;
        }
        #endregion

        #region Resize图片
        /// <summary>
        /// Resize图片
        /// </summary>
        /// <param name="bmp">原始Bitmap</param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的高度</param>
        /// <param name="Mode">保留着，暂时未用</param>
        /// <returns>处理以后的图片</returns>
        public static System.Drawing.Image KiResizeImage(System.Drawing.Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                System.Drawing.Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH),
                    new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();

                return b;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
