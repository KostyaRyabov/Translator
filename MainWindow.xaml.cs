using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using System.Windows.Documents;
using System.Windows.Media;

using System.Text.RegularExpressions;

namespace TAFI_DZ1v3
{
    class Digit
    {
        public string F1, F11, F10;
        public Digit(string F1, string F11, string F10)
        {
            this.F1 = F1;
            this.F11 = F11;
            this.F10 = F10;
        }
    }

    public partial class MainWindow : Window
    {
        List<Digit> DigitBase = new List<Digit>();
        byte i;
        int Res, counter;
        string Text;
        string[] words;

        BrushConverter converter = new BrushConverter();

        TextRange TextRange;

        string input;
        Regex r = new Regex(@"\s+");

        bool Error = false;
        public MainWindow()
        {
            InitializeComponent();

            DigitBase.Add(new Digit("", "zehn", ""));
            DigitBase.Add(new Digit("ein", "elf", ""));
            DigitBase.Add(new Digit("zwei", "zwolf", "zwanzig"));
            DigitBase.Add(new Digit("drei", "dreizehn", "dreizig"));
            DigitBase.Add(new Digit("vier", "vierzehn", "vierzig"));
            DigitBase.Add(new Digit("funf", "funfzehn", "funfzig"));
            DigitBase.Add(new Digit("sechs", "sechzehn", "sechzig"));
            DigitBase.Add(new Digit("sieben", "siebzehn", "siebzig"));
            DigitBase.Add(new Digit("acht", "achtzehn", "achtzig"));
            DigitBase.Add(new Digit("neun", "neunzehn", "neunzig"));

            INPUT.AcceptsReturn = false;
        }
        private bool cUnits()
        {
            if (counter >= words.Length) return false;

            for (i = 1; i <= 9; i++)
            {
                if (words[counter] == DigitBase[i].F1)
                {
                    counter++;
                    Res += i;
                    return true;
                }
            }

            return false;
        }
        private bool c10_19()
        {
            if (counter >= words.Length) return false; 
            
            for (i = 0; i <= 9; i++)
            {
                if (words[counter] == DigitBase[i].F11)
                {
                    counter++;
                    Res += i + 10;
                    return true;
                }
            }

            return false;
        }
        private bool cDecimal()
        {
            if (counter >= words.Length) return false;

            for (i = 2; i <= 9; i++)
            {
                if (words[counter] == DigitBase[i].F10)
                {
                    counter++;
                    Res += i * 10;
                    return true;
                }
            }

            return false;
        }
        private bool cHundred()
        {
            if (counter >= words.Length) return false; 
            
            if (words[counter] == "hundert")
            {
                if (Res % 10 > 0)
                {
                    Res += (Res % 10) * 100;
                    Res -= (Res % 10);
                }
                else
                {
                    Res += 100;
                }

                counter++;
                return true;
            }

            return false;
        }
        private bool cEINS()
        {
            if (counter >= words.Length) return false;

            if (words[counter] == "eins")
            {
                Res += 1;
                counter++;
                return true;
            }

            return false;
        }
        private bool cUND()
        {
            if (counter >= words.Length) return false; 
            
            if (words[counter] == "und")
            {
                counter++;
                return true;
            }

            return false;
        }
        private bool isEnd()
        {
            return (counter >= words.Length);
        }
        private bool cWord()
        {
            return (cEINS() || cUnits() || cUND() || cDecimal() || c10_19() || cHundred());
        }
        private int getWordPos(int index, bool getEndPoint = false)
        {
            int spaceCounter = 0;
            bool SkipSpaces = false;

            for (i = 0; i < Text.Length && Text[i] == ' '; i++);

            for (; i < Text.Length; i++)
            {
                if (Text[i] == ' ')
                {
                    if (!SkipSpaces)
                    {
                        spaceCounter++;
                        SkipSpaces = true;
                    }
                }
                else
                {
                    if (spaceCounter >= index) break;

                    SkipSpaces = false;
                }
            }

            if (getEndPoint)
            {
                for (; i < Text.Length && Text[i] != ' '; i++) ;
            }

            i += 2;

            return i;
        }
        private void Message(string str, int offset, bool strictly = false)
        {
            int Error_pos = counter - offset;

            Error = true;

            TextRange = new TextRange(INPUT.Document.ContentStart, INPUT.Document.ContentStart.GetPositionAtOffset(getWordPos(Error_pos, true)));
            TextRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);

