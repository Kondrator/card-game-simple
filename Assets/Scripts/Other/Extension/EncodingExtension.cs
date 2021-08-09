using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;


public static class EncodingExtension{

	public static string EncodeBase64( this Encoding encoding, string text ){

		if( text == null ){
			return null;
		}

		byte[] textAsBytes = encoding.GetBytes( text );
		return System.Convert.ToBase64String( textAsBytes );
	}

	public static string DecodeBase64( this Encoding encoding, string encodedText ){

		if( encodedText == null ){
			return null;
		}

		byte[] textAsBytes = Convert.FromBase64String( encodedText );
		return encoding.GetString( textAsBytes );
	}

}