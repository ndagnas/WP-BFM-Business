//*******************************************************************************************************************************
// DEBUT DU FICHIER
//*******************************************************************************************************************************

//*******************************************************************************************************************************
// Nom           : CustomMessageBox.cs
// Auteur        : Nicolas Dagnas
// Description   : Implémentation du contrôle CustomMessageBox
// Créé le       : 22/04/2015
// Modifié le    : 06/07/2015
//*******************************************************************************************************************************

//-------------------------------------------------------------------------------------------------------------------------------
#region Using directives
//-------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Navigation;
using System.Windows.Phone.Shell;
using System.Windows.Controls.Primitives;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using Microsoft.Phone.Shell;
using Microsoft.Phone.Controls;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using PI = System.Windows.Phone.Infos;
//-------------------------------------------------------------------------------------------------------------------------------
#endregion
//-------------------------------------------------------------------------------------------------------------------------------

//*******************************************************************************************************************************
// Début du bloc "System.Windows.Phone.Controls"
//*******************************************************************************************************************************
namespace System.Windows.Phone.Controls
	{

	//  #   #  #####   ####   ####   ###    ###    #####         ####    ###   #   #
	//  ## ##  #      #      #      #   #  #   #   #             #   #  #   #   # # 
	//  # # #  ###     ###    ###   #####  #       ###    #####  ####   #   #    #  
	//  #   #  #          #      #  #   #  #   ##  #             #   #  #   #   # # 
	//  #   #  #####  ####   ####   #   #   ### #  #####         ####    ###   #   #

	//***************************************************************************************************************************
	// Classe CustomMessageBox
	//***************************************************************************************************************************
	#region // Déclaration et Implémentation de l'Objet
	//---------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Represents a popup dialog with one or two buttons.
	/// </summary>
	/// <QualityBand>Preview</QualityBand>
	//---------------------------------------------------------------------------------------------------------------------------
	public class CustomMessageBox : ContentControl
		{
		//-----------------------------------------------------------------------------------------------------------------------
		// Section des Attributs Statiques
		//-----------------------------------------------------------------------------------------------------------------------
		private static WeakReference   CurrentInstance = null;
		private static readonly double ScreenWidth     = Application.Current.Host.Content.ActualWidth;
		private static readonly double ScreenHeight    = Application.Current.Host.Content.ActualHeight;
		private static bool            MustRestore     = true;
		//-----------------------------------------------------------------------------------------------------------------------

		//-----------------------------------------------------------------------------------------------------------------------
		// Section des Propriétés Internes
		//-----------------------------------------------------------------------------------------------------------------------
		public static readonly DependencyProperty CaptionProperty;
		public static readonly DependencyProperty IsFullScreenProperty;
		public static readonly DependencyProperty IsLeftButtonEnabledProperty;
		public static readonly DependencyProperty IsRightButtonEnabledProperty;
		public static readonly DependencyProperty LeftButtonContentProperty;
		public static readonly DependencyProperty MessageProperty;
		public static readonly DependencyProperty RightButtonContentProperty;
		public static readonly DependencyProperty TitleProperty;
		//-----------------------------------------------------------------------------------------------------------------------

		//-----------------------------------------------------------------------------------------------------------------------
		// Section des Attributs
		//-----------------------------------------------------------------------------------------------------------------------
		private Grid                  ContentBlock;
		private TextBlock             TitleTextBlock;
		private TextBlock             CaptionTextBlock;
		private TextBlock             MessageTextBlock;
		private Button                LeftButton;
		private Button                RightButton;
		private Popup                 PopupInternal;
		private Grid                  Container;
		private PhoneApplicationFrame AppFrame;
		private IPhoneApplicationPage AppIPage;
		private PhoneApplicationPage  AppPage;
		private bool                  HasApplicationBar;
		private Color                 SystemTrayColor;
		//-----------------------------------------------------------------------------------------------------------------------

		//***********************************************************************************************************************
		#region // Section des Constructeurs
		//-----------------------------------------------------------------------------------------------------------------------

		//***********************************************************************************************************************
		/// <summary>
		/// Constructeur statique de l'objet <b>CustomMessageBox</b>.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		static CustomMessageBox ()
			{
			//-------------------------------------------------------------------------------------------------------------------
			CaptionProperty              = DependencyProperty.Register ( "Caption"             , 
				typeof(string), typeof(CustomMessageBox), new PropertyMetadata ( string.Empty, 
				                                           OnCaptionPropertyChanged            ) );
			//-------------------------------------------------------------------------------------------------------------------
			IsFullScreenProperty         = DependencyProperty.Register ( "IsFullScreen"        , 
				typeof(bool  ), typeof(CustomMessageBox), new PropertyMetadata ( false       , 
				                                           OnIsFullScreenPropertyChanged       ) );
			//-------------------------------------------------------------------------------------------------------------------
			IsLeftButtonEnabledProperty  = DependencyProperty.Register ( "IsLeftButtonEnabled" , 
				typeof(bool  ), typeof(CustomMessageBox), new PropertyMetadata ( true          ) );
			//-------------------------------------------------------------------------------------------------------------------
			IsRightButtonEnabledProperty = DependencyProperty.Register ( "IsRightButtonEnabled", 
				typeof(bool  ), typeof(CustomMessageBox), new PropertyMetadata ( true          ) );
			//-------------------------------------------------------------------------------------------------------------------
			LeftButtonContentProperty    = DependencyProperty.Register ( "LeftButtonContent"   , 
				typeof(Object), typeof(CustomMessageBox), new PropertyMetadata ( null        , 
				                                           OnLeftButtonContentPropertyChanged  ) );
			//-------------------------------------------------------------------------------------------------------------------
			MessageProperty              = DependencyProperty.Register ( "Message"             , 
				typeof(string), typeof(CustomMessageBox), new PropertyMetadata ( string.Empty, 
				                                           OnMessagePropertyChanged            ) );
			//-------------------------------------------------------------------------------------------------------------------
			RightButtonContentProperty   = DependencyProperty.Register ( "RightButtonContent"  , 
				typeof(Object), typeof(CustomMessageBox), new PropertyMetadata ( null        , 
				                                           OnRightButtonContentPropertyChanged ) );
			//-------------------------------------------------------------------------------------------------------------------
			TitleProperty                = DependencyProperty.Register ( "Title"               , 
				typeof(string), typeof(CustomMessageBox), new PropertyMetadata ( string.Empty, 
				                                           OnTitlePropertyChanged              ) );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Initializes a new instance of the CustomMessageBox class.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public CustomMessageBox () : base() { base.DefaultStyleKey = typeof(CustomMessageBox); }
		//***********************************************************************************************************************

		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		#region // Section des Procédures Privées
		//-----------------------------------------------------------------------------------------------------------------------

		//***********************************************************************************************************************
		/// <summary>
		/// Closes the pop up.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		private void ClosePopup ( bool RestoreOriginalValues )
			{
			//-------------------------------------------------------------------------------------------------------------------
			// Remove the popup.
			//-------------------------------------------------------------------------------------------------------------------
			PopupInternal.IsOpen = false;
			PopupInternal        = null;
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			// If there is no other message box displayed.  
			//-------------------------------------------------------------------------------------------------------------------
			if ( RestoreOriginalValues )
				{
				//---------------------------------------------------------------------------------------------------------------
				// Set the system tray back to its original color if necessary.
				//---------------------------------------------------------------------------------------------------------------
				if ( SystemTray.IsVisible ) SystemTray.BackgroundColor = SystemTrayColor;
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				// Bring the application bar if necessary.
				//---------------------------------------------------------------------------------------------------------------
				if ( HasApplicationBar )
					{
					HasApplicationBar                = false;
					AppPage.ApplicationBar.IsVisible = true;
					}                
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			// Dettach event handlers.
			//-------------------------------------------------------------------------------------------------------------------
			if ( AppPage != null )
				{
				//---------------------------------------------------------------------------------------------------------------
				if ( AppIPage != null ) AppIPage.BeginBackKeyPress -= OnBackKeyPress;
				else                    AppPage .BackKeyPress      -= OnBackKeyPress;

				AppPage.OrientationChanged -= OnOrientationChanged;
				AppPage                     = null;                
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			if ( AppFrame != null )
				{
				//---------------------------------------------------------------------------------------------------------------
				AppFrame.Navigating -= OnNavigating;
				AppFrame             = null;
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Called when the visual layout changes.
		/// </summary>
		/// <param name="Sender">The event sender.</param>
		/// <param name="Args">The event information.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void CustomMessageBox_LayoutUpdated ( object Sender, EventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			SwivelTransition BackwardIn = new SwivelTransition { Mode = SwivelTransitionMode.BackwardIn };

			ITransition SwivelTransition = BackwardIn.GetTransition ( this );

			SwivelTransition.Completed += (s1, e1) =>
				{
				SwivelTransition.Stop ();

				if ( this.Opened != null ) this.Opened ( this, EventArgs.Empty );
				};

			SwivelTransition.Begin ();

			LayoutUpdated -= CustomMessageBox_LayoutUpdated;
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Dismisses the message box.
		/// </summary>
		/// <param name="Source">
		/// The source that caused the dismission.
		/// </param>
		/// <param name="UseTransition">
		/// If true, the message box is dismissed after swiveling backward and out.
		/// </param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void Dismiss ( CustomMessageBoxResult Source, bool UseTransition )
			{
			//-------------------------------------------------------------------------------------------------------------------
			// Handle the dismissing event.
			//-------------------------------------------------------------------------------------------------------------------
			var HandlerDismissing = Dismissing;

			if ( HandlerDismissing != null )
				{
				//---------------------------------------------------------------------------------------------------------------
				DismissingEventArgs args = new DismissingEventArgs ( Source );

				HandlerDismissing ( this, args );

				if ( args.Cancel ) return;
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			// Set the current instance to null.
			//-------------------------------------------------------------------------------------------------------------------
			CurrentInstance = null;
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			// Cache this variable to avoid a race condition.
			//-------------------------------------------------------------------------------------------------------------------
			bool RestoreOriginalValues = MustRestore;
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			// Close popup.
			//-------------------------------------------------------------------------------------------------------------------
			if ( UseTransition )
				{
				SwivelTransition BackwardOut = new SwivelTransition { Mode = SwivelTransitionMode.BackwardOut };

				ITransition SwivelTransition = BackwardOut.GetTransition ( this );

				SwivelTransition.Completed += (s, e) =>
					{
					SwivelTransition.Stop ();

					ClosePopup ( RestoreOriginalValues );

					var HandlerDismissed = Dismissed;

					if ( HandlerDismissed != null )
						HandlerDismissed ( this, new DismissedEventArgs ( Source ) );
					};

				SwivelTransition.Begin();
				}
			else { ClosePopup ( RestoreOriginalValues ); }
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Gets the current page orientation.
		/// </summary>
		/// <returns>The current page orientation.</returns>
		//-----------------------------------------------------------------------------------------------------------------------
		private static PageOrientation GetPageOrientation ()
			{
			//-------------------------------------------------------------------------------------------------------------------
			PhoneApplicationFrame Frame = Application.Current.RootVisual as PhoneApplicationFrame;
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			if ( Frame != null )
				{
				PhoneApplicationPage Page = Frame.Content as PhoneApplicationPage;

				if ( Page != null ) return Page.Orientation;
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			return PageOrientation.None;
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Gets a transform for popup elements based
		/// on the current page orientation.
		/// </summary>
		/// <returns>
		/// A composite transform determined by page orientation.
		/// </returns>
		//-----------------------------------------------------------------------------------------------------------------------
		private static Transform GetTransform ()
			{
			//-------------------------------------------------------------------------------------------------------------------
			switch ( GetPageOrientation () )
				{
				//---------------------------------------------------------------------------------------------------------------
				case PageOrientation.LandscapeLeft  :
				case PageOrientation.Landscape      :
					return new CompositeTransform () { Rotation =  90, TranslateX = ScreenWidth  };
				//---------------------------------------------------------------------------------------------------------------
				case PageOrientation.LandscapeRight :
					return new CompositeTransform () { Rotation = -90, TranslateY = ScreenHeight };
				default: return null;
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Gets a rectangle that occupies the entire page.
		/// </summary>
		/// <returns>The width, height and location of the rectangle.</returns>
		//-----------------------------------------------------------------------------------------------------------------------
		private static Rect GetTransformedRect ()
			{
			//-------------------------------------------------------------------------------------------------------------------
			bool LandscapeMode = IsLandscape ( GetPageOrientation () );

			return new Rect ( 0, 0, LandscapeMode ? ScreenHeight : ScreenWidth,
			                        LandscapeMode ? ScreenWidth  : ScreenHeight );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Returns a visibility value based on the value of an object.
		/// </summary>
		/// <param name="Obj">The object.</param>
		/// <returns>
		/// Visibility.Collapsed if the object is null. Visibility.Visible otherwise.
		/// </returns>
		//-----------------------------------------------------------------------------------------------------------------------
		private static Visibility GetVisibilityFromObject ( Object Obj )
			{
			//-------------------------------------------------------------------------------------------------------------------
			return ( Obj == null ) ? Visibility.Collapsed : Visibility.Visible;
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Returns a visibility value based on the value of a string.
		/// </summary>
		/// <param name="Str">The string.</param>
		/// <returns>
		/// Visibility.Collapsed if the string is null or empty. Visibility.Visible otherwise.
		/// </returns>
		//-----------------------------------------------------------------------------------------------------------------------
		private static Visibility GetVisibilityFromString ( string Str )
			{
			//-------------------------------------------------------------------------------------------------------------------
			return ( string.IsNullOrEmpty ( Str ) ) ? Visibility.Collapsed : Visibility.Visible;
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Determines whether the orientation is landscape.
		/// </summary>
		/// <param name="Orientation">The orientation.</param>
		/// <returns>True if the orientation is landscape.</returns>
		//-----------------------------------------------------------------------------------------------------------------------
		private static bool IsLandscape ( PageOrientation Orientation )
			{
			//-------------------------------------------------------------------------------------------------------------------
			return (Orientation == PageOrientation.Landscape     ) || 
			       (Orientation == PageOrientation.LandscapeLeft ) || 
				   (Orientation == PageOrientation.LandscapeRight);
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Called when the back key is pressed. This event handler cancels
		/// the backward navigation and dismisses the message box.
		/// </summary>
		/// <param name="Sender">The event sender.</param>
		/// <param name="Args">The event information.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void OnBackKeyPress ( object Sender, CancelEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			Args.Cancel = true;

			Dismiss ( CustomMessageBoxResult.None, true );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Changes the visibility of the caption text block based on its content.
		/// </summary>
		/// <param name="Sender">The dependency object.</param>
		/// <param name="Args">The event information.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private static void OnCaptionPropertyChanged ( DependencyObject Sender, DependencyPropertyChangedEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			CustomMessageBox Target = (CustomMessageBox)Sender;

			if ( Target.CaptionTextBlock != null )
				{
				string Value = (string)Args.NewValue;

				Target.CaptionTextBlock.Visibility = GetVisibilityFromString ( Value );
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Modifies the vertical alignment of the message box depending
		/// on whether it should occupy the full screen or not.
		/// </summary>
		/// <param name="Sender">The dependency object.</param>
		/// <param name="Args">The event information.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private static void OnIsFullScreenPropertyChanged ( DependencyObject Sender, DependencyPropertyChangedEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			CustomMessageBox Target = (CustomMessageBox)Sender;

			if ( (bool)Args.NewValue ) Target.VerticalAlignment = VerticalAlignment.Stretch;
			else                       Target.VerticalAlignment = VerticalAlignment.Top;
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Changes the visibility of the left button based on its content.
		/// </summary>
		/// <param name="Sender">The dependency object.</param>
		/// <param name="Args">The event information.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private static void OnLeftButtonContentPropertyChanged ( DependencyObject Sender, DependencyPropertyChangedEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			CustomMessageBox Target = (CustomMessageBox)Sender;

			if ( Target.LeftButton != null )
				Target.LeftButton.Visibility = GetVisibilityFromObject ( Args.NewValue );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Changes the visibility of the message text block based on its content.
		/// </summary>
		/// <param name="Sender">The dependency object.</param>
		/// <param name="Args">The event information.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private static void OnMessagePropertyChanged ( DependencyObject Sender, DependencyPropertyChangedEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			CustomMessageBox Target = (CustomMessageBox)Sender;

			if ( Target.MessageTextBlock != null )
				{
				string Value = (string)Args.NewValue;

				Target.MessageTextBlock.Visibility = GetVisibilityFromString ( Value );
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Called when the application frame is navigating.
		/// This event handler dismisses the message box.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="Args">The event information.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void OnNavigating ( object sender, NavigatingCancelEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			Dismiss ( CustomMessageBoxResult.None, false );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Called when the current page changes orientation.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="Args">The event information.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void OnOrientationChanged ( object sender, OrientationChangedEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			SetSizeAndOffset ();
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Changes the visibility of the right button based on its content.
		/// </summary>
		/// <param name="Sender">The dependency object.</param>
		/// <param name="Args">The event information.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private static void OnRightButtonContentPropertyChanged ( DependencyObject Sender, DependencyPropertyChangedEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			CustomMessageBox Target = (CustomMessageBox)Sender;

			if ( Target.RightButton != null )
				Target.RightButton.Visibility = GetVisibilityFromObject ( Args.NewValue );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Changes the visibility of the title text block based on its content.
		/// </summary>
		/// <param name="Sender">The dependency object.</param>
		/// <param name="Args">The event information.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private static void OnTitlePropertyChanged ( DependencyObject Sender, DependencyPropertyChangedEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			CustomMessageBox Target = (CustomMessageBox)Sender;

			if ( Target.TitleTextBlock != null )
				{
				string Value = (string)Args.NewValue;

				Target.TitleTextBlock.Visibility = GetVisibilityFromString ( Value );
				}           
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Sets The vertical and horizontal offset of the popup,
		/// as well as the size of its child container.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		private void SetSizeAndOffset ()
			{
			//-------------------------------------------------------------------------------------------------------------------
			// Set the size the container.
			//-------------------------------------------------------------------------------------------------------------------
			Rect Client = GetTransformedRect ();

			if ( Container != null )
				{
				//---------------------------------------------------------------------------------------------------------------
				Container.RenderTransform = GetTransform ();

				Container.Width  = Client.Width;
				Container.Height = Client.Height;
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			if ( ContentBlock != null )
				{
				//---------------------------------------------------------------------------------------------------------------
				switch ( PI.DeviceInfos.Orientation )
					{
					case Infos.PhoneOrientation.LandscapeLeft :
						{
						ContentBlock.Margin = new Thickness ( PI.DeviceInfos.ApplicationBarHeight, 24, 24, 24 );

						break;
						}
					case Infos.PhoneOrientation.LandscapeRight :
						{
						ContentBlock.Margin = new Thickness ( 24, 24, PI.DeviceInfos.ApplicationBarHeight, 24 );

						break;
						}
					default : { ContentBlock.Margin = new Thickness ( 24 ); break; }
					}
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		#region // Section des Procédures Publiques & Dérivées
		//-----------------------------------------------------------------------------------------------------------------------
		
		//***********************************************************************************************************************
		/// <summary>
		/// Dismisses the message box.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public void Dismiss () { Dismiss ( CustomMessageBoxResult.None, true ); }
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Gets the template parts and attaches event handlers.
		/// Animates the message box when the template is applied to it.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public override void OnApplyTemplate()
			{
			//-------------------------------------------------------------------------------------------------------------------
			if ( LeftButton  != null ) LeftButton .Click -= LeftButton_Click;
			if ( RightButton != null ) RightButton.Click -= RightButton_Click;
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			base.OnApplyTemplate ();
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			ContentBlock     = base.GetTemplateChild ( "ContentBlock"     ) as Grid;
			TitleTextBlock   = base.GetTemplateChild ( "TitleTextBlock"   ) as TextBlock;
			CaptionTextBlock = base.GetTemplateChild ( "CaptionTextBlock" ) as TextBlock;
			MessageTextBlock = base.GetTemplateChild ( "MessageTextBlock" ) as TextBlock;
			LeftButton       = base.GetTemplateChild ( "LeftButton"       ) as Button;
			RightButton      = base.GetTemplateChild ( "RightButton"      ) as Button;
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			double TitleTextFontSize   = 42.667;
			double CaptionTextFontSize = 32.000;
			double MessageTextFontSize = 22.667;
			double LeftButtonFontSize  = 25.333;
			double RightButtonFontSize = 25.333;
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			if ( PI.DeviceInfos.DeviceType == Infos.DeviceType.Phablet )
				{
				//---------------------------------------------------------------------------------------------------------------
				TitleTextFontSize   = 32.000;
				CaptionTextFontSize = 25.333;
				MessageTextFontSize = 18.667;
				LeftButtonFontSize  = 20.000;
				RightButtonFontSize = 20.000;
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			if ( ContentBlock != null )
				{
				//---------------------------------------------------------------------------------------------------------------
				switch ( PI.DeviceInfos.Orientation )
					{
					case Infos.PhoneOrientation.LandscapeLeft :
						{
						ContentBlock.Margin = new Thickness ( PI.DeviceInfos.ApplicationBarHeight, 24, 24, 24 );

						break;
						}
					case Infos.PhoneOrientation.LandscapeRight :
						{
						ContentBlock.Margin = new Thickness ( 24, 24, PI.DeviceInfos.ApplicationBarHeight, 24 );

						break;
						}
					default : { ContentBlock.Margin = new Thickness ( 24 ); break; }
					}
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			if ( TitleTextBlock != null )
				{
				//---------------------------------------------------------------------------------------------------------------
				TitleTextBlock.FontSize   = TitleTextFontSize;
				TitleTextBlock.Visibility = GetVisibilityFromString(Title);
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			if ( CaptionTextBlock != null )
				{
				//---------------------------------------------------------------------------------------------------------------
				CaptionTextBlock.FontSize   = CaptionTextFontSize;
				CaptionTextBlock.Visibility = GetVisibilityFromString(Caption);
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			if ( MessageTextBlock != null )
				{
				//---------------------------------------------------------------------------------------------------------------
				MessageTextBlock.FontSize   = MessageTextFontSize;
				MessageTextBlock.Visibility = GetVisibilityFromString(Message);
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			if ( LeftButton != null )
				{
				//---------------------------------------------------------------------------------------------------------------
				LeftButton.FontSize   = LeftButtonFontSize;
				LeftButton.Click     += LeftButton_Click;
				LeftButton.Visibility = GetVisibilityFromObject(LeftButtonContent);
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			
			//-------------------------------------------------------------------------------------------------------------------
			if ( RightButton != null )
				{
				//---------------------------------------------------------------------------------------------------------------
				RightButton.FontSize   = RightButtonFontSize;
				RightButton.Click     += RightButton_Click;
				RightButton.Visibility = GetVisibilityFromObject(RightButtonContent);
				//---------------------------------------------------------------------------------------------------------------
				}            
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Reveals the message box by inserting it into a popup and opening it.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public void Show()
			{
			//--------------------------------------------------------------------------------------------------------------------
			if ( PopupInternal != null && PopupInternal.IsOpen ) return;
			//--------------------------------------------------------------------------------------------------------------------

			//--------------------------------------------------------------------------------------------------------------------
			LayoutUpdated += CustomMessageBox_LayoutUpdated;

			AppFrame = Application.Current.RootVisual as PhoneApplicationFrame;
			AppIPage = AppFrame.Content               as IPhoneApplicationPage;
			AppPage  = AppFrame.Content               as PhoneApplicationPage;
			//--------------------------------------------------------------------------------------------------------------------
		 
			//--------------------------------------------------------------------------------------------------------------------
			// Change the color of the system tray if necessary.
			//--------------------------------------------------------------------------------------------------------------------
			if ( SystemTray.IsVisible )
				{
				//----------------------------------------------------------------------------------------------------------------
				// Cache the original color of the system tray.
				//----------------------------------------------------------------------------------------------------------------
				SystemTrayColor = SystemTray.BackgroundColor;
				//----------------------------------------------------------------------------------------------------------------

				//----------------------------------------------------------------------------------------------------------------
				// Change the color of the system tray to match the message box.
				//----------------------------------------------------------------------------------------------------------------
				if ( Background is SolidColorBrush )
					SystemTray.BackgroundColor = ((SolidColorBrush)Background).Color;
				//----------------------------------------------------------------------------------------------------------------
				else
					SystemTray.BackgroundColor = (Color)Application.Current.Resources["PhoneChromeColor"];
				//----------------------------------------------------------------------------------------------------------------
				}
			//--------------------------------------------------------------------------------------------------------------------

			//--------------------------------------------------------------------------------------------------------------------
			// Hide the application bar if necessary.
			//--------------------------------------------------------------------------------------------------------------------
			if ( AppPage.ApplicationBar != null )
				{
				//----------------------------------------------------------------------------------------------------------------
				// Cache the original visibility of the system tray.
				//----------------------------------------------------------------------------------------------------------------
				HasApplicationBar = AppPage.ApplicationBar.IsVisible;

				//----------------------------------------------------------------------------------------------------------------
				// Hide it.
				if ( HasApplicationBar ) AppPage.ApplicationBar.IsVisible = false;
				//----------------------------------------------------------------------------------------------------------------
				}
			//--------------------------------------------------------------------------------------------------------------------
			else { HasApplicationBar = false; }
			//--------------------------------------------------------------------------------------------------------------------

			//--------------------------------------------------------------------------------------------------------------------
			// Dismiss the current message box if there is any.
			//--------------------------------------------------------------------------------------------------------------------
			if ( CurrentInstance != null )
				{
				//----------------------------------------------------------------------------------------------------------------
				MustRestore = false;

				CustomMessageBox Target = CurrentInstance.Target as CustomMessageBox;

				if ( Target != null )
					{
					SystemTrayColor   = Target.SystemTrayColor;
					HasApplicationBar = Target.HasApplicationBar;

					Target.Dismiss();
					}
				//----------------------------------------------------------------------------------------------------------------
				}
			//--------------------------------------------------------------------------------------------------------------------

			//--------------------------------------------------------------------------------------------------------------------
			MustRestore = true;
			//--------------------------------------------------------------------------------------------------------------------

			//--------------------------------------------------------------------------------------------------------------------
			// Insert the overlay.
			//--------------------------------------------------------------------------------------------------------------------
			Rectangle Overlay = new Rectangle ();

			Color backgroundColor = (Color)Application.Current.Resources["PhoneBackgroundColor"];

			Overlay.Fill = new SolidColorBrush ( Color.FromArgb ( 0x99, backgroundColor.R, 
			                                                            backgroundColor.G, 
																		backgroundColor.B ) );
			Container = new Grid ();

			Container.Children.Add ( Overlay );

			// Insert the message box.

			Container.Children.Add ( this );

			// Create and open the popup.

			PopupInternal = new Popup ();

			PopupInternal.Child = Container;

			SetSizeAndOffset ();

			PopupInternal.IsOpen = true;

			CurrentInstance = new WeakReference ( this );
			//--------------------------------------------------------------------------------------------------------------------

			//--------------------------------------------------------------------------------------------------------------------
			// Attach event handlers.
			//--------------------------------------------------------------------------------------------------------------------
			if ( AppPage != null )
				{
				//----------------------------------------------------------------------------------------------------------------
				if ( AppIPage != null ) AppIPage.BeginBackKeyPress += OnBackKeyPress;
				else                    AppPage .BackKeyPress      += OnBackKeyPress;

				AppPage.OrientationChanged += OnOrientationChanged;               
				//----------------------------------------------------------------------------------------------------------------
				}
			//--------------------------------------------------------------------------------------------------------------------

			//--------------------------------------------------------------------------------------------------------------------
			if (AppFrame != null) AppFrame.Navigating += OnNavigating;
			//--------------------------------------------------------------------------------------------------------------------
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
		/// Dismisses the message box with the left button.
		/// </summary>
		/// <param name="Sender">The event sender.</param>
		/// <param name="Args">The event information.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void LeftButton_Click ( object Sender, RoutedEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			Dismiss ( CustomMessageBoxResult.LeftButton, true );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Dismisses the message box with the right button.
		/// </summary>
		/// <param name="Sender">The event sender.</param>
		/// <param name="Args">The event information.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void RightButton_Click ( object Sender, RoutedEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			Dismiss ( CustomMessageBoxResult.RightButton, true );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Gets or sets the caption.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public string Caption
			{
			//-------------------------------------------------------------------------------------------------------------------
			get { return (string)GetValue ( CaptionProperty        ); }
			set {                SetValue ( CaptionProperty, value ); }
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Called when the message box is dismissed.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public event EventHandler<DismissedEventArgs> Dismissed;
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Called when the message is being dismissing.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public event EventHandler<DismissingEventArgs> Dismissing;
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Called when the message is being opened.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public event EventHandler Opened;
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Gets or sets whether the message box occupies the whole screen.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public bool IsFullScreen
			{
			//-------------------------------------------------------------------------------------------------------------------
			get { return (bool)GetValue ( IsFullScreenProperty        ); }
			set {              SetValue ( IsFullScreenProperty, value ); }
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Gets or sets whether the left button is enabled.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public bool IsLeftButtonEnabled
			{
			//-------------------------------------------------------------------------------------------------------------------
			get { return (bool)GetValue ( IsLeftButtonEnabledProperty        ); }
			set {              SetValue ( IsLeftButtonEnabledProperty, value ); }
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Gets or sets whether the right button is enabled.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public bool IsRightButtonEnabled
			{
			//-------------------------------------------------------------------------------------------------------------------
			get { return (bool)GetValue ( IsRightButtonEnabledProperty        ); }
			set {              SetValue ( IsRightButtonEnabledProperty, value ); }
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Gets or sets the left button content.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public Object LeftButtonContent
			{
			//-------------------------------------------------------------------------------------------------------------------
			get { return (Object)GetValue ( LeftButtonContentProperty        ); }
			set {                SetValue ( LeftButtonContentProperty, value ); }
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public string Message
			{
			//-------------------------------------------------------------------------------------------------------------------
			get { return (string)GetValue ( MessageProperty        ); }
			set {                SetValue ( MessageProperty, value ); }
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Gets or sets the right button content.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public Object RightButtonContent
			{
			//-------------------------------------------------------------------------------------------------------------------
			get { return (Object)GetValue ( RightButtonContentProperty        ); }
			set {                SetValue ( RightButtonContentProperty, value ); }
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public string Title
			{
			//-------------------------------------------------------------------------------------------------------------------
			get { return (string)GetValue ( TitleProperty        ); }
			set {                SetValue ( TitleProperty, value ); }
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		}
	//---------------------------------------------------------------------------------------------------------------------------
	#endregion
	//***************************************************************************************************************************

	} // Fin du namespace "System.Windows.Phone.Controls"
//*******************************************************************************************************************************

//*******************************************************************************************************************************
// FIN DU FICHIER
//*******************************************************************************************************************************
