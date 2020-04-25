﻿//*******************************************************************************************************************************
// DEBUT DU FICHIER
//*******************************************************************************************************************************

//*******************************************************************************************************************************
// Nom           : BooleanUtils.cs
// Auteur        : Nicolas Dagnas
// Description   : Implémentation de l'objet BooleanUtils
// Créé le       : 29/07/2013
// Modifié le    : 26/02/2014
//*******************************************************************************************************************************

//-------------------------------------------------------------------------------------------------------------------------------
#region Using directives
//-------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Windows;
//-------------------------------------------------------------------------------------------------------------------------------
#endregion
//-------------------------------------------------------------------------------------------------------------------------------

//*******************************************************************************************************************************
// Début du bloc "System"
//*******************************************************************************************************************************
namespace System
	{


	//  ####    ###    ###   #             #####  #   #  #####          ###   ####    ###     ####
	//  #   #  #   #  #   #  #             #      #   #    #           #   #  #   #  #   #   #    
	//  ####   #   #  #   #  #      #####  ###    #   #    #    #####  #####  ####   #        ### 
	//  #   #  #   #  #   #  #             #       # #     #           #   #  #   #  #   ##      #
	//  ####    ###    ###   #####         #####    #      #           #   #  #   #   ### #  #### 

	//***************************************************************************************************************************
	// Classe BooleanEventArgs
	//***************************************************************************************************************************
	#region // Déclaration et Implémentation de l'Objet
	//---------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Fournit des données pour l'événement <b>BooleanEventHandler</b>.
	/// </summary>
	//---------------------------------------------------------------------------------------------------------------------------
	public class BooleanEventArgs : EventArgs
		{
		//***********************************************************************************************************************
		/// <summary>
		/// Initialise une nouvelle instance de l'objet <b>BooleanEventArgs</b>.
		/// </summary>
		/// <param name="Value">Booléen représentant la valeur de l'événement.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		public BooleanEventArgs ( bool Value ) { this.Value = Value; }
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Obtient ou définit la valeur de l'événement.
		/// </summary>
		/// <returns>
		/// Chaine représentant la valeur de l'événement.
		/// </returns>
		//-----------------------------------------------------------------------------------------------------------------------
		public bool Value { get; private set; }
		//***********************************************************************************************************************
		}
	//---------------------------------------------------------------------------------------------------------------------------
	#endregion
	//***************************************************************************************************************************

	//  ####    ###    ###   #             #####  #   #  #####         #   #  ####   #### 
	//  #   #  #   #  #   #  #             #      #   #    #           #   #  #   #  #   #
	//  ####   #   #  #   #  #      #####  ###    #   #    #    #####  #####  #   #  #### 
	//  #   #  #   #  #   #  #             #       # #     #           #   #  #   #  #   #
	//  ####    ###    ###   #####         #####    #      #           #   #  ####   #   #

	//***************************************************************************************************************************
	// Délégué BooleanEventHandler
	//***************************************************************************************************************************
	#region // Déclaration et Implémentation de l'Objet
	//---------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Délégé utilisé pour passer un objet <b>DateTime</b>.
	/// </summary>
	/// <param name="Sender">Objet à l'origine de l'appel.</param>
	/// <param name="Args">
	/// Objet <b>BooleanEventArgs</b> contenant les données de l'événement.
	/// </param>
	//---------------------------------------------------------------------------------------------------------------------------
	public delegate void BooleanEventHandler ( object Sender, BooleanEventArgs Args );
	//---------------------------------------------------------------------------------------------------------------------------
	#endregion
	//***************************************************************************************************************************

	//  ####    ###    ###   #      #####   ###   #   #
	//  #   #  #   #  #   #  #      #      #   #  ##  #
	//  ####   #   #  #   #  #      ###    #####  # # #
	//  #   #  #   #  #   #  #      #      #   #  #  ##
	//  ####    ###    ###   #####  #####  #   #  #   #

	//***************************************************************************************************************************
	// Classe BooleanUtils
	//***************************************************************************************************************************
	#region // Déclaration et Implémentation de l'Objet
	//---------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Fournit des méthodes utilisées pour manipuler les booléens.
	/// </summary>
	//---------------------------------------------------------------------------------------------------------------------------
	public static class BooleanUtils
		{
		//***********************************************************************************************************************
		/// <summary>
		/// Convertit le booléen en chaine minuscule.
		/// </summary>
		/// <param name="Self">Objet <b>Boolean</b>.</param>
		/// <returns>Enum converti.</returns>
		//-----------------------------------------------------------------------------------------------------------------------
		public static string ToLowerString ( this Boolean Self )
			{
			//-------------------------------------------------------------------------------------------------------------------
			return Self.ToString ().ToLower ();
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Convertit le booléen en valeur <b>Visibility</b>.
		/// </summary>
		/// <param name="Self">Objet <b>Boolean</b>.</param>
		/// <returns>Booléen converti.</returns>
		//-----------------------------------------------------------------------------------------------------------------------
		public static Visibility ToVisibility ( this Boolean Self )
			{
			//-------------------------------------------------------------------------------------------------------------------
			return ( Self ) ? Visibility.Visible : Visibility.Collapsed;
			//-------------------------------------------------------------------------------------------------------------------
			}
		//***********************************************************************************************************************
		}
	//---------------------------------------------------------------------------------------------------------------------------
	#endregion
	//***************************************************************************************************************************
	
	} // Fin du namespace "System"
//*******************************************************************************************************************************

//*******************************************************************************************************************************
// FIN DU FICHIER
//*******************************************************************************************************************************
