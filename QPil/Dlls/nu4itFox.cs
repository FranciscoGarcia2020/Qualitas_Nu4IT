using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPil.Dlls
{
    class nu4itFox
    {
        /*******************************************************************************************************************************************************/
        /********************************************************Funciones de FoxPro implementasa en c#*********************************************************/
        /*******************************************************************************************************************************************************/
        public int At(string cSearchFor, string cSearchIn)
        {
            return cSearchIn.IndexOf(cSearchFor) + 1;
        }

        public int At(string cSearchFor, string cSearchIn, int nOccurence)
        {
            return __at(cSearchFor, cSearchIn, nOccurence, 1);
        }

        private int __at(string cSearchFor, string cSearchIn, int nOccurence, int nMode)
        {
            //In this case we actually have to locate the occurence	
            int i = 0;
            int nOccured = 0;
            int nPos = 0;
            if (nMode == 1) { nPos = 0; }
            else { nPos = cSearchIn.Length; }
            //Loop through the string and get the position of the requiref occurence	
            for (i = 1; i <= nOccurence; i++)
            {
                if (nMode == 1) { nPos = cSearchIn.IndexOf(cSearchFor, nPos); }
                else { nPos = cSearchIn.LastIndexOf(cSearchFor, nPos); }
                if (nPos < 0)
                {
                    //This means that we did not find the item			
                    break;
                }
                else
                {
                    //Increment the occured counter based on the current mode we are in			
                    nOccured++;
                    //Check if this is the occurence we are looking for			
                    if (nOccured == nOccurence)
                    {
                        return nPos + 1;
                    }
                    else
                    {
                        if (nMode == 1) { nPos++; }
                        else { nPos--; }
                    }
                }
            }
            //We never found our guy if we reached here	
            return 0;
        }

        public string StrExtract(string cSearchExpression, string cBeginDelim, string cEndDelim, int nBeginOccurence, int nFlags)
        {
            string cstring = cSearchExpression;
            string cb = cBeginDelim;
            string ce = cEndDelim;
            string lcRetVal = "";

            // Si el string del pajar está vacío, regresamos un string vacío
            if (cstring.Length == 0) { return ""; }

            //Check for case-sensitive or insensitive search	
            if (nFlags == 1)
            {
                cstring = cstring.ToLower();
                cb = cb.ToLower();
                ce = ce.ToLower();
            }
            //Lookup the position in the string	
            int nbpos = At(cb, cstring, nBeginOccurence);
            // Si no aparece la aguja en el pajar, regresamos un string vacío
            if (nbpos == 0) { return ""; }
            nbpos = nbpos + cb.Length - 1;

            int nepos = cstring.IndexOf(ce, nbpos + 1);
            //Extract the part of the strign if we get it right	
            if (nepos > nbpos)
            {
                lcRetVal = cSearchExpression.Substring(nbpos, nepos - nbpos);
            }
            return lcRetVal;
        }

        public string StrExtract(string cSearchExpression, string cBeginDelim)
        {
            int nbpos = At(cBeginDelim, cSearchExpression);
            return cSearchExpression.Substring(nbpos + cBeginDelim.Length - 1);
        }

        public string StrExtract(string cSearchExpression, string cBeginDelim, string cEndDelim)
        {
            return StrExtract(cSearchExpression, cBeginDelim, cEndDelim, 1, 0);
        }

        public string StrExtract(string cSearchExpression, string cBeginDelim, string cEndDelim, int nBeginOccurence)
        {
            return StrExtract(cSearchExpression, cBeginDelim, cEndDelim, nBeginOccurence, 0);
        }

        public int Occurs(char tcChar, string cExpression)
        {
            int i,
            nOccured = 0;
            //Loop through the string	
            for (i = 0; i < cExpression.Length; i++)
            {
                //Check if each expression is equal to the one we want to check against		
                if (cExpression[i] == tcChar)
                {
                    //if  so increment the counter			
                    nOccured++;
                }
            }
            return nOccured;
        }

        public int Occurs(string cString, string cExpression)
        {
            int nPos = 0;
            int nOccured = 0;
            do
            {
                //Look for the search string in the expression		
                nPos = cExpression.IndexOf(cString, nPos);
                if (nPos < 0)
                {
                    //This means that we did not find the item			
                    break;
                }
                else
                {
                    //Increment the occured counter based on the current mode we are in			
                    nOccured++;
                    nPos++;
                }
            } while (true);
            //Return the number of occurences	
            return nOccured;
        }

    }
}
