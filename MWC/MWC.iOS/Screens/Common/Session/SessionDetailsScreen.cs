using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.iOS.Screens.Common.Session
{
	/// <summary>
	/// Display session info (name, time, location) using UIKit controls and XIB file
	/// </summary>
	public partial class SessionDetailsScreen : UIViewController
	{
		protected BL.Session _session;
		
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public SessionDetailsScreen ( int sessionID )
			: base (UserInterfaceIdiomIsPhone ? "SessionDetailsScreen_iPhone" : "SessionDetailsScreen_iPad", null)
		{
			Console.WriteLine ( "Creating Session Details Screen, Session ID: " + sessionID.ToString() );
			this._session = BL.Managers.SessionManager.GetSession ( sessionID );
		}
				
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.Title = "Session Details";
			this.TitleLabel.Text = this._session.Title;
			this.SubtitleLine1Label.Text = "TODO";
			this.SubtitleLine2Label.Text = "TODO";
			this.TimeLabel.Text = this._session.Start.ToShortTimeString() + " - " + this._session.End.ToShortTimeString();
			this.SpeakerLabel.Text = this._session.SpeakerNames;
			this.OverviewLabel.Text = this._session.Overview;
			
			this.FavoriteButton.TouchUpInside += (sender, e) => {
				ToggleFavorite ();
			};

			if (FavoritesManager.IsFavorite (_session.Title))
				this.FavoriteButton.SetImage (new UIImage(AppDelegate.ImageIsFavorite), UIControlState.Normal);
			else
				this.FavoriteButton.SetImage (new UIImage(AppDelegate.ImageNotFavorite), UIControlState.Normal);
		}
		bool ToggleFavorite ()
		{
			if (FavoritesManager.IsFavorite (_session.Title)) {
				this.FavoriteButton.SetImage (new UIImage(AppDelegate.ImageNotFavorite), UIControlState.Normal);
				FavoritesManager.RemoveFavoriteSession (_session.Title);
				return false;
			} else {
				this.FavoriteButton.SetImage (new UIImage(AppDelegate.ImageIsFavorite), UIControlState.Normal);
				var fav = new Favorite{SessionID = _session.ID, SessionName = _session.Title};
				FavoritesManager.AddFavoriteSession (fav);
				return true;
			}
		}
	}
}
