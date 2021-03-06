﻿/**
 * Copyright (c) 2014-present, Facebook, Inc. All rights reserved.
 *
 * You are hereby granted a non-exclusive, worldwide, royalty-free license to use,
 * copy, modify, and distribute this software in source code or binary form for use
 * in connection with the web services and APIs provided by Facebook.
 *
 * As with any software that integrates with the Facebook platform, your use of
 * this software is subject to the Facebook Developer Principles and Policies
 * [http://developers.facebook.com/policy/]. This copyright notice shall be
 * included in all copies or substantial portions of the software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;

using Android.OS;
using Android.Util;

using Xamarin.Facebook;

/**
 * This is a simple example to demonstrate how an app could extend FacebookBroadcastReceiver to handle
 * notifications that long-running operations such as photo uploads have finished.
 */

namespace HelloFacebookXamarinAndroidSample
{
	public class HelloFacebookBroadcastReceiver : FacebookBroadcastReceiver
	{
		protected override void OnSuccessfulAppCall (string appCallId, string action, Bundle extras)
		{
			base.OnSuccessfulAppCall (appCallId, action, extras);

			// A real app could update UI or notify the user that their photo was uploaded.
			Console.WriteLine ("HelloFacebook Photo upload by call {0} succeeded.",appCallId);

		}

		protected override void OnFailedAppCall (string appCallId, string action, Bundle extras)
		{
			base.OnFailedAppCall (appCallId, action, extras);

			// A real app could update UI or notify the user that their photo was not uploaded.
			Console.WriteLine ("HelloFacebook Photo uploaded by call {0} failed.",appCallId);
		}
	}
}

