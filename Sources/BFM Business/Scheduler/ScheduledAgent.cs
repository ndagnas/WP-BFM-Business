//*******************************************************************************************************************************
// DEBUT DU FICHIER
//*******************************************************************************************************************************

//*******************************************************************************************************************************
// Nom           : ScheduledAgent.cs
// Auteur        : Nicolas Dagnas
// Description   : Implémentation de l'objet ScheduledAgent
// Créé le       : 29/06/2015
// Modifié le    : 23/07/2015
//*******************************************************************************************************************************

//-------------------------------------------------------------------------------------------------------------------------------
#region Using directives
//-------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Windows.Media;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using Microsoft.Phone.Shell;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Net.NetworkInformation;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using Newtonsoft.Json.Linq;
//-------------------------------------------------------------------------------------------------------------------------------
#endregion
//-------------------------------------------------------------------------------------------------------------------------------

//*******************************************************************************************************************************
// Début du bloc "NextRadio.Scheduler"
//*******************************************************************************************************************************
namespace NextRadio.Scheduler
	{

	//   ####   ###   #   #  #####  ####   #   #  #      #####  #### 
	//  #      #   #  #   #  #      #   #  #   #  #      #      #   #
	//   ###   #      #####  ###    #   #  #   #  #      ###    #   #
	//      #  #   #  #   #  #      #   #  #   #  #      #      #   #
	//  ####    ###   #   #  #####  ####    ###   #####  #####  #### 

	//***************************************************************************************************************************
	// Classe ScheduledAgent
	//***************************************************************************************************************************
	#region // Déclaration et Implémentation de l'Objet
	//---------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Définit le comportement de l'agent.
	/// </summary>
	//---------------------------------------------------------------------------------------------------------------------------
	public class ScheduledAgent : ScheduledTaskAgent
		{
		//***********************************************************************************************************************
		#region // Objet FlipTileOptions
		//-----------------------------------------------------------------------------------------------------------------------
		private class FlipTileOptions
			{
			//*******************************************************************************************************************
			/// <summary>
			/// Obtiens la configuration de la tuile celon son type.
			/// </summary>
			/// <param name="Section"></param>
			/// <returns></returns>
			//-------------------------------------------------------------------------------------------------------------------
			public static FlipTileOptions FromTile ( ShellTile Tile )
				{
				//---------------------------------------------------------------------------------------------------------------
				return FlipTileOptions.FromUri ( Tile.NavigationUri );
				//---------------------------------------------------------------------------------------------------------------
				}
			//*******************************************************************************************************************

			//*******************************************************************************************************************
			/// <summary>
			/// Obtiens la configuration de la tuile celon son type.
			/// </summary>
			/// <param name="Section"></param>
			/// <returns></returns>
			//-------------------------------------------------------------------------------------------------------------------
			public static FlipTileOptions FromUri ( Uri Uri )
				{
				//---------------------------------------------------------------------------------------------------------------
				switch ( Uri.OriginalString )
					{
					//-----------------------------------------------------------------------------------------------------------
					#region // All
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=All" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = string.Empty                         ,
							BackTitle               = "BFM Business"                       ,
							Uri                     = string.Empty                         ,
							SmallBackgroundImage    = "Assets/Tiles/All/FlipTileSmall.png" ,
							BackgroundImage         = "Assets/Tiles/All/FlipTileMedium.png",
							WideBackgroundImage     = "Assets/Tiles/All/FlipTileWide.png"  ,
							BackBackgroundImage     = "Assets/Tiles/All/BackTileMedium.png",
							WideBackBackgroundImage = "Assets/Tiles/All/BackTileWide.png"  ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					#region // Bookmarks
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=Bookmarks" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "Mes favoris"                              ,
							BackTitle               = "Mes favoris"                              ,
							Uri                     = string.Empty                               ,
							SmallBackgroundImage    = "Assets/Tiles/Bookmarks/FlipTileSmall.png" ,
							BackgroundImage         = "Assets/Tiles/Bookmarks/FlipTileMedium.png",
							WideBackgroundImage     = "Assets/Tiles/Bookmarks/FlipTileWide.png"  ,
							BackBackgroundImage     = "Assets/Tiles/Bookmarks/BackTileMedium.png",
							WideBackBackgroundImage = "Assets/Tiles/Bookmarks/BackTileWide.png"  ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------

					//-----------------------------------------------------------------------------------------------------------
					#region // Banking
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=Banking" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "Banque"                                         ,
							BackTitle               = "Banque"                                         ,
							Uri                     = "getArticlesList?category=468726&count=40&page=1",
							SmallBackgroundImage    = "Assets/Tiles/Banking/FlipTileSmall.png"         ,
							BackgroundImage         = "Assets/Tiles/Banking/FlipTileMedium.png"        ,
							WideBackgroundImage     = "Assets/Tiles/Banking/FlipTileWide.png"          ,
							BackBackgroundImage     = "Assets/Tiles/Banking/BackTileMedium.png"        ,
							WideBackBackgroundImage = "Assets/Tiles/Banking/BackTileWide.png"          ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					#region // Construction
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=Construction" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "Immobilier"                                     ,
							BackTitle               = "Immobilier"                                     ,
							Uri                     = "getArticlesList?category=468725&count=40&page=1",
							SmallBackgroundImage    = "Assets/Tiles/Construction/FlipTileSmall.png"    ,
							BackgroundImage         = "Assets/Tiles/Construction/FlipTileMedium.png"   ,
							WideBackgroundImage     = "Assets/Tiles/Construction/FlipTileWide.png"     ,
							BackBackgroundImage     = "Assets/Tiles/Construction/BackTileMedium.png"   ,
							WideBackBackgroundImage = "Assets/Tiles/Construction/BackTileWide.png"     ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					#region // Culture
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=Culture" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "Culture"                                        ,
							BackTitle               = "Culture"                                        ,
							Uri                     = "getArticlesList?category=468727&count=40&page=1",
							SmallBackgroundImage    = "Assets/Tiles/Culture/FlipTileSmall.png"         ,
							BackgroundImage         = "Assets/Tiles/Culture/FlipTileMedium.png"        ,
							WideBackgroundImage     = "Assets/Tiles/Culture/FlipTileWide.png"          ,
							BackBackgroundImage     = "Assets/Tiles/Culture/BackTileMedium.png"        ,
							WideBackBackgroundImage = "Assets/Tiles/Culture/BackTileWide.png"          ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					#region // Distribution
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=Distribution" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "Distribution"                                   ,
							BackTitle               = "Distribution"                                   ,
							Uri                     = "getArticlesList?category=468730&count=40&page=1",
							SmallBackgroundImage    = "Assets/Tiles/Distribution/FlipTileSmall.png"    ,
							BackgroundImage         = "Assets/Tiles/Distribution/FlipTileMedium.png"   ,
							WideBackgroundImage     = "Assets/Tiles/Distribution/FlipTileWide.png"     ,
							BackBackgroundImage     = "Assets/Tiles/Distribution/BackTileMedium.png"   ,
							WideBackBackgroundImage = "Assets/Tiles/Distribution/BackTileWide.png"     ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					#region // Employment
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=Employment" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "Emploi"                                         ,
							BackTitle               = "Emploi"                                         ,
							Uri                     = "getArticlesList?category=468722&count=40&page=1",
							SmallBackgroundImage    = "Assets/Tiles/Employment/FlipTileSmall.png"      ,
							BackgroundImage         = "Assets/Tiles/Employment/FlipTileMedium.png"     ,
							WideBackgroundImage     = "Assets/Tiles/Employment/FlipTileWide.png"       ,
							BackBackgroundImage     = "Assets/Tiles/Employment/BackTileMedium.png"     ,
							WideBackBackgroundImage = "Assets/Tiles/Employment/BackTileWide.png"       ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					#region // Energy
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=Energy" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "Energie"                                        ,
							BackTitle               = "Energie"                                        ,
							Uri                     = "getArticlesList?category=468729&count=40&page=1",
							SmallBackgroundImage    = "Assets/Tiles/Energy/FlipTileSmall.png"          ,
							BackgroundImage         = "Assets/Tiles/Energy/FlipTileMedium.png"         ,
							WideBackgroundImage     = "Assets/Tiles/Energy/FlipTileWide.png"           ,
							BackBackgroundImage     = "Assets/Tiles/Energy/BackTileMedium.png"         ,
							WideBackBackgroundImage = "Assets/Tiles/Energy/BackTileWide.png"           ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					#region // France
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=France" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "France"                                         ,
							BackTitle               = "France"                                         ,
							Uri                     = "getArticlesList?category=468720&count=40&page=1",
							SmallBackgroundImage    = "Assets/Tiles/France/FlipTileSmall.png"          ,
							BackgroundImage         = "Assets/Tiles/France/FlipTileMedium.png"         ,
							WideBackgroundImage     = "Assets/Tiles/France/FlipTileWide.png"           ,
							BackBackgroundImage     = "Assets/Tiles/France/BackTileMedium.png"         ,
							WideBackBackgroundImage = "Assets/Tiles/France/BackTileWide.png"           ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					#region // Industry
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=Industry" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "Industrie"                                      ,
							BackTitle               = "Industrie"                                      ,
							Uri                     = "getArticlesList?category=468733&count=40&page=1",
							SmallBackgroundImage    = "Assets/Tiles/Industry/FlipTileSmall.png"        ,
							BackgroundImage         = "Assets/Tiles/Industry/FlipTileMedium.png"       ,
							WideBackgroundImage     = "Assets/Tiles/Industry/FlipTileWide.png"         ,
							BackBackgroundImage     = "Assets/Tiles/Industry/BackTileMedium.png"       ,
							WideBackBackgroundImage = "Assets/Tiles/Industry/BackTileWide.png"         ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					#region // Innovation
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=Innovation" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "Innovation"                                     ,
							BackTitle               = "Innovation"                                     ,
							Uri                     = "getArticlesList?category=468724&count=40&page=1",
							SmallBackgroundImage    = "Assets/Tiles/Innovation/FlipTileSmall.png"      ,
							BackgroundImage         = "Assets/Tiles/Innovation/FlipTileMedium.png"     ,
							WideBackgroundImage     = "Assets/Tiles/Innovation/FlipTileWide.png"       ,
							BackBackgroundImage     = "Assets/Tiles/Innovation/BackTileMedium.png"     ,
							WideBackBackgroundImage = "Assets/Tiles/Innovation/BackTileWide.png"       ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					#region // Observatory
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=Observatory" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "L'Observatoire"                                 ,
							BackTitle               = "L'Observatoire"                                 ,
							Uri                     = "getArticlesList?category=468734&count=40&page=1",
							SmallBackgroundImage    = "Assets/Tiles/Observatory/FlipTileSmall.png"     ,
							BackgroundImage         = "Assets/Tiles/Observatory/FlipTileMedium.png"    ,
							WideBackgroundImage     = "Assets/Tiles/Observatory/FlipTileWide.png"      ,
							BackBackgroundImage     = "Assets/Tiles/Observatory/BackTileMedium.png"    ,
							WideBackBackgroundImage = "Assets/Tiles/Observatory/BackTileWide.png"      ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					#region // Sport
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=Sport" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "Sport"                                          ,
							BackTitle               = "Sport"                                          ,
							Uri                     = "getArticlesList?category=468728&count=40&page=1",
							SmallBackgroundImage    = "Assets/Tiles/Sport/FlipTileSmall.png"           ,
							BackgroundImage         = "Assets/Tiles/Sport/FlipTileMedium.png"          ,
							WideBackgroundImage     = "Assets/Tiles/Sport/FlipTileWide.png"            ,
							BackBackgroundImage     = "Assets/Tiles/Sport/BackTileMedium.png"          ,
							WideBackBackgroundImage = "Assets/Tiles/Sport/BackTileWide.png"            ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					#region // Tourism
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=Tourism" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "Tourisme"                                       ,
							BackTitle               = "Tourisme"                                       ,
							Uri                     = "getArticlesList?category=468732&count=40&page=1",
							SmallBackgroundImage    = "Assets/Tiles/Tourism/FlipTileSmall.png"         ,
							BackgroundImage         = "Assets/Tiles/Tourism/FlipTileMedium.png"        ,
							WideBackgroundImage     = "Assets/Tiles/Tourism/FlipTileWide.png"          ,
							BackBackgroundImage     = "Assets/Tiles/Tourism/BackTileMedium.png"        ,
							WideBackBackgroundImage = "Assets/Tiles/Tourism/BackTileWide.png"          ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					#region // Transport
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=Transport" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "Transport"                                      ,
							BackTitle               = "Transport"                                      ,
							Uri                     = "getArticlesList?category=468731&count=40&page=1",
							SmallBackgroundImage    = "Assets/Tiles/Transport/FlipTileSmall.png"       ,
							BackgroundImage         = "Assets/Tiles/Transport/FlipTileMedium.png"      ,
							WideBackgroundImage     = "Assets/Tiles/Transport/FlipTileWide.png"        ,
							BackBackgroundImage     = "Assets/Tiles/Transport/BackTileMedium.png"      ,
							WideBackBackgroundImage = "Assets/Tiles/Transport/BackTileWide.png"        ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					#region // World
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=World" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "Monde"                                          ,
							BackTitle               = "Monde"                                          ,
							Uri                     = "getArticlesList?category=468721&count=40&page=1",
							SmallBackgroundImage    = "Assets/Tiles/World/FlipTileSmall.png"           ,
							BackgroundImage         = "Assets/Tiles/World/FlipTileMedium.png"          ,
							WideBackgroundImage     = "Assets/Tiles/World/FlipTileWide.png"            ,
							BackBackgroundImage     = "Assets/Tiles/World/BackTileMedium.png"          ,
							WideBackBackgroundImage = "Assets/Tiles/World/BackTileWide.png"            ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					#region // YourMoney
					//-----------------------------------------------------------------------------------------------------------
					case "/Frames/Frm_Home.xaml?Section=YourMoney" :
						{
						//-------------------------------------------------------------------------------------------------------
						return new FlipTileOptions ()
							{
							//---------------------------------------------------------------------------------------------------
							Title                   = "Votre argent"                                   ,
							BackTitle               = "Votre argent"                                   ,
							Uri                     = "getArticlesList?category=468723&count=40&page=1",
							SmallBackgroundImage    = "Assets/Tiles/YourMoney/FlipTileSmall.png"       ,
							BackgroundImage         = "Assets/Tiles/YourMoney/FlipTileMedium.png"      ,
							WideBackgroundImage     = "Assets/Tiles/YourMoney/FlipTileWide.png"        ,
							BackBackgroundImage     = "Assets/Tiles/YourMoney/BackTileMedium.png"      ,
							WideBackBackgroundImage = "Assets/Tiles/YourMoney/BackTileWide.png"        ,
							//---------------------------------------------------------------------------------------------------
							};
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				
				//---------------------------------------------------------------------------------------------------------------
				return new FlipTileOptions ()
					{
					//-----------------------------------------------------------------------------------------------------------
					Title                   = string.Empty                     ,
					BackTitle               = string.Empty                     ,
					Uri                     = string.Empty                     ,
					SmallBackgroundImage    = "Assets/Tiles/FlipTileSmall.png" ,
					BackgroundImage         = "Assets/Tiles/FlipTileMedium.png",
					WideBackgroundImage     = "Assets/Tiles/FlipTileWide.png"  ,
					BackBackgroundImage     = "Assets/Tiles/BackTileMedium.png",
					WideBackBackgroundImage = "Assets/Tiles/BackTileWide.png"  ,
					//-----------------------------------------------------------------------------------------------------------
					};
				//---------------------------------------------------------------------------------------------------------------
				}
			//*******************************************************************************************************************

			//-------------------------------------------------------------------------------------------------------------------
			public string Title                   { get; private set; }
			public string BackTitle               { get; private set; }
			public string Uri                     { get; private set; }
			public string SmallBackgroundImage    { get; private set; }
			public string BackgroundImage         { get; private set; }
			public string WideBackgroundImage     { get; private set; }
			public string BackBackgroundImage     { get; private set; }
			public string WideBackBackgroundImage { get; private set; }
			//-------------------------------------------------------------------------------------------------------------------
			}
		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		#region // Structure FlipTileInfos
		//-----------------------------------------------------------------------------------------------------------------------
		struct FlipTileInfos
			{
			//-------------------------------------------------------------------------------------------------------------------
			public string   ArticleID   { get; set; }
			public string   Title       { get; set; }
			public DateTime PublishDate { get; set; }
			public string   Link        { get; set; }
			public string   Uri         { get; set; }
			//-------------------------------------------------------------------------------------------------------------------
			}
		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		#region // Classe DownloadDataState
		//-----------------------------------------------------------------------------------------------------------------------
		class DownloadDataState
			{
			//-------------------------------------------------------------------------------------------------------------------
			// Section des Attributs
			//-------------------------------------------------------------------------------------------------------------------
			private string                            Token    = string.Empty;
			private List      <string>                UriList  = null;
			private Dictionary<string, FlipTileInfos> Articles = null;
			//-------------------------------------------------------------------------------------------------------------------

			//*******************************************************************************************************************
			/// <summary>
			/// 
			/// </summary>
			//-------------------------------------------------------------------------------------------------------------------
			public DownloadDataState ()
				{
				//---------------------------------------------------------------------------------------------------------------
				Items    = new List      <        FlipTileInfos> ();
				UriList  = new List      <string               > ();
				Articles = new Dictionary<string, FlipTileInfos> ();

				this.LastAppLaunch  = StorageSettings.GetValue ( "agent-last-app-launch" , DateTime.MinValue );
				this.LastToastCheck = StorageSettings.GetValue ( "agent-last-toast-check", DateTime.MinValue );

				this.CheckToast = ( ScheduledAgent.ToastDelay > 0 && DateTime.Now.Subtract 
				                       ( LastToastCheck ).TotalHours > ScheduledAgent.ToastDelay );
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				this.UriList.Add ( "getLastArticles?count=40&page=1" );

				if ( ScheduledAgent.TileIsActive )
					{
					foreach ( ShellTile Tile in ShellTile.ActiveTiles )
						{
						var Infos = FlipTileOptions.FromTile ( Tile );

						if ( ! string.IsNullOrEmpty ( Infos.Uri ) )
							if ( ! this.UriList.Contains ( Infos.Uri ) )
								this.UriList.Add ( Infos.Uri );
						}
					}
				//---------------------------------------------------------------------------------------------------------------
				}
			//*******************************************************************************************************************
			
			//-------------------------------------------------------------------------------------------------------------------
			public bool                CheckToast     { get; private set; }
			public DateTime            LastAppLaunch  { get; private set; }
			public DateTime            LastToastCheck { get; private set; }
			public List<FlipTileInfos> Items          { get; private set; }
			//-------------------------------------------------------------------------------------------------------------------
			
			//*******************************************************************************************************************
			/// <summary>
			/// 
			/// </summary>
			//-------------------------------------------------------------------------------------------------------------------
			public string BaseUri
				{
				//---------------------------------------------------------------------------------------------------------------
				get
					{
					//-----------------------------------------------------------------------------------------------------------
					return "https://api.nextradiotv.com/bfmbusiness-iphone/3.1/" + 
					                     ( ( string.IsNullOrEmpty ( Token ) ) ? "" : Token + "/" );
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				}
			//*******************************************************************************************************************
			
			//*******************************************************************************************************************
			/// <summary>
			/// 
			/// </summary>
			//-------------------------------------------------------------------------------------------------------------------
			public Uri Uri
				{
				//---------------------------------------------------------------------------------------------------------------
				get
					{
					//-----------------------------------------------------------------------------------------------------------
					return ( UriList.Count == 0 ) ? null : new Uri ( BaseUri + this.UriList[0] );
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				}
			//*******************************************************************************************************************
			
			//*******************************************************************************************************************
			/// <summary>
			/// Définit le Token
			/// </summary>
			//-------------------------------------------------------------------------------------------------------------------
			public void AddUnique ( FlipTileInfos Item )
				{
				//---------------------------------------------------------------------------------------------------------------
				if ( ! this.Articles.ContainsKey ( Item.ArticleID ) )
					{
					//-----------------------------------------------------------------------------------------------------------
					if ( ! string.IsNullOrEmpty ( Item.Uri ) )
						Item.Uri = Item.Uri.Substring ( this.BaseUri.Length );
					
					this.Items.Add ( Item );

					this.Articles[Item.ArticleID] = Item;
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				else
					{
					//-----------------------------------------------------------------------------------------------------------
					FlipTileInfos PrevItem = this.Articles[Item.ArticleID];

					if ( ! string.IsNullOrEmpty ( Item.Uri ) )
						Item.Uri = Item.Uri.Substring ( this.BaseUri.Length );
					
					PrevItem.Uri += ( ";" + Item.Uri );
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				}
			//*******************************************************************************************************************

			//*******************************************************************************************************************
			/// <summary>
			/// Définit le Token
			/// </summary>
			//-------------------------------------------------------------------------------------------------------------------
			public void SetToken ( string Token ) { this.Token = Token; }
			//*******************************************************************************************************************

			//*******************************************************************************************************************
			/// <summary>
			/// Obtiens la prochaine Uri à traiter.
			/// </summary>
			//-------------------------------------------------------------------------------------------------------------------
			public void Notify ()
				{
				//---------------------------------------------------------------------------------------------------------------
				if ( this.UriList.Count > 0 ) this.UriList.RemoveAt ( 0 );
				//---------------------------------------------------------------------------------------------------------------
				}
			//*******************************************************************************************************************

			//*******************************************************************************************************************
			/// <summary>
			/// Prévient que le processus est finit.
			/// </summary>
			//-------------------------------------------------------------------------------------------------------------------
			public void ToastNotify ()
				{
				//---------------------------------------------------------------------------------------------------------------
				StorageSettings.SetValue ( "agent-last-toast-check", DateTime.Now );
				//---------------------------------------------------------------------------------------------------------------
				}
			//*******************************************************************************************************************
			}
		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		#region // Classe FlipTileInfosComparer
		//-----------------------------------------------------------------------------------------------------------------------
		class FlipTileInfosComparer : IComparer<FlipTileInfos>
			{
			//-------------------------------------------------------------------------------------------------------------------
			public int Compare ( FlipTileInfos X, FlipTileInfos Y )
				{ return ( X.PublishDate > Y.PublishDate ) ? -1 : 1; }
			//-------------------------------------------------------------------------------------------------------------------
			}
		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************

		//-----------------------------------------------------------------------------------------------------------------------
		// Section des Constantes
		//-----------------------------------------------------------------------------------------------------------------------
		private const string UserAgent = "BFMTV 4.2.1 (iPhone; iPhone OS 8.0.0; fr_FR)";
		//-----------------------------------------------------------------------------------------------------------------------

		//***********************************************************************************************************************
		#region // Section du Constructeur
		//-----------------------------------------------------------------------------------------------------------------------

		//***********************************************************************************************************************
		/// <remarks>
		/// Le constructeur ScheduledAgent initialise le gestionnaire UnhandledException.
		/// </remarks>
		//-----------------------------------------------------------------------------------------------------------------------
		static ScheduledAgent ()
			{
			//-------------------------------------------------------------------------------------------------------------------
			Deployment.Current.Dispatcher.BeginInvoke ( delegate 
			                   { Application.Current.UnhandledException += UnhandledException; } );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Se produit quand une exception est générée.
		/// </summary>
		/// <param name="Sender">Objet source de l'appel.</param>
		/// <param name="Args">
		/// <b>ApplicationUnhandledExceptionEventArgs</b> qui contient les données d'événement.
		/// </param>
		//-----------------------------------------------------------------------------------------------------------------------
		private static void UnhandledException ( object Sender, ApplicationUnhandledExceptionEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			Args.Handled = true;
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
		/// Découpe la chaine en plusieurs chaines plus petites
		/// </summary>
		/// <param name="Value">Chaine à découper.</param>
		/// <returns>Chaine découpée.</returns>
		//-----------------------------------------------------------------------------------------------------------------------
		private static string WordWrap ( string Value, int Size, int MaxLines )
			{
			//-------------------------------------------------------------------------------------------------------------------
			int LastWhiteSpace = -1;
			var Buffer         = new StringBuilder ();
			var Result         = new StringBuilder ();
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			for ( int Index = 0 ; Index < Value.Length && MaxLines > 0 ; Index ++ )
				{
				//---------------------------------------------------------------------------------------------------------------
				char Car = Value[Index];

				if ( Char.IsWhiteSpace ( Car ) ) LastWhiteSpace = Index;

				Buffer.Append ( Car );
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				if ( Buffer.Length >= Size && LastWhiteSpace > -1 )
					{
					//-----------------------------------------------------------------------------------------------------------
					if ( MaxLines > 0 )
						{
						//-------------------------------------------------------------------------------------------------------
						Result.Append ( Buffer.ToString ( 0, LastWhiteSpace ) );

						if ( MaxLines > 1 )
							{
							Result.Append ( "&#13;" );
							}
						else
							{
							if ( Index == Value.Length ) Result.Append ( "."     );
							else                         Result.Append ( " ..."  );
							}

						Buffer = new System.Text.StringBuilder ();

						Index  = -1;
						MaxLines --;

						Value = Value.Substring ( LastWhiteSpace + 1 );
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					}
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			if ( Buffer.Length > 0 && MaxLines > 0 ) Result.Append ( Buffer.ToString () );

			string FinalResult = Result.ToString ();

			if ( ! FinalResult.EndsWith ( "." ) ) FinalResult += ".";

			return FinalResult;
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		#region // Section de Gestion de la Tuile
		//-----------------------------------------------------------------------------------------------------------------------

		//***********************************************************************************************************************
		/// <summary>
		/// Gère l'affichage final de la tuile.
		/// </summary>
		/// <param name="Tile">Tuile à mettre à jour.</param>
		/// <param name="Images">Configuration de la tuile.</param>
		/// <param name="Content">Texte à afficher.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private static FlipTileData GetTileData ( FlipTileOptions Config, string Content )
			{
			//-------------------------------------------------------------------------------------------------------------------
			string XmlContent = String.Empty;
			//-------------------------------------------------------------------------------------------------------------------
				
			//-------------------------------------------------------------------------------------------------------------------
			string TitleClear     = ( string.IsNullOrEmpty ( Config.Title     ) ) ? "Action=\"Clear\"" : "";
			string BackTitleClear = ( string.IsNullOrEmpty ( Config.BackTitle ) ) ? "Action=\"Clear\"" : "";
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			// Tuile avec article
			//-------------------------------------------------------------------------------------------------------------------
			if ( string.IsNullOrEmpty ( Content ) )
				{
				//---------------------------------------------------------------------------------------------------------------
				#region // Déclaration du corp
				//---------------------------------------------------------------------------------------------------------------
				XmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n"                                   +
					         "<wp:Notification xmlns:wp=\"WPNotification\" Version=\"2.0\">\n"                +
					         " <wp:Tile Id=\"1\" Template=\"FlipTile\">\n"                                    +
					         "  <wp:Count>0</wp:Count>\n"                                                     +
					         "  <wp:Title {0}>{1}</wp:Title>\n"                                               +
					         "  <wp:SmallBackgroundImage IsRelative=\"true\">{2}</wp:SmallBackgroundImage>\n" +
					         "  <wp:BackgroundImage IsRelative=\"true\">{3}</wp:BackgroundImage>\n"           +
					         "  <wp:WideBackgroundImage IsRelative=\"true\">{4}</wp:WideBackgroundImage>\n"   +
					         "  <wp:BackTitle Action=\"Clear\" />\n"                                          +
					         "  <wp:BackContent Action=\"Clear\" />\n"                                        +
					         "  <wp:WideBackContent Action=\"Clear\" />\n"                                    +
					         "  <wp:BackBackgroundImage IsRelative=\"true\" Action=\"Clear\" />\n"            +
					         "  <wp:WideBackBackgroundImage IsRelative=\"true\" Action=\"Clear\" />\n"        +
					         " </wp:Tile>\n"                                                                  +
					         "</wp:Notification>";

				XmlContent = string.Format ( XmlContent, TitleClear                 , 
					                                     Config.Title               , 
					                                     Config.SmallBackgroundImage, 
					                                     Config.BackgroundImage     , 
					                                     Config.WideBackgroundImage );
				//---------------------------------------------------------------------------------------------------------------
				#endregion
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			// Tuile sans article
			//-------------------------------------------------------------------------------------------------------------------
			else
				{
				//---------------------------------------------------------------------------------------------------------------
				#region // Déclaration du corp
				//---------------------------------------------------------------------------------------------------------------
				XmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n"                                          +
					         "<wp:Notification xmlns:wp=\"WPNotification\" Version=\"2.0\">\n"                       +
					         " <wp:Tile Id=\"1\" Template=\"FlipTile\">\n"                                           +
					         "  <wp:Count>0</wp:Count>\n"                                                            +
					         "  <wp:Title {0}>{1}</wp:Title>\n"                                                      +
					         "  <wp:SmallBackgroundImage IsRelative=\"true\">{2}</wp:SmallBackgroundImage>\n"        +
					         "  <wp:BackgroundImage IsRelative=\"true\">{3}</wp:BackgroundImage>\n"                  +
					         "  <wp:WideBackgroundImage IsRelative=\"true\">{4}</wp:WideBackgroundImage>\n"          +
					         "  <wp:BackTitle {5}>{6}</wp:BackTitle>\n"                                              +
					         "  <wp:BackContent>{7}</wp:BackContent>\n"                                              +
					         "  <wp:WideBackContent>{8}</wp:WideBackContent>\n"                                      +
					         "  <wp:BackBackgroundImage IsRelative=\"true\">{9}</wp:BackBackgroundImage>\n"          +
					         "  <wp:WideBackBackgroundImage IsRelative=\"true\">{10}</wp:WideBackBackgroundImage>\n" +
					         " </wp:Tile>\n" +
					         "</wp:Notification>";
					
				XmlContent = string.Format ( XmlContent, TitleClear                     , 
					                                     Config.Title                   , 
					                                     Config.SmallBackgroundImage    , 
					                                     Config.BackgroundImage         , 
					                                     Config.WideBackgroundImage     ,
					                                     BackTitleClear                 , 
					                                     Config.BackTitle               , 
					                                     Content                        , 
					                                     WordWrap ( Content, 26, 3 )    ,
					                                     Config.BackBackgroundImage     ,
					                                     Config.WideBackBackgroundImage );
				//---------------------------------------------------------------------------------------------------------------
				#endregion
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			return new FlipTileData ( XmlContent );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Purge la tuile.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		private static void ClearTiles ()
			{
			//-------------------------------------------------------------------------------------------------------------------
			try
				{
				//---------------------------------------------------------------------------------------------------------------
				Deployment.Current.Dispatcher.BeginInvoke ( () =>
					{
					//-----------------------------------------------------------------------------------------------------------
					foreach ( ShellTile Tile in ShellTile.ActiveTiles )	
						{ ScheduledAgent.ClearTile ( Tile, FlipTileOptions.FromTile ( Tile ) ); }
					//-----------------------------------------------------------------------------------------------------------
					} );
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			catch {}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Gère l'affichage final de la tuile.
		/// </summary>
		/// <param name="Tile">Tuile à mettre à jour.</param>
		/// <param name="Images">Configuration de la tuile.</param>
		/// <param name="Content">Texte à afficher.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private static void ClearTile ( ShellTile Tile, FlipTileOptions Images )
			{
			//-------------------------------------------------------------------------------------------------------------------
			ScheduledAgent.UpdateTile ( Tile, Images, string.Empty );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Gère l'affichage final de la tuile.
		/// </summary>
		/// <param name="Tile">Tuile à mettre à jour.</param>
		/// <param name="Images">Configuration de la tuile.</param>
		/// <param name="Content">Texte à afficher.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private static void UpdateTile ( ShellTile Tile, FlipTileOptions Config, string Content )
			{
			//-------------------------------------------------------------------------------------------------------------------
			try { Tile.Update ( GetTileData ( Config, Content ) ); } catch {}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		#region // Section des Procédures de Traitement
		//-----------------------------------------------------------------------------------------------------------------------

		//***********************************************************************************************************************
		/// <summary>
		/// Met à jour les tuiles et lance les notifications.
		/// </summary>
		/// <param name="State">Object ontenant les informations à traiter.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void OnFinalProcess ( DownloadDataState State )
			{
			//-------------------------------------------------------------------------------------------------------------------
			#region // Implémentation de la Procédure
			//-------------------------------------------------------------------------------------------------------------------
			
			//-------------------------------------------------------------------------------------------------------------------
			Deployment.Current.Dispatcher.BeginInvoke ( () =>
				{
				//---------------------------------------------------------------------------------------------------------------
				State.Items.Sort ( new FlipTileInfosComparer () );
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				// On a du contenue, on récupère la liste des Tuiles
				//---------------------------------------------------------------------------------------------------------------
				var Tiles = new Dictionary<ShellTile, FlipTileOptions> ();

				if ( ScheduledAgent.TileIsActive )
					foreach ( ShellTile Tile in ShellTile.ActiveTiles )
						{ Tiles[Tile] = FlipTileOptions.FromTile ( Tile ); }
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				int           ToastCount   = 0;
				FlipTileInfos ToastContent = new FlipTileInfos ();
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				try
					{
					//-----------------------------------------------------------------------------------------------------------
					// Lecture des Eléments
					//-----------------------------------------------------------------------------------------------------------
					foreach ( FlipTileInfos Item in State.Items )
						{
						//-------------------------------------------------------------------------------------------------------
						var ProcessedTiles = new List <ShellTile> ();

						foreach ( ShellTile Tile in Tiles.Keys )
							{
							FlipTileOptions TileInfos = Tiles[Tile];

							if ( string.IsNullOrEmpty ( TileInfos.Uri ) || Item.Uri.Contains ( TileInfos.Uri ) )
								{
								ScheduledAgent.UpdateTile ( Tile, Tiles[Tile], Item.Title );

								ProcessedTiles.Add ( Tile );
								}
							}

						foreach ( ShellTile Tile in ProcessedTiles ) { Tiles.Remove ( Tile ); }
						//-------------------------------------------------------------------------------------------------------

						//-------------------------------------------------------------------------------------------------------
						// Est ce qu'on check les Toast ?
						//-------------------------------------------------------------------------------------------------------
						if ( State.CheckToast && Item.PublishDate > State.LastToastCheck )
							{
							//---------------------------------------------------------------------------------------------------
							ToastCount ++;
							ToastContent = Item;
							//---------------------------------------------------------------------------------------------------
							}
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				catch {}
				//---------------------------------------------------------------------------------------------------------------
				finally
					{
					//-----------------------------------------------------------------------------------------------------------
					#region // Un nouvel article
					//-----------------------------------------------------------------------------------------------------------
					if ( ToastCount == 1 )
						{
						//-------------------------------------------------------------------------------------------------------
						State.ToastNotify ();
						//-------------------------------------------------------------------------------------------------------

						//-------------------------------------------------------------------------------------------------------
						string Format      = "ArticleID={0}&PublishDate={1}00&Uri={2}";
						string ArticleID   = ToastContent.ArticleID;
						string PublishDate = ToastContent.PublishDate.ToString ( "yyyyMMddHHmm" );
						string Uri         = HttpUtility.UrlEncode ( ToastContent.Link );

						string ToastUri    = string.Format ( Format, ArticleID, PublishDate, Uri );

						(new ShellToast ()
							{
							Title         = "Nouvel article"                                      ,
							Content       = ToastContent.Title                                    ,
							NavigationUri = new Uri ( "/Frames/Frm_Home.xaml?" + ToastUri, UriKind.Relative ),
							}).Show ();
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------

					//-----------------------------------------------------------------------------------------------------------
					#region // Plusieurs nouveaux articles
					//-----------------------------------------------------------------------------------------------------------
					else if ( ToastCount > 1 )
						{
						//-------------------------------------------------------------------------------------------------------
						State.ToastNotify ();
						//-------------------------------------------------------------------------------------------------------

						//-------------------------------------------------------------------------------------------------------
						(new ShellToast ()
							{
							Title         = "BFM Live"                                           ,
							Content       = string.Format ( "tu as {0} nouveaux articles"        +
							                                 " à lire sur BFM Live", ToastCount ),
							NavigationUri = new Uri ( "/Frames/Frm_Home.xaml", UriKind.Relative ),
							}).Show ();
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					#endregion
					//-----------------------------------------------------------------------------------------------------------
					
					//-----------------------------------------------------------------------------------------------------------
					// On ré-initialise les tuiles non mise à jour
					//-----------------------------------------------------------------------------------------------------------
					foreach ( ShellTile Tile in Tiles.Keys )
						{ ScheduledAgent.ClearTile ( Tile, Tiles[Tile] ); }

					base .NotifyComplete ();
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				} );
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			#endregion
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Gère la réponse à la récupération des derniers articles.
		/// </summary>
		/// <param name="Sender">Objet source de l'appel.</param>
		/// <param name="Args"><b>...EventArgs</b> qui contient les données d'événement.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void OnGetLastArticlesSuccess ( object S, DownloadStringCompletedEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			#region // Implémentation de la Procédure
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			DownloadDataState State = (DownloadDataState)Args.UserState;

			bool CancelNotify = false;
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			try
				{
				//---------------------------------------------------------------------------------------------------------------
				// S'il y a eu une erreur ou une exception, on arrête
				//---------------------------------------------------------------------------------------------------------------
				if ( Args.Cancelled                       ) return;
				if ( Args.Error                   != null ) return;
				if ( string.IsNullOrEmpty ( Args.Result ) ) return;
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				// A t'on du contenue ?
				//---------------------------------------------------------------------------------------------------------------
				if ( string.IsNullOrEmpty ( Args.Result ) ) return;

				JToken Racine = JToken.Parse ( Args.Result );

				if ( Racine == null || ! Racine.HasValues ) return;
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				// C'est un Article
				//---------------------------------------------------------------------------------------------------------------
				if ( Racine["articles"] != null )
					{
					//-----------------------------------------------------------------------------------------------------------
					// Lecture des Articles
					//-----------------------------------------------------------------------------------------------------------
					var Article = Racine["articles"].First;

					while ( Article != null )
						{
						//-------------------------------------------------------------------------------------------------------
						string Title = HttpUtility.HtmlDecode ( Article.Value<string>( "title" ) );

						string ArticleID = Article.Value<long  >( "article"  ).ToString ();
						string Link      = Article.Value<string>( "long_url" );

						DateTime Date     = Article.Read<DateTime>( "date"      );
						DateTime EditDate = Article.Read<DateTime>( "edit_date" );

						DateTime PublishDate = ( EditDate > Date ) ? EditDate : Date;

						if ( PublishDate < State.LastAppLaunch ) break;
						//-------------------------------------------------------------------------------------------------------

						//-------------------------------------------------------------------------------------------------------
						State.AddUnique ( new FlipTileInfos ()
							{
							ArticleID   = ArticleID               ,
							Title       = Title                   ,
							PublishDate = PublishDate             ,
							Link        = Link                    ,
							Uri         = State.Uri.OriginalString,
							} );
						//-------------------------------------------------------------------------------------------------------
						
						//-------------------------------------------------------------------------------------------------------
						Article = Article.Next;
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				CancelNotify = true;
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			catch {}
			//-------------------------------------------------------------------------------------------------------------------
			finally
				{
				//---------------------------------------------------------------------------------------------------------------
				State.Notify ();

				if ( ! CancelNotify ) base.NotifyComplete (       );
				else                  this.AsyncCallUri   ( State );
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			#endregion
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Récupère les derniers articles publiés.
		/// </summary>
		/// <returns><b>True</b> si le traitement est en cours, sinon <b>False</b>.</returns>
		//-----------------------------------------------------------------------------------------------------------------------
		private bool AsyncCallUri ( DownloadDataState State )
			{
			//-------------------------------------------------------------------------------------------------------------------
			#region // Implémentation de la Procédure
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			try
				{
				//---------------------------------------------------------------------------------------------------------------
				Uri Uri = State.Uri;

				if ( Uri != null )
					{
					//-----------------------------------------------------------------------------------------------------------
					var WebClient = new WebClient ();

					WebClient.Headers["User-Agent"] = UserAgent;

					WebClient.DownloadStringCompleted += this.OnGetLastArticlesSuccess;

					WebClient.DownloadStringAsync ( Uri, State );
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				else { this.OnFinalProcess ( State ); }
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			catch { return false; }
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			return true;
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			#endregion
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Gère la réponse à l'enregistrement.
		/// </summary>
		/// <param name="S">...</param>
		/// <param name="Args">...</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private void OnRegistationSuccess ( object S, DownloadStringCompletedEventArgs Args )
			{
			//-------------------------------------------------------------------------------------------------------------------
			#region // Implémentation de la Procédure
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			DownloadDataState State = (DownloadDataState)Args.UserState;

			bool CancelNotify = false;
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			try
				{
				//---------------------------------------------------------------------------------------------------------------
				if ( string.IsNullOrEmpty ( Args.Result ) ) return;

				JToken Racine = JToken.Parse ( Args.Result );

				if ( Racine == null || ! Racine.HasValues ) return;

				JToken Session = Racine["session"];

				if ( Session == null || ! Session.HasValues ) return;

				State.SetToken ( Session.Value<string>( "token" ) );

				CancelNotify = this.AsyncCallUri ( State );
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			catch {}
			//-------------------------------------------------------------------------------------------------------------------
			finally { if ( ! CancelNotify ) base.NotifyComplete (); }
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			#endregion
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Enregistre l'agent au près du service.
		/// </summary>
		/// <returns>
		/// <b>True</b> si aucune anomalie n'est apparu lors de l'appel, sinon <b>False</b>.
		/// </returns>
		//-----------------------------------------------------------------------------------------------------------------------
		private bool AsyncRegisterApp ( DownloadDataState State )
			{
			//-------------------------------------------------------------------------------------------------------------------
			#region // Implémentation de la Procédure
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			try
				{
				//---------------------------------------------------------------------------------------------------------------
				string Uri = "https://api.nextradiotv.com/bfmbusiness-iphone/3.1/";

				var WebClient = new WebClient ();

				WebClient.Headers["User-Agent"] = UserAgent;

				WebClient.DownloadStringCompleted += this.OnRegistationSuccess;

				WebClient.DownloadStringAsync ( new Uri ( Uri ), State );
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			catch { return false; }
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			return true;
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			#endregion
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Agent qui exécute une tâche planifiée.
		/// </summary>
		/// <param name="task">La tâche appelée.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		protected override void OnInvoke ( ScheduledTask task )
			{
			//-------------------------------------------------------------------------------------------------------------------
			#region // Implémentation de la Procédure
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			bool CancelNotify = false;
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			try
				{
				//---------------------------------------------------------------------------------------------------------------
				if ( ! TileIsActive && ToastDelay == 0           ) return;
				if ( ! NetworkInterface.GetIsNetworkAvailable () ) return;

				CancelNotify = this.AsyncRegisterApp ( new DownloadDataState () );
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			catch {}
			//-------------------------------------------------------------------------------------------------------------------
			finally
				{
				//---------------------------------------------------------------------------------------------------------------
				if ( ! CancelNotify )
					{
					//-----------------------------------------------------------------------------------------------------------
					base.NotifyComplete       ();
					ScheduledAgent.ClearTiles ();
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------------------------------------
			#endregion
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		#region // PROCEDURE >> Clear
		//-----------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Purge la tuile.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public static void Clear ()
			{
			//-------------------------------------------------------------------------------------------------------------------
			StorageSettings.SetValue ( "agent-last-app-launch" , DateTime.Now                    );
			StorageSettings.SetValue ( "agent-last-toast-check", DateTime.Now.AddMinutes ( -30 ) );

			ScheduledAgent.ClearTiles ();
			//-------------------------------------------------------------------------------------------------------------------
			}
		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		#region // PROCEDURE >> GetTileData
		//-----------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Obtiens les infos nécessaires à la création de la tuile.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public static FlipTileData GetTileData ( Uri Uri )
			{
			//-------------------------------------------------------------------------------------------------------------------
			var TileConfig = FlipTileOptions.FromUri ( Uri );
				
			return ScheduledAgent.GetTileData ( TileConfig, string.Empty );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		#region // PROPERTY >> TileIsActive
		//-----------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Indique si la tuile dynamique est active.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public static bool TileIsActive
			{
			//-------------------------------------------------------------------------------------------------------------------
			get { return (bool)StorageSettings.GetValue ( "agent-tile-is-active", false ); }
			set {              StorageSettings.SetValue ( "agent-tile-is-active", value ); }
			//-------------------------------------------------------------------------------------------------------------------
			}
		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		#region // PROPERTY >> ToastDelay
		//-----------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Indique si les notifications sont actives.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public static int ToastDelay
			{
			//-------------------------------------------------------------------------------------------------------------------
			get
				{
				object Value = StorageSettings.GetValue ( "agent-toast-delay" );

				if ( Value == null )
					{
					bool Active = StorageSettings.GetValue ( "agent-toast-is-active", false );

					Value = ( Active ) ? (int)1 : (int)0;

					StorageSettings.SetValue ( "agent-toast-delay", (int)Value );
					}
				
				return (int)Value;
				}
			//-------------------------------------------------------------------------------------------------------------------
			set
				{
				//---------------------------------------------------------------------------------------------------------------
				switch ( value )
					{
					case 1  : StorageSettings.SetValue ( "agent-toast-delay", value ); break;
					case 2  : StorageSettings.SetValue ( "agent-toast-delay", value ); break;
					case 4  : StorageSettings.SetValue ( "agent-toast-delay", value ); break;
					default : StorageSettings.SetValue ( "agent-toast-delay", 0     ); break;
					}
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************
		}
	//---------------------------------------------------------------------------------------------------------------------------
	#endregion
	//***************************************************************************************************************************

	} // Fin du namespace "NextRadio.Scheduler"
//*******************************************************************************************************************************

//*******************************************************************************************************************************
// FIN DU FICHIER
//*******************************************************************************************************************************
