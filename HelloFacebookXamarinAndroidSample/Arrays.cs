using System;
using System.Collections;
using System.Collections.Generic;

namespace HelloFacebookXamarinAndroidSample
{
	public static class Arrays
	{
		public static IList<T> AsList<T>(params T[] source){
			return source;
		}
	}
}

