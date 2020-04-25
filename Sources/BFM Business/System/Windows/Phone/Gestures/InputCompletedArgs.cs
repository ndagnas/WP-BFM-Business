//*******************************************************************************************************************************
// DEBUT DU FICHIER
//*******************************************************************************************************************************

//*******************************************************************************************************************************
// Nom           : InputCompletedArgs.cs
// Auteur        : Nicolas Dagnas
// Description   : Implémentation de l'objet InputCompletedArgs
// Créé le       : 17/01/2015
// Modifié le    : 17/01/2015
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
// Début du bloc "System.Windows.Phone.Gestures"
//*******************************************************************************************************************************
namespace System.Windows.Phone.Gestures
	{

	//  #  #   #  ####   #   #  #####          ###    ###   #   #  ####   #      #####  #####
	//  #  ##  #  #   #  #   #    #           #   #  #   #  ## ##  #   #  #      #        #  
	//  #  # # #  ####   #   #    #    #####  #      #   #  # # #  ####   #      ###      #  
	//  #  #  ##  #      #   #    #           #   #  #   #  #   #  #      #      #        #  
	//  #  #   #  #       ###     #            ###    ###   #   #  #      #####  #####    #    ##

	//***************************************************************************************************************************
	// Classe InputCompletedArgs
	//***************************************************************************************************************************
	#region // Déclaration et Implémentation de l'Objet
	//---------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Implémentation de la gestion des mouvements.
	/// </summary>
	//---------------------------------------------------------------------------------------------------------------------------
	internal abstract class InputCompletedArgs : InputBaseArgs
		{
		//***********************************************************************************************************************
		/// <summary>
		/// Initialise une nouvelle instance de l'objet <b>InputCompletedArgs</b>.
		/// </summary>
		/// <param name="Source">Origine de l'appel.</param>
		/// <param name="Origin">Point d'origine du mouvement.</param>
		//-----------------------------------------------------------------------------------------------------------------------
		protected InputCompletedArgs ( UIElement Source, Point Origin ) : base (Source, Origin) {}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		/// <summary>
		/// Obtiens la valeur totale du mouvement.
		/// </summary>
		//-----------------------------------------------------------------------------------------------------------------------
		public abstract Point TotalTranslation { get; }
		//***********************************************************************************************************************
		}
	//---------------------------------------------------------------------------------------------------------------------------
	#endregion
	//***************************************************************************************************************************

	} // Fin du namespace "System.Windows.Phone.Gestures"
//*******************************************************************************************************************************

//*******************************************************************************************************************************
// FIN DU FICHIER
//*******************************************************************************************************************************
