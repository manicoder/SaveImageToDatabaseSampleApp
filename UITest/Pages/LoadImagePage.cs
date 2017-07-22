﻿using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.UITest;

using SaveImageToDatabaseSampleApp.Constants;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace SaveImageToDatabaseSampleApp.UITest
{
	public class LoadImagePage : BasePage
	{
		#region Constant Fields
		readonly Query _loadImageButton;
		readonly Query _imageUrlEntry;
		readonly Query _downloadedImage;
		readonly Query _isDownloadingActivityIndicator;
		readonly Query _clearImageButton;
		#endregion

		#region Constructors
		public LoadImagePage(IApp app, Platform platform) : base(app, platform)
		{
			_loadImageButton = x => x.Marked(AutomationIdConstants.LoadImageButton);
			_imageUrlEntry = x => x.Marked(AutomationIdConstants.ImageUrlEntry);
			_downloadedImage = x => x.Marked(AutomationIdConstants.DownloadedImage);
			_isDownloadingActivityIndicator = x => x.Marked(AutomationIdConstants.IsDownloadingActivityIndicator);
			_clearImageButton = x => x.Marked(AutomationIdConstants.ClearImageButton);
		}
		#endregion

		#region Properties
		public string ValidUrl =>
			@"https://www.xamarin.com/content/images/pages/branding/assets/xamarin-logo.png";

		public string InvalidUrl =>
			@"https://www.xamarin.com/abc123";

		public string LoadImageButtonText =>
			GetLoadImageButtonText();

		public bool IsDownloadedImageShown =>
			app.Query(_downloadedImage)?.Length > 0;

		public bool IsErrorPromptVisible =>
			app.Query("Ok")?.Length > 0;
		#endregion

		#region Methods
		public void EnterUrl(string url)
		{
			EnterText(_imageUrlEntry, url);
			app.DismissKeyboard();
			app.Screenshot($"Entered Test: {url}");
		}

		public void TapKeyboardEnterButton()
		{
			app.ScrollUpTo(_imageUrlEntry);
			app.Tap(_imageUrlEntry);
			app.PressEnter();
			app.DismissKeyboard();
			app.Screenshot("Tapped Keyboard Return Button");
		}

		public void TapLoadImageButton()
		{
			app.ScrollUpTo(_imageUrlEntry);
			app.Tap(_loadImageButton);
			app.Screenshot("Tapped Load Image Button");
		}

		public async Task WaitForNoIsDownloadingActivityIndicator(int timeoutInSeconds = 60)
		{
			await Task.Delay(1000);
			app.WaitForNoElement(_isDownloadingActivityIndicator, "Is Downloading Activity Indicator Never Disappeared", TimeSpan.FromSeconds(timeoutInSeconds));
			app.Screenshot("Is Downloading Activity Indicator Dissapeared");
		}

		public void TapOkOnErrorPrompt()
		{
			app.Tap("Ok");
			app.Screenshot("Tapped Ok On Error Prompt");
		}

		public void TapClearImageButton()
		{
			app.ScrollDownTo(_clearImageButton);
			app.Tap(_clearImageButton);
			app.Screenshot("Clear Image Button Tapped");
		}

		string GetLoadImageButtonText()
		{
			if (IsAndroid)
				return app.Query(_loadImageButton)?.FirstOrDefault()?.Text;

			return app.Query(_loadImageButton)?.FirstOrDefault()?.Label;
		}

		void EnterUrlAndTapKeyboardReturnButton(string url)
		{
			EnterText(_imageUrlEntry, url);
			app.Screenshot($"Entered Test: {url}");
			app.PressEnter();
			app.Screenshot($"Pressed Keyboard Return Button");
			app.DismissKeyboard();
		}

		void EnterText(Query query, string url)
		{
			app.ScrollUpTo(query);
			app.ClearText(query);
			app.EnterText(query, url);
		}
		#endregion
	}
}
