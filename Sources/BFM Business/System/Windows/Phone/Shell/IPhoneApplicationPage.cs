﻿//*******************************************************************************************************************************
// DEBUT DU FICHIER
//*******************************************************************************************************************************

//*******************************************************************************************************************************
// Nom           : IPhoneApplicationPage.cs
// Auteur        : Nicolas Dagnas
// Description   : Implémentation d'interface IPhoneApplicationPage
// Créé le       : 18/02/2015
// Modifié le    : 19/05/2015
//*******************************************************************************************************************************

//-------------------------------------------------------------------------------------------------------------------------------
#region Using directives
//-------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.Generic;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using Microsoft.Phone.Shell;
//-------------------------------------------------------------------------------------------------------------------------------
#endregion
//-------------------------------------------------------------------------------------------------------------------------------

//*******************************************************************************************************************************
// Début du bloc "System.Windows.Phone.Shell"
//*******************************************************************************************************************************
namespace System.Windows.Phone.Shell
	{
	
	//   ###   ####   ####          ####    ###    ###    #####
	//  #   #  #   #  #   #         #   #  #   #  #   #   #    
	//  #####  ####   ####   #####  ####   #####  #       ###  
	//  #   #  #      #             #      #   #  #   ##  #    
	//  #   #  #      #             #      #   #   ### #  #####

	//***************************************************************************************************************************
	// Interface IPhoneApplicationPage
	//***************************************************************************************************************************
	#region // Déclaration et Implémentation de l'Objet
	//---------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Définit les propriétés et méthodes d'une page.
	/// </summary>
	//---------------------------------------------------------------------------------------------------------------------------
	public interface IPhoneApplicationPage
		{
		//***********************************************************************************************************************
		/// <summary>
		/// Se produit lors d'un clic sur le bouton précédent.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		event EventHandler<CancelEventArgs> BeginBackKeyPress;
		//***********************************************************************************************************************
		}
	//---------------------------------------------------------------------------------------------------------------------------
	#endregion
	//***************************************************************************************************************************

	//   ###   ####   ####          ####    ###   #### 
	//  #   #  #   #  #   #         #   #  #   #  #   #
	//  #####  ####   ####   #####  ####   #####  #### 
	//  #   #  #      #             #   #  #   #  #   #
	//  #   #  #      #             ####   #   #  #   #

	//***************************************************************************************************************************
	// Classe ApplicationBarUtils
	//***************************************************************************************************************************
	#region // Déclaration et Implémentation de l'Objet
	//---------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Ajoute des fonctions à la barre d'application.
	/// </summary>
	//---------------------------------------------------------------------------------------------------------------------------
	public static class ApplicationBarUtils
		{
		//***********************************************************************************************************************
		#region // Classe CacheItem
		//-----------------------------------------------------------------------------------------------------------------------
		class CacheItem
			{
			//-------------------------------------------------------------------------------------------------------------------
			public CacheItem ( IApplicationBar Owner )
				{
				this.Owner       = Owner;
				this.Buttons     = new ApplicationBarIconButton[0];
				this.IsSuspended = false;
				this.Mode        = Owner.Mode;
				}
			//-------------------------------------------------------------------------------------------------------------------
			
			//-------------------------------------------------------------------------------------------------------------------
			public IApplicationBar            Owner       { get; private  set; }
			public bool                       IsSuspended { get; internal set; }
			public ApplicationBarIconButton[] Buttons     { get; internal set; }
			public ApplicationBarMode         Mode        { get; internal set; }
			//-------------------------------------------------------------------------------------------------------------------
			}
		//-----------------------------------------------------------------------------------------------------------------------
		#endregion
		//***********************************************************************************************************************

		//-----------------------------------------------------------------------------------------------------------------------
		private static Dictionary<IApplicationBar, CacheItem> ApplicationBars = new Dictionary<IApplicationBar, CacheItem> ();
		//-----------------------------------------------------------------------------------------------------------------------
		
		//***********************************************************************************************************************
		/// <summary>
		/// Permet de définir une une fois les boutons de la barre d'application.
		/// </summary>
		/// <param name="Self">Objet concerné par l'appel.</param>
		/// <param name="Buttons">Boutons de la barre d'application.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		private static void Set ( this IApplicationBar Self, ApplicationBarMode Mode, params ApplicationBarIconButton[] Buttons )
			{
			//-------------------------------------------------------------------------------------------------------------------
			if ( Self != null )
				{
				//---------------------------------------------------------------------------------------------------------------
				var Rs = new List<ApplicationBarIconButton                   > ();
				var Is = new List<KeyValuePair<ApplicationBarIconButton, int>> ();
				
				var As = new List<ApplicationBarIconButton> ();
				var Bs = new List<ApplicationBarIconButton> ( Buttons );

				foreach ( ApplicationBarIconButton Button in Self.Buttons ) { As.Add ( Button ); }
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				// On retire tous les boutons du block 1 qui ne sont pas dans le block B
				//---------------------------------------------------------------------------------------------------------------
				for ( int Index = As.Count - 1 ; Index >= 0 ; Index -- )
					{
					//-----------------------------------------------------------------------------------------------------------
					if ( ! Bs.Contains ( As[Index] ) )
						{
						Rs.Add ( As[Index] );

						As.RemoveAt ( Index );
						}
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				// S'il reste des boutons dans le block A, on continue le traitement
				//---------------------------------------------------------------------------------------------------------------
				if ( As.Count > 0 )
					{
					//-----------------------------------------------------------------------------------------------------------
					for ( int Index = 0 ; Index < Bs.Count ; Index ++ )
						{
						//-------------------------------------------------------------------------------------------------------
						if ( Index < As.Count )
							{
							//---------------------------------------------------------------------------------------------------
							// Le bouton n'est pas à la bonne place, on insert
							//---------------------------------------------------------------------------------------------------
							if ( Bs[Index] != As[Index] )
								{
								Is.Add ( new KeyValuePair<ApplicationBarIconButton, int> ( Bs[Index], Index ) );

								As.Insert ( Index, Bs[Index] );
								}
							//---------------------------------------------------------------------------------------------------
							}
						//-------------------------------------------------------------------------------------------------------
						
						//-------------------------------------------------------------------------------------------------------
						// Le block A est trop petit, on insert à la fin
						//-------------------------------------------------------------------------------------------------------
						else
							{
							//---------------------------------------------------------------------------------------------------
							Is.Add ( new KeyValuePair<ApplicationBarIconButton, int> ( Bs[Index], Index ) );

							As.Insert ( Index, Bs[Index] );
							//---------------------------------------------------------------------------------------------------
							}
						//-------------------------------------------------------------------------------------------------------
						}
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				else { Rs.Clear (); }
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				// Des opérations sont a effectuer
				//---------------------------------------------------------------------------------------------------------------
				if ( Rs.Count > 0 || Is.Count > 0 )
					{
					//-----------------------------------------------------------------------------------------------------------
					foreach ( ApplicationBarIconButton Item in Rs )
						{
						Self.Buttons.Remove ( Item );
						}
					//-----------------------------------------------------------------------------------------------------------

					//-----------------------------------------------------------------------------------------------------------
					foreach ( KeyValuePair<ApplicationBarIconButton, int> Item in Is )
						{
						Self.Buttons.Insert ( Item.Value, Item.Key );
						}
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------

				//---------------------------------------------------------------------------------------------------------------
				else if ( As.Count == 0 )
					{
					//-----------------------------------------------------------------------------------------------------------
					Self.Buttons.Clear ();

					var Timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds ( 25 ) };

					Timer.Tick += (sender, args) =>
						{
						Timer.Stop ();

						Self.Buttons.Clear ();

						foreach ( object Button in Buttons ) Self.Buttons.Add ( Button );
						};

					Timer.Start ();
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				
				//---------------------------------------------------------------------------------------------------------------
				Self.Mode = Mode;
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		
		//***********************************************************************************************************************
		/// <summary>
		/// Permet de définir une une fois les boutons de la barre d'application.
		/// </summary>
		/// <param name="Self">Objet concerné par l'appel.</param>
		/// <param name="Buttons">Boutons de la barre d'application.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		public static void Suspend ( this IApplicationBar Self ) { Suspend ( Self, false ); }
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Permet de définir une une fois les boutons de la barre d'application.
		/// </summary>
		/// <param name="Self">Objet concerné par l'appel.</param>
		/// <param name="Buttons">Boutons de la barre d'application.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		public static void Suspend ( this IApplicationBar Self, bool Clear )
			{
			//-------------------------------------------------------------------------------------------------------------------
			if ( Self != null )
				{
				if ( ! ApplicationBars.ContainsKey ( Self ) )
					ApplicationBars[Self] = new CacheItem ( Self );

				ApplicationBars[Self].IsSuspended = true;

				if ( Clear ) Self.Buttons.Clear ();
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Permet de définir une une fois les boutons de la barre d'application.
		/// </summary>
		/// <param name="Self">Objet concerné par l'appel.</param>
		/// <param name="Buttons">Boutons de la barre d'application.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		public static void SetButtons ( this IApplicationBar Self, params ApplicationBarIconButton[] Buttons )
			{
			//-------------------------------------------------------------------------------------------------------------------
			SetButtons ( Self, Self.Mode, Buttons );
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Permet de définir une une fois les boutons de la barre d'application.
		/// </summary>
		/// <param name="Self">Objet concerné par l'appel.</param>
		/// <param name="Mode"></param>
		/// <param name="Buttons">Boutons de la barre d'application.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		public static void SetButtons ( this IApplicationBar Self, ApplicationBarMode Mode,
		                                                params ApplicationBarIconButton[] Buttons )
			{
			//-------------------------------------------------------------------------------------------------------------------
			if ( Self != null )
				{
				//---------------------------------------------------------------------------------------------------------------
				if ( ApplicationBars.ContainsKey ( Self ) && ApplicationBars[Self].IsSuspended )
					{
					//-----------------------------------------------------------------------------------------------------------
					ApplicationBars[Self].IsSuspended = false;

					var buttons = new List<ApplicationBarIconButton> ();

					foreach ( ApplicationBarIconButton Button in Buttons ) buttons.Add ( Button );

					ApplicationBars[Self].Mode    = Mode;
					ApplicationBars[Self].Buttons = buttons.ToArray ();
					//-----------------------------------------------------------------------------------------------------------
					}
				//---------------------------------------------------------------------------------------------------------------
				else { Set ( Self, Mode, Buttons ); }
				//---------------------------------------------------------------------------------------------------------------
				}
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		}
	//---------------------------------------------------------------------------------------------------------------------------
	#endregion
	//***************************************************************************************************************************

	} // Fin du namespace "System.Windows.Phone.Shell"
//*******************************************************************************************************************************

//*******************************************************************************************************************************
// FIN DU FICHIER
//*******************************************************************************************************************************
