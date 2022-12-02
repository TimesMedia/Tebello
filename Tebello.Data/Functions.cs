using System;
using System.Collections.Specialized;

using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Subs.Data
{
    public static class Functions
    {
        static public bool IsInteger(string myString)
        {
            bool Numeric = true;
            foreach (char i in myString)
            {
                if (!char.IsNumber(i)) { Numeric = false; }
            }
            if (!Numeric) { return false; }
            else return true;
        }

        static public bool IsDecimal(string myString)
        {
            Regex myRegEx = new Regex(@"^[\d.-]+$");
            return myRegEx.IsMatch(myString);
        }

        public static int GetCheckedList(CheckedListBox pControl)
        {
            int Number = 0;
            for (int i = 0; i < pControl.Items.Count; i++)
            {
                if (pControl.GetItemChecked(i))
                {
                    Number = Number + (int)Math.Pow(2, i);
                }
            }
            return Number;
        }


        public static void SetCheckedList(CheckedListBox pControl, int Number)
        {
            // Correspondence
            int Size = pControl.Items.Count;

            if (Number > (int)Math.Pow(2, Size) - 1)
            {
                throw new Exception("I cannot handle a number of that size.");
            }

            for (int i = Size - 1; i >= 0; i--)
            {
                if (Number >= (int)Math.Pow(2, i))
                {
                    pControl.SetItemCheckState(i, CheckState.Checked);
                    Number = Number - (int)Math.Pow(2, i);
                }
                else
                {
                    pControl.SetItemCheckState(i, CheckState.Unchecked);
                }
            }

            pControl.Refresh();

        }


        public static bool BitSelector(int pCombination, int pSelector)
        {
            BitVector32  lVector = new BitVector32(pCombination);

            int lMask1 = BitVector32.CreateMask();
            int lMask2 = BitVector32.CreateMask(lMask1);
            int lMask3 = BitVector32.CreateMask(lMask2);
            int lMask4 = BitVector32.CreateMask(lMask3);
            int lMask5 = BitVector32.CreateMask(lMask4);
            int lMask6 = BitVector32.CreateMask(lMask5);
            int lMask7 = BitVector32.CreateMask(lMask6);
            int lMask8 = BitVector32.CreateMask(lMask7);


            switch (pSelector)
            { 
                case 1: return lVector[lMask1];
                case 2: return lVector[lMask2];
                case 3: return lVector[lMask3];
                case 4: return lVector[lMask4];
                case 5: return lVector[lMask5];
                case 6: return lVector[lMask6];
                case 7: return lVector[lMask7];
                case 8: return lVector[lMask8];
                default: return false;       
            }
        }
    }
}
