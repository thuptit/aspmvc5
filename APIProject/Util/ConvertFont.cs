#region Convert Font code Unicode - TCVN3 - VNI
//
// Authors:
//   Pham Tan Loi (mailto: tanloilp@gmail.com)
//
// Copyright (c) 2015 Vietnam - GNU software
//
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APIProject.Util
{
    public class ConvertFont
    {
        private static char[] unichars = {
        'à', 'á', 'ả', 'ã', 'ạ',
        'ă', 'ằ', 'ắ', 'ẳ', 'ẵ', 'ặ',
        'â', 'ầ', 'ấ', 'ẩ', 'ẫ', 'ậ',
        'đ', 'è', 'é', 'ẻ', 'ẽ', 'ẹ',
        'ê', 'ề', 'ế', 'ể', 'ễ', 'ệ',
        'ì', 'í', 'ỉ', 'ĩ', 'ị',
        'ò', 'ó', 'ỏ', 'õ', 'ọ',
        'ô', 'ồ', 'ố', 'ổ', 'ỗ', 'ộ',
        'ơ', 'ờ', 'ớ', 'ở', 'ỡ', 'ợ',
        'ù', 'ú', 'ủ', 'ũ', 'ụ',
        'ư', 'ừ', 'ứ', 'ử', 'ữ', 'ự',
        'ỳ', 'ý', 'ỷ', 'ỹ', 'ỵ',
        'Ă', 'Â', 'Đ', 'Ê', 'Ô', 'Ơ', 'Ư',
        'À', 'Á', 'Ả', 'Ã', 'Ạ',
        'Ằ', 'Ắ', 'Ẳ', 'Ẵ', 'Ặ',
        'Ầ', 'Ấ', 'Ẩ', 'Ẫ', 'Ậ',
        'È', 'É', 'Ẻ', 'Ẽ', 'Ẹ',
        'Ề', 'Ế', 'Ể', 'Ễ', 'Ệ',
        'Ì', 'Í', 'Ỉ', 'Ĩ', 'Ị',
        'Ò', 'Ó', 'Ỏ', 'Õ', 'Ọ',
        'Ồ', 'Ố', 'Ổ', 'Ỗ', 'Ộ',
        'Ờ', 'Ớ', 'Ở', 'Ỡ', 'Ợ',
        'Ù', 'Ú', 'Ủ', 'Ũ', 'Ụ',
        'Ừ', 'Ứ', 'Ử', 'Ữ', 'Ự',
        'Ỳ', 'Ý', 'Ỷ', 'Ỹ', 'Ỵ' };

        private static char[] tcvnchars = {
        'µ', '¸', '¶', '·', '¹',
        '¨', '»', '¾', '¼', '½', 'Æ',
        '©', 'Ç', 'Ê', 'È', 'É', 'Ë',
        '®', 'Ì', 'Ð', 'Î', 'Ï', 'Ñ',
        'ª', 'Ò', 'Õ', 'Ó', 'Ô', 'Ö',
        '×', 'Ý', 'Ø', 'Ü', 'Þ',
        'ß', 'ã', 'á', 'â', 'ä',
        '«', 'å', 'è', 'æ', 'ç', 'é',
        '¬', 'ê', 'í', 'ë', 'ì', 'î',
        'ï', 'ó', 'ñ', 'ò', 'ô',
        '­', 'õ', 'ø', 'ö', '÷', 'ù',
        'ú', 'ý', 'û', 'ü', 'þ',
        '¡', '¢', '§', '£', '¤', '¥', '¦'};

        private static char[] tcvncharssub = {
        'µ', '¸', '¶', '·', '¹',
        '»', '¾', '¼', '½', 'Æ',
        'Ç', 'Ê', 'È', 'É', 'Ë',
        'Ì', 'Ð', 'Î', 'Ï', 'Ñ',
        'Ò', 'Õ', 'Ó', 'Ô', 'Ö',
        '×', 'Ý', 'Ø', 'Ü', 'Þ',
        'ß', 'ã', 'á', 'â', 'ä',
        '«', 'å', 'è', 'æ', 'ç', 'é',
        'ê', 'í', 'ë', 'ì', 'î',
        'ó', 'ñ', 'ò', 'ô',
        '­', 'õ', 'ø', 'ö', '÷', 'ù',
        'ý', 'û', 'ü', 'þ' };

        private static string[] vnichars = {
        "aø", "aù", "aû", "aõ", "aï",
        "aê", "aè", "aé", "aú", "aü", "aë",
        "aâ", "aà", "aá", "aå", "aã", "aä",
        "ñ", "eø", "eù", "eû", "eõ", "eï",
        "eâ", "eà", "eá", "eå", "eã", "eä",
        "ì", "í", "æ", "ó", "ò",
        "oø", "où", "oû", "oõ", "oï",
        "oâ", "oà", "oá", "oå", "oã", "oä",
        "ô", "ôø", "ôù", "ôû", "ôõ", "ôï",
        "uø", "uù", "uû", "uõ", "uï",
        "ö", "öø", "öù", "öû", "öõ", "öï",
        "yø", "yù", "yû", "yõ", "î",
        "AÊ", "AÂ", "Đ", "EÂ", "OÂ", "Ô", "Ö",
        "AØ", "AÙ", "AÛ", "AÕ", "AÏ",
        "AÈ", "AÉ", "AÚ", "AÜ", "AË",
        "AÀ", "AÁ", "AÅ", "AÃ", "AÄ",
        "EØ", "EÙ", "EÛ", "EÕ", "EÏ",
        "EÀ", "EÁ", "EÅ", "EÃ", "EÄ",
        "Ì", "Í", "Æ", "Ó", "Ò",
        "OØ", "OÙ", "OÛ", "OÕ", "OÏ",
        "OÀ", "OÁ", "OÅ", "OÃ", "OÄ",
        "ÔØ", "ÔÙ", "ÔÛ", "ÔÕ", "ÔÏ",
        "UØ", "UÙ", "UÛ", "UÕ", "UÏ",
        "ÖØ", "ÖÙ", "ÖÛ", "ÖÕ", "ÖÏ",
        "YØ", "YÙ", "YÛ", "YÕ", "Î" };

        private static char[] vnicharssub = { 'ñ', 'ì', 'í', 'æ', 'ó', 'ò', 'î', 'Đ', 'Ì', 'Í', 'Ó', 'Ò', 'Î' };
        private static char[] vnicharssub2 = { 'ô', 'ö', 'Ô', 'Ö', 'a', 'A', 'e', 'E', 'o', 'O', 'u', 'U', 'y', 'Y' };

        private static Dictionary<char, int> dicUnichars = null;
        private static Dictionary<char, int> dicTcvnchars = null;
        private static Dictionary<char, int> dicTcvncharsSub = null;
        private static Dictionary<string, int> dicVnichars = null;
        private static Dictionary<char, int> dicVnicharsSub = null;
        private static Dictionary<char, int> dicVnicharsSub2 = null;

        private static HashSet<char> hashUnichars = null;
        private static HashSet<char> hashTcvnchars = null;
        private static HashSet<char> hashTcvncharsSub = null;
        private static HashSet<string> hashVnichars = null;
        private static HashSet<char> hashVnicharsSub = null;
        private static HashSet<char> hashVnicharsSub2 = null;

        private static int lenUniChars = -1, lenTcvnChars = -1, lenVniChars = -1;

        void MapUni()
        {
            if (dicUnichars == null)
            {
                dicUnichars = new Dictionary<char, int>();
                for (int i = 0; i < unichars.Length; i++)
                {
                    dicUnichars.Add(unichars[i], i);
                }
            }

            if (hashUnichars == null)
            {
                hashUnichars = new HashSet<char>();
                for (int i = 0; i < unichars.Length; i++)
                {
                    hashUnichars.Add(unichars[i]);
                }
            }
        }

        void MapTcvn()
        {
            if (dicTcvnchars == null)
            {
                dicTcvnchars = new Dictionary<char, int>();
                for (int i = 0; i < tcvnchars.Length; i++)
                {
                    dicTcvnchars.Add(tcvnchars[i], i);
                }
            }

            if (dicTcvncharsSub == null)
            {
                dicTcvncharsSub = new Dictionary<char, int>();
                for (int i = 0; i < tcvncharssub.Length; i++)
                {
                    dicTcvncharsSub.Add(tcvncharssub[i], i);
                }
            }

            if (hashTcvnchars == null)
            {
                hashTcvnchars = new HashSet<char>();
                for (int i = 0; i < tcvnchars.Length; i++)
                {
                    hashTcvnchars.Add(tcvnchars[i]);
                }
            }

            if (hashTcvncharsSub == null)
            {
                hashTcvncharsSub = new HashSet<char>();
                for (int i = 0; i < tcvncharssub.Length; i++)
                {
                    hashTcvncharsSub.Add(tcvncharssub[i]);
                }
            }
        }

        void MapVni()
        {
            if (dicVnichars == null)
            {
                dicVnichars = new Dictionary<string, int>();
                for (int i = 0; i < vnichars.Length; i++)
                {
                    dicVnichars.Add(vnichars[i], i);
                }
            }

            if (dicVnicharsSub == null)
            {
                dicVnicharsSub = new Dictionary<char, int>();
                for (int i = 0; i < vnicharssub.Length; i++)
                {
                    dicVnicharsSub.Add(vnicharssub[i], i);
                }
            }

            if (dicVnicharsSub2 == null)
            {
                dicVnicharsSub2 = new Dictionary<char, int>();
                for (int i = 0; i < vnicharssub2.Length; i++)
                {
                    dicVnicharsSub2.Add(vnicharssub2[i], i);
                }
            }

            if (hashVnichars == null)
            {
                hashVnichars = new HashSet<string>();
                for (int i = 0; i < vnichars.Length; i++)
                {
                    hashVnichars.Add(vnichars[i]);
                }
            }

            if (hashVnicharsSub == null)
            {
                hashVnicharsSub = new HashSet<char>();
                for (int i = 0; i < vnicharssub.Length; i++)
                {
                    hashVnicharsSub.Add(vnicharssub[i]);
                }
            }

            if (hashVnicharsSub2 == null)
            {
                hashVnicharsSub2 = new HashSet<char>();
                for (int i = 0; i < vnicharssub2.Length; i++)
                {
                    hashVnicharsSub2.Add(vnicharssub2[i]);
                }
            }
        }

        public ConvertFont()
        {
            MapUni();
            MapTcvn();
            MapVni();
            if (lenUniChars == -1) lenUniChars = unichars.Length;
            if (lenTcvnChars == -1) lenTcvnChars = tcvnchars.Length;
            if (lenVniChars == -1) lenVniChars = vnichars.Length;
        }

        public bool Convert(ref string strConv, FontIndex iSource, FontIndex iDestination)
        {
            if (strConv==null )
            {
                return false;
            }
            if (iSource == FontIndex.iNotKnown || iSource == FontIndex.iNOSIGN) iSource = FontIndex.iUNI;
            if (iDestination == FontIndex.iNotKnown || iDestination == FontIndex.iNOSIGN) iDestination = FontIndex.iUNI;
            if (iSource == iDestination)
            {
                return false;
            }
            try
            {
                switch (iSource)
                {
                    case FontIndex.iUNI:
                        {
                            switch (iDestination)
                            {
                                case FontIndex.iVNI:
                                    {
                                        strConv = UniToVni(strConv);
                                    }
                                    break;
                                case FontIndex.iTCV:
                                    {
                                        strConv = UniToTcvn(strConv);
                                    }
                                    break;
                            }
                        }
                        break;
                    case FontIndex.iTCV:
                        {
                            switch (iDestination)
                            {
                                case FontIndex.iVNI:
                                    {
                                        strConv = TcvnToVni(strConv);
                                    }
                                    break;
                                case FontIndex.iUNI:
                                    {
                                        strConv = TcvnToUni(strConv);
                                    }
                                    break;
                            }
                        }
                        break;
                    case FontIndex.iVNI:
                        {
                            switch (iDestination)
                            {
                                case FontIndex.iTCV:
                                    {
                                        strConv = VniToTcvn(strConv);
                                    }
                                    break;
                                case FontIndex.iUNI:
                                    {
                                        strConv = VniToUni(strConv);
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        string UniToVni(string strConv)
        {
            string des = "";
            foreach (char c in strConv)
            {
                if (hashUnichars.Contains(c))
                {
                    des += vnichars[dicUnichars[c]];
                }
                else
                    des += c;
            }
            return des;
        }

        string VniToUni(string strConv)
        {
            string des = "";
            int index = 0;
            int leng = strConv.Length;
            string c2 = "";
            while (index < leng)
            {
                char c = strConv[index];
                if (hashVnicharsSub2.Contains(c))
                {
                    #region sub 2
                    if (c2 != "")
                    {
                        des += c2;
                        c2 = "";
                    }
                    if (index < leng - 1)
                    {
                        char c1 = strConv[index + 1];
                        if (hashVnicharsSub.Contains(c1))
                        {
                            des += c.ToString() + unichars[dicVnichars[c1.ToString()]];
                            index += 2;
                        }
                        else
                        {
                            var c1str = c.ToString();
                            string cc1 = c1str + c1.ToString();
                            if (hashVnichars.Contains(cc1))
                            {
                                des += unichars[dicVnichars[cc1]];
                                index += 2;
                            }
                            else
                            {
                                if (hashVnichars.Contains(c1str))
                                {
                                    des += unichars[dicVnichars[c1str]];
                                }
                                else
                                    des += c1str;
                                index++;
                            }
                        }
                    }
                    else
                    {
                        des += c.ToString();
                        index++;
                    }
                    #endregion
                }
                else if (hashVnicharsSub.Contains(c))
                {
                    if (c2 != "")
                    {
                        des += c2;
                        c2 = "";
                    }
                    des += unichars[dicVnichars[c.ToString()]];
                    index++;
                }
                else
                {
                    if (c2.Length == 2)
                    {
                        if (hashVnichars.Contains(c2))
                            des += unichars[dicVnichars[c2]];
                        else
                            des += c2;

                        c2 = c.ToString();
                    }
                    else
                        c2 += c;

                    index++;
                    if (index >= leng)
                        des += c2;
                }
            }
            return des;
        }

        string UniToTcvn(string strConv)
        {
            string des = "";

            foreach (char c in strConv)
            {
                if (hashUnichars.Contains(c))
                {
                    int index = dicUnichars[c];
                    if (index > lenTcvnChars)
                    {
                        des += tcvncharssub[index - lenTcvnChars];
                    }
                    else
                        des += tcvnchars[index];
                }
                else
                    des += c;
            }
            return des;
        }

        string TcvnToUni(string strConv)
        {
            string des = "";

            foreach (char c in strConv)
            {
                if (hashTcvnchars.Contains(c))
                {
                    des += unichars[dicTcvnchars[c]];
                }
                else
                    des += c;
            }
            return des;
        }

        string TcvnToVni(string strConv)
        {
            string des = "";
            foreach (char c in strConv)
            {
                if (hashTcvnchars.Contains(c))
                {
                    des += vnichars[dicTcvnchars[c]];
                }
                else
                    des += c;
            }
            return des;
        }

        string VniToTcvn(string strConv)
        {
            string des = "";
            int index = 0;
            int leng = strConv.Length;
            string c2 = "";
            try
            {
                while (index < leng)
                {
                    char c = strConv[index];
                    if (hashVnicharsSub2.Contains(c))
                    {
                        #region sub 2
                        if (c2 != "")
                        {
                            des += c2;
                            c2 = "";
                        }
                        if (index < leng - 1)
                        {
                            char c1 = strConv[index + 1];
                            if (hashVnicharsSub.Contains(c1))
                            {
                                int indexc1 = dicVnichars[c1.ToString()];
                                if (indexc1 > lenTcvnChars)
                                {
                                    des += c.ToString() + tcvncharssub[indexc1 - lenTcvnChars];
                                }
                                else
                                    des += c.ToString() + tcvnchars[index];
                                index += 2;
                            }
                            else
                            {
                                var c1str = c.ToString();
                                string cc1 = c1str + c1.ToString();
                                if (hashVnichars.Contains(cc1))
                                {
                                    int indexc1 = dicVnichars[cc1];
                                    if (indexc1 > lenTcvnChars)
                                    {
                                        des += tcvncharssub[indexc1 - lenTcvnChars];
                                    }
                                    else
                                        des += tcvnchars[indexc1];

                                    index += 2;
                                }
                                else
                                {
                                    if (hashVnichars.Contains(c1str))
                                    {
                                        int indexc1 = dicVnichars[c1str];
                                        if (indexc1 > lenTcvnChars)
                                        {
                                            des += tcvncharssub[indexc1 - lenTcvnChars];
                                        }
                                        else
                                            des += tcvnchars[indexc1];
                                    }
                                    else
                                        des += c1str;
                                    index++;
                                }
                            }
                        }
                        else
                        {
                            des += c.ToString();
                            index++;
                        }
                        #endregion
                    }
                    else if (hashVnicharsSub.Contains(c))
                    {
                        if (c2 != "")
                        {
                            des += c2;
                            c2 = "";
                        }
                        int indexc1 = dicVnichars[c.ToString()];
                        if (indexc1 > lenTcvnChars)
                        {
                            des += tcvncharssub[indexc1 - lenTcvnChars];
                        }
                        else
                            des += tcvnchars[indexc1];
                        index++;
                    }
                    else
                    {
                        if (c2.Length == 2)
                        {
                            if (hashVnichars.Contains(c2))
                            {
                                int indexc1 = dicVnichars[c2];
                                if (indexc1 > lenTcvnChars)
                                {
                                    des += tcvncharssub[indexc1 - lenTcvnChars];
                                }
                                else
                                    des += tcvnchars[indexc1];
                            }
                            else
                                des += c2;

                            c2 = c.ToString();
                        }
                        else
                            c2 += c;

                        index++;
                        if (index >= leng)
                            des += c2;
                    }
                }
            }
            catch { }
            return des;
        }
    }

    public enum FontIndex
    {
        iNOSIGN,
        iNotKnown,
        iTCV,
        iUNI,
        iVNI,
    }
}