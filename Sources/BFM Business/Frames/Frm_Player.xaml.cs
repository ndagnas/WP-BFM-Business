//*******************************************************************************************************************************
// DEBUT DU FICHIER
//*******************************************************************************************************************************

//*******************************************************************************************************************************
// Nom           : Frm_Player.xaml.cs
// Auteur        : Nicolas Dagnas
// Description   : Implémentation de l'écran Player
// Créé le       : 26/04/2015
// Modifié le    : 20/10/2016
//*******************************************************************************************************************************

//-------------------------------------------------------------------------------------------------------------------------------
#region Using directives
//-------------------------------------------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.IO.IsolatedStorage;
using System.Windows.Navigation;
using System.Collections.Generic;
using System.Windows.Phone.Controls;
using System.Windows.Media.Animation;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using Microsoft.Phone.Controls;
using Microsoft.PlayerFramework;
using Microsoft.Phone.Net.NetworkInformation;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using SM.Media.MediaPlayer;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using NextRadio.Service;
//-------------------------------------------------------------------------------------------------------------------------------
#endregion
//-------------------------------------------------------------------------------------------------------------------------------

//*******************************************************************************************************************************
// Début du bloc "NextRadio"
//*******************************************************************************************************************************
namespace NextRadio.Frames
	{

	//  #####  ####   #   #         ####   #       ###   #   #  #####  #### 
	//  #      #   #  ## ##         #   #  #      #   #   # #   #      #   #
	//  ###    ####   # # #  #####  ####   #      #####    #    ###    #### 
	//  #      #   #  #   #         #      #      #   #    #    #      #   #
	//  #      #   #  #   #         #      #####  #   #    #    #####  #   #
	
	//***************************************************************************************************************************
	#region // Frame Frm_Player
	//---------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Définit la fenêtre Player.
	/// </summary>
	//---------------------------------------------------------------------------------------------------------------------------
	public partial class Frm_Player : PhoneApplicationPage
		{
		//***********************************************************************************************************************
		#region // Classe LiveStream
		//-----------------------------------------------------------------------------------------------------------------------
		class LiveStream
			{
			//-------------------------------------------------------------------------------------------------------------------
			public LiveStream ( Uri Uri )
				{
				this.BandWidth = 0;
				this.Uri       = Uri;
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			public LiveStream ( int BandWidth, Uri Uri )
				{
				this.BandWidth = BandWidth;
				this.Uri       = Uri;
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			public double BandWidth { get; private set; }
			public Uri    Uri       { get; private set; }
			//-------------------------------------------------------------------------------------------------------------------
			}
		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		#region // Classe LiveStreamComparer
		//-----------------------------------------------------------------------------------------------------------------------
		class LiveStreamComparer : IComparer<LiveStream>
			{
			//-------------------------------------------------------------------------------------------------------------------
			public int Compare ( LiveStream X, LiveStream Y )
				{
				//---------------------------------------------------------------------------------------------------------------
				return ( X.BandWidth > Y.BandWidth ) ? 1 : -1;
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************

		//-----------------------------------------------------------------------------------------------------------------------
		// Section des Attributs
		//-----------------------------------------------------------------------------------------------------------------------
		private const double     Offset           = 122;
		private bool             IsOpen           = false;
		private List<LiveStream> LiveStreams      = null;
		private int              SelectedStream   = 0;
		private MediaPlayer      LiveMediaControl = null;
		//-----------------------------------------------------------------------------------------------------------------------

		//***********************************************************************************************************************
		#region // Section des Constructeurs
		//-----------------------------------------------------------------------------------------------------------------------

		//***********************************************************************************************************************
		/// <summary>
		/// Initialise une nouvelle instance de l'objet <b>Frm_Player</b>.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public Frm_Player ()
			{
			//-------------------------------------------------------------------------------------------------------------------
			this.InitializeComponent ();

			this.LiveStreams = new List<LiveStream> ();

			this.LayoutControls.GetVerticalOffset ().Transform.Y = Offset;
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
			
		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		#region // Section des Procédures Privées
		//-----------------------------------------------------------------------------------------------------------------------
		
		//***********************************************************************************************************************
		/// <summary>
		/// Crée le player.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		private MediaPlayer GetPlayer ( bool AutoClose )
			{
			//-------------------------------------------------------------------------------------------------------------------
			if ( this.LiveMediaControl != null )
				{
				try { this.LiveMediaControl.Stop (); } catch {}

				this.LiveMediaContainer.Child = null;

				this.LiveMediaControl = null;
				}

			if ( this.LiveMediaControl == null )
				{
				//---------------------------------------------------------------------------------------------------------------
				this.LiveMediaControl = new MediaPlayer ();

				this.LiveMediaControl.Stretch                     = Stretch.Uniform;
				this.LiveMediaControl.AutoPlay                    = true;
				this.LiveMediaControl.Volume                      = 1;
				this.LiveMediaControl.IsInteractive               = false;
				this.LiveMediaControl.InteractiveActivationMode   = InteractionType.None;
				this.LiveMediaControl.InteractiveDeactivationMode = InteractionType.None;
				this.LiveMediaControl.CurrentStateChanged        += OnMediaCurrentStateChanged;
				this.LiveMediaControl.MediaQuality                = Microsoft.PlayerFramework.
				                                                       MediaQuality.HighDefinition;

				this.LiveMediaControl.MediaFailed += (_S, _A) =>
					{
					if ( this.SelectedStream > 0 )
						{
						this.SelectedStream --;

						this.QualityButtonL1.Selected   = ( this.SelectedStream == 0 );
						this.QualityButtonL2.Selected   = ( this.SelectedStream == 1 );
						this.QualityButtonL3.Selected   = ( this.SelectedStream == 2 );
						this.QualityButtonL4.Selected   = ( this.SelectedStream == 3 );

						this.Dispatcher.BeginInvoke ( () =>
							{
							MediaPlayer Player = this.GetPlayer ( false );

							this.LiveMediaControl.Source = this.LiveStreams[SelectedStream].Uri;
							} );
						}
					else if ( _A != null )
						{ this.CloseLive ( RestRequestResult.ServiceUnavailable, AutoClose ); }
					};

				this.LiveMediaControl.Plugins.Add ( new StreamingMediaPlugin () );

				this.LiveMediaContainer.Child = this.LiveMediaControl;
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			return this.LiveMediaControl;
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Configure les boutons de qualité celon la disponibilité des flux.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		private void DefineQualityButtons ()
			{
			//-------------------------------------------------------------------------------------------------------------------
			this.SelectedStream = 3;
				
			if ( NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 )
				this.SelectedStream = (int)StorageSettings.GetValue ( "player-mode-wifi", 3 );
			else
				this.SelectedStream = (int)StorageSettings.GetValue ( "player-mode-3g"  , 0 );

			this.SelectedStream = Math.Max ( Math.Min ( this.SelectedStream, this.LiveStreams.Count - 1 ), 0 );
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			if ( this.LiveStreams.Count == 1 )
				{
				//---------------------------------------------------------------------------------------------------------------
				this.QualityButtonL1.Visibility = Visibility.Collapsed;
				this.QualityButtonL2.Visibility = Visibility.Collapsed;
				this.QualityButtonL3.Visibility = Visibility.Collapsed;
				this.QualityButtonL4.Visibility = Visibility.Collapsed;
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			else if ( this.LiveStreams.Count == 2 )
				{
				//---------------------------------------------------------------------------------------------------------------
				this.QualityButtonL1.Content    = ( this.LiveStreams[0].BandWidth / 100000 ).
							                    ToString ( "0.00" ).Replace ( ",", "." ) + " Mbps";
				this.QualityButtonL3.Content    = ( this.LiveStreams[1].BandWidth / 100000 ).
							                    ToString ( "0.00" ).Replace ( ",", "." ) + " Mbps";

				this.QualityButtonL1.Visibility = Visibility.Visible;
				this.QualityButtonL2.Visibility = Visibility.Collapsed;
				this.QualityButtonL3.Visibility = Visibility.Visible;
				this.QualityButtonL4.Visibility = Visibility.Collapsed;

				this.QualityButtonL1.Selected   = ( this.SelectedStream == 0 );
				this.QualityButtonL3.Selected   = ( this.SelectedStream >= 1 );

				this.QualityButtonL1.Tag   = 0;
				this.QualityButtonL3.Tag   = 4;
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			else if ( this.LiveStreams.Count == 3 )
				{
				//---------------------------------------------------------------------------------------------------------------
				this.QualityButtonL1.Content    = ( this.LiveStreams[0].BandWidth / 100000 ).
							                    ToString ( "0.00" ).Replace ( ",", "." ) + " Mbps";
				this.QualityButtonL2.Content    = ( this.LiveStreams[1].BandWidth / 100000 ).
							                    ToString ( "0.00" ).Replace ( ",", "." ) + " Mbps";
				this.QualityButtonL3.Content    = ( this.LiveStreams[2].BandWidth / 100000 ).
							                    ToString ( "0.00" ).Replace ( ",", "." ) + " Mbps";

				this.QualityButtonL1.Visibility = Visibility.Visible;
				this.QualityButtonL2.Visibility = Visibility.Visible;
				this.QualityButtonL3.Visibility = Visibility.Visible;
				this.QualityButtonL4.Visibility = Visibility.Collapsed;

				this.QualityButtonL1.Selected   = ( this.SelectedStream == 0 );
				this.QualityButtonL2.Selected   = ( this.SelectedStream == 1 );
				this.QualityButtonL3.Selected   = ( this.SelectedStream >= 2 );

				this.QualityButtonL1.Tag   = 0;
				this.QualityButtonL2.Tag   = 1;
				this.QualityButtonL3.Tag   = 4;
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			else if ( this.LiveStreams.Count == 4 )
				{
				//---------------------------------------------------------------------------------------------------------------
				this.QualityButtonL1.Content    = ( this.LiveStreams[0].BandWidth / 100000 ).
							                    ToString ( "0.00" ).Replace ( ",", "." ) + " Mbps";
				this.QualityButtonL2.Content    = ( this.LiveStreams[1].BandWidth / 100000 ).
							                    ToString ( "0.00" ).Replace ( ",", "." ) + " Mbps";
				this.QualityButtonL3.Content    = ( this.LiveStreams[2].BandWidth / 100000 ).
							                    ToString ( "0.00" ).Replace ( ",", "." ) + " Mbps";
				this.QualityButtonL4.Content    = ( this.LiveStreams[3].BandWidth / 100000 ).
							                    ToString ( "0.00" ).Replace ( ",", "." ) + " Mbps";

				this.QualityButtonL1.Visibility = Visibility.Visible;
				this.QualityButtonL2.Visibility = Visibility.Visible;
				this.QualityButtonL3.Visibility = Visibility.Visible;
				this.QualityButtonL4.Visibility = Visibility.Visible;

				this.QualityButtonL1.Selected   = ( this.SelectedStream == 0 );
				this.QualityButtonL2.Selected   = ( this.SelectedStream == 1 );
				this.QualityButtonL3.Selected   = ( this.SelectedStream == 2 );
				this.QualityButtonL4.Selected   = ( this.SelectedStream == 3 );

				this.QualityButtonL1.Tag   = 0;
				this.QualityButtonL2.Tag   = 1;
				this.QualityButtonL3.Tag   = 2;
				this.QualityButtonL4.Tag   = 3;
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Ferme la fenêtre du direct.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		private void CloseLive ( RestRequestResult ErrorCode, bool AutoClose )
			{
			//-------------------------------------------------------------------------------------------------------------------
			this.Dispatcher.BeginInvoke ( () =>
				{
				//---------------------------------------------------------------------------------------------------------------
				this.IsIndeterminate = false;
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				string Message = "SERVICE_UNAVAILABLE";

				switch ( ErrorCode )
					{
					//-----------------------------------------------------------------------------------------------------------
					case RestRequestResult.EmptyResponse      : Message = "EMPTY_RESPONSE";       break;
					case RestRequestResult.InvalideFormat     : Message = "INVALIDE_FORMAT";      break;
					case RestRequestResult.PartialContent     : Message = "PARTIAL_CONTENT";      break;
					case RestRequestResult.Forbidden          : Message = "FORBIDDEN";            break;
					case RestRequestResult.NotFound           : Message = "NOT_FOUND";            break;
					case RestRequestResult.Timeout            : Message = "TIMEOUT";              break;
					case RestRequestResult.Unauthorized       : Message = "UNAUTHORIZED_ACCESS";  break;
					//-----------------------------------------------------------------------------------------------------------

					//-----------------------------------------------------------------------------------------------------------
					case RestRequestResult.InternalException  : Message = "INTERNAL_EXCEPTION";   break;
					case RestRequestResult.ProxyAccessRequest : Message = "PROXY_ACCESS_REQUEST"; break;
					case RestRequestResult.ServiceUnavailable : Message = "SERVICE_UNAVAILABLE";  break;
					//-----------------------------------------------------------------------------------------------------------
					}

				Message = string.Format ( "Désolé, nous ne parvenons pas à accéder au direct" +
				                                             ".\n\nCode d'erreur : {0}", Message );
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				var MessageBox = new CustomMessageBox ()
					{ Message = Message, LeftButtonContent = "fermer" };

				MessageBox.Dismissed += (S, A) =>
					{
					if ( AutoClose )
						{
						if ( this.NavigationService.CanGoBack )
							this.NavigationService.GoBack ();
						else
							Instance.RootFrame.Navigate ( new Uri ("/Frames/Frm_Home.xaml", UriKind.Relative) );
						}
					};

				MessageBox.Show ();
				//---------------------------------------------------------------------------------------------------------------
				} );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Affiche la barre des contrôles.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		private void OpenControls ()
			{
			//-------------------------------------------------------------------------------------------------------------------
			this.IsOpen = true;
			//-------------------------------------------------------------------------------------------------------------------
			
			//-------------------------------------------------------------------------------------------------------------------
			var T = this.LayoutControls.GetVerticalOffset ().Transform;

			var Db1 = new DoubleAnimation ()
				{
				To             = 0                                                   ,
				From           = T.Y                                                 ,
				EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn },
				Duration       = TimeSpan.FromMilliseconds ( 300 )                   ,
				};
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			Storyboard.SetTarget         (Db1, T                                                );
			Storyboard.SetTargetProperty (Db1, new PropertyPath ( TranslateTransform.YProperty ));

			var Sb1 = new Storyboard () { BeginTime = TimeSpan.FromMilliseconds ( 0 ) };

			Sb1.Children.Add ( Db1 );
			Sb1.Begin        (     );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Cache la barre des contrôles.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		private void CloseControls ()
			{
			//-------------------------------------------------------------------------------------------------------------------
			this.IsOpen = false;
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			var T = this.LayoutControls.GetVerticalOffset ().Transform;

			var Db1 = new DoubleAnimation ()
				{
				To             = Offset                                               ,
				From           = 0                                                    ,
				EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut },
				Duration       = TimeSpan.FromMilliseconds ( 300 )                    ,
				};
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			Storyboard.SetTarget         (Db1, T                                                );
			Storyboard.SetTargetProperty (Db1, new PropertyPath ( TranslateTransform.YProperty ));

			var Sb1 = new Storyboard () { BeginTime = TimeSpan.FromMilliseconds ( 0 ) };

			Sb1.Children.Add ( Db1 );
			Sb1.Begin        (     );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		#region // Section des Procédures Dérivées
		//-----------------------------------------------------------------------------------------------------------------------
		
		//***********************************************************************************************************************
		/// <summary>
		/// Déclenche l'événement <b>Loaded</b>.
		/// </summary>
		/// <param name="Sender">Objet source de l'appel.</param>
		/// <param name="Args"><b>RoutedEventArgs</b> qui contient les données d'événement.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void OnLoaded ( object Sender, RoutedEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			this.IsIndeterminate = true;
			//-------------------------------------------------------------------------------------------------------------------
			
			//-------------------------------------------------------------------------------------------------------------------
			this.Dispatcher.Async ( () =>
				{
				//---------------------------------------------------------------------------------------------------------------
				RestService.GetLive ( (S, A) =>
					{
					//-----------------------------------------------------------------------------------------------------------
					if ( A.Result == RestRequestResult.ProxyAccessRequest )
						{
						//-------------------------------------------------------------------------------------------------------
						this.Dispatcher.BeginInvoke ( () =>
							{
							//---------------------------------------------------------------------------------------------------
							if ( this.NavigationService.CanGoBack )
								this.NavigationService.GoBack ();
							else
								Instance.RootFrame.Navigate ( new Uri ("/Frames/Frm_Home.xaml", UriKind.Relative) );
							//---------------------------------------------------------------------------------------------------
							} );
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					else if ( A.Result != RestRequestResult.Success )
						{
						//-------------------------------------------------------------------------------------------------------
						this.CloseLive ( A.Result, true );
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					else if ( ! string.IsNullOrEmpty ( A.Content ) )
						{
						//-------------------------------------------------------------------------------------------------------
						this.Orientation           = PageOrientation         .Landscape;
						this.SupportedOrientations = SupportedPageOrientation.Landscape;
						//-------------------------------------------------------------------------------------------------------

						//-------------------------------------------------------------------------------------------------------
						#region // Lecture des Fluxs
						//-------------------------------------------------------------------------------------------------------
						if ( ! A.Content.StartsWith ( "http:" ) )
							{
							using ( StringReader Sr = new StringReader ( A.Content ) )
								{
								int    BandWidth = -1;
								string Line      = string.Empty;

								while ( ( Line = Sr.ReadLine () ) != null )
									{
									if ( Line.StartsWith ( "#EXT-X-STREAM-INF" ) )
										{
										int P1 = Line.IndexOf ( "BANDWIDTH=" );

										if ( P1 == -1 ) continue;

										int P2 = Line.IndexOf ( ",", P1 );

										if ( P2 == -1 ) P2 = Line.Length;

										BandWidth = Line.Substring ( P1 + 10, P2 - P1 - 10 ).ToInteger ( -1 );
										}
									else if ( Line.StartsWith ( "http:" ) && BandWidth != -1 )
										{
										LiveStreams.Add ( new LiveStream ( BandWidth, new Uri ( Line ) ) );
										}
									else if ( Line.EndsWith ( ".m3u8" ) && BandWidth != -1 )
										{
										string Uri = ((Uri)S).AbsoluteUri;

										Uri = Uri.Replace ( Path.GetFileName ( Uri ), Line );
										
										LiveStreams.Add ( new LiveStream ( BandWidth, new Uri ( Uri ) ) );
										}
									else { BandWidth = -1; }
									}
								}

							LiveStreams.Sort ( new LiveStreamComparer () );
							}
						else { LiveStreams.Add ( new LiveStream ( new Uri ( A.Content ) ) ); }

						if ( LiveStreams.Count == 0 && S is Uri )
							{ LiveStreams.Add ( new LiveStream ( (Uri)S ) ); }
						//-------------------------------------------------------------------------------------------------------
						#endregion
						//-------------------------------------------------------------------------------------------------------

						//-------------------------------------------------------------------------------------------------------
						this.DefineQualityButtons ();
						//-------------------------------------------------------------------------------------------------------

						//-------------------------------------------------------------------------------------------------------
						if ( LiveStreams.Count > 0 )
							{
							//---------------------------------------------------------------------------------------------------
							MediaPlayer Player = this.GetPlayer ( true );

							this.LiveMediaControl.Source = this.LiveStreams[SelectedStream].Uri;
							//---------------------------------------------------------------------------------------------------
							}
						//-------------------------------------------------------------------------------------------------------
						else { this.CloseLive ( RestRequestResult.EmptyResponse, true ); }
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					else
						{
						//-------------------------------------------------------------------------------------------------------
						if ( A.Result == RestRequestResult.Success )
							this.CloseLive ( RestRequestResult.EmptyResponse, true );
						else
							this.CloseLive ( A.Result                       , true );
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					} );
				//---------------------------------------------------------------------------------------------------------------
				}, 1000 );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Déclenche l'événement <b>NavigatedFrom</b>.
		/// </summary>
		/// <param name="Sender">Objet source de l'appel.</param>
		/// <param name="Args">
		/// <b>NavigationEventArgs</b> qui contient les données d'événement.
		/// </param>
		//-----------------------------------------------------------------------------------------------------------------------
		protected override void OnNavigatedFrom ( NavigationEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			try { this.LiveMediaControl.Stop (); } catch {}
			
			base.OnNavigatedFrom ( Args );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		#region // Section des Procédures Liées aux Contrôles
		//-----------------------------------------------------------------------------------------------------------------------
		
		//***********************************************************************************************************************
		/// <summary>
		/// Indique l'état actuel de la lecture.
		/// </summary>
		/// <param name="Sender">Objet source de l'appel</param>
		/// <param name="Args"><b>EventArgs</b> qui contient les données d'événement.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void OnMediaCurrentStateChanged ( object Sender, RoutedEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			switch ( this.LiveMediaControl.CurrentState )
				{
				//---------------------------------------------------------------------------------------------------------------
				case MediaElementState.Closed           : 
					{
					//-----------------------------------------------------------------------------------------------------------
					this.IsIndeterminate = false;

					this.PlayLiveAppBarButton .Visibility = Visibility.Visible;
					this.PlayLiveAppBarButton .IsEnabled  = false;
					this.PauseLiveAppBarButton.Visibility = Visibility.Collapsed;            break;
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				case MediaElementState.Opening          : 
				case MediaElementState.Buffering        : 
					{
					//-----------------------------------------------------------------------------------------------------------
					this.IsIndeterminate = false;

					this.PlayLiveAppBarButton .Visibility = Visibility.Visible;
					this.PlayLiveAppBarButton .IsEnabled  = false;
					this.PauseLiveAppBarButton.Visibility = Visibility.Collapsed;            break;
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				case MediaElementState.Playing          : 
					{
					//-----------------------------------------------------------------------------------------------------------
					this.IsIndeterminate = false;

					this.PlayLiveAppBarButton .Visibility = Visibility.Collapsed;
					this.PauseLiveAppBarButton.Visibility = Visibility.Visible;            
					//-----------------------------------------------------------------------------------------------------------

					//-----------------------------------------------------------------------------------------------------------
					break;
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				case MediaElementState.Paused           :
				case MediaElementState.Stopped          : 
					{
					//-----------------------------------------------------------------------------------------------------------
					this.IsIndeterminate = false;

					this.PlayLiveAppBarButton .Visibility = Visibility.Visible;
					this.PlayLiveAppBarButton .IsEnabled  = true;
					this.PauseLiveAppBarButton.Visibility = Visibility.Collapsed;            break;
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				case MediaElementState.Individualizing  : 
				case MediaElementState.AcquiringLicense : 
					{
					//-----------------------------------------------------------------------------------------------------------
					this.IsIndeterminate = false;

					this.PlayLiveAppBarButton .Visibility = Visibility.Visible;
					this.PlayLiveAppBarButton .IsEnabled  = false;
					this.PauseLiveAppBarButton.Visibility = Visibility.Collapsed;            break;
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Indique l'état actuel de la lecture.
		/// </summary>
		/// <param name="Sender">Objet source de l'appel</param>
		/// <param name="Args"><b>EventArgs</b> qui contient les données d'événement.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void OnLayoutTap ( object Sender, System.Windows.Input.GestureEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			Point P = Args.GetPosition ( this.Layout );

			if ( IsOpen && P.Y >= Application.Current.Host.Content.ActualWidth - 100 ) return;

			if ( IsOpen ) this.CloseControls ();
			else          this.OpenControls  ();
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Permet de définir la qualité de lecture.
		/// </summary>
		/// <param name="Sender">Objet source de l'appel</param>
		/// <param name="Args"><b>EventArgs</b> qui contient les données d'événement.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void OnQualityButtonClick ( object Sender, EventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			FrameworkElement Self = Sender as FrameworkElement;

			if ( Self != null )
				{
				//---------------------------------------------------------------------------------------------------------------
				this.SelectedStream = (int)Self.Tag;
				
				if ( NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 )
					StorageSettings.SetValue ( "player-mode-wifi", this.SelectedStream );
				else
					StorageSettings.SetValue ( "player-mode-3g"  , this.SelectedStream );

				this.SelectedStream = Math.Max ( Math.Min ( this.SelectedStream, this.LiveStreams.Count ), 0 );
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				this.QualityButtonL1.Selected = ( Sender == this.QualityButtonL1 );
				this.QualityButtonL2.Selected = ( Sender == this.QualityButtonL2 );
				this.QualityButtonL3.Selected = ( Sender == this.QualityButtonL3 );
				this.QualityButtonL4.Selected = ( Sender == this.QualityButtonL4 );
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				this.Dispatcher.BeginInvoke ( () =>
					{
					//-----------------------------------------------------------------------------------------------------------
					MediaPlayer Player = this.GetPlayer ( false );

					this.LiveMediaControl.Source = this.LiveStreams[SelectedStream].Uri;
					//-----------------------------------------------------------------------------------------------------------
					} );
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Indique l'état actuel de la lecture.
		/// </summary>
		/// <param name="Sender">Objet source de l'appel</param>
		/// <param name="Args"><b>EventArgs</b> qui contient les données d'événement.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void OnPlayButtonClick ( object Sender, EventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			if ( this.PlayLiveAppBarButton.Visibility == Visibility.Visible )
				{
				//---------------------------------------------------------------------------------------------------------------
				try { this.LiveMediaControl.Play (); } catch {}
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Indique l'état actuel de la lecture.
		/// </summary>
		/// <param name="Sender">Objet source de l'appel</param>
		/// <param name="Args"><b>EventArgs</b> qui contient les données d'événement.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void OnPauseButtonClick ( object Sender, EventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			if ( this.PauseLiveAppBarButton.Visibility == Visibility.Visible )
				{
				//---------------------------------------------------------------------------------------------------------------
				try { this.LiveMediaControl.Pause (); } catch {}
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Contrôle la barre de progression.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		private bool IsIndeterminate
			{
			//-------------------------------------------------------------------------------------------------------------------
			#region // Ascesseur Get
			//-------------------------------------------------------------------------------------------------------------------
			get { return ( this.ProgressBar.IsIndeterminate ); }
			//-------------------------------------------------------------------------------------------------------------------
			#endregion
			//-------------------------------------------------------------------------------------------------------------------
			#region // Ascesseur Set
			//-------------------------------------------------------------------------------------------------------------------
			set
				{
				//---------------------------------------------------------------------------------------------------------------
				if ( ! value )
					{
					//-----------------------------------------------------------------------------------------------------------
					this.ProgressControl.Visibility      = Visibility.Collapsed;
					this.ProgressBar    .IsIndeterminate = false;
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				else
					{
					//-----------------------------------------------------------------------------------------------------------
					this.ProgressBar.IsIndeterminate = true;
					this.ProgressControl.Visibility  = Visibility.Visible;
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			#endregion
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		}
	//---------------------------------------------------------------------------------------------------------------------------
	#endregion
	//***************************************************************************************************************************

	//***************************************************************************************************************************
	#region // Contrôle PlayerButton
	//---------------------------------------------------------------------------------------------------------------------------
	public class PlayerButton : System.Windows.Controls.Button
		{
		//***********************************************************************************************************************
		/// <summary>
		/// Initialise une nouvelle instance de l'objet <b>PlayerButton</b>.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public PlayerButton () { base.DefaultStyleKey = typeof (PlayerButton); }
		//***********************************************************************************************************************
		}
	//---------------------------------------------------------------------------------------------------------------------------
	#endregion
	//***************************************************************************************************************************

	//***************************************************************************************************************************
	#region // Contrôle QualityButton
	//---------------------------------------------------------------------------------------------------------------------------
	public class QualityButton : System.Windows.Controls.Button
		{
		//-----------------------------------------------------------------------------------------------------------------------
		// Section des Attributs
		//-----------------------------------------------------------------------------------------------------------------------
		public static readonly DependencyProperty SelectedProperty;
		//-----------------------------------------------------------------------------------------------------------------------
		
		//***********************************************************************************************************************
		/// <summary>
		/// Constructeur statique de l'objet <b>QualityButton</b>.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		static QualityButton ()
			{
			//-------------------------------------------------------------------------------------------------------------------
			SelectedProperty = DependencyProperty.Register ( "Selected", typeof (bool), 
			           typeof (QualityButton), new PropertyMetadata ( false, OnPropertyChanged ) );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Initialise une nouvelle instance de l'objet <b>QualityButton</b>.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public QualityButton () { base.DefaultStyleKey = typeof (QualityButton); }
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Se produit lorsque qu'une des propriétés est modifiée.
		/// </summary>
		/// <param name="Sender">Objet source de l'appel.</param>
		/// <param name="Args">
		/// <b>DependencyPropertyChangedEventArgs</b> qui contient les données d'événement.
		/// </param>
		//-----------------------------------------------------------------------------------------------------------------------
		private static void OnPropertyChanged ( DependencyObject Sender, DependencyPropertyChangedEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			QualityButton Self = Sender as QualityButton;

			if ( Self != null )
				{
				if ( Self.Selected )
					{
					Self.SetValue (BackgroundProperty, new SolidColorBrush ( Colors.White       ));
					Self.SetValue (ForegroundProperty, new SolidColorBrush ( Colors.Black       ));
					}
				else
					{
					Self.SetValue (BackgroundProperty, new SolidColorBrush ( Colors.Transparent ));
					Self.SetValue (ForegroundProperty, new SolidColorBrush ( Colors.White       ));
					}
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Indique si le bouton est sélectionné. 
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public bool Selected
			{
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			get { return (bool)base.GetValue ( SelectedProperty ); }
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			set { base.SetValue ( SelectedProperty, value ); }
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			}
		//***********************************************************************************************************************
		}
	//---------------------------------------------------------------------------------------------------------------------------
	#endregion
	//***************************************************************************************************************************

	} // Fin du namespace "NextRadio"
//*******************************************************************************************************************************

//*******************************************************************************************************************************
// FIN DU FICHIER
//*******************************************************************************************************************************
