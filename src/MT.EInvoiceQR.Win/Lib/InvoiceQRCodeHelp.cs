using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.EInvoiceQR.Win
{
    /// <summary>
    /// 发票实体类
    /// </summary>
    public class Invoice
    {
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 税号
        /// </summary>
        public string TaxNumber { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 开户行名称 
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 银行账户
        /// </summary>
        public string BankAccount { get; set; }      
       
    }
    /// <summary>
    /// 发票二维码帮助类
    /// </summary>
    public class InvoiceQRCodeHelp
    {
        private const int crc16 = 0x8005;

        /// <summary>
        /// 计算发票QR码字符串
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetInvoiceQRCodeString(Invoice model)
        {
            //CRC标识符为4字符。从第四位开始到CRC标识符之前所有内容，包括“</>”分隔符采用CRC-16算法。
            //具体算法：P(X)=X16+X15+X2+1高位在前，低位在后
            //便捷开票二维码内容格式如下： 起始符 + 版本号 + base64（名称</>纳税人识别号</>地址电话</>开户行及账号</>CRC）+ 结束符
            //这里文档里面有个坑 应该是：  起始符 + 版本号 + base64（名称</>纳税人识别号</>地址|电话</>开户行|账号</>CRC）+ 结束符

            string crcInput = string.Format("{0}</>{1}</>{2}|{3}</>{4}|{5}</>",
                model.Title,
                model.TaxNumber,
                model.Address,
                model.Telephone,
                model.BankName,
                model.BankAccount); //CRC校验内容
            string crcNo = CRC16(crcInput); //计算CRC16校验码

            return string.Format("$01{0}$", Convert.ToBase64String(Encoding.UTF8.GetBytes(crcInput + crcNo)));
        }

        /// <summary>
        /// 计算CRC16校验码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CRC16(string input)
        {
            sbyte[] inputs = GetSBytesForEncoding("GBK", input);

            int a = 0x0000;
            for (int i = 0; i < inputs.Length; i++)
            {
                a = div(inputs[i], a);
            }

            sbyte r = 0;
            a = div(r, a);
            a = div(r, a);

            return a.ToString("X4");
        }

        /// <summary>
        /// 位移
        /// </summary>
        /// <param name="input"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        private static int div(sbyte input, int a)
        {
            int temp;
            int data = input;
            for (int i = 0; i < 8; i++)
            {
                temp = a & 0x8000;
                a = a << 1;
                a = a & 0x0000ffff;

                int numIn = data & 0x80;
                numIn = numIn >> 7;

                a = a ^ numIn;

                if (temp == 0x8000)
                {
                    a = a ^ crc16;
                }

                data = data << 1;
                a = a & 0x0000ffff;
            }
            return a;
        }

        /// <summary>
        /// 字符串转换为指定编码的字节数组
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private static sbyte[] GetSBytesForEncoding(string encoding, string s)
        {
            return GetSBytesForEncoding(Encoding.GetEncoding(encoding), s);
        }
        /// <summary>
        /// 字符串转换为指定编码的字节数组
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private static sbyte[] GetSBytesForEncoding(Encoding encoding, string s)
        {
            sbyte[] sbytes = new sbyte[encoding.GetByteCount(s)];
            encoding.GetBytes(s, 0, s.Length, (byte[])(object)sbytes, 0);
            return sbytes;
        }
    }
}
