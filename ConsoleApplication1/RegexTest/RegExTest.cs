using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace ConsoleApplication1.RegExTest
{
	class RegExTest
	{

		/// <summary>
		/// 
		/// </summary>
		public static void Replace()
		{
			/*
				$1为正则表达式匹配到的第一组	 
			*/
			string s = "123.254800000009";
			string result= Regex.Replace(s, @"(\.\d\d[1-9]?)\d*", "${name}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

			/*
			 ()--括号的用法可以是分组，也可以用来分支选择 
			 (.|\n)表达式表示匹配一个除“\n”之外的任何单个字符或者匹配一个\n，也就是匹配任意一个字符
			 ?:--分组不被捕获
			 (<(.|\n)*?>).*(</(.|\n)*?>)表达式用有四个分组，在匹配<h1>shidos</h1>的时候得出的分组结果是
			 $1=<h1>,$2=1,$3=</h1>,$4=1,
			 (.|\n)分组能匹配到h和1，所以分组捕获的结果是匹配到的最后一个，也就是1，而如果加上了?:，让分组不被捕获，表达式改为
			 (<(?:.|\n)*?>).*(</(.|\n)*?>)，那么匹配<h1>shidos</h1>的时候得出的分组结果变成了
			 $1=<h1>,$2=</h1>,$3=1,
			 */
			string str1 = "<h1>shidos</h1>";
			string result1 = Regex.Replace(str1, @"(<(?:.|\n)*?>).*(</(.|\n)*?>)", "$1$2", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		}
		/// <summary>
		/// 环视
		/// </summary>
		public static void LoolAround()
		{
			Regex reg = new Regex(@"(?<=\d+吨)(?!标煤)");
			string value= reg.Replace("年能耗500吨以下", "标煤");
		}

		public static void RepeatWorld()
		{

		}
	}
}
