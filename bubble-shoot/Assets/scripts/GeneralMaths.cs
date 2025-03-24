using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeneralMaths : MonoBehaviour
{
    public static int RoundValue(float value)
    {
        int roundvalue;
        string strValue = value.ToString();
       
        //start of the number
        string truncated = strValue.Contains(".") ? strValue.Split('.')[0] : strValue;
        int intValue = int.Parse(truncated);

        //first digit of the decimal
        string decimalPart = strValue.Contains(".") ? strValue.Split('.')[1] : "0";
        char firstDecimalDigit = decimalPart.Length > 0 ? decimalPart[0] : '0';
        int ParsedecimalPart = int.Parse(firstDecimalDigit.ToString());

        if (ParsedecimalPart < 5)
        {
            roundvalue = intValue;
        }

        else
        {
            roundvalue = intValue + 1;
        }

        

        return roundvalue;
    }

    public int roundvalue2(float value)
    {
        int roundedval=0;
        /*truncate the float to get an int without the decimal points,
         * take the modulo of the number to just get the decimal, if its .5 or more add else subtract 1 */

        return roundedval;
    }
}
