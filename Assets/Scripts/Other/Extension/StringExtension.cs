using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class StringExtension {


    public static string Cut( this string target, int length, string append = "..." ) {

        if( target.Length > length ) {
            target = target.Substring( 0, length ).TrimEnd() + append;
        }

        return target;
    }


}
