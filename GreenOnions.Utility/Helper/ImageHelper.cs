﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GreenOnions.Utility.Helper
{
    public static class ImageHelper
    {
        public static Stream StreamAntiShielding(this Stream ms)
        {
            Bitmap bmp = new Bitmap(Image.FromStream(ms));
            bmp.AntiShielding();
            ms.Close();
            ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            return ms;
        }

        public static MemoryStream HorizontalMirrorImageStream(this MemoryStream ms)
        {
            Bitmap bmp = new Bitmap(Image.FromStream(ms));
            bmp.HorizontalFlip();
            ms.Close();
            ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            return ms;
        }

        public static MemoryStream VerticalMirrorImageStream(this MemoryStream ms)
        {
            Bitmap bmp = new Bitmap(Image.FromStream(ms));
            bmp.VerticalFlip();
            ms.Close();
            ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            return ms;
        }

        public static Bitmap HorizontalFlip(this Bitmap bmp)
        {
            try
            {
                var width = bmp.Width;
                var height = bmp.Height;
                Graphics g = Graphics.FromImage(bmp);
                Rectangle rect = new Rectangle(0, 0, width, height);
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                g.DrawImage(bmp, rect);
                return bmp;
            }
            catch (Exception ex)
            {
                ErrorHelper.WriteErrorLogWithUserMessage("水平镜像图片错误", ex.Message);
                return bmp;
            }
        }

        public static Bitmap VerticalFlip(this Bitmap bmp)
        {
            try
            {
                Graphics g = Graphics.FromImage(bmp);
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                g.DrawImage(bmp, rect);
                return bmp;
            }
            catch (Exception ex)
            {
                ErrorHelper.WriteErrorLogWithUserMessage("垂直镜像图片错误", ex.Message);
                return bmp;
            }
        }

        public static void AntiShielding(this Bitmap bmp)
        {
            Random r = new Random();
            int x = r.Next(0, bmp.Width);
            int y = r.Next(0, bmp.Height);

            Color c = bmp.GetPixel(x, y);
            if (c.A > 128)
            {
                bmp.SetPixel(x, y, Color.FromArgb(c.A - 1, c.R, c.G, c.B));
            }
            else
            {
                bmp.SetPixel(x, y, Color.FromArgb(c.A + 1, c.R, c.G, c.B));
            }
        }
    }

    public class TencentJianHuangResponse
    {
        public List<ImgInfo> result_list = new List<ImgInfo>();
    }

    public class ImgInfo
    {

        /// <summary>
        /// 错误码，0 为成功
        /// </summary>
        public int code { set; get; }

        /// <summary>
        /// 服务器返回的信息
        /// </summary>
        public string message { set; get; }

        /// <summary>
        /// 当前图片的 url
        /// </summary>
        public string url { set; get; }

        /// <summary>
        /// 具体查询数据，具体见实体
        /// </summary>
        public Data data;
    }

    public class Data
    {
        /// <summary>
        /// 0 正常，1 黄图，2 疑似图片
        /// </summary>
        public int result { set; get; }

        /// <summary>
        /// 封禁状态，0 表示正常，1 表示图片已被封禁（只有存储在腾讯云的图片才会被封禁）
        /// </summary>
        public int forbid_status { set; get; }

        /// <summary>
        /// 识别为黄图的置信度，分值 0-100；是 normal_score , hot_score , porn_score的综合评分
        /// </summary>
        public decimal confidence { set; get; }

        /// <summary>
        /// 图片为正常图片的评分
        /// </summary>
        public decimal hot_score { set; get; }

        /// <summary>
        /// 图片为性感图片的评分
        /// </summary>
        public decimal normal_score { set; get; }

        /// <summary>
        /// 图片为色情图片的评分
        /// </summary>
        public decimal porn_score { set; get; }
    }
}
