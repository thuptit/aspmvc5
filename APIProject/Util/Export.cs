using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Word;
using APIProject.Models;

namespace APIProject.Util
{
    public  class Export
    {

		private  char[] TCVN3 = {
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
        '¡', '¢', '§', '£', '¤', '¥', '¦'
    };

		private  char[] Unicode = {
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
        'Ă', 'Â', 'Đ', 'Ê', 'Ô', 'Ơ', 'Ư'
    };

		private  char ConvertChar(char ch)
		{
			for (int i = 0; i < 213; i++)
			{
				if (ch == TCVN3[i])
				{
					return Unicode[i];
				}
			}
			return (char)ch;
		}
		private char ConvertCharTCVN(char ch)
		{
			for (int i = 0; i < TCVN3.Length; i++)
			{
				if (ch == Unicode[i])
				{
					return TCVN3[i];
				}
			}
			return (char)ch;
		}
		
		public string ConvertText( string goc)
		{
			int i;
			int n = goc.Length;
			string s = "";
			for (i = 0; i < n; i++)
			{
				var c = ConvertCharTCVN(goc[i]);
				s = s + c;
			}
			return s;
		}


	}
}