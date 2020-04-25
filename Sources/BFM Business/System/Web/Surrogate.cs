//*******************************************************************************************************************************
// DEBUT DU FICHIER
//*******************************************************************************************************************************

//*******************************************************************************************************************************
// Nom           : Surrogate.cs
// Auteur        : Nicolas Dagnas
// Description   : Implémentation de l'objet Surrogate
// Créé le       : 01/03/2015
// Modifié le    : 24/05/2015
//*******************************************************************************************************************************

//-------------------------------------------------------------------------------------------------------------------------------
#region Using directives
//-------------------------------------------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Globalization;
//-------------------------------------------------------------------------------------------------------------------------------
#endregion
//-------------------------------------------------------------------------------------------------------------------------------

//*******************************************************************************************************************************
// Début du bloc "System.Web"
//*******************************************************************************************************************************
namespace System.Web
	{

	//   ####  #   #  ####   ####    ###    ###     ###   #####  #####
	//  #      #   #  #   #  #   #  #   #  #   #   #   #    #    #    
	//   ###   #   #  ####   ####   #   #  #       #####    #    ###  
	//      #  #   #  #   #  #   #  #   #  #   ##  #   #    #    #    
	//  ####    ###   #   #  #   #   ###    ### #  #   #    #    #####

	//***************************************************************************************************************************
	// Classe Surrogate
	//***************************************************************************************************************************
	#region // Déclaration et Implémentation de l'Objet
	//---------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Fournit un convertisseur de caractères dit Surrogate.
	/// </summary>
	//---------------------------------------------------------------------------------------------------------------------------
	public static class Surrogate
		{
		//-----------------------------------------------------------------------------------------------------------------------
		// Section des Attributs
		//-----------------------------------------------------------------------------------------------------------------------
		private const char HIGH_SURROGATE_START   = '\uD800';
		private const char LOW_SURROGATE_START    = '\uDC00';
		private const int  UNICODE_PLANE01_START  = 0x10000;
		private const int  UnicodeReplacementChar = '\uFFFD';
		//-----------------------------------------------------------------------------------------------------------------------

		//***********************************************************************************************************************
		private static int IndexOfHtmlEncodingChars ( string Str, int StartPos )
			{
			int Sch = Str.Length;
			int Cch = Str.Length - StartPos;
			
			for ( int Pch = StartPos ; Cch > 0 && Pch < Sch ; Cch --, Pch ++ )
				{
				char Ch = Str[Pch];

				if ( Char.IsSurrogate ( Ch ) ) return Str.Length - Cch;

				if ( Ch > 127 ) return Str.Length - Cch;
				}

			return -1;
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		private static int GetNextUnicodeScalarValueFromUtf16Surrogate ( string Str, 
		                                                                 ref int Pch, ref int Cch )
			{
			if ( Cch <= 1 ) return UnicodeReplacementChar;

			char leadingSurrogate  = Str[Pch    ];
			char trailingSurrogate = Str[Pch + 1];

			if ( Char.IsSurrogatePair ( leadingSurrogate, trailingSurrogate ) )
				{
				Pch ++;
				Cch --;

				return ( ( ( leadingSurrogate - HIGH_SURROGATE_START ) * 0x400 ) + 
				             ( trailingSurrogate - LOW_SURROGATE_START ) + UNICODE_PLANE01_START );
				}
			else { return UnicodeReplacementChar; }
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		public static void Encode ( string Str, TextWriter Output )
			{
			if ( Str    == null ) return;
			if ( Output == null ) throw new ArgumentNullException ( "Output" );

			int Index = IndexOfHtmlEncodingChars ( Str, 0 );

			if ( Index == -1 ) { Output.Write ( Str ); return; }

			if ( Index > 0 ) Output.Write ( Str.Substring ( 0, Index ) );
			
			int Size = Str.Length;
			int Cch  = Str.Length - Index;
			
			for ( int Pch = Index ; Cch > 0 && Pch < Size ; Cch --, Pch ++ )
				{
				char Ch = Str[Pch];

				int valueToEncode = -1;

				if ( Char.IsSurrogate ( Ch ) )
					{
					int scalarValue = GetNextUnicodeScalarValueFromUtf16Surrogate ( Str, ref Pch, ref Cch );
					if ( scalarValue >= UNICODE_PLANE01_START )
						{
						valueToEncode = scalarValue;
						}
					else { Ch = (char)scalarValue; }
					}
				else if ( Ch > 127 )
					{
					valueToEncode = Ch;
					}

				if ( valueToEncode >= 0 )
					{
					Output.Write ( "&#"                                                      );
					Output.Write ( valueToEncode.ToString ( NumberFormatInfo.InvariantInfo ) );
					Output.Write ( ';'                                                       );
					}
				else { Output.Write ( Ch ); }
				}
			}
		//***********************************************************************************************************************

		//***********************************************************************************************************************
		public static string Encode ( string Str )
			{
			if ( String.IsNullOrEmpty ( Str ) ) return Str;

			Str = Str.Replace ( "\r\n", "\n" ).Replace ( "\r", "\n" );

			int Index = IndexOfHtmlEncodingChars ( Str, 0 );

			if ( Index == -1 ) return Str;

			StringWriter Writer = new StringWriter ( CultureInfo.InvariantCulture );

			Encode ( Str, Writer );

			return Writer.ToString ();
			}
		//***********************************************************************************************************************
		}
	//---------------------------------------------------------------------------------------------------------------------------
	#endregion
	//***************************************************************************************************************************

	} // Fin du namespace "System.Web"
//*******************************************************************************************************************************

//*******************************************************************************************************************************
// FIN DU FICHIER
//*******************************************************************************************************************************