            TextRange = new TextRange(INPUT.Document.ContentStart.GetPositionAtOffset(getWordPos(Error_pos)), INPUT.Document.ContentEnd);

            if (strictly)
            {
                TextRange.ApplyPropertyValue(TextElement.BackgroundProperty, (Brush)converter.ConvertFromString("#F78181"));
                OUTPUT.Text = "некорректный ввод";
            }
            else
            {
                TextRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Orange);
                //OUTPUT.Text = "логическая ошибка";
                OUTPUT.Text = "";
            }

            OUTPUT2.Text = str;
        }
        private string TranslateNumToRoman()
        {
            string result = "";

            if (Res >= 900)
            {
                Res -= 900;
                result += "CM";
            }

            while (Res >= 500)
            {
                Res -= 500;
                result += 'D';
            }

            if (Res >= 400)
            {
                Res -= 400;
                result += "CD";
            }

            while (Res >= 100)
            {
                Res -= 100;
                result += 'C';
            }

            if (Res >= 90)
            {
                Res -= 90;
                result += "XC";
            }

            while (Res >= 50)
            {
                Res -= 50;
                result += 'L';
            }

            if (Res >= 40)
            {
                Res -= 40;
                result += "XL";
            }

            while (Res >= 10)
            {
                Res -= 10;
                result += 'X';
            }

            if (Res >= 9)
            {
                Res -= 9;
                result += "IX";
            }

            while (Res >= 5)
            {
                Res -= 5;
                result += 'V';
            }

            if (Res >= 4)
            {
                Res -= 4;
                result += "IV";
            }

            while (Res >= 1)
            {
                Res -= 1;
                result += 'I';
            }

            return result;
        }
        private void TranslateTextToNum()
        {
            if (c10_19())
            {
                if (c10_19()) Message("обнаружен 2й 10-19 формат. После формата 10-19 ожидался конец", 1);
                else if (cUnits())
                {
                    if (cUND())
                    {
                        if (isEnd()) Message("После формата 10-19 ожидался конец",2);
                        else Message("После формата 10-19 ожидался конец", 2);
                    }
                    else Message("После формата 10-19 ожидался конец", 1);
                }
                else if (cWord()) Message("После формата 10-19 ожидался конец", 1);
                else if (!isEnd()) Message("после формата 10-19 ожидался конец", 0, true);
            }
            else if (cDecimal())
            {
                if (cDecimal()) Message("обнаружен 2й десятичный формат. После десятичного формата ожидался конец",1);
                else if (cUnits())
                {
                    if (cUND())
                    {
                        if (cDecimal()) Message("обнаружен 2й десятичный формат. После десятичного формата ожидался конец",3);
                        else if (isEnd()) Message("После десятков ожидался конец",2);
                        else Message("после десятичного формата ожидался конец",2);
                    }
                    else Message("После десятичного формата ожидался конец",1);
                }
                else if (cWord()) Message("После десятичного формата ожидался конец", 1);
                else if (!isEnd()) Message("После десятичного формата ожидался конец", 0, true);
            }
            else if (cHundred())
            {
                if (c10_19())
                {
                    if (cDecimal() || cEINS()) Message("После формата 10-19 ожидался конец", 1);
                    else if (c10_19()) Message("обнаружен 2й 10-19 формат. После формата 10-19 ожидался конец", 1);
                    else if (cUnits())
                    {
                        if (cUND())
                        {
                            if (cDecimal()) Message("После формата 10-19 ожидался конец",3);
                            else if (isEnd()) Message("После формата 10-19 ожидался конец",2);
                            else Message("после формата 10-19 ожидался конец",2);
                        }
                        else if (cHundred()) Message("обнаружен 2й сотенный формат. После формата 10-19 ожидался конец", 2);
                        else Message("После формата 10-19 ожидался конец",1);
                    }
                    else if (cHundred()) Message("обнаружен 2й сотенный формат. После формата 10-19 ожидался конец", 1);
                    else if (!isEnd()) Message("После формата 10-19 ожидался конец", 0, true);
                }
                else if (cDecimal())
                {
                    if (cDecimal()) Message("обнаружен 2й десятичный формат. После десятичного формата ожидался конец", 1);
                    else if (c10_19() || cEINS()) Message("После десятичного формата ожидался конец",1);
                    else if (cUnits())
                    {
                        if (cUND())
                        {
                            if (cDecimal()) Message("обнаружен 2й десятичный формат. После десятичного формата ожидался конец", 3);
                            else if (isEnd()) Message("После десятичного формата ожидался конец", 2);
                            else Message("после десятичного формата ожидался конец", 2);
                        }
                        else if (cHundred()) Message("обнаружен 2й сотенный формат. После десятичного формата ожидался конец",2);
                        else Message("После десятичного формата ожидался конец",1);
                    }
                    else if (cHundred()) Message("обнаружен 2й сотенный формат. После десятичного формата ожидался конец",1);
                    else if (!isEnd()) Message("После десятичного формата ожидался конец", 0, true);
                }
                else if (cHundred()) Message("обнаружен 2й сотенный формат. После сотенного формата ожидались единичный/десятичный/10-19 формат или конец",1);
                else if (cEINS())
                {
                    if (cUnits())
                    {
                        if (cHundred()) Message("обнаружен 2й сотенный формат. После единичного формата ожидался конец", 2);
                        else Message("обнаружен 2й единичный формат. После единичного формата ожидался конец", 1);
                    }
                    else if (cWord()) Message("вместо 'eins' ожидалось ein", 1);
                    else if (!isEnd()) Message("вместо 'eins' ожидалось ein", 0, true);
                }
                else if (cUnits())
                {
                    if (cUND())
                    {
                        if (cDecimal())
                        {
                            if (cDecimal()) Message("обнаружен 2й десятичный формат. После десятичного формата ожидался конец", 1);
                            else if (c10_19()) Message("После десятичного формата ожидался конец",1);
                            else if (cHundred()) Message("обнаружен 2й сотенный формат. После десятичного формата ожидался конец",1);
                            else if (cUnits())
                            {
                                if (cHundred()) Message("обнаружен 2й сотенный формат. После десятичного формата ожидался конец",2);
                                else if (cUND())
                                {
                                    if (cDecimal()) Message("обнаружен 2й десятичный формат. После десятичного формата ожидался конец",3);
                                    else if (isEnd()) Message("После десятичного формата ожидался конец",2);
                                    else Message("после десятичного формата ожидался конец",2, true);
                                }
                                else Message("обнаружен 2й единичный формат. После десятичного формата ожидался конец",1);
                            }
                            else if (!isEnd()) Message("после десятичного формата ожидался конец", 0, true);
                        }
                        else if (isEnd()) Message("После 'und' ожидался десятичный формат", 1);
                        else Message("после 'und' ожидался десятичный формат", 1, true);
                    }
                    else if (cDecimal()) Message("После единичного формата ожидался 'und' или конец",1);
                    else if (cHundred()) Message("обнаружен 2й сотенный формат. После сотенного формата ожидался единичный/десятичный/10-19 формат или конец",2);
                    //else if (Res % 10 == 1 && isEnd()) Message("после единиц ('ein') ожидался 'und' или вместо 'ein' ожидался 'eins'",1);
                    else
                    {
                        if (cUnits() || cEINS()) Message("обнаружен 2й единичный формат. после единичного формата ожидался конец или 'und'", 1);
                        else if (!isEnd()) Message("После единичного формата ожидался 'und' или конец", 0, true);
                    }
                }
                else if (!isEnd()) Message("После сотененного формата ожидался единичный/десятичный/10-19 формат или конец", 0, true);
            }
            else if (cEINS())
            {
                if (cUnits() || cEINS()) Message("обнаружен 2й единичный формат. После единичного формата ожидался конец", 1);
                else if (cHundred() || cUND()) Message("ожидался конец или вместо 'eins' ожидалось 'ein'", 1);
                else if (cWord()) Message("После единичного формата ожидался конец", 1);
                else if (!isEnd()) Message("После единичного формата ожидался конец", 0, true);
            }
            else if (cUnits())
            {
                //if (Res % 10 == 1 && isEnd()) Message("после единиц ('ein') ожидался 'und', 'hundert' или вместо 'ein' ожидался 'eins'", 1); else
                if (cHundred())
                {
                    if (c10_19())
                    {
                        if (cDecimal()) Message("После формата 10-19 ожидался конец", 1);
                        else if (c10_19()) Message("обнаружен 2й 10-19 формат. После формата 10-19 ожидался конец", 1);
                        else if (cUnits())
                        {
                            if (cUND()) Message("после формата 10-19 ожидался конец", 2);
                            else if (cHundred()) Message("обнаружен 2й сотeнный формат. После формата 10-19 ожидался конец", 2);
                            else Message("После формата 10-19 ожидался конец", 1);
                        }
                        else if (cEINS()) Message("После формата 10-19 ожидался конец", 1);
                        else if (cHundred()) Message("обнаружен 2й сотунный  формат. После формата 10-19 ожидался конец", 1);
                        else if (!isEnd()) Message("После формата 10-19 ожидался конец", 0, true);
                    }
                    else if (cDecimal())
                    {
                        if (cDecimal()) Message("обнаружен 2й десятичный формат. После десятков ожидался конец",1);
                        else if (c10_19() || cEINS()) Message("После десятков ожидался конец",1);
                        else if (cUnits())
                        {
                            if (cUND())
                            {
                                if (cDecimal()) Message("обнаружен 2й десятичный формат. После десятков ожидался конец",3);
                                else if (isEnd()) Message("После десятков ожидался конец",2);
                                else Message("после десятков ожидался конец",2);
                            }
                            //if (Res % 10 == 1 && isEnd()) Message("после единиц ('ein') ожидался конец или вместо 'ein' ожидался 'eins'", 1); else
                            else if (cHundred()) Message("обнаружен 2й сотенный формат. После десятков ожидался конец",2);
                            else Message("После десятков ожидался конец",1);
                        }
                        else if (cHundred()) Message("обнаружен 2й сотенный формат. После десятков ожидался конец", 1);
                        else if (!isEnd()) Message("После десятков ожидался конец", 0, true);
                    }
                    else if (cHundred()) Message("обнаружен 2й сотенный формат. После сотененного формата ожидалcя единичный/десятичный/10-19 формат или конец", 1);
                    else if (cEINS())
                    {
                        if (cUnits())
                        {
                            if (cHundred()) Message("обнаружен 2й сотый формат. После единиц ожидался конец",2);
                            else Message("обнаружен 2й единичный формат. После единиц ожидался конец", 1);
                        }
                        else if (cHundred() || cUND()) Message("вместо 'eins' ожидалось ein", 1);
                        else if (!isEnd()) Message("вместо 'eins' ожидалось ein", 0, true);
                    }
                    else if (cUnits())
                    {
                        if (cUND())
                        {
                            if (cDecimal())
                            {
                                if (cDecimal()) Message("обнаружен 2й десятичный формат. После десятичного формата ожидался конец", 1);
                                else if (c10_19()) Message("После десятичного формата ожидался конец",1);
                                else if (cEINS()) Message("обнаружен 2й единичный формат. После десятичного формата ожидался конец", 1);
                                else if (cHundred()) Message("обнаружен 2й сотенный формат. После единичного формата ожидался конец",1);
                                else if (cUnits())
                                {
                                    if (cHundred()) Message("обнаружен 2й сотенный формат. После единичного формата ожидался конец",2);
                                    else if (cUND())
                                    {
                                        if (cDecimal()) Message("обнаружен 2й десятичный формат. После десятичного формата ожидался конец",3);
                                        else if (isEnd()) Message("После десятичного формата ожидался конец",2);
                                        else Message("после десятичного формата ожидался конец",2);
                                    }
                                    else Message("обнаружен 2й единичный формат. После десятичного формата ожидался конец",1);
                                }
                                else if (!isEnd()) Message("после десятичного формата ожидался конец", 0, true);
                            }
                            else if (isEnd()) Message("После 'und' ожидался десятичный формат", 1);
                            else Message("после 'und' ожидался десятичный формата", 1, true);
                        }
                        else if (cHundred()) Message("обнаружен 2й сотенный формат. После сотенного формата ожидался едининичный/десятичный/10-19 формат или конец",2);
                        //else if (Res % 10 == 1 && isEnd()) Message("после единиц ('ein') ожидался 'und' или вместо 'ein' ожидался 'eins'", 1);
                        else if (cUnits() || cEINS()) Message("обнаружен 2й единичный формат. После единичного формата ожидался конец или 'und'", 1);
                        else if (cDecimal() || c10_19()) Message("После единичного формата ожидался конец или 'und'", 1);
                        else if (!isEnd()) Message("После единичного формата ожидался 'und' или конец", 0, true);
                    }
                    else if (!isEnd()) Message("После сотенного формата ожидался единчный/десятичный/10-19 формат или конец", 0, true);
                }
                else if (cUND())
                {
                    if (cDecimal())
                    {
                        if (cDecimal()) Message("обнаружен 2й десятичный формат. После десятичного формата ожидался конец",1);
                        else if (c10_19()) Message("обнаружен 2й десятичный формат. После десятичного формата ожидался конец",1);
                        else if (cUnits())
                        {
                            if (cUND())
                            {
                                if (cDecimal()) Message("обнаружен 2й десятичный формат. После десятичного формата ожидался конец", 3);
                                else if (isEnd()) Message("обнаружен 2й единичный формат. После десятичного формата ожидался конец", 2);
                                else Message("обнаружен 2й единичный формат. После десятичного формата ожидался конец", 2, true);
                            }
                            if (cWord()) Message("После десятичного формата ожидался конец", 2);
                            else Message("обнаружен 2й единичный формат. После десятичного формата ожидался конец", 1);
                        }
                        else if (!isEnd()) Message("После десятичного формата ожидался конец", 0, true);
                    }
                    else if (isEnd()) Message("После 'und' ожидался десятичный формат", 1);
                    else if (cWord()) Message("После 'und' ожидался десятичный формат", 1);
                    else Message("После 'und' ожидался десятичный формат", 0, true);
                }
                else if (cEINS()) Message("обнаружен 2й единичный формат. После единичного формата ожидался 'und', 'hundert' или конец", 1);
                else if (cUnits()) Message("обнаружен 2й единичный формат. После единичного формата ожидался 'und', 'hundert' или конец", 1);
                else if (cWord()) Message("После единичного формата ожидался 'und', 'hundert' или конец", 1);
                else if (!isEnd()) Message("После единичного формата ожидался 'und', 'hundert' или конец", 0, true);
            }
            else Message("ожидался единичный/десятичный/10-19/сотенный формат",0, true);
        }
        private void INPUT_KeyUp(object sender, KeyEventArgs e)
        {
            Text = new TextRange(INPUT.Document.ContentStart, INPUT.Document.ContentEnd).Text;

            if (Text.Trim().Length > 0)
            {
                input = Text.Trim().ToLower();
                input = r.Replace(input, @" ");

                words = input.Split(' ');

                Res = 0;
                counter = 0;

                TranslateTextToNum();
            }

            if (!Error)
            {
                if (Res == 0) OUTPUT.Text = "";
                else OUTPUT.Text = Res.ToString();

                TextRange = new TextRange(INPUT.Document.ContentStart, INPUT.Document.ContentEnd);
                TextRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);

                OUTPUT2.Text = TranslateNumToRoman();
            }
            else Error = false;
        }
    }
}